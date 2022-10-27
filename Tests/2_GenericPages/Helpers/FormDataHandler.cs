using System.Globalization;
using System.Reflection;
using Bellatrix;
using Bellatrix.Assertions;
using Bellatrix.Web;

namespace SeleniumVsBellatrixAutomatedTests.Tests.GenericPages.Helpers
{
    public class FormDataHandler<TFormModel>
        where TFormModel : class, new()
    {
        private readonly ComponentCreateService _elementCreateService;

        public FormDataHandler()
        {
            _elementCreateService = ServicesCollection.Current.Resolve<ComponentCreateService>();
        }

        public void FillForm(TFormModel item)
        {
            // Get all properties from the model and iterate over them
            var properties = typeof(TFormModel).GetProperties().ToList();
            foreach (PropertyInfo property in properties)
            {
                var valueToSet = property.GetValue(item);
                if (valueToSet == null)
                {
                    continue;
                }

                PropertyData propData = ExtractPropFormData(property);

                if (propData.ControlType.IsGenericType)
                {
                    string[] values = ((string)valueToSet).Split(';');
                    Type[] genericTypes = propData.ControlType.GetGenericArguments();

                    for (int i = 0; i < values.Length; i++)
                    {
                        Type type = genericTypes[i];
                        propData.ControlType = type;
                        SetElementValue(propData, values[i]);
                    }
                }
                else
                {
                    SetElementValue(propData, valueToSet);
                }
            }
        }

        public void FillGridRow(GridRow gridRow, TFormModel item)
        {
            // Get all properties from the model and iterate over them
            var properties = typeof(TFormModel).GetProperties().ToList();
            foreach (PropertyInfo property in properties)
            {
                var valueToSet = property.GetValue(item);
                PropertyData propData = ExtractPropGridData(property);

                if (valueToSet == null)
                {
                    continue;
                }

                try
                {
                    dynamic element = gridRow.GetCell(propData.HeaderName).As();
                    if (element != null)
                    {
                        dynamic controlDataHandler = ControlDataHandlerResolver.ResolveEditableDataHandler(propData.ControlType);
                        if (controlDataHandler != null)
                        {
                            // Set the value passed in the item variable
                            controlDataHandler.SetData(element, valueToSet.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to set the value '{valueToSet}' into control with type '{propData.ControlType}' and Header name '{propData.HeaderName}'", ex);
                }
            }
        }

        public TFormModel ExtractFormData(params string[] propertiesNotToExtract)
        {
            var item = new TFormModel();
            var properties = typeof(TFormModel).GetProperties().ToList();

            foreach (PropertyInfo property in properties)
            {
                if (propertiesNotToExtract.Contains(property.Name))
                {
                    continue;
                }

                PropertyData propData = ExtractPropFormData(property);

                try
                {
                    dynamic elementValue;

                    if (propData.ControlType.IsGenericType)
                    {
                        Type[] genericTypes = propData.ControlType.GetGenericArguments();

                        var controlValues = genericTypes.Select(s =>
                        {
                            propData.ControlType = s;
                            return GetElementValue(propData);
                        }).ToList();

                        elementValue = string.Join(';', controlValues).Trim(';');
                    }
                    else
                    {
                        elementValue = GetElementValue(propData);
                    }

                    Type type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                    if (type == typeof(DateTime) || type == typeof(DateTime?))
                    {
                        DateTime.TryParse(elementValue, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime dateTime);
                        elementValue = dateTime == default ? null : (DateTime?)dateTime;
                    }
                    else
                    {
                        elementValue = type == typeof(string) && string.IsNullOrEmpty(elementValue) ? default : Convert.ChangeType(elementValue, type, CultureInfo.InvariantCulture);
                    }

                    property.SetValue(item, elementValue, null);
                }
                catch (Exception ex)
                {
                    Logger.LogWarning($"The data from field with label '{propData.Label}' cannot be extracted.", ex);
                }
            }

            return item;
        }

        public void AssertFilledForm(TFormModel expected)
        {
            var propsNotToCompare = expected
                .GetType()
                .GetProperties()
                .Where(p => p.GetValue(expected) == null)
                .Select(n => n.Name)
                .ToArray();

            TFormModel actual = ExtractFormData(propsNotToCompare);

            EntitiesAsserter.AreEqual<TFormModel>(expected, actual, propsNotToCompare);
        }

        #region PrivateMethods
        private void SetElementValue(PropertyData propData, object valueToSet)
        {
            dynamic element = CreateElement(propData.Label, propData.ControlType, propData.Section);
            dynamic controlDataHandler = ControlDataHandlerResolver.ResolveEditableDataHandler(propData.ControlType);

            // This check if the control is Editable and skip ReadOnly ones
            if (controlDataHandler == null)
            {
                return;
            }

            try
            {
                controlDataHandler.SetData(element, valueToSet.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to set the value '{valueToSet}' into field with label '{propData.Label}'", ex);
            }
        }

        private dynamic GetElementValue(PropertyData propData)
        {
            dynamic elementValue;

            dynamic element = CreateElement(propData.Label, propData.ControlType, propData.Section);
            dynamic controlDataHandler = ControlDataHandlerResolver.ResolveReadonlyDataHandler(propData.ControlType);

            try
            {
                elementValue = controlDataHandler.GetData(element);
            }
            catch (System.TimeoutException)
            {
                element = CreateElement(propData.Label, typeof(Span), propData.Section);
                controlDataHandler = ControlDataHandlerResolver.ResolveReadonlyDataHandler(typeof(Span));
                elementValue = controlDataHandler.GetData(element);
            }

            return elementValue;
        }

        private dynamic CreateElement(string labelText, Type controlType, string? sectionName = null)
        {
            Component parent = null;

            var repo = new ComponentRepository();
            dynamic element = repo.CreateComponentWithParent(new FindControlLabelStrategy(labelText, string.Empty, "."), parent?.WrappedElement, controlType, false);

            return element;
        }

        private PropertyData ExtractPropFormData(PropertyInfo property)
        {
            var propData = new PropertyData
            {
                // Determine the label name and control type by the appropriate attributes
                Label = GetPropertyName(property),
                Section = GetSectionName(property),
            };

            // Determine the control type by a custom attribute - if missing - default to TextField
            var controlTypeAttribute = (ControlTypeAttribute)property.GetCustomAttributes(typeof(ControlTypeAttribute)).FirstOrDefault();
            propData.ControlType = controlTypeAttribute != null && controlTypeAttribute.ControlType != null ? controlTypeAttribute.ControlType : typeof(TextField);

            return propData;
        }

        private PropertyData ExtractPropGridData(PropertyInfo property)
        {
            var propData = new PropertyData
            {
                // Determine the label name and control type by the appropriate attributes
                HeaderName = GetHeaderName(property),
            };

            // Determine the control type by a custom attribute - if missing - default to TextField
            var controlTypeAttribute = (ControlTypeAttribute)property.GetCustomAttributes(typeof(ControlTypeAttribute)).FirstOrDefault();
            propData.ControlType = controlTypeAttribute != null && controlTypeAttribute.ControlType != null ? controlTypeAttribute.ControlType : typeof(Label);

            return propData;
        }

        private string GetPropertyName(PropertyInfo property)
        {
            var labelNameAttribute = (LabelNameAttribute)property.GetCustomAttributes(typeof(LabelNameAttribute)).FirstOrDefault();

            return labelNameAttribute != null ? labelNameAttribute.Name : property.Name;
        }

        private string GetHeaderName(PropertyInfo property)
        {
            var headerNameAttribute = (HeaderNameAttribute)property.GetCustomAttributes(typeof(HeaderNameAttribute)).FirstOrDefault();

            return headerNameAttribute != null ? headerNameAttribute.Name : property.Name;
        }

        private string GetSectionName(PropertyInfo property)
        {
            var labelNameAttribute = (LabelNameAttribute)property.GetCustomAttributes(typeof(LabelNameAttribute)).FirstOrDefault();
            return labelNameAttribute?.SectionName;
        }

        private class PropertyData
        {
            public string Section { get; set; }

            public string Label { get; set; }

            public string HeaderName { get; set; }

            public Type ControlType { get; set; }
        }
        #endregion
    }
}
namespace SeleniumVsBellatrixAutomatedTests.Tests.GenericPages.Helpers
{
    /// <summary>
    ///   Custom Attribute that is mainly used for tagging Form/Grid model's element to indicate the type of the element.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ControlTypeAttribute : Attribute
    {
        public ControlTypeAttribute(Type controlType) => ControlType = controlType;

        public ControlTypeAttribute() => ControlType = null;

        public Type? ControlType { get; set; }
    }
}
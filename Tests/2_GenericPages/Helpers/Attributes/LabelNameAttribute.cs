namespace SeleniumVsBellatrixAutomatedTests.Tests.GenericPages.Helpers
{
    /// <summary>
    ///   Custom attribute that is used for indicating the label name of the elements inside form models.
    ///   Later the value of the attribute is used to locate the element on the page by its label.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class LabelNameAttribute : Attribute
    {
        public LabelNameAttribute(string name) => Name = name;

        public LabelNameAttribute(string name, string sectionName)
        {
            Name = name;
            SectionName = sectionName;
        }

        public string Name { get; set; }

        public string SectionName { get; set; }
    }
}

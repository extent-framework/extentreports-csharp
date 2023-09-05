namespace AventStack.ExtentReports.Model
{
    public abstract class NamedAttribute : IBaseEntity
    {
        protected NamedAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            var attr = obj as NamedAttribute;
            return attr != null && attr.Name == Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}

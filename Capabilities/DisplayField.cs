namespace Capabilities
{
    public class DisplayField
    {
        public DisplayField(DisplayFieldName name, IMappingField requiredField)
        {
            RequiredField = requiredField;
            Name = name;
        }

        public IMappingField RequiredField { get; private set; }
        public DisplayFieldName Name { get; private set; }
    }
}
using System;

namespace POSFileParser.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldNameAttribute : Attribute
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public FieldNameAttribute(string name)
        {
            _name = name;
        }
    }
}

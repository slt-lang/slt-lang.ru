using System;

namespace sltlang.Common.Common
{
    public class ComponentTypeAttribute : Attribute
    {
        public Type Type { get; set; }
        public ComponentTypeAttribute(Type type) => Type = type;
    }
}

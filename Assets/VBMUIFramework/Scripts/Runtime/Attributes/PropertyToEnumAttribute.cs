using System;
using UnityEngine;

namespace VBM {
    [AttributeUsage(AttributeTargets.Enum, AllowMultiple = true)]
    public class PropertyToEnumAttribute : PropertyAttribute {
        public Type classType { get; protected set; }
        public string propertyName { get; protected set; }
        public PropertyToEnumAttribute(Type classType, string propertyName = null) {
            this.classType = classType;
            this.propertyName = propertyName;
        }
    }
}
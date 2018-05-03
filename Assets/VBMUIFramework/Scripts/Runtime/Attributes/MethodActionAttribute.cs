using System;
using System.Reflection;
using UnityEngine;

namespace VBM {
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class MethodActionAttribute : PropertyAttribute {
        public BindingFlags bindingFlags { get; private set; }
        public Type attributeType { get; private set; }

        public MethodActionAttribute(BindingFlags bindingFlags, Type attributeType) {
            this.bindingFlags = bindingFlags;
            this.attributeType = attributeType;
        }
    }
}
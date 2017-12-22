using System;

namespace GeneralEditor {
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CustomDrawerAttribute : Attribute {
        public readonly Type type;
        public readonly bool includeChild;

        public CustomDrawerAttribute(Type type, bool includeChild = false) {
            this.type = type;
            this.includeChild = includeChild;
        }
    }
}

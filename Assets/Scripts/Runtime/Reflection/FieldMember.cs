using System;
using System.Reflection;

namespace VBM.Reflection {
    public sealed class FieldMember : MemberAccessor {
        protected FieldInfo fieldInfo;
        public override Type MemberType { get { return fieldInfo.FieldType; } }
        public override MemberInfo MemberInfo { get { return fieldInfo; } }

        public FieldMember(FieldInfo fieldInfo) {
            this.fieldInfo = fieldInfo;
            getter = (object container) => { return fieldInfo.GetValue(container); };
            setter = (object container, object value) => { fieldInfo.SetValue(container, value); };
        }
    }
}
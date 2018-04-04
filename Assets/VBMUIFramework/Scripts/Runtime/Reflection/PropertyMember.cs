using System.Reflection;

namespace VBM.Reflection {
    public class PropertyMember : MemberAccessor {
        protected PropertyInfo propertyInfo;
        public override System.Type MemberType { get { return propertyInfo.PropertyType; } }
        public override MemberInfo MemberInfo { get { return propertyInfo; } }

        public PropertyMember(PropertyInfo propertyInfo) {
            this.propertyInfo = propertyInfo;
            getter = (object container) => { return propertyInfo.GetValue(container, null); };
            setter = (object container, object value) => { propertyInfo.SetValue(container, value, null); };
        }
    }
}
using System;
using System.Collections.Generic;
using System.Reflection;
using VBM.Reflection;

namespace VBM {
    public class ReflectionModel : Model {
        protected static Dictionary<Type, Dictionary<string, MemberAccessor>> memberAccessorMap = new Dictionary<Type, Dictionary<string, MemberAccessor>>();

        public ReflectionModel() {
            Type type = GetType();
            if (!memberAccessorMap.ContainsKey(type))
                memberAccessorMap.Add(type, new Dictionary<string, MemberAccessor>());
        }

        public override object GetProperty(string propertyName) {
            MemberAccessor accessor = GetPropertyAccessor(GetType(), propertyName);
            return accessor.GetValue(this);
        }

        public override void SetProperty(string propertyName, object value) {
            MemberAccessor accessor = GetPropertyAccessor(GetType(), propertyName);
            accessor.SetValue(this, value);
        }

        public static MemberAccessor GetPropertyAccessor(Type type, string propertyName) {
            MemberAccessor accessor;
            Dictionary<string, MemberAccessor> accessorMap = memberAccessorMap[type];
            if (!accessorMap.TryGetValue(propertyName, out accessor)) {
                accessor = CreatePropertyAccessor(type, propertyName, BindingFlags.Instance | BindingFlags.Public);
                accessorMap.Add(propertyName, accessor);
            }
            return accessor;
        }

        public static MemberAccessor CreatePropertyAccessor(Type type, string propertyName, BindingFlags bindingAttr) {
            PropertyInfo propertyInfo = type.GetProperty(propertyName, bindingAttr);
            return MemberAccessor.Create(propertyInfo);
        }
    }
}
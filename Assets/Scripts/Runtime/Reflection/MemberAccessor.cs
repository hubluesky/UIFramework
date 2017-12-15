using System;
using System.Reflection;
using System.Reflection.Emit;

namespace VBM.Reflection {
    public abstract class MemberAccessor {
        protected static int accessorNumber;
        protected Func<object, object> getter;
        protected Action<object, object> setter;

        public static MemberAccessor Create(MemberInfo memberInfo) {
            if (memberInfo.MemberType == MemberTypes.Field) {
#if UNITY_IPHONE
                return new FieldMember((FieldInfo)memberInfo);
#else
                return new FieldMemberAccessor((FieldInfo)memberInfo);
#endif
            } else if (memberInfo.MemberType == MemberTypes.Property) {
#if UNITY_IPHONE
                return new PropertyMember((PropertyInfo)memberInfo);
#else
                return new PropertyMemberAccessor((PropertyInfo)memberInfo);
#endif
            } else {
                throw new NotSupportedException(memberInfo.MemberType.ToString());
            }
        }

        public object GetValue(object obj) {
            return getter(obj);
        }

        public void SetValue(object obj, object value) {
            setter(obj, value);
        }

        public abstract Type MemberType { get; }

        public abstract MemberInfo MemberInfo { get; }

        protected static void EmitTypeConversion(ILGenerator generator, Type declaringType) {
            if (declaringType.IsValueType) {
                generator.Emit(OpCodes.Unbox, declaringType);
            } else {
                generator.Emit(OpCodes.Castclass, declaringType);
            }
        }
    }
}
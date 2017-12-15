using System;
using System.Reflection;
using System.Reflection.Emit;

namespace VBM.Reflection {
    public sealed class FieldMemberAccessor : MemberAccessor {
        protected FieldInfo fieldInfo;
        public override Type MemberType { get { return fieldInfo.FieldType; } }
        public override MemberInfo MemberInfo { get { return fieldInfo; } }

        public FieldMemberAccessor(FieldInfo fieldInfo) {
            this.fieldInfo = fieldInfo;
            InitializeGetter(fieldInfo);
            InitializeSetter(fieldInfo);
        }

        void InitializeGetter(FieldInfo fieldInfo) {
            string methodName = fieldInfo.ReflectedType.FullName + ".get_" + fieldInfo.Name;
            DynamicMethod method = new DynamicMethod(methodName, typeof(object), new Type[] { typeof(object) }, fieldInfo.Module, true);
            ILGenerator generator = method.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            if (fieldInfo.DeclaringType.IsValueType)
                generator.Emit(OpCodes.Unbox, fieldInfo.DeclaringType);
            generator.Emit(OpCodes.Ldfld, fieldInfo);
            if (fieldInfo.FieldType.IsValueType) {
                generator.Emit(OpCodes.Box, fieldInfo.FieldType);
            }
            generator.Emit(OpCodes.Ret);
            getter = (Func<object, object>)method.CreateDelegate(typeof(Func<object, object>));
        }
        void InitializeSetter(FieldInfo fieldInfo) {
            string methodName = fieldInfo.ReflectedType.FullName + ".set_" + fieldInfo.Name;
            DynamicMethod method = new DynamicMethod(methodName, typeof(void), new Type[] { typeof(object), typeof(object) }, fieldInfo.Module, true);
            ILGenerator generator = method.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            EmitTypeConversion(generator, fieldInfo.DeclaringType);
            generator.Emit(OpCodes.Ldarg_1);
            if (fieldInfo.FieldType.IsValueType) {
                generator.Emit(OpCodes.Unbox_Any, fieldInfo.FieldType);
            } else {
                generator.Emit(OpCodes.Castclass, fieldInfo.FieldType);
            }
            generator.Emit(OpCodes.Stfld, fieldInfo);
            generator.Emit(OpCodes.Ret);
            setter = (Action<object, object>)method.CreateDelegate(typeof(Action<object, object>));
        }
    }
}
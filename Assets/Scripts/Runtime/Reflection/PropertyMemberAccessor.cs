using System;
using System.Reflection;
using System.Reflection.Emit;

namespace VBM.Reflection {

    public class PropertyMemberAccessor : MemberAccessor {
        protected PropertyInfo propertyInfo;
        public override Type MemberType { get { return propertyInfo.PropertyType; } }
        public override MemberInfo MemberInfo { get { return propertyInfo; } }

        public PropertyMemberAccessor(PropertyInfo propertyInfo) {
            this.propertyInfo = propertyInfo;
            InitializeGetter(propertyInfo);
            InitializeSetter(propertyInfo);
        }

        void InitializeGetter(PropertyInfo propertyInfo) {
            string methodName = propertyInfo.ReflectedType.FullName + ".get_property_" + propertyInfo.Name;
            DynamicMethod method = new DynamicMethod(methodName, typeof(object), new[] { typeof(object) }, propertyInfo.Module, true);
            ILGenerator generator = method.GetILGenerator();
            generator.DeclareLocal(typeof(object));
            generator.Emit(OpCodes.Ldarg_0);
            EmitTypeConversion(generator, propertyInfo.DeclaringType);
            generator.EmitCall(OpCodes.Callvirt, propertyInfo.GetGetMethod(), null);
            if (propertyInfo.PropertyType.IsValueType) {
                generator.Emit(OpCodes.Box, propertyInfo.PropertyType);
            }
            generator.Emit(OpCodes.Ret);
            getter = (Func<object, object>)method.CreateDelegate(typeof(Func<object, object>));
        }

        void InitializeSetter(PropertyInfo propertyInfo) {
            string methodName = propertyInfo.ReflectedType.FullName + ".set_property_" + propertyInfo.Name;
            DynamicMethod method = new DynamicMethod(methodName, typeof(void), new[] { typeof(object), typeof(object) }, propertyInfo.Module, true);
            ILGenerator generator = method.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            EmitTypeConversion(generator, propertyInfo.DeclaringType);
            generator.Emit(OpCodes.Ldarg_1);
            EmitTypeConversion(generator, propertyInfo.PropertyType);
            generator.EmitCall(OpCodes.Callvirt, propertyInfo.GetSetMethod(), null);
            generator.Emit(OpCodes.Ret);
            setter = (Action<object, object>)method.CreateDelegate(typeof(Action<object, object>));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Reflection;

namespace VBM.Reflection {
    public static class ReflectionUtility {
        public static List<Type> GetClassTypeFromAssembly(Type baseType) {
            List<Type> list = new List<Type>();
            Assembly[] assemblys = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblys) {
                foreach (Type type in assembly.GetExportedTypes()) {
                    if (type.IsClass && !type.IsAbstract && !type.IsGenericType && baseType.IsAssignableFrom(type))
                        list.Add(type);
                }
            }
            return list;
        }

        public static List<string> GetClassNameFromAssembly(Type baseType) {
            List<string> list = new List<string>();
            Assembly[] assemblys = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblys) {
                foreach (Type type in assembly.GetExportedTypes()) {
                    if (type.IsClass && !type.IsAbstract && !type.IsGenericType && baseType.IsAssignableFrom(type))
                        list.Add(type.FullName);
                }
            }
            return list;
        }

        public static List<FieldInfo> GetClassSerialzedFields(Type type) {
            List<FieldInfo> list = new List<FieldInfo>();
            foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy)) {
                if (fieldInfo.MemberType == MemberTypes.Field && fieldInfo.IsPublic || fieldInfo.IsDefined(typeof(UnityEngine.SerializeField), false))
                    list.Add(fieldInfo);
            }
            return list;
        }

        public static List<string> GetClassProperty(Type type) {
            List<string> list = new List<string>();
            foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public)) {
                if (propertyInfo.CanRead)
                    list.Add(propertyInfo.Name);
            }
            return list;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace VBM.Reflection {
    public static class ReflectionProperty {
        internal static void GetTypeReflectionMember(List<string> list, string prefix, Type type) {
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetField);
            foreach (FieldInfo field in fields) {
                list.Add(prefix + field.Name);
            }
            PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);
            foreach (PropertyInfo prop in props) {
                list.Add(prefix + prop.Name);
            }
        }

        public static void GetReflectionMember(List<string> list, GameObject gameObject) {
            Type type = gameObject.GetType();
            GetTypeReflectionMember(list, type.Name + "/", type);

            foreach (Component component in gameObject.GetComponents<Component>()) {
                GetTypeReflectionMember(list, component.GetType().Name + "/", component.GetType());
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace GeneralEditor {
    public static class PropertyDrawerMgr {
        private static Dictionary<Type, PropertyDrawer> drawersMap = new Dictionary<Type, PropertyDrawer>();
        private static ArrayPropertyDrawer arrayPropertyDrawer = new ArrayPropertyDrawer();

        static PropertyDrawerMgr() {
            Assembly[] assemblys = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblys) {
                foreach (Type type in assembly.GetExportedTypes()) {
                    InitPropertyDrawer(type);
                }
            }
        }

        private static void InitPropertyDrawer(Type type) {
            if (typeof(PropertyDrawer).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract) {
                CustomDrawerAttribute attribute = Attribute.GetCustomAttribute(type, typeof(CustomDrawerAttribute), true) as CustomDrawerAttribute;
                if (attribute == null)
                    return;

                PropertyDrawer drawer = Activator.CreateInstance(type, true) as PropertyDrawer;
                if (drawersMap.ContainsKey(attribute.type))
                    drawersMap[attribute.type] = drawer;
                else
                    drawersMap.Add(attribute.type, drawer);

                if (attribute.includeChild) {
                    IEnumerable<Type> subTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(t => t.IsSubclassOf(attribute.type));
                    foreach (Type subType in subTypes) {
                        if (!subType.IsAbstract && !drawersMap.ContainsKey(subType))
                            drawersMap.Add(subType, drawer);
                    }
                }
            }
        }

        public static PropertyDrawer GetPropertyDrawer(Type type, FieldInfo info) {
            PropertyDrawer drawer;
            if (info != null) {
                Attribute attribute = Attribute.GetCustomAttribute(info, typeof(PropertyAttribute), true);
                if (attribute != null && drawersMap.TryGetValue(attribute.GetType(), out drawer))
                    return drawer;
            }

            drawersMap.TryGetValue(type, out drawer);
            return drawer;
        }

        public static void PropertyField(SerializedProperty property, GUIContent label, params GUILayoutOption[] options) {
            if (property.PropertyValue == null && !property.PropertyType.IsAbstract)
                property.CreatePropertyValue();

            if (property.IsArray) {
                arrayPropertyDrawer.OnGUI(property, label, options);
            } else {
                PropertyDrawer drawer = GetPropertyDrawer(property.PropertyType, property.FieldInfo);
                if (drawer != null)
                    drawer.OnGUI(property, label, options);
                else
                    LayoutDrawer.PropertyField(property, label, options);
            }
        }
    }
}
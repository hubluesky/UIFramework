using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VBM;
using VBM.Reflection;

namespace VBMEditor {
    public class PropertyBindingArrayDrawer : GeneralEditor.ArrayPropertyDrawer {
        private static List<System.Type> typeList = new List<System.Type>();

        static PropertyBindingArrayDrawer() {
            ReflectionUtility.ForeachSubClassTypeFromAssembly(typeof(PropertyBinding), (type) => {
                typeList.Add(type);
                return true;
            });
        }

        protected override void OnAddButton(GeneralEditor.SerializedProperty property) {
            GenericMenu menu = new GenericMenu();
            foreach (System.Type type in typeList) {
                menu.AddItem(new GUIContent(type.FullName), false, () => {
                    property.CreateArrayElementAtIndex(property.ArraySize, type);
                });
            }
            menu.ShowAsContext();
        }
    }
}
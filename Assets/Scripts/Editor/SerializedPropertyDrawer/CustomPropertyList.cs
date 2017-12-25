using UnityEngine;

namespace GeneralEditor {
    public class CustomPropertyList {
        public System.Action<SerializedProperty> OnAddCallback;
        public System.Action<SerializedProperty> OnRemoveCallback;

        class ListPropertyDrawer : ArrayPropertyDrawer {
            CustomPropertyList propertyList;
            public ListPropertyDrawer(CustomPropertyList propertyList) {
                this.propertyList = propertyList;
            }

            protected override void OnAddButton(SerializedProperty property) {
                if (propertyList.OnAddCallback != null)
                    propertyList.OnAddCallback(property);
                else
                    property.CreateArrayElementAtIndex(property.ArraySize, null);
            }

            protected override void OnRemoveButton(SerializedProperty property) {
                if (propertyList.OnRemoveCallback != null)
                    propertyList.OnRemoveCallback(property);
                else
                    property.DeleteArrayElementAtIndex(property.ArraySize - 1);
            }
        }

        private SerializedProperty property;
        private ListPropertyDrawer drawer;
        private GUIContent label;
        public CustomPropertyList(SerializedProperty property, GUIContent content = null) {
            this.property = property;
            this.label = content != null ? content : new GUIContent(property.DisplayName);
            drawer = new ListPropertyDrawer(this);
        }

        public void DoLayout() {
            drawer.OnGUI(property, label);
        }
    }
}
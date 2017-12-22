using System.Reflection;
using UnityEngine;

namespace GeneralEditor {
    public abstract class PropertyDrawer {
        public abstract void OnGUI(SerializedProperty property, GUIContent label, params GUILayoutOption[] options);
    }
}

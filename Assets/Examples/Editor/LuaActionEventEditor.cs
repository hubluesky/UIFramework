using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VBM;
using VBMEditor;
using XLua;

[CustomEditor(typeof(ActionEvent), true)]
public class LuaActionEventEditor : ActionEventEditor {

    protected override void OnEnable() {
        base.OnEnable();
    }

    protected override void AddMembers(GenericMenu menu, string modelId, System.Type paramType) {
        List<string> list = LuaEnvManager.Instance.GetLuaModelFunctionList(modelId);
        if (list == null) {
            base.AddMembers(menu, modelId, paramType);
        } else {
            foreach (string funcName in list)
                menu.AddItem(new GUIContent(funcName), false, OnAddMemberBinding, funcName);
        }
    }

    private void OnAddMemberBinding(object value) {
        memberNameProperty.stringValue = value.ToString();
        memberNameProperty.serializedObject.ApplyModifiedProperties();
    }
}
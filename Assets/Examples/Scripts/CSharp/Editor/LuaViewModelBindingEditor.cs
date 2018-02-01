using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VBM;
using VBMEditor;
using XLua;

[CustomEditor(typeof(ViewModelBinding), true), CanEditMultipleObjects]
public class LuaViewModelBindingEditor : ViewModelBindingEditor {
    private List<string> luaModelNameList;

    protected new void OnEnable() {
        base.OnEnable();
        InitLua();
    }

    void InitLua() {
        luaModelNameList = LuaEnvManager.Instance.GetLuaModelNameList();
    }

    public override int IndexOfModelProperty(string name) {
        int index = luaModelNameList.IndexOf(name);
        if (index != -1)
            return index + modelTypeList.Count;
        return base.IndexOfModelProperty(name);
    }

    public override List<string> GetModelPropertyList(int index) {
        if (index < modelTypeList.Count)
            return base.GetModelPropertyList(index);

        List<string> propertiesList = new List<string>();
        index = index - modelTypeList.Count;
        LuaTable table = LuaEnvManager.Instance.luaenv.Global.Get<object, LuaTable>(luaModelNameList[index]);
        LuaFunction declarePropertiesFunc = table.Get<string, LuaFunction>("DeclareProperties");
        if (declarePropertiesFunc != null) {
            using(LuaTable propertiesTable = LuaEnvManager.Instance.luaenv.NewTable()) {
                declarePropertiesFunc.Call(propertiesTable);
                foreach (var memberName in propertiesTable.GetKeys<string>())
                    propertiesList.Add(memberName);
            }
        }

        return propertiesList;
    }

    private void AddLuaModelTypeMenus(SerializedProperty property, GenericMenu menu) {
        foreach (string name in luaModelNameList) {
            GUIContent content = new GUIContent(name);
            menu.AddItem(content, false, () => {
                property.stringValue = name;
                serializedObject.ApplyModifiedProperties();
            });
        }
    }

    protected override void ShowAddMemberMenu(SerializedProperty property) {
        GenericMenu menu = new GenericMenu();
        AddModelTypeMenus(property, menu);
        menu.AddSeparator(string.Empty);
        AddLuaModelTypeMenus(property, menu);
        menu.ShowAsContext();
    }
}
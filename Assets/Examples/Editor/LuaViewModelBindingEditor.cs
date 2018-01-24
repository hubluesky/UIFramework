using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VBM;
using VBMEditor;
using XLua;

[CustomEditor(typeof(ViewModelBinding), true), CanEditMultipleObjects]
public class LuaViewModelBindingEditor : ViewModelBindingEditor {
    private List<string> luaModelNameList = new List<string>();
    private LuaEnv luaenv;

    protected new void OnEnable() {
        base.OnEnable();
        InitLua();
    }

    void InitLua() {
        luaModelNameList.Clear();
        luaenv = new LuaEnv();
        string LuaScriptPath = System.IO.Path.Combine(Application.dataPath, "Examples/XLua");
        luaenv.AddLoader((ref string filename) => {
            string filepath = System.IO.Path.Combine(LuaScriptPath, filename + ".lua");
            if (System.IO.File.Exists(filepath)) {
                return System.IO.File.ReadAllBytes(filepath);
            } else {
                return null;
            }
        });
        luaenv.DoString("require 'Main'");

        foreach (var entryName in luaenv.Global.GetKeys<string>()) {
            LuaTable table = luaenv.Global.Get<object, LuaTable>(entryName);
            if (table == null)
                continue;
            LuaFunction getTypeFunc = table.Get<string, LuaFunction>("GetType");
            if (getTypeFunc == null)
                continue;

            LuaFunction instanceFunc = luaenv.Global.Get<LuaFunction>("InstanceOf");
            var result = instanceFunc.Call(table, "LuaModel");
            if (!(bool) result[0])
                continue;

            luaModelNameList.Add(entryName);
        }
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
        LuaTable table = luaenv.Global.Get<object, LuaTable>(luaModelNameList[index]);
        LuaFunction declarePropertiesFunc = table.Get<string, LuaFunction>("DeclareProperties");
        if (declarePropertiesFunc != null) {
            using(LuaTable propertiesTable = luaenv.NewTable()) {
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
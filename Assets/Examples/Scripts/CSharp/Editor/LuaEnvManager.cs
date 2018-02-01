using System.Collections.Generic;
using UnityEngine;
using XLua;

public class LuaEnvManager {
    private static LuaEnvManager instance = new LuaEnvManager();
    private bool initialized;
    public LuaEnv luaenv { get; private set; }

    public static LuaEnvManager Instance {
        get {
            if (!instance.initialized)
                instance.Initialized();
            return instance;
        }
    }

    private void Initialized() {
        initialized = true;
        luaenv = new LuaEnv();
        string LuaScriptPath = System.IO.Path.Combine(Application.dataPath, "Examples/Scripts/XLua");
        luaenv.AddLoader((ref string filename) => {
            string filepath = System.IO.Path.Combine(LuaScriptPath, filename + ".lua");
            if (System.IO.File.Exists(filepath)) {
                return System.IO.File.ReadAllBytes(filepath);
            } else {
                return null;
            }
        });
        luaenv.DoString("require 'Main'");
    }

    public List<string> GetLuaModelNameList() {
        List<string> list = new List<string>();
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

            list.Add(entryName);
        }
        return list;
    }

    public List<string> GetLuaModelFunctionList(string modelName) {
        LuaTable table = luaenv.Global.Get<object, LuaTable>(modelName);
        if (table == null) {
            return null;
        } else {
            List<string> list = new List<string>();
            foreach (string funcName in table.GetKeys<string>()) {
                LuaFunction function = table.Get<LuaFunction>(funcName);
                if (function != null)
                    list.Add(funcName);
            }
            return list;
        }
    }
}
using System;
using System.Collections.Generic;
using VBM;
using XLua;

public class ModelProxy : Model {
    public System.Func<string, object> GetPropertyFunc;
    public System.Func<string, LuaFunction> GetMethodFunc;
    public System.Func<string, LuaFunction> GetMethodParam1Func;
    protected Dictionary<string, Delegate> functionMap = new Dictionary<string, Delegate>();

    public override object GetProperty(string propertyName) {
        return GetPropertyFunc(propertyName);
    }

    public override Action GetFunction(string funcName) {
        Delegate function;
        if (!functionMap.TryGetValue(funcName, out function)) {
            LuaFunction luaFunction = GetMethodFunc(funcName);
            if (luaFunction != null) {
                function = luaFunction.Cast<Action>();
                functionMap.Add(funcName, function);
            }
        }

        return function as Action;
    }

    public override Action<T> GetFunctionParam1<T>(string funcName) {
        Delegate function;
        if (!functionMap.TryGetValue(funcName, out function)) {
            LuaFunction luaFunction = GetMethodParam1Func(funcName);
            if (luaFunction != null) {
                function = luaFunction.Cast<Action<T>>();
                functionMap.Add(funcName, function);
            }
        }

        return function as Action<T>;
    }
}
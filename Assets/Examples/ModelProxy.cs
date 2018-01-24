using System;
using VBM;
using XLua;

public class ModelProxy : Model {
    public System.Func<string, object> GetPropertyFunc;
    public System.Func<string, LuaFunction> GetMethodFunc;
    public System.Func<string, LuaFunction> GetMethodParam1Func;

    public override object GetProperty(string propertyName) {
        return GetPropertyFunc(propertyName);
    }

    public override Action GetFunction(string funcName) {
        LuaFunction luaFunction = GetMethodFunc(funcName);
        return luaFunction != null ? luaFunction.Cast<Action>() : null;
    }

    public override Action<T> GetFunctionParam1<T>(string funcName) {
        LuaFunction luaFunction = GetMethodParam1Func(funcName);
        return luaFunction != null ? luaFunction.Cast<Action<T>>() : null;
    }
}
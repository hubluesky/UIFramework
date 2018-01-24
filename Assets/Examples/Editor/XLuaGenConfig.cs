using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VBM;
using XLua;

public static class XLuaGenConfig {
    //lua中要使用到C#库的配置，比如C#标准库，或者Unity API，第三方库等。
    [LuaCallCSharp]
    public static List<System.Type> LuaCallCSharp = new List<System.Type>() {
        typeof(ModelManager),
        typeof(ViewManager),
        typeof(Model),
        typeof(ListModel),
        typeof(View),
    };

    //C#静态调用Lua的配置（包括事件的原型），仅可以配delegate，interface
    [CSharpCallLua]
    public static List<System.Type> CSharpCallLua = new List<System.Type>() {
        typeof(System.Action<string, object>),
        typeof(System.Func<string, object>),
        typeof(System.Func<string, Action>),
        typeof(System.Func<string, LuaFunction>),

        typeof(System.Action),
        typeof(System.Func<double, double, double>),
        typeof(System.Action<string>),
        typeof(System.Action<double>),
        typeof(UnityEngine.Events.UnityAction),
        typeof(System.Collections.IEnumerator)
    };
}
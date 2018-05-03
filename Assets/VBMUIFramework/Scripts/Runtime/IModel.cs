using System.Collections.Generic;

namespace VBM {
    public interface IModel {
        event System.Action<string, object> propertyChanged;
        T GetProperty<T>(string propertyName);
        object GetProperty(string propertyName);
        System.Action GetFunction(string funcName);
        System.Delegate GetFunctionParam1(string funcName, System.Type paramType);
        System.Action<T> GetFunctionParam1<T>(string funcName);
        bool CheckElementModel(ViewModelBinding binding);
    }
}
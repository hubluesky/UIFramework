using System.Collections.Generic;

namespace VBM {
    public abstract class Model : IModel{
        public event System.Action<string, object> propertyChanged;

        public virtual T GetProperty<T>(string propertyName) {
            return (T)GetProperty(propertyName);
        }

        public abstract object GetProperty(string propertyName);

        public abstract System.Action GetFunction(string funcName);

        public abstract System.Action<T> GetFunctionParam1<T>(string funcName);

        public void NotifyPropertyChanged(string propertyName, object value) {
            if (propertyChanged != null)
                propertyChanged(propertyName, value);
        }
    }
}
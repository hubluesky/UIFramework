using System.Collections.Generic;

namespace VBM {
    public abstract class Model {
        public event System.Action<string, object> propertyChanged;

        public virtual T GetProperty<T>(string propertyName) {
            return (T)GetProperty(propertyName);
        }

        public abstract object GetProperty(string propertyName);

        public void NotifyPropertyChanged(string propertyName, object value) {
            if (propertyChanged != null)
                propertyChanged(propertyName, value);
        }
    }
}
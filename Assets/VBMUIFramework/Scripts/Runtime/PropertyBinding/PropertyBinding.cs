using System.Reflection;
using UnityEngine;

namespace VBM {
    public abstract class PropertyBinding {
        [PropertyBindingName]
        public string propertyName;
        [MethodActionAttribute(BindingFlags.Static | BindingFlags.Public, typeof(PropertyConverterAttribute))]
        public MethodAction converter;
        public bool refresh { get; internal set; }
        public IModel model { get; private set; }

        public void SetProperty(object value) {
            OnPropertyChange(Converter(model, value));
        }

        public object Converter(IModel model, object bindingValue) {
            if (converter == null || string.IsNullOrEmpty(converter.methodName)) return bindingValue;
            return converter.Invoke(model);
        }

        public void SetModel(IModel model) {
            this.model = model;
        }

        public abstract void OnPropertyChange(object value);

        public virtual void Initialized() { }
        public virtual void Finalized() { }
    }
}
// using System.Dynamic;
using UnityEngine;

public abstract class Model {
    public event System.Action<string> propertyChanged;

    public abstract string GetStringProperty(string propertyName);
    public abstract Sprite GetSpriteProperty(string propertyName);

    public virtual void Initialized() {
        // ExpandoObject expando;
    }

    public virtual void Finalized() {
    }

    protected void NotifyPropertyChanged(string propertyName) {
        if (propertyChanged != null)
            propertyChanged(propertyName);
    }
}
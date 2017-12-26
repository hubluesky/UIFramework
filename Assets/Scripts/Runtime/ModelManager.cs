using System.Collections.Generic;
using UnityEngine;

namespace VBM {
    public sealed class ModelManager : Singleton<ModelManager> {
        private Dictionary<string, Model> modelMap = new Dictionary<string, Model>();

        public T CreateModel<T>() where T : Model, new() {
            return CreateModel<T>(typeof(T).Name);
        }

        public T CreateModel<T>(string uniqueId) where T : Model, new() {
            T model = new T();
            RegisterModel(uniqueId, model);
            return model;
        }

        public void RegisterModel<T>(T model) where T : Model {
            RegisterModel(typeof(T).Name, model);
        }

        public void RegisterModel(string uniqueId, Model model) {
            if (modelMap.ContainsKey(uniqueId)) {
                Debug.LogWarningFormat("Register model failed! the {0} model had ben register.", uniqueId);
                return;
            }
            modelMap.Add(uniqueId, model);
        }

        public bool UnregisterModel(Model model) {
            return UnregisterModel(model.GetType().Name);
        }

        public bool UnregisterModel(string uniqueId) {
            return modelMap.Remove(uniqueId);
        }

        public T GetModel<T>() where T : Model {
            return GetModel(typeof(T).Name) as T;
        }

        public T GetModel<T>(string uniqueId) where T : Model {
            return GetModel(uniqueId) as T;
        }

        public Model GetModel(string uniqueId) {
            Model model;
            modelMap.TryGetValue(uniqueId, out model);
            return model;
        }
    }
}
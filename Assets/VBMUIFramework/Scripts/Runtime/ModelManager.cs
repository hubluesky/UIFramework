using System.Collections.Generic;
using UnityEngine;

namespace VBM {
    public sealed class ModelManager : Singleton<ModelManager> {
        private Dictionary<string, IModel> modelMap = new Dictionary<string, IModel>();

        public T CreateModel<T>() where T : IModel, new() {
            return CreateModel<T>(typeof(T).Name);
        }

        public T CreateModel<T>(string uniqueId) where T : IModel, new() {
            T model = new T();
            RegisterModel(uniqueId, model);
            return model;
        }

        public void RegisterModel<T>(T model) where T : IModel {
            RegisterModel(typeof(T).Name, model);
        }

        public void RegisterModel(string uniqueId, IModel model) {
            if (modelMap.ContainsKey(uniqueId)) {
                Debug.LogWarningFormat("Register model failed! the {0} model had ben register.", uniqueId);
                return;
            }
            modelMap.Add(uniqueId, model);
        }

        public bool UnregisterModel(IModel model) {
            return UnregisterModel(model.GetType().Name);
        }

        public bool UnregisterModel(string uniqueId) {
            return modelMap.Remove(uniqueId);
        }

        public T GetModel<T>() where T : class, IModel {
            return GetModel(typeof(T).Name) as T;
        }

        public T GetModel<T>(string uniqueId) where T : class, IModel {
            return GetModel(uniqueId) as T;
        }

        public IModel GetModel(string uniqueId) {
            IModel model;
            modelMap.TryGetValue(uniqueId, out model);
            return model;
        }
    }
}
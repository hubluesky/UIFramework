using System;
using UnityEngine;

namespace VBM {
    [AddComponentMenu("VBMUIFramework/ActionEvents/BindedModelEvent")]
    public class BindedModelEvent : ActionEvent {
        public override Type ParameterType { get { return typeof(ViewModelBinding); } }

        void Awake() {
            viewModelBinding.BindedModelEvent += OnBindedModel;
            if (viewModelBinding.model != null)
                OnBindedModel(viewModelBinding.model);
        }

        void OnBindedModel(IModel model) {
            CallMemberFunctions<ViewModelBinding>(viewModelBinding);
        }
    }
}
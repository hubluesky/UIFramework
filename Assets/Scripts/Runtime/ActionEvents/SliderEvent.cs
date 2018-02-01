using System;
using UnityEngine;
using UnityEngine.UI;

namespace VBM {
    [RequireComponent(typeof(Slider))]
    public class SliderEvent : ActionEvent {
        private Slider slider;
        public override Type ParameterType { get { return typeof(float); } }

        void Start() {
            if (CheckViewModelBinding()) {
                slider = GetComponent<Slider>();
                slider.onValueChanged.AddListener(OnEventChanged);
            }
        }

        void OnDestroy() {
            if (slider != null)
                slider.onValueChanged.RemoveListener(OnEventChanged);
        }

        void OnEventChanged(float value) {
            CallMemberFunctions(value);
        }
    }
}
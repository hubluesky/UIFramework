using UnityEngine;

namespace VBM {

    [System.Serializable]
    public enum AnimatorPropertyType {
        Trigger,
        Bool,
        Int,
        Float,
    }

    [System.Serializable]
    public class AnimatorPropertyBinding : PropertyBinding {
        public Animator component;
        public string parameterName;
        public AnimatorPropertyType type;

        private int parameterId;

        public override void OnAfterDeserialize() {
            base.OnAfterDeserialize();
            parameterId = Animator.StringToHash(parameterName);
        }

        public override void OnPropertyChange(object value) {
            switch (type) {
                case AnimatorPropertyType.Trigger:
                    component.SetTrigger(parameterId);
                    break;
                case AnimatorPropertyType.Bool:
                    component.SetBool(parameterId, (bool) value);
                    break;
                case AnimatorPropertyType.Int:
                    component.SetInteger(parameterId, (int) value);
                    break;
                case AnimatorPropertyType.Float:
                    component.SetFloat(parameterId, (float) value);
                    break;
            }
        }
    }
}
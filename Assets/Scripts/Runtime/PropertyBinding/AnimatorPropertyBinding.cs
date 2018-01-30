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
        public Animator animator;
        public string parameterName;
        public AnimatorPropertyType type;

        private int parameterId;

        public override void OnAfterDeserialize() {
            base.OnAfterDeserialize();
            parameterId = Animator.StringToHash(parameterName);
        }

        public override void OnPropertyChange(object value) {
            Debug.Log("parametername " + value);
            switch (type) {
                case AnimatorPropertyType.Trigger:
                    animator.SetTrigger(parameterId);
                    break;
                case AnimatorPropertyType.Bool:
                    animator.SetBool(parameterId, (bool) value);
                    break;
                case AnimatorPropertyType.Int:
                    animator.SetInteger(parameterId, (int) value);
                    break;
                case AnimatorPropertyType.Float:
                    animator.SetFloat(parameterId, (float) value);
                    break;
            }
        }
    }
}
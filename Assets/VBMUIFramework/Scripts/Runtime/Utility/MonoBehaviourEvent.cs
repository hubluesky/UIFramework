using UnityEngine;

public class MonoBehaviourEvent : MonoBehaviour {
    public event System.Action onAwakeEvent;
    public event System.Action onStartEvent;
    public event System.Action onEnableEvent;
    public event System.Action onDisableEvent;
    public event System.Action onDestroyEvent;

    public void AwakeAndEnabled() {
        Awake();
        OnEnable();
    }

    void Awake() {
        if (onAwakeEvent != null)
            onAwakeEvent();
    }

    void OnEnable() {
        if (onEnableEvent != null)
            onEnableEvent();
    }

    void Start() {
        if (onStartEvent != null)
            onStartEvent();
    }

    void OnDisable() {
        if (onDisableEvent != null)
            onDisableEvent();
    }

    void OnDestroy() {
        if (onDestroyEvent != null)
            onDestroyEvent();
    }
}
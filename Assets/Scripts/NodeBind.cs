using System.Collections.Generic;
using UnityEngine;

public class NodeBind {
    protected List<NodeBind> childList = new List<NodeBind>();
    protected List<BindWidget> bindList = new List<BindWidget>();
    protected RectTransform transform;
    protected GameObjectEvent gameObjectEvent;
    public string transformName { get; protected set; }
    public int childCount { get { return childList.Count; } }

    public NodeBind(string transformName) {
        this.transformName = transformName;
    }

    public NodeBind this[int index] { 
        get { return childList[index]; }
        set { childList[index] = value; }
    }

    public NodeBind CreateChild(string transformName) {
        NodeBind child = new NodeBind(transformName);
        AddChild(child);
        return child;
    }

    public void AddChild(NodeBind node) {
        childList.Add(node);
    }

    public void InsertChild(int index, NodeBind node) {
        childList.Insert(index, node);
    }

    public void RemoveChild(NodeBind node) {
        childList.Remove(node);
    }

    public void RemoveChildAt(int index) {
        childList.RemoveAt(index);
    }

    public void RemoveChildRange(int index, int count) {
        childList.RemoveRange(index, count);
    }

    public void Clear() {
        childList.Clear();
    }

    public void SetTransform(RectTransform transform) {
        this.transform = transform;
        gameObjectEvent = transform.gameObject.AddComponent<GameObjectEvent>();
        gameObjectEvent.onAwakeEvent += InitChildren;
        gameObjectEvent.onEnableEvent += RefreshChildren;
        if (gameObjectEvent.isActiveAndEnabled)
            gameObjectEvent.AwakeAndEnabled(); // 由于Unity的Awake和Enabled在AddComponent的时候就已经回调过了，所以添加事件后再调一次。
        Debug.Log("SetTransform: " + transform.name + " " + transform.gameObject.activeInHierarchy);
    }

    protected virtual void InitChildren() {
        Debug.Log("InitChildren: " + transform.name);
        foreach (NodeBind nodeBind in childList) {
            Transform child = transform.SearchChild(nodeBind.transformName);
            if (child == null)
                Debug.LogWarningFormat("Bind {0} transform {1} child failed!", transform.name, nodeBind.transformName);
            else
                nodeBind.SetTransform(child as RectTransform);
        }

        foreach (BindWidget bindWidget in bindList) {
            Transform child = transform.SearchChild(bindWidget.widgetName);
            if (child == null)
                Debug.LogWarningFormat("Bind {0} widget {1} child failed!", transform.name, bindWidget.widgetName);
            else
                bindWidget.SetWidgetTransform(child);
        }
    }

    protected virtual void RefreshChildren() {
        Debug.Log("RefreshChildren: " + transform.name);
        foreach (BindWidget bindWidget in bindList) {
            if (bindWidget.refresh && bindWidget.CheckUpdateWidget())
                bindWidget.RefreshWidget();
        }
    }

    public void BindText(Model model, string propertyName, string textName, System.Action<Model, string, BindWidget> action = null) {
        bindList.Add(new BindText(model, propertyName, textName, action));
    }

    public void BindImage(Model model, string propertyName, string imageName, System.Action<Model, string, BindWidget> action = null) {
        bindList.Add(new BindImage(model, propertyName, imageName, action));
    }
}
using UnityEngine;

public class NodeListBind : NodeBind {
    public event System.Action<ElementData, NodeBind> AddElementEvents;
    public System.Func<Transform, int, Transform> GetChild = GetOrCreateChild;

    public NodeListBind(string transformName) : base(transformName) {
    }

    protected override void InitChildren() {
        Debug.Log("InitChildren: " + transform.name);
        for (int i = 0; i < childList.Count; i++) {
            Transform child = GetChild(transform, i);
            childList[i].SetTransform(child as RectTransform);
        }
    }

    protected override void RefreshChildren() {
    }

    public static Transform GetOrCreateChild(Transform parent, int index) {
        if (index < parent.childCount)
            return parent.GetChild(index);
        return Object.Instantiate(parent.GetChild(index), parent, false);
    }

    public void BindList(ListData list) {
        list.elementAdded += ElementAdd;
        list.elementInserted += ElementInsert;
        list.elementRemoved += ElementRemoved;
        list.elementRemovRanged += ElementRemovRanged;
        list.elementCleared += ElementCleared;
    }

    public void UnbindList(ListData list) {
        list.elementAdded -= ElementAdd;
        list.elementInserted -= ElementInsert;
        list.elementRemoved -= ElementRemoved;
        list.elementRemovRanged -= ElementRemovRanged;
        list.elementCleared -= ElementCleared;
    }

    protected void ElementAdd(ElementData data) {
        NodeBind node = CreateChild(data.name);
        if (AddElementEvents != null)
            AddElementEvents(data, node);
    }

    protected void ElementInsert(int index, ElementData data) {
        NodeBind node = new NodeBind(data.name);
        InsertChild(index, node);
        if (AddElementEvents != null)
            AddElementEvents(data, node);
    }

    protected void ElementRemoved(int index) {
        RemoveChildAt(index);
    }

    protected void ElementRemovRanged(int index, int count) {
        RemoveChildRange(index, count);
    }

    protected void ElementCleared() {
        Clear();
    }
}
using System.Collections.Generic;
using UnityEngine;

public class ListData {
    public event System.Action<ElementData, string> elementPropertyChanged;
    public event System.Action<ElementData> elementAdded;
    public event System.Action<int, ElementData> elementInserted;
    public event System.Action<int> elementRemoved;
    public event System.Action<int, int> elementRemovRanged;
    public event System.Action elementCleared;
    protected List<ElementData> list = new List<ElementData>();

    public ElementData this[int index] {
        get { return list[index]; }
        set { list[index] = value; }
    }

    public void Add(ElementData item) {
        list.Add(item);
        elementAdded(item);
    }

    public void AddRange(IEnumerable<ElementData> collection) {
        list.AddRange(collection);
        foreach (ElementData item in collection) 
            elementAdded(item);
    }

    public void Insert(int index, ElementData item) {
        list.Insert(index, item);
        elementInserted(index, item);
    }

    public bool Remove(ElementData item) {
        int index = list.IndexOf(item);
        if(index != -1) {
            RemoveAt(index);
            return true;
        }
        return false;
    }

    public void RemoveAt(int index) {
        list.RemoveAt(index);
        elementRemoved(index);
    }

    public void RemoveRange(int index, int count) {
        list.RemoveRange(index, count);
        elementRemovRanged(index, count);
    }

    public void Sort(int index, int count, IComparer<ElementData> comparer) {
        list.Sort(index, count, comparer);
    }

    public void Sort(System.Comparison<ElementData> comparison) {
        list.Sort(comparison);
    }

    public void Sort(IComparer<ElementData> comparer) {
        list.Sort(comparer);
    }

    public void Sort() {
        list.Sort();
    }

    public bool Contains(ElementData item) {
        return list.Contains(item);
    }

    public ElementData Find(System.Predicate<ElementData> match) {
        return list.Find(match);
    }

    public void Clear() {
        list.Clear();
    }

    public int BinarySearch(int index, int count, ElementData item, IComparer<ElementData> comparer) {
        return list.BinarySearch(index, count, item, comparer);
    }

    public int BinarySearch(ElementData item) {
        return list.BinarySearch(item);
    }

    public int BinarySearch(ElementData item, IComparer<ElementData> comparer) {
        return list.BinarySearch(item, comparer);
    }

    internal void NotifyPropertyChanged(ElementData element, string propertyName) {
        if (elementPropertyChanged != null)
            elementPropertyChanged(element, propertyName);
    }
}
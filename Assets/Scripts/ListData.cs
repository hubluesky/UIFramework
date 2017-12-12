using System.Collections.Generic;
using UnityEngine;

public class ListData {
    public event System.Action<ElementData, string> elementPropertyChanged;
    protected List<ElementData> list = new List<ElementData>();

    public ElementData this[int index] {
        get { return list[index]; }
        set { list[index] = value; }
    }

    public void Add(ElementData item) {
        list.Add(item);
    }

    public void AddRange(IEnumerable<ElementData> collection) {
        list.AddRange(collection);
    }

    public void Insert(int index, ElementData item) {
        list.Insert(index, item);
    }

    public bool Remove(ElementData item) {
        return list.Remove(item);
    }

    public void RemoveAt(int index) {
        list.RemoveAt(index);
    }

    public void RemoveRange(int index, int count) {
        list.RemoveRange(index, count);
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
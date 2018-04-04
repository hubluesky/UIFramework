using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VBM {
    public class ListModel : IEnumerable<IModel> {
        public event System.Action<IModel> elementAdded;
        public event System.Action<int, IModel> elementInserted;
        public event System.Action<int> elementRemoved;
        public event System.Action<int, int> elementRemovRanged;
        public event System.Action elementCleared;
        public event System.Action<int, int> elementSwaped;
        protected List<IModel> list = new List<IModel>();

        public int Count { get { return list.Count; } }

        public IModel this [int index] {
            get { return list[index]; }
            set { list[index] = value; }
        }

        public void Add(IModel item) {
            list.Add(item);
            if (elementAdded != null)
                elementAdded(item);
        }

        public void AddRange(IEnumerable<IModel> collection) {
            list.AddRange(collection);
            if (elementAdded != null) {
                foreach (IModel item in collection)
                    elementAdded(item);
            }
        }

        public void Insert(int index, IModel item) {
            list.Insert(index, item);
            if (elementInserted != null)
                elementInserted(index, item);
        }

        public bool Remove(IModel item) {
            int index = list.IndexOf(item);
            if (index != -1) {
                RemoveAt(index);
                return true;
            }
            return false;
        }

        public void RemoveAt(int index) {
            list.RemoveAt(index);
            if (elementRemoved != null)
                elementRemoved(index);
        }

        public void RemoveRange(int index, int count) {
            list.RemoveRange(index, count);
            if (elementRemovRanged != null)
                elementRemovRanged(index, count);
        }

        public void Swap(int index1, int index2) {
            if (index1 == index2) return;
            IModel temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
            if (elementSwaped != null)
                elementSwaped(index1, index2);
        }

        private int Partition(int low, int high, System.Comparison<IModel> comparer) {
            IModel privotKey = list[low];
            while (low < high) {
                while (low < high && comparer(list[high], privotKey) >= 0) --high;
                Swap(low, high);
                while (low < high && comparer(list[low], privotKey) <= 0) ++low;
                Swap(low, high);
            }
            return low;
        }

        private void QuickSort(int low, int high, System.Comparison<IModel> comparer) {
            if (low >= high) return;
            int privotLoc = Partition(low, high, comparer);
            QuickSort(low, privotLoc - 1, comparer);
            QuickSort(privotLoc + 1, high, comparer);
        }

        public void Sort(int index, int count, System.Comparison<IModel> comparer) {
            QuickSort(index, index + Count - 1, comparer);
        }

        public void Sort(System.Comparison<IModel> comparer) {
            Sort(0, list.Count, comparer);
        }

        public bool Contains(IModel item) {
            return list.Contains(item);
        }

        public IModel Find(System.Predicate<IModel> match) {
            return list.Find(match);
        }

        public void Clear() {
            list.Clear();
            if (elementCleared != null)
                elementCleared();
        }

        public int BinarySearch(int index, int count, IModel item, IComparer<IModel> comparer) {
            return list.BinarySearch(index, count, item, comparer);
        }

        public int BinarySearch(IModel item) {
            return list.BinarySearch(item);
        }

        public int BinarySearch(IModel item, IComparer<IModel> comparer) {
            return list.BinarySearch(item, comparer);
        }

        public IEnumerator<IModel> GetEnumerator() {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return list.GetEnumerator();
        }
    }
}
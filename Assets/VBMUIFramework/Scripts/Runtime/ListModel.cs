using System.Collections;
using System.Collections.Generic;

namespace VBM {
    public class ListModel : IEnumerable<IModel> {
        public event System.Action<IModel> elementAdded;
        public event System.Action<IModel> elementRemoved;
        public event System.Action elementCleared;


        protected List<IModel> list = new List<IModel>();

        public int Count { get { return list.Count; } }

        public IModel this[int index] {
            get { return list[index]; }
            set { list[index] = value; }
        }

        public IModel Get(int index) {
            return list[index];
        }

        public T Get<T>(int index) where T : class, IModel {
            return list[index] as T;
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
            if (elementAdded != null)
                elementAdded(item);
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
            if (index < 0 || index >= list.Count) return;
            if (elementRemoved != null)
                elementRemoved(list[index]);
            list.RemoveAt(index);
        }

        public void RemoveRange(int index, int count) {
            if (elementRemoved != null) {
                for (int i = index; i < count; i++)
                    elementRemoved(list[index]);
            }
            list.RemoveRange(index, count);
        }

        public bool Contains(IModel item) {
            return list.Contains(item);
        }

        public IModel Find(System.Predicate<IModel> match) {
            return list.Find(match);
        }

        public T Find<T>(System.Predicate<T> match) where T : class, IModel {
            foreach (IModel model in list) {
                T t = model as T;
                if (match(t)) return t;
            }
            return null;
        }

        public int FindIndex<T>(System.Predicate<T> match) where T : class, IModel {
            for (int i = 0; i < list.Count; i++) {
                if (match(list[i] as T)) return i;
            }
            return -1;
        }

        public void Clear() {
            list.Clear();
            if (elementCleared != null)
                elementCleared();
        }

        public IEnumerator<IModel> GetEnumerator() {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return list.GetEnumerator();
        }
    }
}
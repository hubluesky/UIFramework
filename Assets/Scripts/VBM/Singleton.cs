namespace VBM {
    public class Singleton<T> where T : new () {
        private static T instance = new T ();
        public static T Instance { get { return instance; } }
    }
}
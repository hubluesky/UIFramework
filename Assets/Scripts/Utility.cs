using UnityEngine;

public static class Utility {
    public static Transform SearchChild(this Transform transform, string childname) {
        for (int i = 0; i < transform.childCount; i++) {
            Transform child = transform.GetChild(i);
            if (child.name == childname)
                return child;
            child = SearchChild(child, childname);
            if (child != null)
                return child;
        }
        return null;
    }
}
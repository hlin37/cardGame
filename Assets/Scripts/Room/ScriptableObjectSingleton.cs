using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObject
{
    private static T _instance = null;

    public static T instance {
        get {
            if (_instance == null) {
                T[] results = Resources.FindObjectsOfTypeAll<T>();
                if (results.Length == 0) {
                    Debug.Log("Results length is 0");
                    return null;
                }
                if (results.Length > 1) {
                    Debug.Log("Results length is greater than 1");
                    return null;
                }
                _instance = results[0];
            }
            return _instance;
        }
    }
}

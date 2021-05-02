using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager
{
    string pathWherePrefabs = "Prefabs/";

    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        if(path.Contains("Prefabs/") == false)
        {
            path = $"{pathWherePrefabs}{path}";
        }
        GameObject go = Load<GameObject>(path);

        if (go == null)
        {
            Debug.Log("Load Error");
            return null;
        }

        return GameObject.Instantiate(go, parent);
    }
}

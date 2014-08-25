using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ObjectPool : System.IDisposable
{
    GameObject original;
    Stack<GameObject> availableGameObjects;
    List<GameObject> allGameObjects;

    public ObjectPool(GameObject gameObject, int initialCapacity)
    {
        GameObject localGameObject = null;

        original = gameObject;
        availableGameObjects = new Stack<GameObject>();
        allGameObjects = new List<GameObject>();

        for (int index = 0; index < initialCapacity; index++)
        {
            localGameObject = MakeGameObject();
            allGameObjects.Add(localGameObject);
            availableGameObjects.Push(localGameObject);
        }
    }

    private GameObject MakeGameObject()
    {
        GameObject result = GameObject.Instantiate(original, Vector3.zero, Quaternion.identity) as GameObject;

        result.name = original.name + "_" + (allGameObjects.Count + 1).ToString();
        result.SetActive(false);

        return result;
    }

    #region Methods
    public GameObject GetGameObject()
    {
        return GetGameObject(Vector3.zero);
    }

    public GameObject GetGameObject(Vector3 position)
    {
        return GetGameObject(null, position);
    }

    public GameObject GetGameObject(Transform parent)
    {
        return GetGameObject(parent, Vector3.zero);
    }

    public GameObject GetGameObject(Transform parent, Vector3 position)
    {
        GameObject result = null;

        if (!availableGameObjects.Any())
        {
            result = MakeGameObject();
            allGameObjects.Add(result);
            availableGameObjects.Push(result);
        }
        result = availableGameObjects.Pop();
        result.transform.parent = parent;
        result.transform.localPosition = position;
        result.SetActive(true);

        return result;
    }

    public bool Destroy(GameObject target)
    {
        availableGameObjects.Push(target);
        target.SetActive(false);

        return true;
    }

    private void Clear()
    {
        foreach (var item in availableGameObjects)
        {
            GameObject.Destroy(item);
        }

        foreach (var item in allGameObjects)
        {
            GameObject.Destroy(item);
        }
        availableGameObjects.Clear();
        allGameObjects.Clear();
    }

    public void Reset()
    {
        availableGameObjects.Clear();

        foreach (var item in allGameObjects)
        {
            item.SetActive(false);
            availableGameObjects.Push(item);
        }
    }

    public void Dispose()
    {
        Clear();
    }

    #endregion
}
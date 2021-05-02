using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UI_Base : MonoBehaviour
{
    Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] typeNames = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[typeNames.Length];
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < objects.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Utils.FindChild(gameObject, typeNames[i], true);
            else
                objects[i] = Utils.FindChild<T>(gameObject, typeNames[i], true);
        }
    }

    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;

        _objects.TryGetValue(typeof(T), out objects);
        if (objects == null)
            return null;
        return objects[idx] as T;
    }

    public static void AddUIHandler(GameObject go, Action<PointerEventData> action, Defines.UIEvent type = Defines.UIEvent.Click)
    {
        UI_EventHandler evt = Utils.GetOrAddComponete<UI_EventHandler>(go);

        switch (type)
        {
            case Defines.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
        }
    }

    protected Button GetButton(int idx)
    {
        return Get<Button>(idx);
    }

    protected Text GetText(int idx)
    {
        return Get<Text>(idx);
    }
}

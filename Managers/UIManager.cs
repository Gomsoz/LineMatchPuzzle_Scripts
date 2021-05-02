using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    int m_order = 10;

    Stack<UI_PopUp> m_popupStack = new Stack<UI_PopUp>();
    Dictionary<string, UI_Scene> m_sceneUI = new Dictionary<string, UI_Scene>();

    Transform m_uiHolder;

    public void UIMnangerInit()
    {
        m_uiHolder = new GameObject { name = "UI Holder" }.transform;
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = go.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = m_order;
            m_order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    public T ShowSceneUI<T>(Defines.SceneType sceneType) where T : UI_Scene
    {
        if(m_sceneUI == null)
        {
            m_sceneUI = new Dictionary<string, UI_Scene>();
        }
        string sceneName = System.Enum.GetName(typeof(Defines.SceneType), sceneType);
        string name = typeof(T).Name;

        GameObject go = Managers.Resources.Instantiate($"UI/{sceneName}/Scene/{name}");
        T scene = go.GetComponent<T>();
        m_sceneUI.Add(name, scene);

        go.transform.SetParent(m_uiHolder);
        return scene;
    }

    public void ClearSceneUI()
    {
        m_sceneUI = null;
    }

    public T GetSceneUI<T>() where T :UI_Scene
    {
        string name = typeof(T).Name;

        if (m_sceneUI.ContainsKey(name))
        {
            return (T)m_sceneUI[name];
        }

        return null;
    }

    public T GetLastPopupUI<T>() where T : UI_PopUp
    {
        string name = typeof(T).Name;
        if (m_popupStack.Peek().name.Contains("Clone"))
            name += $"(Clone)";

        if (m_popupStack.Peek().name == name)
        {
            return (T)m_popupStack.Peek();
        }

        return null;
    }

    public T ShowPopupUI<T>(Defines.SceneType sceneType) where T: UI_PopUp
    {
        string sceneName = System.Enum.GetName(typeof(Defines.SceneType), sceneType);
        string name = typeof(T).Name;

        GameObject go = Managers.Resources.Instantiate($"UI/{sceneName}/Popup/{name}");
        T popup = go.GetComponent<T>();
        m_popupStack.Push(popup);

        go.transform.SetParent(m_uiHolder);
        return popup;
    }

    public void ClosePopupAll()
    {
        while (m_popupStack.Count > 0)
            ClosePopupUI();
    }

    public void ClosePopupUI(UI_PopUp popup)
    {
        if (m_popupStack.Count == 0)
            return;

        if(m_popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed");
            return;
        }

        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        if (m_popupStack.Count == 0)
            return;

        UI_PopUp popup = m_popupStack.Pop();
        Debug.Log(popup.name);
        GameObject.Destroy(popup.gameObject);
        popup = null;
        m_order--;
    }
}

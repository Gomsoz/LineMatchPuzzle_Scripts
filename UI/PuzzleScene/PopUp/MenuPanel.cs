using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuPanel : UI_PopUp
{
    enum MenuButtons
    {
        ResumeButton,
        ExitButton,
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(MenuButtons));

        AddUIHandler(GetButton((int)MenuButtons.ResumeButton).gameObject, ClickResumeButton);
        AddUIHandler(GetButton((int)MenuButtons.ExitButton).gameObject, ClickExitButton);
    }

    void ClickResumeButton(PointerEventData evt)
    {
        GameManager.GameMgr.GamePause();
        Managers.UI.ClosePopupUI();
    }

    void ClickExitButton(PointerEventData evt)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

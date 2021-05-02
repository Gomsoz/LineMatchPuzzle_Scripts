using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FixedStartSceneUI : UI_Scene
{
    
    enum Buttons
    {
        StartButton,
        SettingButton,
        ExitButton,
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));

        AddUIHandler(GetButton((int)Buttons.StartButton).gameObject, ClickStartButton);
        AddUIHandler(GetButton((int)Buttons.SettingButton).gameObject, ClickSettingButton);
        AddUIHandler(GetButton((int)Buttons.ExitButton).gameObject, ClickExitButton);
    }

    void ClickStartButton(PointerEventData evt)
    {
        Managers.UI.ShowPopupUI<StageSelectPanelUI>(Defines.SceneType.StartScene);
    }

    void ClickSettingButton(PointerEventData evt)
    {
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

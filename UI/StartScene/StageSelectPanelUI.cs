using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StageSelectPanelUI : UI_PopUp
{
    int test;
    enum Buttons
    {
        StageStartButton_1,
        StageStartButton_2,
        StageStartButton_3,
        StageStartButton_4,
        StageStartButton_5,
        StartButtonCount,
        CloseButton,
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));

        for(int i = 0; i < (int)Buttons.StartButtonCount; i++)
        {
            AddUIHandler(GetButton(i).gameObject, ClickStageButton);
        }

        AddUIHandler(GetButton((int)Buttons.CloseButton).gameObject, ClickCloseButton);

        UnrockStage();

    }
    void ClickStageButton(PointerEventData evt)
    {
        RaycastResult evtt = evt.pointerCurrentRaycast;
        if (evtt.gameObject.transform.name == "Lock")
            return;
        string[] target = evtt.gameObject.transform.name.Split('_');
        Managers.Stage.curLevel = int.Parse(target[1]);
        Managers.Scene.LoadScene(Defines.SceneType.PuzzleScene);
    }

    void ClickCloseButton(PointerEventData evt)
    {
        Managers.UI.ClosePopupUI(); 
    }

    public void UnrockStage()
    {
        int i = 0;
        while (Managers.Stage._clearStageList[i])
        {
            Utils.FindChild<Transform>(GetButton(i).gameObject, "Lock", true).gameObject.SetActive(false);
            i++;
        }
    }
}

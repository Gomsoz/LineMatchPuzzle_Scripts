using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StageFinishPanel : UI_PopUp
{
    enum Texts
    {
        ClearOrFailedText,
        Score_Best,
        Score_Player,
    }

    enum Buttons
    {
        Button_Replay,
        Button_Exit,
    }

    public override void Init()
    {
        base.Init();

        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        AddUIHandler(GetButton((int)Buttons.Button_Replay).gameObject, ClickReplayButton);
        AddUIHandler(GetButton((int)Buttons.Button_Exit).gameObject, ClickExitButton);

        if (StageManager._isStageClear == true)
            GetText((int)Texts.ClearOrFailedText).text = $"Stage Clear!";
        else
            GetText((int)Texts.ClearOrFailedText).text = $"Stage Failed!";

        GetText((int)Texts.Score_Best).text = $"Best Score : {Managers.Stage.m_maxScore.ToString()}";
        GetText((int)Texts.Score_Player).text = $"Your Score : {Managers.Stage.m_totalScore.ToString()}";
    }

    void ClickReplayButton(PointerEventData evt)
    {
        Managers.Scene.LoadScene(Defines.SceneType.PuzzleScene);
    }

    void ClickExitButton(PointerEventData evt)
    {
        Managers.Scene.LoadScene(Defines.SceneType.StartScene);
    }
}

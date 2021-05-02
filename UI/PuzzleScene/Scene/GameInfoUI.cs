using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameInfoUI : UI_Scene
{
    enum Texts
    {
        ScoreText,
        TimeText,
        StageText,
        ReadyText,
    }

    enum Buttons
    {
        MenuButton,
    }

    public int test = 1;

    public override void Init()
    {
        base.Init();

        Bind<Text>(typeof(Texts));
        GameManager.GameMgr.TimeEvent -= SetTimeText;
        GameManager.GameMgr.TimeEvent += SetTimeText;

        Bind<Button>(typeof(Buttons));

        AddUIHandler(GetButton((int)Buttons.MenuButton).gameObject, ClickMenuButton);
    }

    void ClickMenuButton(PointerEventData evt)
    {
        GameManager.GameMgr.GamePause();
        Managers.UI.ShowPopupUI<MenuPanel>(Defines.SceneType.PuzzleScene);
    }

    public void SetTimeText(int time)
    {
        GetText((int)Texts.TimeText).text = $"Time : {time}";
    }

    public void SetScoreText(int score)
    {
        GetText((int)Texts.ScoreText).text = $"Score : {score}";
    }

    public void SetStageText(int level)
    {
        GetText((int)Texts.StageText).text = $"Stage : {level}";
    }

    public void ReadyText(string text)
    {
        GetText((int)Texts.ReadyText).text = $"{text}";
    }

    private void OnDestroy()
    {
        Debug.Log("destroy");
        GameManager.GameMgr.TimeEvent -= SetTimeText;
    }
}

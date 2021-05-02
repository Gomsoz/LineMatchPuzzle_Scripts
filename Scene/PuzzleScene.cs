using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleScene : BaseScene
{
    bool m_waitForNextStage = false;

    protected override void Init()
    {
        base.Init();

        Managers.Board.BoardManagerInit();
        Managers.Sound.Init();

        Managers.Input.PressKeyboard -= StartPuzzle;
        Managers.Input.PressKeyboard += StartPuzzle;

        sceneType = Defines.SceneType.PuzzleScene;
        Managers.UI.ShowSceneUI<GameInfoUI>(Defines.SceneType.PuzzleScene);
        Managers.UI.GetSceneUI<GameInfoUI>().SetStageText(Managers.Stage.curLevel);
        Managers.UI.GetSceneUI<GameInfoUI>().SetScoreText(0);
        WaitForNextStage();
    }

    public void WaitForNextStage()
    {
        GameManager.GameMgr.GamePause();
        m_waitForNextStage = true;  
    }

    public void StartPuzzle()
    {
        if (m_waitForNextStage == true && Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.GameMgr.GamePause();
            m_waitForNextStage = false;
            StartCoroutine(StartDelay(3));
        }
    }       

    IEnumerator StartDelay(int time)
    {
        while(time > 0)
        {
            Managers.Sound.Play("CountDownSound", 1, Defines.SoundType.StartSound);
            Managers.UI.GetSceneUI<GameInfoUI>().ReadyText(time.ToString());
            yield return new WaitForSeconds(1f);
            time--;
        }
        Managers.Sound.Play("StartSound", 1, Defines.SoundType.StartSound);
        Managers.UI.GetSceneUI<GameInfoUI>().ReadyText("G o");
        yield return new WaitForSeconds(0.5f);
        Managers.UI.GetSceneUI<GameInfoUI>().ReadyText("");
        Managers.Sound.Play("PuzzleBgm", 1, Defines.SoundType.Bgm);

        Managers.Stage.LoadStage(Managers.Stage.curLevel);    
    }

    public override void Clear()
    {
        Managers.Input.PressKeyboard -= StartPuzzle;
        //Managers.UI.ClosePopupAll();
        Managers.UI.ClearSceneUI();
    }
}

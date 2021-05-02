using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager
{
    // 스테이지 관리.
    StageLevelData m_levelData;
    public StageLevelData LevelData { get { return m_levelData; } }

    public int totalStageCnt = 5;

    int m_matchingLineCnt;

    public int curLevel;
    public int m_totalScore;
    public int m_maxScore;
    public bool[] _clearStageList;
    public static bool _isStageClear = true;
    public static bool _isPlayingPuzzle;

    public void StageMgrInit()
    {
        if(_clearStageList == null)
        {
            _clearStageList = new bool[totalStageCnt];
            _clearStageList[0] = true;
        }

        GameManager.GameMgr.ZeroTimeEvent -= FinishStage;
        GameManager.GameMgr.ZeroTimeEvent += FinishStage;
    }

    public void UpdateMatchingInfomation(int score)
    {
        m_matchingLineCnt++;
        m_totalScore += score;
        Managers.UI.GetSceneUI<GameInfoUI>().SetScoreText(m_totalScore);
        if (m_matchingLineCnt == m_levelData.numOfGoalLines)
        {
            FinishStage();
            ClearStageValuesOnPuzzleScene();
        }
    }

    public void LoadStage(int level)
    {
        _isPlayingPuzzle = true;
        m_totalScore = 0;
        m_levelData = Managers.Json.LoadStageLevelData(level);
        m_maxScore = Managers.Json.LoadScore(curLevel);
        Managers.Board.ClearAndSetBoard(m_levelData);

        GameManager.GameMgr.SetTiemer(30);
    }

    void FinishStage()
    {
        _isPlayingPuzzle = false;
        if (m_totalScore > m_maxScore)
            Managers.Json.SaveScore(curLevel, m_totalScore);
        Managers.UI.ShowPopupUI<StageFinishPanel>(Defines.SceneType.PuzzleScene);
    }

    void ClearStageValuesOnPuzzleScene()
    {
        _isPlayingPuzzle = false;
        _isStageClear = true;
        _clearStageList[curLevel] = true;
        m_matchingLineCnt = 0;
    }
}

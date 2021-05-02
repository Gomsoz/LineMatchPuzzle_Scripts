using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoardManager
{
    // 블록을 생성해 주는 class
    MakeStageMaps m_makeStageMaps = new MakeStageMaps();
    MatchingsManager m_matchingsMgr = new MatchingsManager();
    // 블록을 움직이게 하는 class
    BoardController m_boardController = new BoardController();

    public Action<List<Defines.Coordinate>> MatchingSucceed = null;

    #region InformationOfBoard

    int m_blockScore = 10;
    float m_defaultgap = 1f;
    Vector2 m_startPos;

    Transform m_cellHolder;
    Transform m_blockHolder;
    Transform m_matchCellHolder;

    string[] m_existingColor;

    Defines.BlockColors m_targetColor;

    int m_blockXCnt;
    int m_blockYCnt;
    int m_theNumberOfColors;
    public int BlockXCnt { get { return m_blockXCnt; } }
    public int BlockYCnt { get { return m_blockYCnt; } }

    Transform[,] m_arrayOfCell;
    Transform[,] m_arrayOfBlock;
    public Transform[,] ArrayOfBlock { get { return m_arrayOfBlock; } }
    string[,] m_arrayOfBlockColor;

    Transform[,] m_arrayOfMatchCell;
    Transform[,] m_arrayOfMatchBlock;
    string[,] m_arrayOfMatchBlockColor;

    #endregion


    public void BoardManagerInit()
    {
        m_boardController.Init();

        MatchingSucceed -= DestroyMatchingBlcok;
        MatchingSucceed += DestroyMatchingBlcok;

        // 하이어라키에서 셀과 블록을 담을 배열을 초기화시킴
        m_cellHolder = new GameObject { name = "CellHolder" }.transform;
        m_blockHolder = new GameObject { name = "BlockHolder" }.transform;
        m_matchCellHolder = new GameObject { name = "MatchCellHolder" }.transform;
    }

    public void ClearAndSetBoard(StageLevelData levelData)
    {
        ClearBoard();

        m_blockXCnt = levelData.boardSize;
        m_blockYCnt = levelData.boardSize;
        m_theNumberOfColors = levelData.numOfBlockColors;

        m_existingColor = System.Enum.GetNames(typeof(Defines.BlockColors));

        // 짝수일 때 gap : 1.5 시작지점 : - (gap * ((block 의 개수 / 2) - 1) + 0.5)
        // 홀수일 때 gap : 1.5 시작지점 : - gap * (block 의 개수 / 2)
        float _targetPosX = -(m_defaultgap * ((m_blockXCnt / 2) - 1 + (m_blockXCnt % 2)) + 0.5f - (0.5f * (m_blockYCnt % 2)));
        float _targetPosY = -(m_defaultgap * ((m_blockYCnt / 2) - 1 + (m_blockXCnt % 2)) + 0.5f - (0.5f * (m_blockYCnt % 2)));

        m_startPos = new Vector2(_targetPosX, _targetPosY);

        m_arrayOfCell = new Transform[m_blockXCnt + 1, m_blockYCnt + 1];
        m_arrayOfBlock = new Transform[m_blockXCnt + 1, m_blockYCnt + 1];
        m_arrayOfBlockColor = new string[m_blockXCnt + 1, m_blockYCnt + 1];

        m_arrayOfMatchCell = new Transform[(int)Defines.FourDirection.Count, m_blockYCnt + 1];
        m_arrayOfMatchBlock = new Transform[(int)Defines.FourDirection.Count, m_blockYCnt + 1];
        m_arrayOfMatchBlockColor = new string[(int)Defines.FourDirection.Count, m_blockYCnt + 1];

        SetAndMakeStageMap();
    }

    public void SetAndMakeStageMap()
    {
        m_makeStageMaps.SetStageInfo(m_defaultgap, m_startPos);

        // 블록 생성
        for(int i = 1; i <= m_blockXCnt; i++)
        {
            for(int j = 1; j <= m_blockYCnt; j++)
            {
                m_arrayOfCell[i, j] = m_makeStageMaps.InstantiateCell(i, j, Managers.Stage.LevelData.boardMap[i,j]);
                m_arrayOfCell[i, j].transform.parent = m_cellHolder;

                if (m_arrayOfCell[i, j].tag == "NoneCell")
                    continue;

                m_arrayOfBlock[i, j] = m_makeStageMaps.InstantiateBlock(m_arrayOfCell[i, j],m_theNumberOfColors,out m_arrayOfBlockColor[i, j]);
                m_arrayOfBlock[i, j].transform.parent = m_blockHolder;
            }
        }

        // 좌, 우 매칭블록 생성
        for (int i = 1; i <= m_blockYCnt; i++)
        {
            m_arrayOfMatchCell[(int)Defines.FourDirection.Left, i] = m_makeStageMaps.InstantiateMatchingCell(Defines.FourDirection.Left, i);
            m_arrayOfMatchBlock[(int)Defines.FourDirection.Left, i] 
                = m_makeStageMaps.InstantiateMatchingBlock(m_arrayOfMatchCell[(int)Defines.FourDirection.Left, i], m_theNumberOfColors, out m_arrayOfMatchBlockColor[(int)Defines.FourDirection.Left, i]);
        }

        for (int i = 1; i <= m_blockYCnt; i++)
        {
            m_arrayOfMatchCell[(int)Defines.FourDirection.Right, i] = m_makeStageMaps.InstantiateMatchingCell(Defines.FourDirection.Right, i);
                m_arrayOfMatchBlock[(int)Defines.FourDirection.Right, i]
                    = m_makeStageMaps.InstantiateMatchingBlock(m_arrayOfMatchCell[(int)Defines.FourDirection.Right, i], m_theNumberOfColors, out m_arrayOfMatchBlockColor[(int)Defines.FourDirection.Right, i]);
        }

        // 상, 하 매칭블록 생성
        for (int i = 1; i <= m_blockXCnt; i++)
        {
            m_arrayOfMatchCell[(int)Defines.FourDirection.Upper, i] = m_makeStageMaps.InstantiateMatchingCell(Defines.FourDirection.Upper, i);
            m_arrayOfMatchBlock[(int)Defines.FourDirection.Upper, i]
                = m_makeStageMaps.InstantiateMatchingBlock(m_arrayOfMatchCell[(int)Defines.FourDirection.Upper, i], m_theNumberOfColors, out m_arrayOfMatchBlockColor[(int)Defines.FourDirection.Upper, i]);
        }

        for (int i = 1; i <= m_blockYCnt; i++)
        {
            m_arrayOfMatchCell[(int)Defines.FourDirection.Lower, i] = m_makeStageMaps.InstantiateMatchingCell(Defines.FourDirection.Lower, i);
            m_arrayOfMatchBlock[(int)Defines.FourDirection.Lower, i]
                = m_makeStageMaps.InstantiateMatchingBlock(m_arrayOfMatchCell[(int)Defines.FourDirection.Lower, i], m_theNumberOfColors, out m_arrayOfMatchBlockColor[(int)Defines.FourDirection.Lower, i]);
        }

        m_matchingsMgr.Init(m_makeStageMaps.PointerPositions);
        m_targetColor = (Defines.BlockColors)UnityEngine.Random.Range(1, m_theNumberOfColors + 1);
        SetMatchingsPointer(m_targetColor);

        m_matchingsMgr.JudgeMatchingBlocks(m_targetColor, m_arrayOfBlockColor);
    }

    void ClearBoard()
    {
        for(int i = 1; i < m_blockXCnt + 1; i++)
        {
            for(int j = 1; j < m_blockYCnt + 1; j++)
            {
                GameObject.Destroy(m_arrayOfCell[i, j]);
                GameObject.Destroy(m_arrayOfBlock[i, j]);
            }
        }

        for(int i = 1; i < (int)Defines.FourDirection.Count; i++)
        {
            for (int j = 1; j < m_blockYCnt + 1; j++)
            {
                GameObject.Destroy(m_arrayOfMatchCell[i, j]);
                GameObject.Destroy(m_arrayOfMatchBlock[i, j]);
            }
        }
    }

    public void ChangeBlockOnVerticalLine(int line)
    {
        if (line > m_blockYCnt)
            return;

        m_matchingsMgr.TurnOffBlocksEffect();

        // 라인의 첫부분의 블록을 제거한다.
        Transform tempBlock = m_arrayOfBlock[line, 1];
        RemoveBlockOnArray(line, 1);
        GameObject.Destroy(tempBlock.gameObject);

        int noneCnt = 0;
        // 두번째 부터 차례로 아래로 내린다.
        for (int i = 2; i <= m_blockYCnt; i++)
        {
            if(m_arrayOfCell[line, i].tag == "NoneCell")
            {
                noneCnt++;
                continue;
            }
            m_arrayOfBlock[line, i].position = m_arrayOfCell[line, i - 1 - noneCnt].position;
            m_arrayOfBlock[line, i - 1 - noneCnt] = m_arrayOfBlock[line, i];
            m_arrayOfBlockColor[line, i - 1 - noneCnt] = m_arrayOfBlockColor[line, i];
            noneCnt = 0;
        }

        // 마지막부분은 블록을 생성 해준다.
        m_arrayOfBlock[line, m_blockYCnt] = m_makeStageMaps.InstantiateBlock
            (m_arrayOfCell[line, m_blockYCnt], m_theNumberOfColors, out m_arrayOfBlockColor[line, m_blockYCnt], true);
        m_arrayOfBlock[line, m_blockYCnt].transform.parent = m_blockHolder;

        Managers.Sound.Play("VerticalMoveSound", 1f, Defines.SoundType.MoveBlockSound);
        m_matchingsMgr.JudgeMatchingBlocks(m_targetColor, m_arrayOfBlockColor);
    }

    public void ChangeBlockOnHorizonLine(int line)
    {
        if (line > m_blockXCnt)
            return;

        m_matchingsMgr.TurnOffBlocksEffect();

        // 라인의 첫부분의 블록을 제거한다.
        Transform tempBlock = m_arrayOfBlock[1, line];
        RemoveBlockOnArray(1, line);
        GameObject.Destroy(tempBlock.gameObject);

        int noneCnt = 0;
        // 두번째 부터 차례로 아래로 내린다.
        for (int i = 2; i <= m_blockXCnt; i++)
        {
            if (m_arrayOfCell[i, line].tag == "NoneCell")
            {
                noneCnt++;
                continue;
            }
            m_arrayOfBlock[i, line].position = m_arrayOfCell[i - 1 - noneCnt, line].position;
            m_arrayOfBlock[i - 1 - noneCnt, line] = m_arrayOfBlock[i, line];
            m_arrayOfBlockColor[i - 1 - noneCnt, line] = m_arrayOfBlockColor[i, line];
            noneCnt = 0;
        }

        // 마지막부분은 블록을 생성 해준다.
        m_arrayOfBlock[m_blockXCnt, line] = m_makeStageMaps.InstantiateBlock
            (m_arrayOfCell[m_blockXCnt, line], m_theNumberOfColors, out m_arrayOfBlockColor[m_blockXCnt, line], true);
        m_arrayOfBlock[m_blockXCnt, line].transform.parent = m_blockHolder;

        Managers.Sound.Play("HorizonMoveSound", 1f, Defines.SoundType.MoveBlockSound);
        m_matchingsMgr.JudgeMatchingBlocks(m_targetColor, m_arrayOfBlockColor);
    }

    void RemoveBlockOnArray(int x, int y)
    {
        m_arrayOfBlock[x, y] = null;
        m_arrayOfBlockColor[x, y] = null;
    }

    public void SetMatchingsPointer(Defines.BlockColors targetColor)
    {
        string colorName = System.Enum.GetName(typeof(Defines.BlockColors), targetColor);

        Defines.FourDirection alphaDir = (Defines.FourDirection)UnityEngine.Random.Range(1, (int)Defines.FourDirection.Count);
        Defines.FourDirection betaDir = (Defines.FourDirection)UnityEngine.Random.Range(1, (int)Defines.FourDirection.Count);

        int alphaTargetIdx = 0;
        int betaTargetIdx = 0;

        if(alphaDir == betaDir)
        {
            betaDir += 1;
            if (betaDir == Defines.FourDirection.Count)
                betaDir = Defines.FourDirection.Left;
        }

        for(int i = 1; i <= m_theNumberOfColors; i++)
        {
            if (m_arrayOfMatchBlockColor[(int)alphaDir, i] == colorName)
                alphaTargetIdx = i;
            if (m_arrayOfMatchBlockColor[(int)betaDir, i] == colorName)
                betaTargetIdx = i;

            if (alphaTargetIdx > 0 && betaTargetIdx > 0)
                break;
        }

        m_matchingsMgr.SetPointerPosition(Defines.MatchingsPointers.Alpha, alphaDir, alphaTargetIdx);
        m_matchingsMgr.SetPointerPosition(Defines.MatchingsPointers.Beta, betaDir, betaTargetIdx);
    }

    public void DestroyMatchingBlcok(List<Defines.Coordinate> queue)
    {
        int score = 0;

        foreach (Defines.Coordinate pos in queue)
        {
            Debug.Log(queue.Count);
            Transform tempBlock = m_arrayOfBlock[pos.posX, pos.posY];
            RemoveBlockOnArray(pos.posX, pos.posY);
            tempBlock.GetComponent<BlockBehavior>().PlayDestroyBlock();
            score += m_blockScore;
            m_arrayOfBlock[pos.posX, pos.posY] 
                = m_makeStageMaps.InstantiateBlock(m_arrayOfCell[pos.posX, pos.posY], m_theNumberOfColors, out m_arrayOfBlockColor[pos.posX, pos.posY]);
        }

        m_matchingsMgr.TurnOffBlocksEffect();

        Managers.Stage.UpdateMatchingInfomation(score);
        m_targetColor = (Defines.BlockColors)UnityEngine.Random.Range(1, m_theNumberOfColors + 1);
        SetMatchingsPointer(m_targetColor);
    }
}
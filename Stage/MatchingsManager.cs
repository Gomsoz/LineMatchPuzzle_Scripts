using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MatchingsManager
{
    Transform m_matchingTargetPointer;
    string m_pointerPrefabsPath = "Prefabs/MatchingTargetPointer";

    Transform[] m_pointers;

    Vector2[,] m_pointerPositions;
    Defines.Coordinate[] m_pointerIdxOnBoard;

    List<Transform> m_EffectOnBlocks;
    bool[,] m_findBlock;

    List<Defines.Coordinate> _succeedMatchingBlock = new List<Defines.Coordinate>();

    public void Init(Vector2[,] pointerPositions)
    {
        m_pointers = new Transform[(int)Defines.MatchingsPointers.Count];
        m_matchingTargetPointer = Managers.Resources.Load<Transform>(m_pointerPrefabsPath);
        m_pointerPositions = pointerPositions;

        m_pointerIdxOnBoard = new Defines.Coordinate[(int)Defines.MatchingsPointers.Count];
        m_EffectOnBlocks = new List<Transform>();
        m_findBlock = new bool[Managers.Board.BlockXCnt + 1, Managers.Board.BlockYCnt + 1];

        InstantiateMatchingsPointer();
    }

    void InstantiateMatchingsPointer()
    {
        for(int i = 1; i < (int)Defines.MatchingsPointers.Count; i++)
        {
            m_pointers[i] = GameObject.Instantiate(m_matchingTargetPointer);
            m_pointers[i].GetComponent<PointerBehavior>().Init(m_pointerPositions);
        }
    }

    public void SetPointerPosition(Defines.MatchingsPointers targetPointer, Defines.FourDirection direction, int idx)
    {
       m_pointers[(int)targetPointer].GetComponent<PointerBehavior>().SetPointerPos(direction, idx, m_pointerPositions[(int)direction, idx]);
    }

    public void JudgeMatchingBlocks(Defines.BlockColors color, string[,] blockColorsOnBoard)
    {       
        // Alpha Pointer - Start Pointer
        // Beta Pointer - Finish Pointer
        // 1. 알파 포인터가 베타 포인터 위치까지 탐색을 진행하면 매칭에 성공, 베타 포인터는 탐색을 진행하지 않음.
        // 2. 알파 포인터가 매칭에 실패하였으면 베타포인터가 탐색을 진행.

        string _targetColor = System.Enum.GetName(typeof(Defines.BlockColors), color);

        // 매칭 포인터들의 위치를 블럭의 위치로 전환
        for(int i = 1; i < (int)Defines.MatchingsPointers.Count; i++)
        {
            Defines.FourDirection _pointerDir = m_pointers[i].GetComponent<PointerBehavior>().PointerDir;
            int _pointerIdx = m_pointers[i].GetComponent<PointerBehavior>().PointerIdx;
            m_pointerIdxOnBoard[i] = MatchingIdxToBoardIdx(_pointerDir, _pointerIdx);
        }

        if (SearchMatchingBlock(m_pointerIdxOnBoard[(int)Defines.MatchingsPointers.Alpha], _targetColor, blockColorsOnBoard, true) == false)
        {
            SearchMatchingBlock(m_pointerIdxOnBoard[(int)Defines.MatchingsPointers.Beta], _targetColor, blockColorsOnBoard);
            return;
        }

        if (Managers.Board.MatchingSucceed != null)
            Managers.Board.MatchingSucceed.Invoke(_succeedMatchingBlock);

        Managers.Sound.Play("MatchingSound", 1, Defines.SoundType.StartSound);
        Debug.Log("Matching!");
    }

    bool SearchMatchingBlock(Defines.Coordinate startPos, string targetColor, string[,] blockColorsOnBoard, bool checkEnd = false)
    {
        bool _isMatchingSucceed = false;

        Queue<Defines.Coordinate> queue = new Queue<Defines.Coordinate>();
        queue.Enqueue(new Defines.Coordinate(startPos.posX, startPos.posY));

        int[] idxPatternX = new int[(int)Defines.FourDirection.Count] { 0, -1, 1, 0, 0 };
        int[] idxPatternY = new int[(int)Defines.FourDirection.Count] { 0, 0, 0, 1, -1 };

        // 시작 위치의 블록 색이 원하는 색이 아니면 종료
        if (blockColorsOnBoard[startPos.posX, startPos.posY] != targetColor)
            return false;

        Managers.Board.ArrayOfBlock[startPos.posX, startPos.posY].GetComponent<BlockBehavior>().PlayBlockEffect(true);
        
        while (queue.Count > 0)
        {
            Defines.Coordinate position = queue.Dequeue();

            // Beta Pointer 와 만난다면 True 를 반환
            if (checkEnd == true && position.Equals(m_pointerIdxOnBoard[(int)Defines.MatchingsPointers.Beta]))
                _isMatchingSucceed = true;

            int nextX = 0;
            int nextY = 0;


            // 4방향 탐색 진행
            for (int i = 1; i < (int)Defines.FourDirection.Count; i++)
            {
                nextX = position.posX + idxPatternX[i];
                nextY = position.posY + idxPatternY[i];

                // 경계 검사, 탐색여부 검사, 색 검사
                if (nextX < 1 || nextX > Managers.Board.BlockXCnt || nextY < 1 || nextY > Managers.Board.BlockYCnt)
                    continue;
                if (m_findBlock[nextX, nextY] == true)
                    continue;
                if (blockColorsOnBoard[nextX, nextY] != targetColor)
                    continue;

                // 이펙트 발동
                Managers.Board.ArrayOfBlock[nextX, nextY].GetComponent<BlockBehavior>().PlayBlockEffect(true);
                m_EffectOnBlocks.Add(Managers.Board.ArrayOfBlock[nextX, nextY]);
                m_findBlock[nextX, nextY] = true;
                queue.Enqueue(new Defines.Coordinate(nextX, nextY));
                _succeedMatchingBlock.Add(new Defines.Coordinate(nextX, nextY));
            }

        }

        return _isMatchingSucceed;
    }

    Defines.Coordinate MatchingIdxToBoardIdx(Defines.FourDirection direction, int idx)
    {
        Defines.Coordinate returnPos = new Defines.Coordinate(0, 0);

        // 시작 위치 결정
        switch (direction)
        {
            case Defines.FourDirection.Left:
                returnPos.posX = 1;
                returnPos.posY = idx;
                break;
            case Defines.FourDirection.Right:
                returnPos.posX = Managers.Board.BlockXCnt;
                returnPos.posY = idx;
                break;
            case Defines.FourDirection.Upper:
                returnPos.posX = idx;
                returnPos.posY = Managers.Board.BlockYCnt;
                break;
            case Defines.FourDirection.Lower:
                returnPos.posX = idx;
                returnPos.posY = 1;
                break;
        }
        return returnPos;
    }

    public void TurnOffBlocksEffect()
    {
        m_findBlock = new bool[Managers.Board.BlockXCnt + 1, Managers.Board.BlockYCnt + 1];
        _succeedMatchingBlock.Clear();

        foreach (Transform blocks in m_EffectOnBlocks)
        {
            blocks.GetComponent<BlockBehavior>().PlayBlockEffect(false);
        }
        m_EffectOnBlocks.Clear();
    }
}

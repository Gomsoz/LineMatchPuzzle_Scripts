using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MakeStageMaps
{
    float m_gapBetweenBlocks; // 블록 사이의 거리
    float m_gapBetweenBlockAndMatchCell = 1f;
    float m_gapBetweenMatchingsAndPointer = 1f;

    Vector2 m_baseBlockPos; // 블록의 시작 지점

    // 포인터의 포지션을 저장하는 배열
    Vector2[,] m_pointerPositions;
    public Vector2[,] PointerPositions { get { return m_pointerPositions; } }

    string m_blockSpritePath = "Sprites/BlockSprites/BlockSprite_"; // blocksprite 위치
    string[] m_speciesOfBlockColor = new string[(int)Defines.BlockColors.Count]; // 블록의 종류
    Sprite[] m_blockColors; // 블록의 sprite를 Load 해서 담아놓는 배열

    Stack<Defines.BlockColors> m_blockColorsStack = new Stack<Defines.BlockColors>();

    public void SetStageInfo(float gap, Vector2 startPos)
    {
        m_gapBetweenBlocks = gap;
        m_baseBlockPos = startPos;

        // 추후 Init으로 처리해야 할 부분
        // Color 정보를 Load 한다.
        m_speciesOfBlockColor = Enum.GetNames(typeof(Defines.BlockColors));
        m_blockColors = new Sprite[(int)Defines.BlockColors.Count + 1];
        for(int i = 0; i < (int)Defines.BlockColors.Count; i++)
        {
            m_blockColors[i] = Managers.Resources.Load<Sprite>(m_blockSpritePath + m_speciesOfBlockColor[i]);
        }

        m_pointerPositions = new Vector2[(int)Defines.FourDirection.Count,Managers.Board.BlockXCnt + 1];
    }

    public Transform InstantiateMatchingCell(Defines.FourDirection direction, int cellPos)
    {
        if (direction < Defines.FourDirection.Left || direction > Defines.FourDirection.Lower)
            return null;

        int[] matchingsPatternX = new int[5] {0, -1, 1, 0, 0 };
        int[] matchingsPatternY = new int[5] {0, 0, 0, 1, -1 };

        int _targetPosX = 0;
        int _targetPosY = 0;       

        Transform _matchingCell;
        int _targetIdx = (int)direction;

        switch (direction)
        {
            case Defines.FourDirection.Left:
                _targetPosX = 1;
                _targetPosY = cellPos;
                break;
            case Defines.FourDirection.Right:
                _targetPosX = Managers.Board.BlockXCnt;
                _targetPosY = cellPos;
                break;
            case Defines.FourDirection.Upper:
                _targetPosX = cellPos;
                _targetPosY = Managers.Board.BlockYCnt;
                break;
            case Defines.FourDirection.Lower:
                _targetPosX = cellPos;
                _targetPosY = 1;
                break;
        }
        _matchingCell = InstantiateCell(_targetPosX, _targetPosY, 1, "Prefabs/MatchCell");
        _matchingCell.position +=
                    new Vector3(m_gapBetweenBlockAndMatchCell * matchingsPatternX[_targetIdx], m_gapBetweenBlockAndMatchCell * matchingsPatternY[_targetIdx]);
        m_pointerPositions[(int)direction, cellPos] = 
            _matchingCell.position + new Vector3(m_gapBetweenMatchingsAndPointer * matchingsPatternX[_targetIdx], m_gapBetweenMatchingsAndPointer * matchingsPatternY[_targetIdx]);
        return _matchingCell;
    }

    public Transform InstantiateMatchingBlock
        (Transform targetCell, int theNumberOfColors, out string blockColor, string blockPath = "Prefabs/Block")
    {
        Transform _targetBlock = Managers.Resources.Instantiate(blockPath).transform;

        // 색을 중복하지 않게 하기위해 Stack에 색을 저장하고 꺼낸다.
        if(m_blockColorsStack.Count == 0)
        {
            Defines.BlockColors tempColor = Defines.BlockColors.None;
            int peekColorIdx = 1;
            m_blockColorsStack.Push(tempColor + peekColorIdx);
            while (m_blockColorsStack.Count < theNumberOfColors)
            {
                peekColorIdx = UnityEngine.Random.Range(2, theNumberOfColors + 1);
                if (m_blockColorsStack.Contains(tempColor + peekColorIdx))
                    continue;
                m_blockColorsStack.Push(tempColor + peekColorIdx);
            }
        }

        _targetBlock.position = targetCell.transform.position;

        int _targetColorIdx = (int)m_blockColorsStack.Pop();

        // 블록의 색을 바꿈
        _targetBlock.GetComponent<SpriteRenderer>().sprite = m_blockColors[_targetColorIdx];
        _targetBlock.localScale = new Vector3(0.3f, 0.3f);
        blockColor = m_speciesOfBlockColor[_targetColorIdx];

        return _targetBlock;
    }

    public Transform InstantiateCell(int x, int y, int mapCode = 1, string path = "Prefabs/Cell")
    {
        Transform _targetCell = mapCode == 1 ?
            Managers.Resources.Instantiate(path).transform : Managers.Resources.Instantiate("Prefabs/NoneCell").transform;
        _targetCell.position 
            = new Vector2(m_baseBlockPos.x + (m_gapBetweenBlocks * (x - 1)), m_baseBlockPos.y + (m_gapBetweenBlocks * (y - 1)));
        return _targetCell;
    }

    public Transform InstantiateBlock
        (Transform targetCell, int theNumberOfColors, out string blockColor, bool noneBlock = false, string blockPath = "Prefabs/Block")
    {
        int _addNoneBlock = noneBlock ? 1 : 0;
        Transform _targetBlock = Managers.Resources.Instantiate(blockPath).transform;

        _targetBlock.position = targetCell.transform.position;

        int _targetColorIdx = UnityEngine.Random.Range(1 - _addNoneBlock, theNumberOfColors + 1);

        // 블록의 색을 바꿈
        _targetBlock.GetComponent<SpriteRenderer>().sprite = m_blockColors[_targetColorIdx];
        blockColor = m_speciesOfBlockColor[_targetColorIdx];

        return _targetBlock;
    }
}

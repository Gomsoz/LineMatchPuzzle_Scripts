using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defines
{
    public enum BoardState
    {
        Even,
        Odd,
    }

    public enum FourDirection
    {
        None,
        Left,
        Right,
        Upper,
        Lower,
        Count,
    }

    public enum BlockColors
    {
        None,
        Red,
        Green,
        Blue,
        Purple,
        Yellow,
        Count,
    }

    public enum MatchingsPointers
    {
        None,
        Alpha,
        Beta,
        Count,
    }

    public enum UIEvent
    {
        None,
        Click,
        Count,
    }

    public enum SceneType
    {
        Unknown,
        StartScene,
        PuzzleScene,
    }

    public enum SoundType
    {
        Bgm,
        MoveBlockSound,
        MatchingBlockSound,
        StageClearSound,
        StartSound,
        SoundCnt,
    }

    public struct Coordinate
    {
        public int posX;
        public int posY;
        public Coordinate(int x, int y)
        {
            posX = x;
            posY = y;
        }
    }
}

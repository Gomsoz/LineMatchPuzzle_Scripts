using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController
{
    enum VerticalLineNumber
    {
        None,
        Q,
        W,
        E,
        R,
        T,
    }

    enum HorizonLineNumber
    {
        None,
        A,
        S,
        D,
        F,
        G,
    }

    public void Init()
    {
        Managers.Input.PressKeyboard -= MoveBoardLine;
        Managers.Input.PressKeyboard += MoveBoardLine;
    }

    // 나중에 패턴으로 수정할 필요 있음.
    public void MoveBoardLine()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Managers.Board.ChangeBlockOnVerticalLine((int)VerticalLineNumber.Q);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Managers.Board.ChangeBlockOnVerticalLine((int)VerticalLineNumber.W);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Managers.Board.ChangeBlockOnVerticalLine((int)VerticalLineNumber.E);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Managers.Board.ChangeBlockOnVerticalLine((int)VerticalLineNumber.R);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Managers.Board.ChangeBlockOnVerticalLine((int)VerticalLineNumber.T);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Managers.Board.ChangeBlockOnHorizonLine((int)HorizonLineNumber.A);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Managers.Board.ChangeBlockOnHorizonLine((int)HorizonLineNumber.S);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Managers.Board.ChangeBlockOnHorizonLine((int)HorizonLineNumber.D);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Managers.Board.ChangeBlockOnHorizonLine((int)HorizonLineNumber.F);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Managers.Board.ChangeBlockOnHorizonLine((int)HorizonLineNumber.G);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerBehavior : MonoBehaviour
{
    Defines.FourDirection m_pointerDir;
    int m_pointerIdx;
    public Defines.FourDirection PointerDir { get { return m_pointerDir; } }
    public int PointerIdx { get { return m_pointerIdx; } }

    // 포인터의 전체 위치 배열
    Vector2[,] m_movePos;

    public void Init(Vector2[,] pointerPositions)
    {
        m_movePos = pointerPositions;
    }

    public void SetPointerPos(Defines.FourDirection direction, int idx, Vector2 targetPosition)
    {
        transform.position = targetPosition;

        m_pointerDir = direction;
        m_pointerIdx = idx;

        switch (direction)
        {
            case Defines.FourDirection.Left:
                transform.eulerAngles = new Vector3(0, 0, 90);
                break;
            case Defines.FourDirection.Right:
                transform.eulerAngles = new Vector3(0, 0, 270);
                break;
            case Defines.FourDirection.Upper:
                transform.eulerAngles = new Vector3(0, 0, 0);
                break;
            case Defines.FourDirection.Lower:
                transform.eulerAngles = new Vector3(0, 0, 180);
                break;
        }
    }
    
}

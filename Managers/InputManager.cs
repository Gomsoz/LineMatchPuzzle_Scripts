using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager
{
    public Action PressKeyboard = null;
    public void InputUpdate()
    {
        if (PressKeyboard != null && Input.anyKeyDown)
        {
            PressKeyboard.Invoke();
        }
    }
}

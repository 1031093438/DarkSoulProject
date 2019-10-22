using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton 
{
    public bool OnPressed;
    public bool OnReleased;
    public bool IsPressing;

    public bool currentState;
    public bool lastState;

    public void Tick(bool input)
    {
        currentState = input;

        IsPressing = currentState;

        OnPressed = false;
        OnReleased = false;

        if(currentState != lastState)
        {
            if(currentState == true)
            {
                OnPressed = true;
            }
            else
            {
                OnReleased = true;
            }
        }
        lastState = currentState;
    }
}

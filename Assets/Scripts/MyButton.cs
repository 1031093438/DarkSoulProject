using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton 
{
    public bool OnPressed;
    public bool OnReleased;
    public bool IsPressing;
    public bool IsExtending;
    public bool IsDelaying;

    public bool currentState;
    public bool lastState;

    private MyTimer extTimer = new MyTimer();
    private MyTimer delayTimer = new MyTimer();

    public void Tick(bool input)
    {
        extTimer.Tick();
        delayTimer.Tick();

        currentState = input;

        IsPressing = currentState;

        OnPressed = false;
        OnReleased = false;
        IsExtending = false;
        IsDelaying = false;

        if(currentState != lastState)
        {
            if(currentState == true)
            {
                OnPressed = true;
                StartTimer(delayTimer, 0.5f);
            }
            else
            {
                OnReleased = true;
                StartTimer(extTimer, 0.5f);
            }
        }
        lastState = currentState;

        if(extTimer.state == MyTimer.STATE.RUN)
        {
            IsExtending = true;
        }

        if(delayTimer.state == MyTimer.STATE.RUN)
        {
            IsDelaying = true;
        }
    }

    public void StartTimer(MyTimer timer , float duration)
    {
        timer.duration = duration;
        timer.Go();
    }
}

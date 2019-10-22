using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : MonoBehaviour {
    public string DUp;
    public string DDown;
    public string DLeft;
    public string DRight;

    public string JUp;
    public string JDown;
    public string JLeft;
    public string JRight;

    public string keyJump;
    public string keyAttack;
    public string keyRun;

    public float Dright;
    public float Dup;
    public float Jright;
    public float Jup;
    private float TargetDright;
    private float TargetDup;

    public Vector3 Dvec;
    public float Dmagnitude;
    public bool inputEnabled;

    public bool jump;
    public bool run;
    public bool roll;

    public MyButton buttonJump = new MyButton();
    public MyButton buttonRun = new MyButton();
    public MyButton buttonRoll = new MyButton();

    void Update()
    {
        TargetDright = (Input.GetKey (DRight) ? 1.0f : 0) - (Input.GetKey (DLeft) ? 1.0f : 0);
        TargetDup = (Input.GetKey (DUp) ? 1.0f : 0) - (Input.GetKey (DDown) ? 1.0f : 0);

        Jright = (Input.GetKey(JRight) ? 1.0f : 0) - (Input.GetKey(JLeft) ? 1.0f : 0);
        Jup = (Input.GetKey(JUp) ? 1.0f : 0) - (Input.GetKey(JDown) ? 1.0f : 0);
        if (!inputEnabled) {
            Dup = 0;
            Dright = 0;
            jump = false;
        } else {
            Dup = Mathf.Lerp (Dup, TargetDup, 0.5f);
            Dright = Mathf.Lerp (Dright, TargetDright, 0.5f);
        }
        Vector2 output = SquareToCircle(new Vector2(Dright,Dup));

        Dvec = ((transform.right * output.x) + (transform.forward * output.y)).normalized;
        Dmagnitude = Mathf.Sqrt (output.y * output.y + output.x * output.x);

        //Key Setting
        buttonJump.Tick(Input.GetKey(keyJump));
        buttonRun.Tick(Input.GetKey(keyRun));
        //buttonRoll.Tick(Input.GetKey(keyRoll));

        jump = buttonJump.OnPressed;
        run = buttonRun.IsPressing;
        //roll = buttonRoll.OnPressed;
    }

    public Vector2 SquareToCircle(Vector2 input)
    {
        Vector2 output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2);
        return output;
    }

}
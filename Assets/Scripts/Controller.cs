using System;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public int Index { get; private set; }
    public bool IsAssigned { get; set; }

    string attackButton;
    string specialAttackButton;
    string cancelButton;
    string jumpButton;
    string dodgeButton;
    string horizontalAxis;
    string verticalAxis;
    string startButton;
    
    public bool attack;
    public bool attackPressed;
    public bool specialAttackPressed;
    public bool dodge;
    public bool dodgePressed;
    public bool jump;
    public bool jumpPressed;
    public float horizontal;
    public bool cancel;
    public bool cancelPressed;
    public bool start;
    public bool startPressed;

    string leftBumperButton;
    string rightBumperButton;
    public bool leftBumper;
    public bool rightBumper;
    public bool leftBumperPressed;
    public bool rightBumperPressed;



    public float vertical;

    void Update()
    {
        if (!string.IsNullOrEmpty(attackButton))
        {
            attack = Input.GetButton(attackButton);
            attackPressed = Input.GetButtonDown(attackButton);
            specialAttackPressed = Input.GetButtonDown(specialAttackButton);
            jump = Input.GetButton(jumpButton);
            jumpPressed = Input.GetButtonDown(jumpButton);
            dodge = Input.GetButton(dodgeButton);
            dodgePressed = Input.GetButtonDown(dodgeButton);
            cancel = Input.GetButton(cancelButton);
            cancelPressed = Input.GetButtonDown(cancelButton);
            
            horizontal = Input.GetAxis(horizontalAxis);
            vertical = Input.GetAxis(verticalAxis);
            leftBumper = Input.GetButton(leftBumperButton);
            leftBumperPressed = Input.GetButtonDown(leftBumperButton);
            rightBumper = Input.GetButton(rightBumperButton);
            rightBumperPressed = Input.GetButtonDown(rightBumperButton);
            start = Input.GetButton(startButton);
            startPressed = Input.GetButtonDown(startButton);
        }
        
    }

    internal void SetIndex(int index)
    {
        Index = index;
        attackButton = "Attack" + Index;
        specialAttackButton = "SpecialAttack" + Index;
        jumpButton = "Jump" + Index;
        dodgeButton = "Dodge" + Index;
        cancelButton = "Cancel" + Index;
        horizontalAxis = "Horizontal" + Index;
        verticalAxis = "Vertical" + Index;
        gameObject.name = "Controller" + Index;
        leftBumperButton = "LeftBumper" + Index;
        rightBumperButton = "RightBumper" + Index;
        startButton = "Start" + Index;
    }

    internal bool AnyButtonDown()
    {
        return attack;
    }

    internal Vector3 GetDirection()
    {
        return new Vector3(horizontal, 0, -vertical);
    }

    internal bool ButtonDown(PlayerButton button)
    {
        switch (button)
        {
            case PlayerButton.Attack:
                return attackPressed;
            case PlayerButton.SpecialAttack:
                return specialAttackPressed;
            case PlayerButton.Jump:
                return jumpPressed;
            case PlayerButton.Dodge:
                return dodgePressed;
            default:
                return false;
        }
    }
}

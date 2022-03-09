using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    // Assign dice game object to script in editor
    public GameObject Die; //Name of the die in Unity
    private Dice dice;

    void Awake()
    {
        // Retrieve the script from the gameobject 
        dice = Die.GetComponent<Dice>();
    }

    public void RollDiceOnClick()
    {
        // Call the roll dice method
        dice.RollDice();
    }
}

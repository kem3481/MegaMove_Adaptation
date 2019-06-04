using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.UI;

public class Verify : MonoBehaviour
{
    public bool positionsCorrect;
    public GameObject hand;
    public GameObject headset;
    private ControllerCheck controller;
    private HeadCheck head;

    private void Start()
    {
        positionsCorrect = false;
        controller = hand.GetComponent<ControllerCheck>();
        head = headset.GetComponent<HeadCheck>();
    }

    private void Update()
    {
        if (head.headPosition == true && controller.handPosition == true)
        {
            positionsCorrect = true;
            Debug.Log("Positions are correct");
        }
    }
}
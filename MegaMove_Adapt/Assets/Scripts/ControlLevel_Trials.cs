using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using USE_States;
using Valve.VR;

public class ControlLevel_Trials : ControlLevel
{
    public GameObject controllerPosition;
    public GameObject playerPosition;
    public GameObject gamecontroller;

    public GameObject beginText;
    public GameObject endText;
    private Verify verifyPositions;
    private Controls controls;
    private ControllerCheck controller;
    private HeadCheck head;
    private int trials = 0;
    private int score = 0;

    // Vive Control GameObjects
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean squeezeAction;

    public override void DefineControlLevel()
    { 
        // Accessing other scripts and objects
        verifyPositions = GetComponent<Verify>();
        controls = GetComponent<Controls>();
        controller = GetComponent<ControllerCheck>();
        head = GetComponent<HeadCheck>();

        // Defining States
        State begin = new State("Begin"); // Step 1 and 2 in Procedure
        State stimOn = new State("Stimulus"); // Step 3
        State collectResponse = new State("CollectResponse"); // Step 4
        State feedback = new State("Feedback"); // Step 5
        AddActiveStates(new List<State> { begin, stimOn, collectResponse, feedback });

        begin.AddInitializationMethod(() =>
        {
            controllerPosition.SetActive(true);
            beginText.SetActive(true);
            endText.SetActive(false);
        });
        begin.SpecifyStateTermination(() => head.headPosition == true && controller.handPosition == true, stimOn);

        stimOn.AddInitializationMethod(() =>
        {
            controls.testobject.SetActive(true);
            beginText.SetActive(false);
            controllerPosition.SetActive(false);
            trials++;

            Debug.Log("Overlap: " + controls.testobject);
            Debug.Log("Radius: " + controls.radius);
            Debug.Log("Angle: " + controls.angle);
        });
        stimOn.SpecifyStateTermination(() => controls.testobject == null, collectResponse);

        collectResponse.AddInitializationMethod(() =>
        {
            bool GetSqueeze()
            {
                return squeezeAction.GetStateDown(handType);
            }

            if (GetSqueeze())
            {
                controllerPosition.SetActive(true);
                beginText.SetActive(true);

                /*if ()
                {
                    score = score + 100;
                }

                if ("position is within the penlty subtract 50")
                {
                    score = score - 50;
                }

                if ("poition is not within either, add nothing")
                {
                    score = score;
                }*/
            }

            Debug.Log("Trigger pull position: " + gamecontroller.transform.localPosition.x + ", " + gamecontroller.transform.localPosition.y);
        });
        collectResponse.SpecifyStateTermination(() => trials < 90, begin);
        collectResponse.SpecifyStateTermination(() => trials == 90, feedback);

        feedback.AddInitializationMethod(() =>
        {
            endText.SetActive(true);
        });

    }
}
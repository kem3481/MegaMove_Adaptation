using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using USE_States;
using Valve.VR;

public class ControlLevel_Trials : ControlLevel
{
    [System.NonSerialized]
    public GameObject positionsCorrect;

    public GameObject controllerPosition;
    public GameObject playerPosition;
    public GameObject createStimulus;

    private GameObject beginText;
    private GameObject endText;
    private Verify verifyPositions;
    private Controls controls;
    private int trials = 0;
    private int score = 0;

    // Vive Control GameObjects
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean squeezeAction;

    public override void DefineControlLevel()
    { 
        // Accessing other scripts and objects
        beginText = GameObject.FindGameObjectWithTag("beginText");
        endText = GameObject.FindGameObjectWithTag("endText");
        verifyPositions = GetComponent<Verify>();
        controls = GetComponent<Controls>();

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
        begin.SpecifyTermination(() => positionsCorrect != null && verifyPositions.positionsTimer > 2, stimOn);

        stimOn.AddInitializationMethod(() =>
        {
            createStimulus.SetActive(true);
            beginText.SetActive(false);
            controllerPosition.SetActive(false);
            trials++;

            Debug.Log("Overlap: " + controls.testobject);
            Debug.Log("Radius: " + controls.radius);
            Debug.Log("Angle: " + controls.angle);
        });
        stimOn.SpecifyTermination(() => createStimulus != null, collectResponse);

        collectResponse.AddInitializationMethod(() =>
        {
            bool GetSqueeze()
            {
                return squeezeAction.GetStateDown(handType);
            }

            if (GetSqueeze())
            {
                createStimulus.SetActive(false);
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

            Debug.Log("Trigger pull position: " + verifyPositions.rightController.transform.localPosition.x + ", " + verifyPositions.rightController.transform.localPosition.y);
        });
        collectResponse.SpecifyTermination(() => trials < 90, begin);
        collectResponse.SpecifyTermination(() => trials == 90, feedback);

        feedback.AddInitializationMethod(() =>
        {
            endText.SetActive(true);
        });

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using USE_States;
using Valve.VR;

public class ControlLevel_Trials : ControlLevel
{
    public GameObject controllerPosition; // Hand position column
    public GameObject playerPosition; // headset poisiton column
    public GameObject gamecontroller; // Set to which controller is being used
    public GameObject manager; // control levels game object, holding all scirpts
    public GameObject beginText; // instructions canvas
    public GameObject endText; // thank you for playing canvas

    private Verify verifyPositions; // script
    private Controls controls; // script
    private ControllerCheck controller; // scripts
    private HeadCheck head; // scripts
    private int trials = 0; // number of total trials
    private int score = 0; // player score
    private int test = 0; // response testing

    // Vive Control GameObjects
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean squeezeAction;

    public override void DefineControlLevel()
    { 
        // Defining States
        State begin = new State("Begin"); // Step 1 and 2 in Procedure
        State stimOn = new State("Stimulus"); // Step 3
        State collectResponse = new State("CollectResponse"); // Step 4
        State feedback = new State("Feedback"); // Step 5
        AddActiveStates(new List<State> { begin, stimOn, collectResponse, feedback });
        
        // Accessing other scripts
            verifyPositions = manager.GetComponent<Verify>();
            controls = manager.GetComponent<Controls>();
            controller = controllerPosition.GetComponent<ControllerCheck>();
            head = playerPosition.GetComponent<HeadCheck>();

        begin.AddStateInitializationMethod(() =>
        {
            controllerPosition.SetActive(true);
            beginText.SetActive(true);
            endText.SetActive(false);
            controls.testobject.SetActive(false);
            
        });
        begin.SpecifyStateTermination(() => verifyPositions.positionsCorrect, stimOn);
        begin.AddDefaultTerminationMethod(() => beginText.SetActive(false));

        stimOn.AddStateInitializationMethod(() =>
        {
            controls.testobject.SetActive(true);
            beginText.SetActive(false);
            controllerPosition.SetActive(false);
            trials++;
            
            // Debug.Log("Overlap: " + controls.testobject);
            // Debug.Log("Radius: " + controls.radius);
            // Debug.Log("Angle: " + controls.angle);
        });
        stimOn.SpecifyStateTermination(() => controls.testobject == null, collectResponse);

        collectResponse.AddStateInitializationMethod(() =>
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

        feedback.AddStateInitializationMethod(() =>
        {
            endText.SetActive(true);
        });

    }
}
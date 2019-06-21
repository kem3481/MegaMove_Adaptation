using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using USE_States;
using Valve.VR;
using System;
using System.IO;
using System.Text;


public class ControlLevel_Trials : ControlLevel
{
    public GameObject controllerPosition; // Hand position column
    public GameObject playerPosition; // headset poisiton column
    public GameObject gamecontroller; // Set to which controller is being used (icon)
    public GameObject test; // leftcontroller or right controller in heirarchy
    public GameObject target;
    public GameObject manager; // control levels game object, holding all scirpts
    public GameObject beginText; // instructions canvas
    public GameObject endText; // thank you for playing canvas
    public Text scoreDisplay;
    private GameObject penalty;
    public string startTime;
    public string endTime;
    public GameObject startingPositions;
    private GameObject headset;

    public int numberoftrials = 20;
    private Verify verifyPositions; // script
    private Controls controls; // script
    private ControllerCheck controller; // scripts
    private HeadCheck head; // scripts
    private TriggerPull triggered; // scripts
    public int trials = 0; // number of total trials
    private int score; // player score
    private int trialScore;
    private int end = 0;
    public bool data;
    private int j, i, k = 0;
    private int orientation;
    private int random;
    private float Timer;

    public int cases;
    public int[] trialTypes;

    private GameObject rightController;
    private GameObject leftController;

    [System.NonSerialized]
    public GameObject testobject;
    [System.NonSerialized]
    public float polarAngle, elevationAngle, a;
    [System.NonSerialized]
    public float radius, trigger_x, trigger_y, trigger_z, target_x, target_y, target_z, penalty_x, penalty_y, penalty_z;

    // Vive Control GameObjects
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean squeezeAction;

    public override void DefineControlLevel()
    { 
        // Defining States
        State begin = new State("Begin"); // Step 1 and 2 in Procedure
        State stimOn = new State("Stimulus"); // Step 3
        State collectResponse = new State("CollectResponse"); // Step 4
        State penaltyState = new State("TimeOutPenalty");
        State scoreState = new State("Score");
        State destination = new State("Destination"); // sends the script either back to begin or to feeback
        State feedback = new State("Feedback"); // Step 5
        AddActiveStates(new List<State> { begin, stimOn, collectResponse, penaltyState, scoreState, destination, feedback });
        
        // Accessing other scripts
        verifyPositions = manager.GetComponent<Verify>();
        controls = manager.GetComponent<Controls>();
        controller = controllerPosition.GetComponent<ControllerCheck>();
        head = playerPosition.GetComponent<HeadCheck>();
        triggered = test.GetComponent<TriggerPull>();
        rightController = GameObject.FindGameObjectWithTag("rightController");
        leftController = GameObject.FindGameObjectWithTag("leftController");
        headset = GameObject.FindGameObjectWithTag("Camera");

        scoreDisplay.text = "Score: " + score;

        begin.AddStateInitializationMethod(() =>
        {

            data = false;
            startTime = System.DateTime.UtcNow.ToString("HH:mm:ss");
            if (trials == 0)
            {
                score = 0;
            }

            trialScore = 0;
            controllerPosition.SetActive(true);
            if (trials < 1)
            {
                beginText.SetActive(true);
            }
            endText.SetActive(false);

            trigger_x = 0;
            trigger_y = 0;
            trigger_z = 0;
            target_x = 0;
            target_y = 0;
            target_z = 0;
            penalty_x = 0;
            penalty_y = 0;
            penalty_z = 0;
            
            if (gamecontroller == rightController)
            {
                leftController.SetActive(false);
            }
            if (gamecontroller == leftController)
            {
                rightController.SetActive(false);
            }

        });
        begin.SpecifyStateTermination(() => verifyPositions.positionsCorrect, stimOn);
        begin.AddDefaultTerminationMethod(() => beginText.SetActive(false));

        stimOn.AddStateInitializationMethod(() =>
        {
        beginText.SetActive(false);
        controllerPosition.SetActive(false);
        trials++;

        orientation = UnityEngine.Random.Range(0, 2);
        polarAngle = (Mathf.Deg2Rad * UnityEngine.Random.Range(0, 359));
        radius = .5f;
        a = (radius * Mathf.Cos(elevationAngle));
        random = controls.trialTypes[UnityEngine.Random.Range(0, controls.trialTypes.Length)];
        for (int i = 0; i < 9; i++)
        {
            if (random == i)
            {
                elevationAngle = controls.allAngles[i] * (Mathf.Deg2Rad);
                target = controls.allTargets[i];
            }
        }
            if (testobject == null)
        {
                testobject = Instantiate(target);
                testobject.transform.position = playerPosition.transform.position + new Vector3((a * Mathf.Cos(polarAngle)), (radius * Mathf.Sin(elevationAngle)), (a * Mathf.Sin(polarAngle)));
                if (orientation == 1)
                {
                    testobject.transform.eulerAngles = new Vector3(0f, -polarAngle * Mathf.Rad2Deg, 0f);
                }
                if (orientation == 0)
                {
                    testobject.transform.eulerAngles = new Vector3(0f,( -polarAngle * Mathf.Rad2Deg) + 180, 0f);
                }
            }

        penalty = GameObject.FindGameObjectWithTag("PenaltyonTarget");
  
            Debug.Log("Overlap: " + testobject);
            Debug.Log("Polar: " + polarAngle * Mathf.Rad2Deg);
            Debug.Log("Elevation: " + elevationAngle * Mathf.Rad2Deg);
            Debug.Log("Trial: " + trials);
        });
        stimOn.AddTimer(.1f, collectResponse);

        collectResponse.AddStateInitializationMethod(() =>
        {
            Timer = 0;
        });
        collectResponse.AddUpdateMethod(() =>
        {

            if (triggered.trigger == true)
            {
                trigger_x = gamecontroller.transform.position.x;
                trigger_y = gamecontroller.transform.position.y;
                trigger_z = gamecontroller.transform.position.z;
                target_x = testobject.transform.localPosition.x;
                target_y = testobject.transform.localPosition.y;
                target_z = testobject.transform.localPosition.z;
                penalty_x = penalty.transform.position.x;
                penalty_y = penalty.transform.position.y;
                penalty_z = penalty.transform.position.z;

                Destroy(testobject);
            }

            Timer += Time.deltaTime;
            
        });
        collectResponse.SpecifyStateTermination(() => testobject == null, scoreState);
        collectResponse.SpecifyStateTermination(() => Timer > 5f, penaltyState);

        penaltyState.AddStateInitializationMethod(() =>
        {
            Destroy(testobject);
            trialScore = -500;
        });
        penaltyState.AddTimer(.01f, destination);

        scoreState.AddStateInitializationMethod(() =>
        {
            if ((trigger_x > (target_x - .03f)) && (trigger_x < (target_x + .03f)) &&
               (trigger_y > (target_y - .03f)) && (trigger_y < (target_y + .03f)) &&
               (trigger_z > (target_z - .03f)) && (trigger_z < (target_z + .03f)))
            {
                trialScore = 100;
            }
            else
            {
                trialScore = 0;
            }

            if ((trigger_x > (penalty_x - .03f)) && (trigger_x < (penalty_x + .03f)) &&
               (trigger_y > (penalty_y - .03f)) && (trigger_y < (penalty_y + .03f)) &&
               (trigger_z > (penalty_z - .03f)) && (trigger_z < (penalty_z + .03f)))
            {
                trialScore = -100;
            }

            
        });
        scoreState.AddTimer(1f, destination);

        destination.AddStateInitializationMethod(() =>
        {
            score = score + trialScore;
            scoreDisplay.text = "Score: " + score;

            Debug.Log("Trigger pull position: " + trigger_x + ", " + trigger_y + ", " + trigger_z);
            Debug.Log("Target position: " + target_x + ", " + target_y + ", " + target_z);
            Debug.Log("Score: " + score);

            if (trials < numberoftrials)
            {
                end = 1;
            }
            else
            {
                end = 2;
            }
        });
        destination.SpecifyStateTermination(() => end == 1, begin);
        destination.SpecifyStateTermination(() => end == 2, feedback);

        feedback.AddStateInitializationMethod(() =>
        {
            endText.SetActive(true);
            data = true;
        });

    }

}

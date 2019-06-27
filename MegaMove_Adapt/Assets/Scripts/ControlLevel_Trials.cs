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
    // Canvases and Text objects
    public GameObject beginText; // instructions canvas
    public GameObject endText; // thank you for playing canvas
    public Text scoreDisplay;
    
    // Other script declarations
    private Verify verifyPositions;
    private Controls controls; 
    private ControllerCheck controller; 
    private HeadCheck head; 
    private TriggerPull triggered; 
    
    // Data Writeout objects
    public string startTime;
    public string endTime;
    public int trials = 0; 
    private int score; 
    [System.NonSerialized]
    public float polarAngle, elevationAngle, a;
    [System.NonSerialized]
    public float radius, trigger_x, trigger_y, trigger_z, target_x, target_y, target_z, penalty_x, penalty_y, penalty_z;
    
    // Placeholders for loops and accessing members lists
    private int j, i, k = 0;
    private int orientation;
    private int random1, random2; 
    public int cases;
    
    // Intermediate objects
    private float Timer;
    private int trialScore;
    private int end = 0;
    public bool data;
    public int numberoftrials = 20;
    
    // Physical objects
    private GameObject rightController;
    private GameObject leftController;
    private GameObject headset;
    
    // Empty Game objects
    private GameObject trigger;
    public GameObject manager; // control levels game object, holding all scirpts
    
    // Unity Space objects
    public GameObject controllerPosition; // Hand position column
    public GameObject playerPosition; // headset poisiton column
    public GameObject gamecontroller; // Set to which controller is being used (icon)
    public GameObject test; // leftcontroller or right controller in heirarchy
    public GameObject target;
    private GameObject penalty;
    private GameObject targetonObject;
    public GameObject startingPositions;

    public int Fix;

    [System.NonSerialized]
    public GameObject testobject;
    
    // List of 20 of each trial type
    public int[] trialTypes;
    
    public override void DefineControlLevel()
    {
        // Defining States
        State calib = new State("Calibration");
        State begin = new State("Begin"); // Step 1 and 2 in Procedure
        State stimOn = new State("Stimulus"); // Step 3
        State collectResponse = new State("CollectResponse"); // Step 4
        State penaltyState = new State("TimeOutPenalty");
        State scoreState = new State("Score");
        State destination = new State("Destination"); // sends the script either back to begin or to feeback
        State feedback = new State("Feedback"); // Step 5
        AddActiveStates(new List<State> { calib, begin, stimOn, collectResponse, penaltyState, scoreState, destination, feedback });
        
        // Accessing other scripts
        verifyPositions = manager.GetComponent<Verify>();
        controls = manager.GetComponent<Controls>();
        controller = controllerPosition.GetComponent<ControllerCheck>();
        head = playerPosition.GetComponent<HeadCheck>();
        triggered = gamecontroller.GetComponent<TriggerPull>();
        rightController = GameObject.FindGameObjectWithTag("rightController");
        leftController = GameObject.FindGameObjectWithTag("leftController");
        headset = GameObject.FindGameObjectWithTag("Camera");


        trigger = GameObject.FindGameObjectWithTag("Trigger");

        trialTypes = controls.trialTypes;
        scoreDisplay.text = "Score: " + score;

        calib.AddStateInitializationMethod(() =>
        {
            beginText.SetActive(false);
            endText.SetActive(false);
            controllerPosition.SetActive(false);
            playerPosition.SetActive(false);
            scoreDisplay.text = string.Empty;

            Fix = UnityEngine.Random.Range(0, 1);
            if (Fix == 0)
            {
                Debug.Log("Start in Fixation");
            }
            if (Fix == 1)
            {
                Debug.Log("Start with Free Motion");
            }
        });
        calib.SpecifyStateTermination(() => Input.GetKeyDown("space"), begin);


        begin.AddStateInitializationMethod(() =>
        {
            trigger.SetActive(false);
            data = false;
            startTime = System.DateTime.UtcNow.ToString("HH:mm:ss");
            if (trials == 0)
            {
                score = 0;
            }

            trialScore = 0;
            controllerPosition.SetActive(true);
            playerPosition.SetActive(true);
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
        
        random2 = UnityEngine.Random.Range(0, controls.trialTypes.Length);
        random1 = controls.trialTypes[random2];
        for (int i = 0; i < 9; i++)
        {
            if (random1 == i)
            {
                elevationAngle = controls.allAngles[i] * (Mathf.Deg2Rad);
                target = controls.allTargets[i];
            }
        }
        
        /*for (int i = 0; i < random2; i++)
        {
            trialTypes[i] = trialTypes[i];
        }*/
        for (int i = random2; i < controls.trialTypes.Length - 1; i++)
        {
            controls.trialTypes[i] = controls.trialTypes[i+1];
        }
        
        Array.Resize(ref controls.trialTypes, controls.trialTypes.Length - 1);
        
        if (testobject == null)
        {
                
                testobject = Instantiate(target);
               
                targetonObject = GameObject.FindGameObjectWithTag("Object");
                penalty = GameObject.FindGameObjectWithTag("PenaltyonTarget");
                testobject.transform.position = playerPosition.transform.position + new Vector3((a * Mathf.Cos(polarAngle)), (radius * Mathf.Sin(elevationAngle)), (a * Mathf.Sin(polarAngle)));
                if (orientation == 1)
                {
                    testobject.transform.eulerAngles = new Vector3(0f, -polarAngle * Mathf.Rad2Deg + 90, 0f);
                }
                if (orientation == 0)
                {
                    testobject.transform.eulerAngles = new Vector3(0f,( -polarAngle * Mathf.Rad2Deg) + 270, 0f);
                }

                target_x = targetonObject.transform.position.x;
                target_y = targetonObject.transform.position.y;
                target_z = targetonObject.transform.position.z;
                penalty_x = penalty.transform.position.x;
                penalty_y = penalty.transform.position.y;
                penalty_z = penalty.transform.position.z;
            }
        
  
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

            
            if (triggered.passedRadius == true)
            {
                trigger_x = test.transform.position.x;
                trigger_y = test.transform.position.y;
                trigger_z = test.transform.position.z;
                Destroy(testobject);
            }



            if (trigger.activeSelf == true)
            {
                trigger_x = test.transform.position.x;
                trigger_y = test.transform.position.y;
                trigger_z = test.transform.position.z;
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
            if (triggered.targetTouched == true)
            {
                trialScore = 100;
                triggered.targetTouched = false;
            }
            else
            {
                trialScore = 0;
            }

            if (triggered.penaltyTouched == true)
            {
                trialScore = -100;
                triggered.penaltyTouched = false;
            }

            trigger.SetActive(false);
            data = true;
        });
        scoreState.AddTimer(1f, destination);

        destination.AddStateInitializationMethod(() =>
        {
            score = score + trialScore;
            
            scoreDisplay.text = "Score: " + score;
            data = false;
            Debug.Log("Trigger pull position: " + trigger_x + ", " + trigger_y + ", " + trigger_z);
            Debug.Log("Target position: " + target_x + ", " + target_y + ", " + target_z);
            Debug.Log("Score: " + score);

            if (trials < 180)
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
            
        });

    }

}

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
    public GameObject gamecontroller; // Set to which controller is being used (icon)
    public GameObject test; // leftcontroller or right controller in heirarchy
    public GameObject manager; // control levels game object, holding all scirpts
    public GameObject beginText; // instructions canvas
    public GameObject endText; // thank you for playing canvas
    public Text scoreDisplay;
    private GameObject penalty;

    private Verify verifyPositions; // script
    private Controls controls; // script
    private ControllerCheck controller; // scripts
    private HeadCheck head; // scripts
    private TriggerPull triggered; // scripts
    private PointSystem points; // scripts
    private int trials = 0; // number of total trials
    private int score; // player score
    private int trialScore;
    private int end = 0;

    [System.NonSerialized]
    public GameObject testobject;
    [System.NonSerialized]
    public float angle;
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
        State scoreState = new State("Score");
        State destination = new State("Destination"); // sends the script either back to begin or to feeback
        State feedback = new State("Feedback"); // Step 5
        AddActiveStates(new List<State> { begin, stimOn, collectResponse, scoreState, destination, feedback });
        
        // Accessing other scripts
        verifyPositions = manager.GetComponent<Verify>();
        controls = manager.GetComponent<Controls>();
        controller = controllerPosition.GetComponent<ControllerCheck>();
        head = playerPosition.GetComponent<HeadCheck>();
        triggered = test.GetComponent<TriggerPull>();
        points = manager.GetComponent<PointSystem>();

        scoreDisplay.text = "Score: " + score;

        begin.AddStateInitializationMethod(() =>
        {
            if (trials == 0)
            {
                score = 0;
            }

            trialScore = 0;
            controllerPosition.SetActive(true);
            beginText.SetActive(true);
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
            
        });
        begin.SpecifyStateTermination(() => verifyPositions.positionsCorrect, stimOn);
        begin.AddDefaultTerminationMethod(() => beginText.SetActive(false));

        stimOn.AddStateInitializationMethod(() =>
        {
            beginText.SetActive(false);
            controllerPosition.SetActive(false);
            trials++;

            angle = Random.RandomRange(0, 359);
            radius = controls.radii[UnityEngine.Random.Range(0, 2)];
            if (testobject == null)
            {
                testobject = Instantiate(controls.targets[UnityEngine.Random.Range(0, 2)]);
                testobject.transform.position = new Vector3((radius * Mathf.Cos(angle * (Mathf.PI / 180))), (radius * Mathf.Sin(angle * (Mathf.PI / 180))) + 2.75f, 2.5f);
                testobject.transform.eulerAngles = new Vector3(0f, 0f, angle);
            }

            penalty = GameObject.FindGameObjectWithTag("PenaltyonTarget");

            Debug.Log("Overlap: " + testobject);
            Debug.Log("Radius: " + radius);
            Debug.Log("Angle: " + angle);
            Debug.Log("Trial: " + trials);
        });
        stimOn.AddTimer(.1f, collectResponse);
        
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
            
        });
        collectResponse.SpecifyStateTermination(() => testobject == null, scoreState);

        scoreState.AddStateInitializationMethod(() =>
        {
            if ((trigger_x > (target_x - .1f)) && (trigger_x < (target_x + .1f)) &&
               (trigger_y > (target_y - .1f)) && (trigger_y < (target_y + .1f)) &&
               (trigger_z > (target_z - .1f)) && (trigger_z < (target_z + .1f)))
            {
                trialScore = 100;
            }
            else
            {
                trialScore = 0;
            }

            if ((trigger_x > (penalty_x - .1f)) && (trigger_x < (penalty_x + .1f)) &&
               (trigger_y > (penalty_y - .1f)) && (trigger_y < (penalty_y + .1f)) &&
               (trigger_z > (penalty_z - .1f)) && (trigger_z < (penalty_z + .1f)))
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

            if (trials < 90)
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
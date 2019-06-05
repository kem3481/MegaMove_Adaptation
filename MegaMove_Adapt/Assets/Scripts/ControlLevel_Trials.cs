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

    private Verify verifyPositions; // script
    private Controls controls; // script
    private ControllerCheck controller; // scripts
    private HeadCheck head; // scripts
    private TriggerPull triggered; // scripts
    private int trials = 0; // number of total trials
    private int score = 0; // player score
    private int end = 0;

    [System.NonSerialized]
    public GameObject testobject;
    [System.NonSerialized]
    public float angle;
    [System.NonSerialized]
    public float radius;

    // Vive Control GameObjects
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean squeezeAction;

    public override void DefineControlLevel()
    { 
        // Defining States
        State begin = new State("Begin"); // Step 1 and 2 in Procedure
        State stimOn = new State("Stimulus"); // Step 3
        State collectResponse = new State("CollectResponse"); // Step 4
        State destination = new State("Destination"); // sends the script either back to begin or to feeback
        State feedback = new State("Feedback"); // Step 5
        AddActiveStates(new List<State> { begin, stimOn, collectResponse, destination, feedback });
        
        // Accessing other scripts
        verifyPositions = manager.GetComponent<Verify>();
        controls = manager.GetComponent<Controls>();
        controller = controllerPosition.GetComponent<ControllerCheck>();
        head = playerPosition.GetComponent<HeadCheck>();
        triggered = test.GetComponent<TriggerPull>();

        begin.AddStateInitializationMethod(() =>
        {
            controllerPosition.SetActive(true);
            beginText.SetActive(true);
            endText.SetActive(false);
            
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
                testobject.transform.position = new Vector3((radius * Mathf.Cos(angle)), (radius * Mathf.Sin(angle)) + 2.75f, 2.5f);
                testobject.transform.eulerAngles = new Vector3(0f, 0f, angle*(Mathf.PI/180));
            }

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
                Destroy(testobject);
                Debug.Log("Trigger pull position: " + test.transform.localPosition.x + ", " + test.transform.localPosition.y);
                Debug.Log("Target position: " + testobject.transform.localPosition.x + ", " + testobject.transform.localPosition.y);
            }
        });
        collectResponse.SpecifyStateTermination(() => testobject == null, destination);


        destination.AddStateInitializationMethod(() =>
        {
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
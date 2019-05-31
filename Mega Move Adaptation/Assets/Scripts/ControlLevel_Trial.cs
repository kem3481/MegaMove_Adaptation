using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using USE_States;

public class ControlLevel_Trial : ControlLevel
{
    public GameObject beginText;
    public GameObject smallOverlap;
    public GameObject mediumOverlap;
    public GameObject largeOverlap;

    private int smallRadius = 10;
    private int mediumRadius = 20;
    private int largeRadius = 30;
    private int radius;

    private GameObject[] targets;
    private GameObject[] radii;
    private GameObject testobject;

    private VerifyPositions verifyPositions;

    public override void DefineControlLevel()
    {
        // Defining States
        State positionReady = new State("Positions are correct");
        State stimOn = new State("Stimulus");
        AddActiveStates(new List<State> { positionReady, stimOn });

        // Accessing other scripts
        ControlLevel_Trial end = transform.GetComponent<ControlLevel_Trial>();
        positionReady.AddChildLevel(end);

        verifyPositions = GetComponent<VerifyPositions>();

        // Making a list of target overlap options
        targets = new GameObject[3];
        targets[0] = smallOverlap;
        targets[1] = mediumOverlap;
        targets[2] = largeOverlap;

        // List of radii
        radii = new GameObject[3];
        radii[0] = smallRadius;
        radii[1] = mediumRadius;
        radii[2] = largeRadius;
        radius = radii[UnityEngine.Random.Range(0, 2)];

        positionReady.AddInitializationMethod(() =>
        {
            beginText.SetActive(true);
            Debug.Log("Setting Positions");

            if (verifyPositions.positionCorrect !=null)
            {
                beginText.SetActive(false);
                Debug.Log("Positions are correct");
            }
        });
        positionReady.SpecifyTermination(() => end.Terminated == true, stimOn);

        stimOn.AddInitializationMethod(() =>
        {
            testobject = Instantiate(targets[UnityEngine.Random.Range(0, 2)]);
            testobject.transform.position = new Vector3( radius*Mathf.Cos(testobject.transform.rotation.z), radius*Mathf.Sin(testobject.transform.rotation.z), 0f);
            testobject.transform.rotation = new Vector3(0f, 0f, UnityEngine.Random.Range(0, 359));
        });
    }
}

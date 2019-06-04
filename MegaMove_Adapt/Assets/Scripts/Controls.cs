using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    private int smallRadius = 1;
    private int mediumRadius = 2;
    private int largeRadius = 3;
    
    public GameObject smallOverlap;
    public GameObject mediumOverlap;
    public GameObject largeOverlap;

    private GameObject[] targets;
    private int[] radii;
    [System.NonSerialized]
    public GameObject testobject;
    [System.NonSerialized]
    public int radius, angle;

    // Start is called before the first frame update
    void Start()
    {
        // Presentation Angle
        angle = Random.RandomRange(0, 359);

        // Making a list of target overlap options
        targets = new GameObject[3];
        targets[0] = smallOverlap;
        targets[1] = mediumOverlap;
        targets[2] = largeOverlap;

        // List of radii
        radii = new int[3];
        radii[0] = smallRadius;
        radii[1] = mediumRadius;
        radii[2] = largeRadius;
        radius = radii[UnityEngine.Random.Range(0, 2)];

        //Setting Test Object
        testobject = Instantiate(targets[UnityEngine.Random.Range(0, 2)]);
        testobject.transform.position = new Vector3(radius * Mathf.Cos(testobject.transform.rotation.z), radius * Mathf.Sin(testobject.transform.rotation.z), 0f);
        testobject.transform.eulerAngles = new Vector3(0f, 0f, angle);

        testobject.SetActive(false);
    }
}

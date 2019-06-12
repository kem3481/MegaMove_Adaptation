using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    private float small = 75f;
    private float medium = 60f;
    private float large =  45f;
    
    public GameObject smallOverlap;
    public GameObject mediumOverlap;
    public GameObject largeOverlap;

    public GameObject[] targets;
    public float[] angles;
    
    // Start is called before the first frame update
    void Start()
    {
        // Making a list of target overlap options
        targets = new GameObject[3];
        targets[0] = smallOverlap;
        targets[1] = mediumOverlap;
        targets[2] = largeOverlap;

        // List of radii
        angles = new float[3];
        angles[0] = small;
        angles[1] = medium;
        angles[2] = large;
      
    }
    
}

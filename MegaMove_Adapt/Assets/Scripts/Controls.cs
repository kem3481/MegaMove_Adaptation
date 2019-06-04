using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    private float smallRadius = .5f;
    private float mediumRadius = 1f;
    private float largeRadius = 1.5f;
    
    public GameObject smallOverlap;
    public GameObject mediumOverlap;
    public GameObject largeOverlap;

    public GameObject[] targets;
    public float[] radii;
    
    // Start is called before the first frame update
    void Start()
    {
        // Making a list of target overlap options
        targets = new GameObject[3];
        targets[0] = smallOverlap;
        targets[1] = mediumOverlap;
        targets[2] = largeOverlap;

        // List of radii
        radii = new float[3];
        radii[0] = smallRadius;
        radii[1] = mediumRadius;
        radii[2] = largeRadius;
      
    }
    
}

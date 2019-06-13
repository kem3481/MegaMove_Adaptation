﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    private float small = 85f;
    private float medium = 80f;
    private float large =  75f;
    
    public GameObject smallOverlap;
    public GameObject mediumOverlap;
    public GameObject largeOverlap;

    public GameObject[] targets;
    public GameObject[] allTargets;
    public float[] angles;
    public float[] allAngles;
    private int i, j, k;

    private ControlLevel_Trials controls;

    public int[] trialTypes;

    // Start is called before the first frame update
    void Start()
    {
        k = 0;

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

        // List of all angles and targets corresponding to form every case
        allAngles = new float[9];
        allAngles[0] = small;
        allAngles[1] = medium;
        allAngles[2] = large;
        allAngles[3] = small;
        allAngles[4] = medium;
        allAngles[5] = large;
        allAngles[6] = small;
        allAngles[7] = medium;
        allAngles[8] = large;

        allTargets = new GameObject[9];
        allTargets[0] = smallOverlap;
        allTargets[1] = smallOverlap;
        allTargets[2] = smallOverlap;
        allTargets[3] = mediumOverlap;
        allTargets[4] = mediumOverlap;
        allTargets[5] = mediumOverlap;
        allTargets[6] = largeOverlap;
        allTargets[7] = largeOverlap;
        allTargets[8] = largeOverlap;

        trialTypes = new int[180];

        for (int j = 0; j < 9; j ++)
        {
            for (int i = 0; i < 20; i++)
            {
                trialTypes[i + j*19] = j;
            }
        }
        
    }
    
}

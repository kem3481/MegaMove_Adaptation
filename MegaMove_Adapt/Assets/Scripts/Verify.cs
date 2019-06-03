using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.UI;

public class Verify : MonoBehaviour
{
    [System.NonSerialized]
    public GameObject rightController, head;
    
    public GameObject handStart;
    public GameObject headStart;
    public GameObject handPosition;
    public GameObject headPosition;
    public GameObject positionsCorrect;

    [System.NonSerialized]
    public float positionsTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        rightController = GameObject.FindGameObjectWithTag("rightController");
        head = GameObject.FindGameObjectWithTag("Camera");

        if (rightController == null)
        {
            Debug.Log("No right controller found");
        }

        if (head == null)
        {
            Debug.Log("no head found");
        }
    }

    // Update is called once per frame
    public void Update()
    {
        // If the controller is within the sphere that designates beginning hand position and the unlit hand prefab exists, destroy the unlit hand prefab and create lit hand prefab
        if (handPosition == null &&
            rightController.transform.localPosition.x > handStart.transform.localPosition.x - .05f &&
            rightController.transform.localPosition.x < handStart.transform.localPosition.x + .05f &&
            rightController.transform.localPosition.y > handStart.transform.localPosition.y - .05f &&
            rightController.transform.localPosition.y < handStart.transform.localPosition.y + .05 &&
            rightController.transform.localPosition.z > handStart.transform.localPosition.z - .05 &&
            rightController.transform.localPosition.z < handStart.transform.localPosition.z + .05)
        {
            Instantiate(handPosition);
        }

        // If the controller is not within the start sphere and the lithand prefab exists, destory the lit hand prefab and create the unlit hand prefab
        if (handPosition != null &&
           (rightController.transform.localPosition.x < handStart.transform.localPosition.x - .05f ||
           rightController.transform.localPosition.x > handStart.transform.localPosition.x + .05f ||
           rightController.transform.localPosition.y < handStart.transform.localPosition.y - .05f ||
           rightController.transform.localPosition.y > handStart.transform.localPosition.y + .05f ||
           rightController.transform.localPosition.z < handStart.transform.localPosition.z - .05f ||
           rightController.transform.localPosition.z > handStart.transform.localPosition.z + .05f))
        {
            Destroy(handPosition);
        }

        // If the headset is within the start column and the lithead prefab does not exist, destory the unlit head prefab and create the lit head prefab
        if (headPosition == null &&
           head.transform.localPosition.x > headStart.transform.localPosition.x - .05f &&
           head.transform.localPosition.x < headStart.transform.localPosition.x + .05f &&
           head.transform.localPosition.y > headStart.transform.localPosition.y - .05f &&
           head.transform.localPosition.y < headStart.transform.localPosition.y + .05f &&
           head.transform.localPosition.z > headStart.transform.localPosition.z - .05f &&
           head.transform.localPosition.z < headStart.transform.localPosition.z + .05f)
        {
            Instantiate(headPosition);
        }

        // If the headset is not within the start column and the lithead prefab does exist, destory the lit head prefab and create the unlit head prefab
        if (headPosition != null &&
           (head.transform.localPosition.x < headStart.transform.localPosition.x - .05f ||
           head.transform.localPosition.x > headStart.transform.localPosition.x + .05f ||
           head.transform.localPosition.y < headStart.transform.localPosition.y - .05f ||
           head.transform.localPosition.y > headStart.transform.localPosition.y + .05f ||
           head.transform.localPosition.z < headStart.transform.localPosition.z - .05f ||
           head.transform.localPosition.z > headStart.transform.localPosition.z + .05f))
        {
            Destroy(headPosition);
        }

        if (headPosition != null && handPosition != null)
        {
            Instantiate(positionsCorrect);
        }

        while (positionsCorrect != null)
        {
            positionsTimer = positionsTimer + Time.deltaTime;
        }
    }

}
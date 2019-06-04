using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.UI;

public class Verify : MonoBehaviour
{
    [System.NonSerialized]
     public GameObject rightController, leftController, head;

     public GameObject handStart;
     public GameObject headStart;

     private bool handPosition;
     private bool headPosition;
     public bool positionsCorrect;

     [System.NonSerialized]
     public float positionsTimer = 0;

     // Start is called before the first frame update
     public void Start()
     {
         rightController = GameObject.FindGameObjectWithTag("rightController");
         leftController = GameObject.FindGameObjectWithTag("leftController");
         head = GameObject.FindGameObjectWithTag("Camera");

         handPosition = false;
         headPosition = false;
         positionsCorrect = false;
     }

     // Update is called once per frame
     public void Update()
     {
         // If the controller is within the sphere that designates beginning hand position and the unlit hand prefab exists, destroy the unlit hand prefab and create lit hand prefab

         if (handPosition == false &&
             rightController.transform.localPosition.x > handStart.transform.position.x - .05f &&
             rightController.transform.localPosition.x < handStart.transform.position.x + .05f &&
             rightController.transform.localPosition.y > handStart.transform.position.y - .05f &&
             rightController.transform.localPosition.y < handStart.transform.position.y + .05 &&
             rightController.transform.localPosition.z > handStart.transform.position.z - .05 &&
             rightController.transform.localPosition.z < handStart.transform.position.z + .05)
         {
             handPosition = true;
         }

        if (handPosition == false &&
            leftController.transform.localPosition.x > handStart.transform.position.x - .05f &&
            leftController.transform.localPosition.x < handStart.transform.position.x + .05f &&
            leftController.transform.localPosition.y > handStart.transform.position.y - .05f &&
            leftController.transform.localPosition.y < handStart.transform.position.y + .05 &&
            leftController.transform.localPosition.z > handStart.transform.position.z - .05 &&
            leftController.transform.localPosition.z < handStart.transform.position.z + .05)
        {
            handPosition = true;
        }

        // If the controller is not within the start sphere and the lithand prefab exists, destory the lit hand prefab and create the unlit hand prefab
        if (handPosition == true &&
            (rightController.transform.localPosition.x < handStart.transform.position.x - .05f ||
            rightController.transform.localPosition.x > handStart.transform.position.x + .05f ||
            rightController.transform.localPosition.y < handStart.transform.position.y - .05f ||
            rightController.transform.localPosition.y > handStart.transform.position.y + .05f ||
            rightController.transform.localPosition.z < handStart.transform.position.z - .05f ||
            rightController.transform.localPosition.z > handStart.transform.position.z + .05f))
         {
             handPosition = false;
         }

        if (handPosition == true &&
            (leftController.transform.localPosition.x < handStart.transform.position.x - .05f ||
            leftController.transform.localPosition.x > handStart.transform.position.x + .05f ||
            leftController.transform.localPosition.y < handStart.transform.position.y - .05f ||
            leftController.transform.localPosition.y > handStart.transform.position.y + .05f ||
            leftController.transform.localPosition.z < handStart.transform.position.z - .05f ||
            leftController.transform.localPosition.z > handStart.transform.position.z + .05f))
        {
            handPosition = false;
        }

        // If the headset is within the start column and the lithead prefab does not exist, destory the unlit head prefab and create the lit head prefab
        if (headPosition == false &&
            head.transform.localPosition.x > headStart.transform.position.x - .05f &&
            head.transform.localPosition.x < headStart.transform.position.x + .05f &&
            head.transform.localPosition.y > headStart.transform.position.y - .05f &&
            head.transform.localPosition.y < headStart.transform.position.y + .05f &&
            head.transform.localPosition.z > headStart.transform.position.z - .05f &&
            head.transform.localPosition.z < headStart.transform.position.z + .05f)
         {
             headPosition = true;
         }


         // If the headset is not within the start column and the lithead prefab does exist, destory the lit head prefab and create the unlit head prefab
         if (headPosition == true &&
            (head.transform.localPosition.x < headStart.transform.position.x - .05f ||
            head.transform.localPosition.x > headStart.transform.position.x + .05f ||
            head.transform.localPosition.y < headStart.transform.position.y - .05f ||
            head.transform.localPosition.y > headStart.transform.position.y + .05f ||
            head.transform.localPosition.z < headStart.transform.position.z - .05f ||
            head.transform.localPosition.z > headStart.transform.position.z + .05f))
         {
             headPosition = false;
         }

         if (headPosition == true && handPosition == true)
         {
             positionsCorrect = true;
             Debug.Log("Positions are correct");
         }

         while (positionsCorrect == true)
         {
             positionsTimer = positionsTimer + Time.deltaTime;
         }
     }

}
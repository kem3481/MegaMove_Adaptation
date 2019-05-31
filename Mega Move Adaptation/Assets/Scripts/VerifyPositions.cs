using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using USE_States;

public class VerifyPositions : ControlLevel
{
    // Position is correct game object
    public GameObject positionCorrect;

    // Physical Objects
    private GameObject rightController;
    private GameObject head;

    // Starting Positions
    private GameObject handSphere;
    private GameObject headColumn;

    //  Verification Lights
    private GameObject unlitHead;
    private GameObject litHead;
    private GameObject unlitHand;
    private GameObject litHand;

    // Light prefabs
    public GameObject LightPrefab_unlit;
    public GameObject LightPrefab_lit;


    public override void DefineControlLevel()
    {
        State verify = new State("Verified");
        State wait = new State("Waiting");
        State end = new State("Proceeding");
        AddActiveStates(new List<State> { verify, wait, end });

        verify.AddInitializationMethod(() =>
       {
           // Initially the position is not correct
           positionCorrect.SetActive(false);

           // Adding physical objects to game objects
           rightController = GameObject.FindGameObjectWithTag("RightController");
           head = GameObject.FindGameObjectWithTag("Camera");

           // Creating unlit position verification lights
           unlitHand = Instantiate(LightPrefab_unlit);
           unlitHand.transform.position = new Vector3(0f, 0f, .5f);

           unlitHead = Instantiate(LightPrefab_unlit);
           unlitHead.transform.position = new Vector3(0f, 0f, -.5f);

           // If the controller is within the sphere that designates beginning hand position and the unlit hand prefab exists, destroy the unlit hand prefab and create lit hand prefab
           if (unlitHand != null &&
               rightController.transform.localPosition.x > .20 &&
               rightController.transform.localPosition.x < .30 &&
               rightController.transform.localPosition.y > 1.1 &&
               rightController.transform.localPosition.y < 1.2 &&
               rightController.transform.localPosition.z > -.3 &&
               rightController.transform.localPosition.z < -.2)
           {
               Destroy(unlitHand);
               litHand = Instantiate(LightPrefab_lit);
               litHand.transform.position = new Vector3(0f, 0f, .5f);
           }

           // If the controller is not within the start sphere and the lithand prefab exists, destory the lit hand prefab and create the unlit hand prefab
           if (litHand != null &&
              (rightController.transform.localPosition.x < .20 ||
              rightController.transform.localPosition.x > .30 ||
              rightController.transform.localPosition.y < 1.1 ||
              rightController.transform.localPosition.y > 1.2 ||
              rightController.transform.localPosition.z < -.3 ||
              rightController.transform.localPosition.z > -.2))
           {
               Destroy(litHand);
               unlitHand = Instantiate(LightPrefab_unlit);
               unlitHand.transform.position = new Vector3(0f, 0f, .5f);
           }

           // If the headset is within the start column and the lithead prefab does not exist, destory the unlit head prefab and create the lit head prefab
           if (unlitHead != null &&
              head.transform.localPosition.x > -.05 &&
              head.transform.localPosition.x < .05 &&
              head.transform.localPosition.y > 1.2 &&
              head.transform.localPosition.y < 1.6 &&
              head.transform.localPosition.z > -.55 &&
              head.transform.localPosition.z < -.45)
           {
               Destroy(unlitHead);
               litHead = Instantiate(LightPrefab_lit);
               litHead.transform.position = new Vector3(0f, 0f, -.5f);
           }

           // If the headset is not within the start column and the lithead prefab does exist, destory the lit head prefab and create the unlit head prefab
           if (litHead != null &&
              (head.transform.localPosition.x < -.05 ||
              head.transform.localPosition.x > .05 ||
              head.transform.localPosition.y < 1.2 ||
              head.transform.localPosition.y > 1.6 ||
              head.transform.localPosition.z < -.55 ||
              head.transform.localPosition.z > -.45))
           {
               Destroy(litHead);
               unlitHead = Instantiate(LightPrefab_unlit);
               unlitHead.transform.position = new Vector3(0f, 0f, -.5f);
           }

           // If both are in correct position, set the position correct gameobject to true
           if (litHead != null && litHand != null)
           {
               positionCorrect.SetActive(true);
           }
           
       });
        verify.SpecifyTermination(() => positionCorrect != null, wait);
        
        wait.AddTimer(3f, end);

        end.AddInitializationMethod(() =>
        {
            this.AddTerminationSpecification(() => positionCorrect != null);
        });
    }
}

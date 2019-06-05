using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSystem : MonoBehaviour
{
    public bool withinTarget;
    public bool withinPenalty;

    public GameObject manager;
    
    private Controls controls;
    private ControlLevel_Trials controlLevels;

    private double difference = 0;

    // Start is called before the first frame update
    void Start()
    {
        withinTarget = false;
        withinPenalty = false;

        controls = manager.GetComponent<Controls>();
        controlLevels = manager.GetComponent<ControlLevel_Trials>();
    }

    // Update is called once per frame
    void Update()
    { if (controlLevels.testobject != null)
        {
            if ((controlLevels.trigger_x > controlLevels.target_x + .1 &&
                controlLevels.trigger_x < controlLevels.target_x - .1 &&
                controlLevels.trigger_y > controlLevels.target_y + .1 &&
                controlLevels.trigger_y < controlLevels.target_y - .1 &&
                controlLevels.trigger_z > controlLevels.target_z + .1 &&
                controlLevels.trigger_z < controlLevels.target_z - .1) &&
                !(controlLevels.trigger_x > controlLevels.target_x + difference + .1 &&
                controlLevels.trigger_x < controlLevels.target_x + difference - .1 &&
                controlLevels.trigger_y > controlLevels.target_y + difference + .1 &&
                controlLevels.trigger_y < controlLevels.target_y + difference - .1 &&
                controlLevels.trigger_z > controlLevels.target_z + difference + .1 &&
                controlLevels.trigger_z < controlLevels.target_z + difference - .1))
            {
                withinTarget = true;
                Debug.Log("within taret");
            }

            if (controls.targets[2] == controlLevels.testobject)
            {
                difference = .15;
            }

            if (controls.targets[1] == controlLevels.testobject)
            {
                difference = ((10 / 3) * .05);
            }

            if (controls.targets[0] == controlLevels.testobject)
            {
                difference = ((11 / 3) * .05);
            }

            if (controlLevels.trigger_x > controlLevels.target_x + difference + .1 &&
                controlLevels.trigger_x < controlLevels.target_x + difference - .1 &&
                controlLevels.trigger_y > controlLevels.target_y + difference + .1 &&
                controlLevels.trigger_y < controlLevels.target_y + difference - .1 &&
                controlLevels.trigger_z + 2.5 > controlLevels.target_z + difference + .1 &&
                controlLevels.trigger_z + 2.5 < controlLevels.target_z + difference - .1)
            {
                withinPenalty = true;
                Debug.Log("within penalty");
            }
        }
    }
}

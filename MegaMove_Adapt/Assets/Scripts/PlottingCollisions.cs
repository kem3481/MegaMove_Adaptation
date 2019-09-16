using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlottingCollisions : MonoBehaviour
{
    private ControlLevel_Trials controlLevels;
    private Vector3 relativePositions;
    public GameObject collision;
    private GameObject Collision;
    public GameObject MANAGER;

    private void Start()
    {
        controlLevels = MANAGER.GetComponent<ControlLevel_Trials>();
    }

    void Update()
    {
        if (controlLevels.data == true)
        {
            relativePositions = new Vector3(controlLevels.trigger_x, controlLevels.trigger_y, controlLevels.trigger_z);
            Plotting();
        }
    }

    void Plotting()
    {
        Collision = Instantiate(collision);
        Collision.transform.position = relativePositions;
    }
}

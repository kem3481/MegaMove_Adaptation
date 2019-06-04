using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TriggerPull : MonoBehaviour
{
    public bool trigger;

    // Vive Control GameObjects
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean squeezeAction;

    // Update is called once per frame
    void Update()
    {
        bool GetSqueeze()
        {
            return squeezeAction.GetStateDown(handType);
        }

        if (GetSqueeze())
        {
            trigger = true;
        }
        else
        {
            trigger = false;
        }

    }
}

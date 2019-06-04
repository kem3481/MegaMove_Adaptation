using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCheck : MonoBehaviour
{
    public bool handPosition;

    // Start is called before the first frame update
    void Start()
    {
        handPosition = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("leftController") || other.gameObject.CompareTag("rightController"))
        {
            handPosition = true;
            Debug.Log("Hand Position correct");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("leftController") || other.gameObject.CompareTag("rightController"))
        {
            handPosition = false;
        }
    }
}

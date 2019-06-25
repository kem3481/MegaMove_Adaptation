using UnityEngine;

public class TriggerPull : MonoBehaviour
{
    private GameObject trigger;
    private ControlLevel_Trials controlLevels;
    public GameObject manager;

    public void Start()
    {
        trigger = GameObject.FindGameObjectWithTag("Trigger");
        manager = GameObject.FindGameObjectWithTag("Manager");
        controlLevels = manager.GetComponent<ControlLevel_Trials>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("leftController") || other.gameObject.CompareTag("rightController"))
        {
            trigger.SetActive(true);
        }
    }
}

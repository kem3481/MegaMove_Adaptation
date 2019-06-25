using UnityEngine;

public class TriggerPull : MonoBehaviour
{
    private GameObject trigger;
    public bool passedRadius;
    private ControlLevel_Trials controlLevels;
    public GameObject manager;

    public void Start()
    {
        trigger = GameObject.FindGameObjectWithTag("Trigger");
        manager = GameObject.FindGameObjectWithTag("Manager");
        controlLevels = manager.GetComponent<ControlLevel_Trials>();
        passedRadius = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Object"))
        {
            trigger.SetActive(true);
            Destroy(other);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            passedRadius = true;
        }
    }

    private void Update()
    {
        if (passedRadius == true)
        {
            passedRadius = false;
        }
    }
}

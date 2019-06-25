using UnityEngine;

public class TriggerPull : MonoBehaviour
{
    private GameObject trigger;

    public void Start()
    {
        trigger = GameObject.FindGameObjectWithTag("Trigger");
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("leftController") || other.gameObject.CompareTag("rightController"))
        {
            Destroy(this);
            trigger.SetActive(true);
        }
    }
}

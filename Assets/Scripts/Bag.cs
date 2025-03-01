using UnityEngine;
using System.Collections.Generic;

public class Bag : MonoBehaviour
{
    public Transform pickupPosition;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            other.transform.position = pickupPosition.position;
            other.transform.parent = pickupPosition;
            other.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

}

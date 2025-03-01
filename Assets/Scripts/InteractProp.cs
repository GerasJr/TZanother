using UnityEngine;

public class InteractProp : MonoBehaviour
{
    public bool IsInteract { get; private set; } = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Bag>())
        {
            IsInteract = false;
        }
    }
}

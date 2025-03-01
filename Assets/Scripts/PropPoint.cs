using UnityEngine;

public class PropPoint : MonoBehaviour
{
    public bool IsHaveProp { get; private set; } = false;

    public void Add()
    {
        IsHaveProp = true;
    }
}

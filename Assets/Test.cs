using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform parent;

    private void Start()
    {
        transform.SetParent(parent,false);
    }
}
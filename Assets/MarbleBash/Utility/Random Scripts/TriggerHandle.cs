using System;
using UnityEngine;

public class TriggerHandle : MonoBehaviour
{
    public event Action<Collider> onTriggerEnter;

    private void OnTriggerEnter(Collider other)
    {
        onTriggerEnter?.Invoke(other);
    }
}

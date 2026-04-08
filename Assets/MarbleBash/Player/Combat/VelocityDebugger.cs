using UnityEngine;

public class VelocityDebugger : MonoBehaviour
{
    public Rigidbody rb;

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"{name} : {rb.linearVelocity.magnitude:0.0}");
    }
}

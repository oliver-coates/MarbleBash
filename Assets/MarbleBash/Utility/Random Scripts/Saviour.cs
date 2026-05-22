using UnityEngine;

public class Saviour : MonoBehaviour
{
    void Update()
    {
        if (transform.position.y < -10f)
        {
            transform.position = Vector3.one;
        }
    }
}

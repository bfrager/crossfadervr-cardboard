using UnityEngine;
using System.Collections;

public class Spin_Node : MonoBehaviour
{
    public float speed = 10f;
    
    
    void Update ()
    {
        transform.Rotate(0,0,1 * speed * Time.deltaTime);
    }
}
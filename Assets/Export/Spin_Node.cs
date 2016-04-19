using UnityEngine;
using System.Collections;

public class Spin_Node : MonoBehaviour
{
    public float speed = 10f;
    
    
    void Update ()
    {
		transform.Rotate(0,1,0 * speed * Time.deltaTime, Space.World);
    }
}
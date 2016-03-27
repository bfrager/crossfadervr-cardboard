using UnityEngine;
using System.Collections;

public class NebulaScript : MonoBehaviour {

	public float speed = 1;
	public GameObject upMover;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		upMover.transform.Translate(0,1 * speed,0);
	}
}

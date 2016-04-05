using UnityEngine;
using System.Collections;

public class Spinning_Script : MonoBehaviour {

	public GameObject spinningMesh;
	public float speed = 1;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		spinningMesh.transform.Rotate(0,0,1 * speed) ;
	}
}

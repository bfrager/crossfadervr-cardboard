using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WireVeinController : MonoBehaviour {

	public GameObject[] nodes;
	public List<Transform> cachedNodes;


	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void GatherAvaliableNodes()
	{
		nodes = GameObject.FindGameObjectsWithTag("djNodes");

		Debug.Log(nodes);
	}

	void CacheNodes(GameObject[] goList)
	{

	}
}

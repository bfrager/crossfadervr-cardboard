using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WireVeinController : MonoBehaviour {

	public float distanceBuffer = 3;
	public GameObject[] nodes;
	public List<GameObject> sampledNodes;

	public List<List<Vector3>> forkList;
	private List<Vector3> gatheredNodes = new List<Vector3>();


	public GameObject nextTarget;
	public GameObject finalTarget;

	void Awake()
	{
		GatherAvaliableNodes();
		CalculateForks();
	}
	// Use this for initialization
	void Start () 
	{


	}

	// Update is called once per frame
	void Update () 
	{

	}


	void CalculateForks()
	{
		foreach(List<Vector3> fList in forkList)
		{
			foreach(Vector3 v in gatheredNodes)
			{
				//Debug.Log("punch");
			}

		}

	}

	//
	void GatherAvaliableNodes()
	{
		nodes = GameObject.FindGameObjectsWithTag("djNode");
		GameObject lastNode;
		forkList = new List<List<Vector3>>();
		gatheredNodes = new List<Vector3>();

		for(int i = 1; i < gatheredNodes.Count; i++)
		{
			
		}

		foreach(GameObject go in sampledNodes)
		{
			//int count = nodes.Length;
			gatheredNodes = new List<Vector3>();
			lastNode = go;

			//for(int i = 1; i < count; i++)
			foreach(GameObject g in nodes)
			{
				if(Vector3.Distance(lastNode.transform.position, g.transform.position) < distanceBuffer)
				{
					gatheredNodes.Add(g.transform.position);
				}

			forkList.Add(gatheredNodes);
			}

		}

//		foreach(List<Vector3> list in forkList)
//		{
//			Debug.Log(list.ToString());
//			foreach(Vector3 list2 in gatheredNodes)
//			{
//				Debug.Log("Here's list 2" + list2.ToString());
//			}
//		}

	}
}

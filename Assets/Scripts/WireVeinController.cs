using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WireVeinController : MonoBehaviour {

	//variables
	public int layerRand_min;
	public int layerRand_max;
	public float layerFactor;
//	public int sampleRange_min;
//	public int sampleRange_max;
	public float distanceBuffer = 3;
	public GameObject finalTarget;

	//lists
	private List<Vector3> layers = new List<Vector3>();
	private List<Vector3> outlyers = new List<Vector3>();
	private List<Vector3> gatheredNodes = new List<Vector3>();
	private List<Vector3> sampledNodes = new List<Vector3>();
	private GameObject[] initialNodes;

	//utilities
	private int randomSeed;
	private List<Vector3> randomNodes = new List<Vector3>();
	private List<Vector3> outlyerNodes = new List<Vector3>();
	private Vector3 lastTarget;
	private GameObject nextTarget;

	Vector3 IndexIntoLayer(int i, Vector3 v)
	{
		Vector3 position;
		position = Vector3.Lerp(v, finalTarget.transform.position, i + layerFactor);
		return position;
	}

	void Awake()
	{
		initialNodes = GameObject.FindGameObjectsWithTag("djNode");
		foreach(GameObject go in initialNodes)
		{
			//could be 100 initial nodes
			sampledNodes.Add(go.transform.position);
		}

		//Random Seed is for layers should be used before layer calculation;
		randomSeed = Random.Range(layerRand_min, layerRand_max);
		CalculateLayers(randomSeed);
		GatherAvaliableNodes(0, sampledNodes);
	}
//	// Use this for initialization
//	void Start () 
//	{
//		
//
//	}

	void GatherAvaliableNodes(int layerIndex, List<Vector3> sampledNodes)
	{
		gatheredNodes = new List<Vector3>();
		outlyers = new List<Vector3>();

		for(int i = 1; i < sampledNodes.Count; i++)
		{
			//Get a random listing of nodes via distance distrubtion
			//Add selected list to gatheredNodes
		}

		List<Vector3> passingNodes = new List<Vector3>();

		for(int i = 1; i < gatheredNodes.Count; i++)
		{
			//sample closest nodes
			if(Vector3.Distance(gatheredNodes[i], lastTarget) <= distanceBuffer)
			{
				passingNodes.Add(gatheredNodes[i]);
			}
			else
				outlyers.Add(gatheredNodes[i]);
				

			//place the average center of all nodes into list 
			//Createlines to the next layerIndex[i];
			IndexIntoLayer(layerIndex, gatheredNodes[i]);
			lastTarget = gatheredNodes[i];
		}


		for(int i = 1; i < outlyers.Count; i++)
		{
			
			IndexIntoLayer(layerIndex, outlyers[i]);
		}
			
			
	}


	void CalculateLayers(int seed)
	{

		layers = new List<Vector3>();
		seed = Random.Range(layerRand_min, layerRand_max);

		for(int i = 1; i < seed; i++)
		{
			
		}
	}
}

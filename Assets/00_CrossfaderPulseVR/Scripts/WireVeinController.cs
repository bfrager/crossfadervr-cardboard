using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

public class WireVeinController : MonoBehaviour {

	//variables
	public int layerRand_min;
	public int layerRand_max;
	private float layerFactorPercent;
//	public int sampleRange_min;
//	public int sampleRange_max;
	public float distanceBuffer = 3;
	public GameObject finalTarget;
	public GameObject testTarget;

	//public List<GameObject>  testNode = new List<GameObject>();


	//lists
	private GameObject[] initialNodes;
	public List<Vector3> sampledNodes = new List<Vector3>();
	private List<Vector3> layers = new List<Vector3>();
	public List<Vector3> gatheredNodes = new List<Vector3>();
	public List<Vector3> passingNodes = new List<Vector3>();
	private List<Vector3> outlyers = new List<Vector3>();


	//utilities
	private LinesGraphicRender lGR;
	private int randomSeed;
	private List<Vector3> randomNodes = new List<Vector3>();
	private List<Vector3> outlyerNodes = new List<Vector3>();
	private Vector3 lastTarget;
	private GameObject nextTarget;

	Vector3 IndexIntoLayer(float i, Vector3 v)
	{
		Vector3 position;
		position =  (finalTarget.transform.position + v) * i;  //Vector3.Lerp(v, finalTarget.transform.position, i + layerFactor/1);
		//Debug.Log(position.ToString());
		return position;
	}

	void Awake()
	{
		lGR = gameObject.GetComponent<LinesGraphicRender>();
		initialNodes = GameObject.FindGameObjectsWithTag("djNode");


		//this is where I'm calling the other script
		//only part that doesn't work....
		lGR.CreateLines(testTarget.transform.position, finalTarget.transform.position);

		foreach(GameObject go in initialNodes)
		{
			sampledNodes.Add(go.transform.position);
		}
		randomSeed = Random.Range(layerRand_min, layerRand_max);
		CreateNewLayer(randomSeed, 0, sampledNodes);
	}
//	// Use this for initialization
//	void Start () 
//	{
//		
//
//	}

	void CreateNewLayer(int Layer, float layerIndexPercent, List<Vector3> sampledNodes)
	{
		gatheredNodes = new List<Vector3>();
		outlyers = new List<Vector3>();
		bool nextRound = false;
		bool finished = false;

		gatheredNodes.Add(sampledNodes[0]);
		gatheredNodes.Add(sampledNodes[3]);
		gatheredNodes.Add(sampledNodes[9]);

		for(int i = 0; i < sampledNodes.Count; i++)
		{
			//Get a random listing of nodes via distance distrubtion
			//Add selected list to gatheredNodes

			//gatheredNodes.Add(sampledNodes[/*i*/]);

			//print("Sample Vectors Cached");
		}

		for(int i = 0; i < gatheredNodes.Count; i++)
		{
			Vector3 average =  Vector3.zero;
			RaycastHit[] hit = new RaycastHit[gatheredNodes.Count];
			hit = Physics.SphereCastAll(gatheredNodes[i], distanceBuffer/i, Vector3.forward);

			foreach(RaycastHit h in hit)
			{
				passingNodes.Add(h.transform.position);
				int j = 0;

				while(j < passingNodes.Count)
				{
					average += passingNodes[j];
					j++;
				}
				average = IndexIntoLayer(layerIndexPercent, average/passingNodes.Count);
				sampledNodes.Add(average);
				//print("new vector created");
				//lGR.CreateLines(passingNodes[j], finalTarget.transform.position);
				//print(passingNodes.Count.ToString());
			}
		}
		for(int k = 0; k < outlyers.Count; k++)
		{
			IndexIntoLayer(layerIndexPercent, outlyers[k]);
			//lGR.CreateLines(outlyers[k], finalTarget.transform.position);
			//print(outlyers.Count.ToString());
		}

//		nextRound = true;
//		if(nextRound && !finished)
//		{
//			sampledNodes = new List<Vector3>();
//			print("newRound");
//			//sammoh
//			//sampledNodes.Add(passingNodes);
//			//sampledNodes.Add(outlyerNodes);
//			//CreateNewLayer(-Layer++, layerIndexPercent, sampledNodes);
//			Layer = 0;
//			print("FinalLayerCount");
//			if(Layer == 0)
//				finished = true;
//		}
//		if(finished == true)
//		{
//			print("Vein Setup Success!!");
//		}
//		else{
//			finished = true;
//			print("Finished setup!");
//		}
	}
}

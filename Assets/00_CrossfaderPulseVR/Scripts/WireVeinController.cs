using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WireVeinController : MonoBehaviour {

	//variables
	public int layerRand_min;
	public int layerRand_max;
	private float layerFactorPercent;
	public float distanceBuffer = 1;
	public GameObject finalTarget;
	public GameObject testTarget;
	public float lineSize = 1;

	//lists
	private GameObject[] initialNodes;
	public List<Vector3> sampledNodes = new List<Vector3>();
	public List<Vector3> gatheredNodes = new List<Vector3>();
	public List<Vector3> passingNodes = new List<Vector3>();
	private List<Vector3> outlyers = new List<Vector3>();


	//utilities
	private LinesGL lGL;
	private LinesGraphicRender lGR;
	private int randomSeed;
	private List<Vector3> randomNodes = new List<Vector3>();
	private List<Vector3> outlyerNodes = new List<Vector3>();
	private Vector3 lastTarget;
	private GameObject nextTarget;

	Vector3 IndexIntoLayer(float i, Vector3 v)
	{
		Vector3 position;
		position =  (finalTarget.transform.position + v) * i;
		return position;
	}
		

	void Awake()
	{
		initialNodes = GameObject.FindGameObjectsWithTag("djNode");

		//store nodes
		foreach(GameObject go in initialNodes)
		{
			sampledNodes.Add(go.transform.position);
		}
		randomSeed = Random.Range(layerRand_min, layerRand_max);
		print("Sampled Nodes:" + sampledNodes.Count);
	}

	void Start()
	{
		labelStyle = new GUIStyle();
		labelStyle.normal.textColor = Color.black;

		linkStyle = new GUIStyle();
		linkStyle.normal.textColor = Color.blue;

		ml = new Mesh();
		lmat = new Material(shader);
		lmat.color = new Color(0,0,0,0.3f);

		ms = new Mesh();
		smat = new Material(shader);
		smat.color = new Color(0,0,0,0.1f);


		CreateNewLayer(randomSeed, 0, sampledNodes);
	}

	void CreateNewLayer(int layer, float layerIndexPercent, List<Vector3> sampledNodes)
	{
		layer--;
		if(layer == 0){ print("Veins All Set Up!!" + layer); return;}
		gatheredNodes = new List<Vector3>();
		outlyers = new List<Vector3>();
		bool nextRound = false;
		bool finished = false;

//		gatheredNodes.Add(sampledNodes[0]);
//		gatheredNodes.Add(sampledNodes[3]);
//		gatheredNodes.Add(sampledNodes[9]);

		print("Gathered Nodes:" + gatheredNodes.Count);

		for(int i = 0; i < sampledNodes.Count; i++)
		{
			gatheredNodes.Add(sampledNodes[i]);
			//Get a random listing of nodes via distance distrubtion
			//Add selected list to gatheredNodes
			//print("Sample Vectors Cached");
		}

		for(int i = 0; i < gatheredNodes.Count; i++)
		{
			Vector3 average =  Vector3.zero;
			RaycastHit[] hit;
			hit = Physics.SphereCastAll(gatheredNodes[i], 3, Vector3.up, 0);
			Debug.Log("Calculating Gathered Node " + gatheredNodes[i]);
			sampledNodes.Clear();

			//calculate neighbor nodes
			for(i = 0; i < hit.Length; i++)
			{

				passingNodes.Add(hit[i].transform.position);
				average += hit[i].transform.position;

				sampledNodes.Add(average);

				//start next round
				if (i == hit.Length - 1)
				{
					average = average/hit.Length;
					average = IndexIntoLayer(layerIndexPercent, average);
					CreateLines(passingNodes, average);
				}
			}
		}
		//OUTLYERS
		for(int i = 0; i < outlyers.Count; i++)
		{
			Vector3 newPos = outlyers[i];
			IndexIntoLayer(layerIndexPercent, newPos);
			CreateLines(outlyers, newPos);
			sampledNodes.Add(newPos);

			//print("Other Nodes:" + passingNodes.Count);
		}
		//sammoh Finally Create New Layer...
		CreateNewLayer(layer, gatheredNodes.Count, sampledNodes);
	}

//Here is the line rendering

	public Shader shader;

	private Mesh ml;
	private Material lmat;

	private Mesh ms;
	private Material smat;

	private Vector3 s;

	private GUIStyle labelStyle;
	private GUIStyle linkStyle;

	private Point first;

	private float speed = 5.0f;

	void CreateLines(List<Vector3> startPoints ,Vector3 finalPoint)
	{
		for(int i = 0; i < startPoints.Count; i++)
		{
			Mesh m = new Mesh();
			Vector3 s = transform.InverseTransformPoint(startPoints[i]);
			AddLine(m, MakeQuad(finalPoint, s, lineSize), false);
		}

		//print(ml.vertexCount);
		Draw();
	}

	void AddLine(Mesh m, Vector3[] quad, bool tmp) 
	{
		int vl = m.vertices.Length;

		Vector3[] vs = m.vertices;
		if(!tmp || vl == 0) vs = resizeVertices(vs, 4);
		else vl -= 4;

		vs[vl] = quad[0];
		vs[vl+1] = quad[1];
		vs[vl+2] = quad[2];
		vs[vl+3] = quad[3];

		int tl = m.triangles.Length;

		int[] ts = m.triangles;
		if(!tmp || tl == 0) ts = resizeTraingles(ts, 6);
		else tl -= 6;
		ts[tl] = vl;
		ts[tl+1] = vl+1;
		ts[tl+2] = vl+2;
		ts[tl+3] = vl+1;
		ts[tl+4] = vl+3;
		ts[tl+5] = vl+2;

		m.vertices = vs;
		m.triangles = ts;
		m.RecalculateBounds();
	}

	void Draw() {
		Graphics.DrawMesh(ml, transform.localToWorldMatrix, lmat, 0);
		Graphics.DrawMesh(ms, transform.localToWorldMatrix, smat, 0);
	}

	//dynamic variables
	Vector3[] resizeVertices(Vector3[] ovs, int ns) {
		Vector3[] nvs = new Vector3[ovs.Length + ns];
		for(int i = 0; i < ovs.Length; i++) nvs[i] = ovs[i];
		return nvs;
	}
	int[] resizeTraingles(int[] ovs, int ns) {
		int[] nvs = new int[ovs.Length + ns];
		for(int i = 0; i < ovs.Length; i++) nvs[i] = ovs[i];
		return nvs;
	}
	Vector3[] MakeQuad(Vector3 s, Vector3 e, float w) {
		w = w / 2;
		Vector3[] q = new Vector3[4];

		Vector3 n = Vector3.Cross(s, e);
		Vector3 l = Vector3.Cross(n, e-s);
		l.Normalize();

		q[0] = transform.InverseTransformPoint(s + l * w);
		q[1] = transform.InverseTransformPoint(s + l * -w);
		q[2] = transform.InverseTransformPoint(e + l * w);
		q[3] = transform.InverseTransformPoint(e + l * -w);

		return q;
	}
}

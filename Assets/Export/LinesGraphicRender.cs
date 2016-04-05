using UnityEngine;
using System.Collections.Generic;

class Point {
	public Vector3 p;
	public Point next;
}

public class LinesGraphicRender : MonoBehaviour {

	public Shader shader;

	private Mesh ml;
	private Material lmat;
	
	private Mesh ms;
	private Material smat;
	
	private Vector3 s;

	public float lineSize = 0.4f;
	
	private GUIStyle labelStyle;
	private GUIStyle linkStyle;
	
	private Point first;
	
	//private float speed = 5.0f;

	public WireVeinController wireController;
	private List<Vector3> nodes;
	private GameObject fT;

	void Awake()
	{
		//sammoh initialize my wireController
		//wireController = GameObject.GetComponent<WireVeinController>();

	}

	void Start () {

		//nodes = wireController;
		fT = wireController.finalTarget;


		//sammoh this is where the debug information comes from.
		labelStyle = new GUIStyle();
		labelStyle.normal.textColor = Color.black;
		
		linkStyle = new GUIStyle();
		linkStyle.normal.textColor = Color.blue;

		//initialize new meshes
		ml = new Mesh();
		lmat = new Material(shader);
		lmat.color = new Color(0,0,0,1f);
		
		ms = new Mesh();
		smat = new Material(shader);
		smat.color = new Color(0,0,0,0.1f);

		CreateLines();
	}

//
//	void Update1() {
//
//		//sammoh This is where the mouse controls are
//		if(Input.GetMouseButton(0)) {
//			
//			Vector3 e = GetNewPoint();
//			
//			if(first == null) {
//				first = new Point();
//				first.p = transform.InverseTransformPoint(e);
//			}
//			
//			if(s != Vector3.zero) {
//				Vector3 ls = transform.TransformPoint(s);
//				AddLine(ml, MakeQuad(ls, e, lineSize), false);
//				
//				Point points = first;
//				while(points.next != null) {
//					Vector3 next = transform.TransformPoint(points.p);
//					float d = Vector3.Distance(next, ls);
//					if(d < 1 && Random.value > 0.9f) {
//						AddLine(ms, MakeQuad(next, ls, lineSize), false);
//					}
//					points = points.next;
//				}
//				
//				Point np = new Point();
//				np.p = transform.InverseTransformPoint(e);
//				points.next = np;
//
//			}
//			
//			s = transform.InverseTransformPoint(e);
//		} else {
//			s = Vector3.zero;
//		}
//		
//		Draw();
//		processInput();
//	}
	
	void Draw() {
		Graphics.DrawMesh(ml, transform.localToWorldMatrix, lmat, 0);
		Graphics.DrawMesh(ms, transform.localToWorldMatrix, smat, 0);
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
	
//	void processInput() {
//		float s = speed * Time.deltaTime;
//		if(Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift)) s = s * 10;
//		if(Input.GetKey(KeyCode.UpArrow)) transform.Rotate(-s, 0, 0);
//		if(Input.GetKey(KeyCode.DownArrow)) transform.Rotate(s, 0, 0);
//		if(Input.GetKey(KeyCode.LeftArrow)) transform.Rotate(0, -s, 0);
//		if(Input.GetKey(KeyCode.RightArrow)) transform.Rotate(0, s, 0);
//		
//		if(Input.GetKeyDown(KeyCode.C)) {
//			ml = new Mesh();
//			ms = new Mesh();
//			transform.rotation = Quaternion.identity;
//			first = null;
//		}
//	}
	
	Vector3 GetNewPoint() {
		return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z * -1.0f));
	}
	
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

	//sammoh Gui text//
	void OnGUI()
	{
		int vc = ml.vertices.Length + ms.vertices.Length;
		GUI.Label (new Rect (10, 26, 300, 24), "Drawing " + vc + " vertices", labelStyle);
	}
	
	/** Replace the Update function with this one for a click&drag drawing option */
	void Update() {
		
		//processInput();
		
		Vector3 e;
		
		if(Input.GetMouseButtonDown(0)) {
			s = transform.InverseTransformPoint(GetNewPoint());
		}
		
		if(Input.GetMouseButton(0)) {
			e = GetNewPoint();
			AddLine(ml, MakeQuad(transform.TransformPoint(s), e, lineSize), true);
		}

		if(Input.GetMouseButtonUp(0)) {
			e = GetNewPoint();
			AddLine(ml, MakeQuad(transform.TransformPoint(s), e, lineSize), false);
		}
		
		Draw();
	}


	void CreateLines()
	{
		Vector3 e;
		s = transform.InverseTransformPoint(fT.transform.position);

		foreach(Vector3 go in nodes)
		{
			//e = go.transform.position;

			//AddLine(ml, MakeQuad(go.transform.TransformPoint(e), s, lineSize), false);

		}

		Draw();
	}
}








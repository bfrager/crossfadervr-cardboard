using UnityEngine;
using System.Collections;

public class GroundFlameEffect : MonoBehaviour {

	public float flameSpeed = 0;
	public float maskSpeed = 0;

	private ProceduralMaterial mat;

	// Use this for initialization
	void Start () 
	{
		mat = Resources.Load("Substance Materials/GroundPlaneSubstance", typeof(ProceduralMaterial)) as ProceduralMaterial;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//mat.SetProceduralVector("Transform1", (new Vector4(Time.deltaTime/2, 0, 0, Time.deltaTime/2)));
		//mat.SetProceduralVector("Transform2", (new Vector4(Time.deltaTime/2, 0, 0, Time.deltaTime/2)));
		//mat.SetProceduralVector("MaskTransform", (new Vector4(1, 0, 0, 1)));

		//mat.RebuildTextures();
	}
}

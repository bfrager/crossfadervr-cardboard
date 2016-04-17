using UnityEngine;
using System.Collections;

public class CountryHighlighter : MonoBehaviour {

	private GameObject worldMap;

	public int countryID = 0;
	public float maxOpacity = 0.3f;
	public bool triggerPing;
	private float pingTime = 1;

	private ProceduralMaterial substance;



	// Use this for initialization
	void Start () 
	{
		substance = Resources.Load("Substance Materials/MapHighlight", typeof(ProceduralMaterial)) as ProceduralMaterial;
	}
	
	// Update is called once per frame
	void Update () 
	{

		if(countryID != 0)
		{
			substance.SetProceduralFloat("CountrySelection", countryID);
			substance.SetProceduralFloat("Mask_Opacity", Mathf.PingPong(Time.time, maxOpacity));
			StartCoroutine(WaitTime());
			substance.RebuildTextures ();
		}
			
	}

	IEnumerator WaitTime()
	{
		yield return new WaitForSeconds(pingTime);
		countryID = 0;
		substance.SetProceduralFloat("Mask_Opacity", 0);
		substance.RebuildTextures ();

	}


}

﻿using UnityEngine;
using System.Collections;

public class CountryHighlighter : MonoBehaviour {

	private GameObject worldMap;

	public int countryID = 0;
	public float maxOpacity = 0.3f;
	public bool triggerPing;
	public float pingTime = 3f;

	private ProceduralMaterial substance;

	// Use this for initialization
	void Start () 
	{
		substance = Resources.Load("Substance Materials/MapHighlight", typeof(ProceduralMaterial)) as ProceduralMaterial;
		substance.SetProceduralFloat("Mask_Opacity", 0);
		substance.RebuildTextures ();
	}
	
	// Update is called once per frame
	void Update () 
	{

//		 if(countryID != 0)
//		 {
//			substance.SetProceduralFloat("input_number", countryID);
//		 	substance.SetProceduralFloat("Mask_Opacity", Mathf.PingPong(Time.time, maxOpacity));
//		 	//StartCoroutine(WaitTime());
//		 	substance.RebuildTextures ();
//		 }
			
	}

	IEnumerator WaitTime()
	{
		while (true) 
		{
			yield return new WaitForSeconds(pingTime);
			countryID = 0;
			substance.SetProceduralFloat("Mask_Opacity", 0);
			substance.RebuildTextures ();
		}
	}
			
	public void updateCountry(int countryID) 
	{
		if(countryID != 0)
		{
			//substance = Resources.Load("Substance Materials/MapHighlight", typeof(ProceduralMaterial)) as ProceduralMaterial;		
			substance.SetProceduralFloat("input_number", countryID);

			substance.SetProceduralFloat("Mask_Opacity", maxOpacity);
			substance.RebuildTextures ();

			StartCoroutine(WaitTime());
		}
	}


}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingInNewFlags : MonoBehaviour {

	//load flag
	public SpriteRenderer spriteRenderer;
	private Sprite flagSprite;
	public string countryName;


	//highlight country
	private GameObject worldMap;

	public int countryID = 0;
	public float maxOpacity = 0.3f;
	public bool triggerPing;
	private float pingTime = 1;

	private ProceduralMaterial substance;

	void Start () 
	{
		//load sprite
		if(spriteRenderer.sprite == null)
		{
			flagSprite = Resources.Load<Sprite>("flags/Unknown");
			Debug.LogError("Sprite Not Found!");
		}
		else{
			flagSprite = Resources.Load<Sprite>("flags/"+ countryName);
		}

		spriteRenderer.sprite = flagSprite;

	}

	IEnumerator WaitTime()
	{
		yield return new WaitForSeconds(pingTime);
		substance.SetProceduralFloat("Mask_Opacity", 0);
		countryID = 0;
		substance.RebuildTextures ();

	}

	public void PingCountry(bool triggerPing)
	{
		//loadsubstanceMaterial
		substance = Resources.Load("Substance Materials/MapHighlight", typeof(ProceduralMaterial)) as ProceduralMaterial;

		if (countryID == 0)Debug.LogError("Country Not Set!");
		if(countryID != 0 && triggerPing)
		{
			triggerPing = false;
			substance.SetProceduralFloat("CountrySelection", countryID);
			substance.SetProceduralFloat("Mask_Opacity", Mathf.PingPong(Time.time, maxOpacity));
			StartCoroutine(WaitTime());
			substance.RebuildTextures();
		}
	}


}

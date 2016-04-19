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
	public int tempCode;

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

	IEnumerator WaitTime(int id)
	{
		yield return new WaitForSeconds(pingTime);
		substance.SetProceduralFloat("Mask_Opacity", 0);
		id = 0;
		substance.RebuildTextures ();

	}

	public void PingCountry(bool triggerPing, int id)
	{
		//loadsubstanceMaterial
		substance = Resources.Load("Substance Materials/MapHighlight",typeof(ProceduralMaterial)) as ProceduralMaterial;
		tempCode = id;
		if (id == 0)Debug.LogError("Country Not Set!");
		if(id != 0 && triggerPing)
		{
			triggerPing = false;
			substance.SetProceduralFloat("CountrySelection", id);
			substance.SetProceduralFloat("Mask_Opacity", Mathf.PingPong(Time.time, maxOpacity));
			StartCoroutine(WaitTime(id));
			substance.RebuildTextures();
			id = tempCode;
		}
	}


}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingInNewFlags : MonoBehaviour {

	public SpriteRenderer spriteRenderer;
	private Sprite flagSprite;
	public string countryName;

	void Start () 
	{

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

}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingInNewFlags : MonoBehaviour {

	private SpriteRenderer sr;
	public Sprite flagSprite;
	public string countryName;
	public Sprite[] flaglist ;


	void awake()
	{

		
	}
	// Use this for initialization
	void Start () 
	{
		sr = gameObject.GetComponent<SpriteRenderer>();
		//flaglist = Resources.LoadAll<Sprite>("flags");
		flagSprite = Resources.Load<Sprite>("flags/"+ countryName);
		sr.sprite = flagSprite;
		if(flagSprite.packed) Debug.LogError("Image is Altased!");

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;

public class LoadAvatarImage : MonoBehaviour {

	public ApiCall api;
	public Texture avatarTexture;
	public string performanceId;

	//utility
	public MeshRenderer mr;
	public Material avatarMat;

	void Awake ()
	{

	}

	// Use this for initialization
	void Start () 
	{
		api = GameObject.Find("PersistentData").GetComponent<ApiCall>();
		//Debug.Log(api.performancesDict);
		performanceId = gameObject.name;
			Debug.Log(performanceId);
		mr = gameObject.transform.Find("Diamond").GetComponentInChildren<MeshRenderer>();
		Debug.Log(api.performancesDict[performanceId]);
		StartCoroutine("_LoadAvatarTexture");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
		IEnumerator _LoadAvatarTexture()
	{
		string avatarUrl = api.performancesDict[performanceId]["users"][0]["avatar"].ToString();
		string[] temp = avatarUrl.Split('\"');
		avatarUrl = temp[1];
		print("This is the djNode url: " + temp[1]);
		WWW imgUrl = new WWW(avatarUrl);
		yield return imgUrl;
		mr.materials[1].mainTexture = imgUrl.texture;


	}
}

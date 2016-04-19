using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadAvatarImage : MonoBehaviour {

	public ApiCall api;
	public Texture avatarTexture;
	public string performanceId;

	//utility
	public MeshRenderer mr;
	public Material avatarMat;

	void Awake ()
	{
		api = GameObject.Find("PersistentData").GetComponent<ApiCall>();
		performanceId = gameObject.name;
		mr = gameObject.transform.Find("Diamond").GetComponentInChildren<MeshRenderer>();
	}

	// Use this for initialization
	void Start () 
	{
		StartCoroutine(WaitForCall());
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
	
	IEnumerator WaitForCall()
    {
 	    yield return new WaitForSeconds(2);
		Debug.Log("Grabbing dictionary");
		 
		Debug.Log(api.performancesDict);
		foreach(KeyValuePair<string,JSONObject> performance in api.performancesDict)		
		{
			Debug.Log(performance.Key);
		}
   
		StartCoroutine("_LoadAvatarTexture");
    }
}

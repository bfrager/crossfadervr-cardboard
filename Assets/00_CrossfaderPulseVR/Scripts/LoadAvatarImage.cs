using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LoadAvatarImage : MonoBehaviour {

	public ApiCall api;
	public Texture avatarTexture;
	public string performanceId;
	private bool loading = true;
	
	//DJ Info:
	public TextMesh djName;
	public TextMesh tags;
	public TextMesh listens;
	public TextMesh location;
	public Transform canvas;

	//utility
	public MeshRenderer mr;
	public Material avatarMat;


	void Awake ()
	{
		api = GameObject.Find("PersistentData").GetComponent<ApiCall>();
		
		// add DJ node objects
		performanceId = gameObject.name;
		mr = gameObject.transform.Find("Diamond").GetComponentInChildren<MeshRenderer>();
        canvas = gameObject.transform.Find("Dj_Info_Canvas");
        djName = canvas.transform.Find("Dj_Name").GetComponent<TextMesh>();
        location = canvas.transform.Find("Location").GetComponent<TextMesh>();
        listens = canvas.transform.Find("Hearts").GetComponent<TextMesh>();
        tags = canvas.transform.Find("Tags").GetComponent<TextMesh>();
	}

	// Use this for initialization
	void Start () 
	{
		StartCoroutine(WaitForCall());
		StartCoroutine(ScaleUpNodes(1f));
		string country = gameObject.GetComponent<LoadingInNewFlags>().countryName;
		location.text = country;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	IEnumerator ScaleUpNodes(float time)
    {
		while(loading)       
		yield return new WaitForSeconds(0.1f);
		//LOAD DJ NODES
        Vector3 originalScale = gameObject.transform.localScale;
        Vector3 destinationScale = new Vector3(0.25f, 0.25f, 0.25f);
         
         float currentTime = 0.0f;
         
         do
         {
             gameObject.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
             currentTime += Time.deltaTime;
             yield return null;
         } while (currentTime <= time);         
	}
	
	IEnumerator WaitForCall()
    {
 	    loading = true;
		yield return new WaitForSeconds(2f);
		Debug.Log("Grabbing dictionary");
		 
		Debug.Log(api.performancesDict);
		foreach(KeyValuePair<string,JSONObject> performance in api.performancesDict)		
		{
			Debug.Log(performance.Key);
		}
		StartCoroutine("_LoadAvatarTexture");
		StartCoroutine("_LoadName");
		StartCoroutine("_LoadTags");
		StartCoroutine("_LoadListens");
		loading = false;
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
	
	IEnumerator _LoadName()
	{
		string name = api.performancesDict[performanceId]["users"][0]["dj_name"].ToString();
		yield return name;
		djName.text = name;
	}
	
	// IEnumerator _LoadLocation()
	// {
	// 	string country = api.performancesDict[performanceId]["users"][0]["country"].ToString();
	// 	yield return country;
	// 	location.text = country;
	// }
	
	IEnumerator _LoadListens()
	{
		string listens_count = api.performancesDict[performanceId]["performance"]["listens_count"].ToString();
		yield return listens_count;
		listens.text = listens_count;
	}
	
		
	IEnumerator _LoadTags()
	{
		string firstTag = api.performancesDict[performanceId]["performance"]["tags"][0].ToString();
		yield return firstTag;
		tags.text = firstTag;
	}
	
}

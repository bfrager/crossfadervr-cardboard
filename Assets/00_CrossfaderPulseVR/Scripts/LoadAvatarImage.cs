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
	public Text djName;
	public Text tags;
	public Text listens;
	public Text location;
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
        djName = canvas.transform.Find("Dj_Name").GetComponent<Text>();
        location = canvas.transform.Find("Location").GetComponent<Text>();
        listens = canvas.transform.Find("Hearts").GetComponent<Text>();
        tags = canvas.transform.Find("Tags").GetComponent<Text>();

	}

	// Use this for initialization
	void Start () 
	{
		//Set Canvas Style
		// djName.font = Gravity;
		// location.font = Gravity;
		// listens.font = Gravity;
		// tags.font = Gravity;
		
		
		//Load objects
		StartCoroutine(WaitForCall());
		StartCoroutine(ScaleUpNodes(1f, 0.35f));
		string country = gameObject.GetComponent<LoadingInNewFlags>().countryName;
		location.text = country;
		Debug.Log(location.text);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	IEnumerator ScaleUpNodes(float time, float scale)
    {
		while(loading)       
		yield return new WaitForSeconds(0.1f);
		//LOAD DJ NODES
        Vector3 originalScale = gameObject.transform.localScale;
        Vector3 destinationScale = new Vector3(scale, scale, scale);
         
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

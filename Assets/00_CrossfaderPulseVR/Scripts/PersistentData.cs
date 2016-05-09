using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class PersistentData : MonoBehaviour {

	public static PersistentData PD;
	public float curSongTime;
	public string performanceId;
	public int loadNum = 0;


	enum Fade {In, Out};

	private ApiCall api;
	public Texture avatarTexture;
	public Texture bgTexture;


	//utility
	public MeshRenderer mr;
	public MeshRenderer iconMr;
	public MeshRenderer boothMr;
	public TextMesh djName;
	public string AvatarURL; 
	public string trackName;

	public Material avatarMat;

	// Use this for initialization
	void Awake () 
	{
		DontDestroyOnLoad(gameObject);
		PD = this;
		api = gameObject.GetComponent<ApiCall>();
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
	
	void LoadPerformanceData()
	{
		StartCoroutine("_LoadName");
		StartCoroutine("_LoadAvatarFromUrl");
		StartCoroutine("_LoadBGFromUrl");
		StartCoroutine("_LoadProfArtFromUrl");
	}

	void OnLevelWasLoaded()
	{
		loadNum++;
		Debug.Log(loadNum);
		
		if (loadNum > 1)
		{
			GameObject.Find("OnboardingUI").SetActive(false);
		}
		
		GameObject.Find("Planet960tris").GetComponent<MeshRenderer>().enabled = true;
		AudioListener.volume = 0;
		StartCoroutine(FadeAudio(5, Fade.In));
		
		if (SceneManager.GetActiveScene().name == "02_Cardboard_DJLevel_v2")
		{
			// print (GameObject.Find("DJ_Room").transform.name);
			GameObject audioSampler = GameObject.Find("AudioSampler");
			audioSampler.GetComponent<AudioSource>().clip = Resources.Load("Mixes/"+performanceId, typeof (AudioClip)) as AudioClip;
			audioSampler.GetComponent<AudioSource>().time = curSongTime;
			audioSampler.GetComponent<AudioSource>().Play();
			mr = GameObject.FindGameObjectWithTag("Stage").GetComponent<MeshRenderer>();
			iconMr = GameObject.FindGameObjectWithTag("DjIcon").GetComponent<MeshRenderer>();
			boothMr = GameObject.FindGameObjectWithTag("DjBooth").GetComponent<MeshRenderer>();	djName = GameObject.FindGameObjectWithTag("DjName").GetComponent<TextMesh>();		
			
			//sammoh loading...
			LoadPerformanceData();
		}
		else
		{
			curSongTime = 0;
		}
	}

	IEnumerator FadeAudio (float timer, Fade fadeType) 
	{
		float start = fadeType == Fade.In? 0.0F : 1.0F;
		float end = fadeType == Fade.In? 1.0F : 0.0F;
		float i = 0.0F;
		float step = 1.0F/timer;
	
		while (i <= 1.0F) {
			i += step * Time.deltaTime;
			AudioListener.volume = Mathf.Lerp(start, end, i);
			yield return new WaitForSeconds(step * Time.deltaTime);
		}
	}

	IEnumerator _LoadName()
	{
		string name = api.performancesDict[performanceId]["users"][0]["dj_name"].ToString();
		yield return name;
		djName.text = name;
	}

	IEnumerator _LoadAvatarFromUrl()
	{
		string avatarUrl = api.performancesDict[performanceId]["users"][0]["avatar"].ToString();
		string[] temp = avatarUrl.Split('\"');
		avatarUrl = temp[1];
		WWW imgUrl = new WWW(avatarUrl);
		yield return imgUrl;
		iconMr.materials[1].mainTexture = imgUrl.texture;
	}

	IEnumerator _LoadBGFromUrl()
	{
		string bgUrl = api.performancesDict[performanceId]["performance"]["background_image"].ToString();
		string[] temp = bgUrl.Split('\"');
		bgUrl = temp[1];
		WWW imgUrl = new WWW(bgUrl);
		yield return imgUrl;
		mr.material.mainTexture = imgUrl.texture;
	}
	
	IEnumerator _LoadProfArtFromUrl()
	{
		string coverUrl = api.performancesDict[performanceId]["users"][0]["cover_photo"].ToString();
		string[] temp = coverUrl.Split('\"');
		coverUrl = temp[1];
		WWW imgUrl = new WWW(coverUrl);
		yield return imgUrl;
		boothMr.material.mainTexture = imgUrl.texture;
	}
}

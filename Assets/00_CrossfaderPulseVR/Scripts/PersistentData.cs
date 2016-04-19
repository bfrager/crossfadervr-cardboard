using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class PersistentData : MonoBehaviour {

	public static PersistentData PD;
	public float curSongTime;
	public string performanceId;


	enum Fade {In, Out};

	private ApiCall api;
	public Texture avatarTexture;
	public Texture bgTexture;


	//utility
	public MeshRenderer mr;
	public MeshRenderer iconMr;
	public string url = "https://dxzw8fe3xavok.cloudfront.net/performances/550352/550352-background-performancePhoto.jpeg?1449747147";
	public string AvatarURL; 
	public string trackName;
	public string djName;
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
		StartCoroutine("_LoadAvatarUserName");
		StartCoroutine("_LoadAvatarFromUrl");
		StartCoroutine("_LoadBGFromUrl");
	}

	void OnLevelWasLoaded()
	{
		AudioListener.volume = 0;
		StartCoroutine(FadeAudio(5, Fade.In));

		print ("listner: " + AudioListener.volume);
		if (SceneManager.GetActiveScene().name == "02_Cardboard_DJLevel_v2")
		{
			print (GameObject.Find("DJ_Room").transform.name);
			GameObject.Find("AudioSampler").GetComponent<AudioSource>().clip = Resources.Load("Mixes/"+performanceId, typeof (AudioClip)) as AudioClip;
			GameObject.Find("AudioSampler").GetComponent<AudioSource>().time = curSongTime;
			GameObject.Find("AudioSampler").GetComponent<AudioSource>().Play();

			Debug.Log("perf id = " + performanceId);

		}
		else{
			curSongTime = 0;
		}

		mr = GameObject.FindGameObjectWithTag("Stage").GetComponent<MeshRenderer>();
		iconMr = GameObject.FindGameObjectWithTag("DjIcon").GetComponent<MeshRenderer>();

		//sammoh loading...
		LoadPerformanceData();
	}

	public void PubFadeAudio(float timer, int fadeType, Transform gameObject)
	{
		if (fadeType ==0)
		{
			//fadeout
			StartCoroutine(FadeAudio(timer, Fade.Out));
		}
	}

	IEnumerator FadeAudio (float timer, Fade fadeType) {
	    // TODO: check whether gameObject volume is at 0 or 1
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
		djName = name;
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
		StartCoroutine("_LoadAvatarFromUrl");
	}
}

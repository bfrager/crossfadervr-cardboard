using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
// added for Cardboard camera fade
using VRStandardAssets.Utils;


public class PersistentData : MonoBehaviour {

	public static PersistentData PD;
	private ApiCall api;
	
	// variables passed between scenes
	public float curSongTime;
	public string performanceId;
	public string name;
	public int loadNum = 0;
	// private bool isFading = false;
	
	public bool netVerified;
	public string scene;
	public GameObject[] audioSources;
	public GameObject camera;

	enum Fade {In, Out};

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
		if (PD == null)
		{
			PD = this;
			DontDestroyOnLoad(gameObject);
			Debug.Log("PD instance created");
		}
		
		api = gameObject.GetComponent<ApiCall>();
	}
	
	void Start ()
	{
		netVerified = (InternetReachabilityVerifier.Instance.status == InternetReachabilityVerifier.Status.NetVerified);
		Debug.Log(netVerified);
		if (!netVerified && loadNum == 0)
		{
			Debug.Log("Warning");
			GameObject.Find("ErrorCanvas/IntroCanvas").GetComponent<Canvas>().enabled = true;
		}
		// else
		// {
		// 	Debug.Log("No Warning");
		// 	GameObject.Find("ErrorCanvas/IntroCanvas").GetComponent<Canvas>().enabled = false;
		// }
	}
	
	void LoadPerformanceData()
	{
		// StartCoroutine("_LoadName");
		StartCoroutine("_LoadAvatarFromUrl");
		StartCoroutine("_LoadBGFromUrl");
		StartCoroutine("_LoadProfArtFromUrl");
	}

	void OnLevelWasLoaded()
	{
		Debug.Log("Level Loaded");
		loadNum++;
		GameObject onboardingUI = GameObject.Find("OnboardingUI");
		if (onboardingUI != null && loadNum > 1)
		{
			GameObject.Find("OnboardingUI").SetActive(false);
			GameObject.Find("ErrorCanvas").SetActive(false);
		}
		
		GameObject.Find("Planet960tris").GetComponent<MeshRenderer>().enabled = true;
		
		AudioListener.volume = 0;
		StartCoroutine(FadeAudioListener(5, Fade.In));
		
		Debug.Log("Playhead in = " + curSongTime);
		
		if (SceneManager.GetActiveScene().name == "02_Cardboard_DJLevel_v2")
		{
			// print (GameObject.Find("DJ_Room").transform.name);
			GameObject audioSampler = GameObject.Find("AudioSampler");
			audioSampler.GetComponent<AudioSource>().clip = Resources.Load("Mixes/"+performanceId, typeof (AudioClip)) as AudioClip;
			audioSampler.GetComponent<AudioSource>().time = curSongTime;
			audioSampler.GetComponent<AudioSource>().Play();
			
			mr = GameObject.FindGameObjectWithTag("Stage").GetComponent<MeshRenderer>();
			iconMr = GameObject.FindGameObjectWithTag("DjIcon").GetComponent<MeshRenderer>();
			boothMr = GameObject.FindGameObjectWithTag("DjBooth").GetComponent<MeshRenderer>();	
			djName = GameObject.FindGameObjectWithTag("DjName").GetComponent<TextMesh>();	
			djName.text = name;	
			
			if (netVerified)
			{
				LoadPerformanceData();
			}
		}
		
		// else if (SceneManager.GetActiveScene().name == "01_Cardboard_RootLevel_v1")
		// {
		// 	curSongTime = 0;
		// }
	}
	

	// IEnumerator _LoadName()
	// {
	// 	string name = api.performancesDict[performanceId]["users"][0]["dj_name"].ToString();
	// 	yield return name;
	// 	djName.text = name.Trim('"');
	// }

	IEnumerator _LoadAvatarFromUrl()
	{
		Debug.Log(api);
		Debug.Log(performanceId);
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

	IEnumerator FadeAudioListener (float timer, Fade fadeType) 
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


	// KEPT IN CARDBOADCONTROLLER.CS INSTEAD
	// public void ChangeLevel (int sceneBuild, float fadeDur, GameObject node)
	// {
	// 	camera = GameObject.Find("Main Camera");
	// 	scene = SceneManager.GetActiveScene().name;
	// 	StartCoroutine(FadeLevelChange(sceneBuild, fadeDur, node));
	// }

    // IEnumerator FadeLevelChange(int sceneBuild, float fadeDur, GameObject node)
    // {
    // 	isFading = true;
    //     camera.GetComponent<VRCameraFade>().FadeOut(fadeDur, false);
	// 	if (scene == "01_Cardboard_RootLevel_v1")
	// 	{
	// 		audioSources = GameObject.FindGameObjectsWithTag("Audio");
    		
	// 		// for (int i = 0; i < audioSources.Count; i++)
	// 		foreach (GameObject audioSource in audioSources)
	// 		{
	// 			StartCoroutine(FadeAudioSource(fadeDur, Fade.Out, audioSource.transform));
	// 			Debug.Log("Fading audio source at " + audioSource);
	// 		}
	// 	}
	// 	else if (scene == "02_Cardboard_DJLevel_v2")
	// 	{
    //     	StartCoroutine(FadeAudioListener(fadeDur, Fade.Out));
	// 	}
	// 	yield return new WaitForSeconds(fadeDur);
	// 	camera.SetActive(false);
    //     //cache current time of song
	// 	if (scene == "01_Cardboard_RootLevel_v1")
	// 	{
	// 		if (node != null)
	// 		{
	// 			curSongTime = node.GetComponent<CardboardAudioSource>().audioSource.time;
	// 			performanceId = node.transform.parent.name;
	// 		}
	// 		else
	// 		{
	// 			Debug.Log("Node has already been destroyed");
	// 		}
	// 	}
	// 	else if (scene == "02_Cardboard_DJLevel_v2")
	// 	{
	// 		curSongTime = GameObject.Find("AudioSampler").GetComponent<AudioSource>().time;
	// 	}
	// 	Debug.Log("Playhead out = " + curSongTime);
		
    //     SceneManager.LoadScene(sceneBuild); 
    //     isFading = false;
    // }
	
	// IEnumerator FadeAudioSource (float timer, Fade fadeType, Transform djNode) 
    // {
    //     // float start = fadeType == Fade.In? 0.0F : 1.0F;
    //     float end = fadeType == Fade.In? 1.0F : 0.0F;
    //     float i = 0.0F;
    //     float step = 1.0F/timer;
    //     float currentVolume = djNode.GetComponent<CardboardAudioSource>().volume;

    //     while (i <= 1.0F) 
    //     {
    //         i += step * Time.deltaTime;
    //         djNode.GetComponent<CardboardAudioSource>().volume = Mathf.Lerp(currentVolume, end, i);
    //         yield return new WaitForSeconds(step * Time.deltaTime);
    //     }
    // }

}

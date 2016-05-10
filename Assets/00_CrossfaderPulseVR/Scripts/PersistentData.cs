using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
// added for Cardboard camera fade
using VRStandardAssets.Utils;


public class PersistentData : MonoBehaviour {

	public static PersistentData PD;
	public float curSongTime;
	public string performanceId;
	public int loadNum = 0;
	private bool isFading = false;
	public string scene;
	public List<AudioSource> audioSources;

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
		
		if (loadNum > 1)
		{
			GameObject.Find("OnboardingUI").SetActive(false);
		}
		
		GameObject.Find("Planet960tris").GetComponent<MeshRenderer>().enabled = true;
		AudioListener.volume = 0;
		StartCoroutine(FadeAudioListener(5, Fade.In));
		
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
		// else if (SceneManager.GetActiveScene().name == "01_Cardboard_RootLevel_v1")
		// {
		// 	curSongTime = 0;
		// }
	}

	public void ChangeLevel (int sceneBuild, float fadeDur, GameObject node)
	{
		StartCoroutine(FadeLevelChange(sceneBuild, fadeDur, node));
	}

    IEnumerator FadeLevelChange(int sceneBuild, float fadeDur, GameObject node)
    {
    	isFading = true;
		scene = SceneManager.GetActiveScene().name;
        GameObject camera = GameObject.Find("Main Camera");
        camera.GetComponent<VRCameraFade>().FadeOut(fadeDur, false);
		if (scene == "01_Cardboard_RootLevel_v1")
		{
			audioSources = AudioVisualizer.AudioSampler.instance.audioSources;
    		for (int i = 0; i < audioSources.Count; i++)
			{
				StartCoroutine(FadeAudioSource(fadeDur, Fade.Out, audioSources[i]));
			}
		}
		else if (scene == "02_Cardboard_DJLevel_v2")
		{
        	StartCoroutine(FadeAudioListener(fadeDur, Fade.Out));
		}
		yield return new WaitForSeconds(fadeDur);
		camera.SetActive(false);
        //cache current time of song
		if (scene == "01_Cardboard_RootLevel_v1")
		{
			if (node != null)
			{
				curSongTime = node.GetComponent<CardboardAudioSource>().audioSource.time;
			performanceId = node.transform.parent.name;
			}
			else
			{
				Debug.Log("Node has already been destroyed");
			}
		}
		else if (scene == "02_Cardboard_DJLevel_v2")
		{
			curSongTime = GameObject.Find("AudioSampler").GetComponent<AudioSource>().time;
		}
		Debug.Log("Playhead = " + curSongTime);
		
        SceneManager.LoadScene(sceneBuild); 
        isFading = false;
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
	
	IEnumerator FadeAudioSource (float timer, Fade fadeType, AudioSource audioSource) 
    {
        // float start = fadeType == Fade.In? 0.0F : 1.0F;
        float end = fadeType == Fade.In? 1.0F : 0.0F;
        float i = 0.0F;
        float step = 1.0F/timer;
        float currentVolume = audioSource.volume;

        while (i <= 1.0F) 
        {
            i += step * Time.deltaTime;
            audioSource.volume = Mathf.Lerp(currentVolume, end, i);
            yield return new WaitForSeconds(step * Time.deltaTime);
        }
    }

	IEnumerator _LoadName()
	{
		string name = api.performancesDict[performanceId]["users"][0]["dj_name"].ToString();
		yield return name;
		djName.text = name.Trim('"');
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

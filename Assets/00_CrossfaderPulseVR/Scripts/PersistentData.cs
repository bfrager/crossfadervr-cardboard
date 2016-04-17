using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class PersistentData : MonoBehaviour {

	public static PersistentData PD;
	public float curSongTime;

	enum Fade {In, Out};

	// Use this for initialization
	void Awake () 
	{
		DontDestroyOnLoad(gameObject);
		PD = this;



	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnLevelWasLoaded()
	{
		AudioListener.volume = 0;
		StartCoroutine(FadeAudio(5, Fade.In));

		print ("listner: " + AudioListener.volume);
		if (SceneManager.GetActiveScene().name == "02_Cardboard_DJLevel_v2")
		{
			print (GameObject.Find("DJ_Room").transform.name);
			GameObject.Find("AudioSampler").GetComponent<AudioSource>().time = curSongTime;
		}
		else{
			curSongTime = 0;
		}
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
}

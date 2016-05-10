using UnityEngine;
using System.Collections;

public class DJSongPassOff : MonoBehaviour {

	AudioSource source;
	// Use this for initialization
	void Start () 
	{
		source = gameObject.GetComponent<AudioSource>();
		source.time = PersistentData.PD.curSongTime;
		source.Play();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

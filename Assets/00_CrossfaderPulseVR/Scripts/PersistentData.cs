using UnityEngine;
using System.Collections;

public class PersistentData : MonoBehaviour {

	public static PersistentData PD;
	public float curSongTime;

	// Use this for initialization
	void Start () 
	{
		DontDestroyOnLoad(gameObject);
		PD = this;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}

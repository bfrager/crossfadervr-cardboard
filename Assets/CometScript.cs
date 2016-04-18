using UnityEngine;
using System.Collections;

public class CometScript : MonoBehaviour {

	public float resetTime = 5;

	public GameObject comet1;
	public GameObject comet2;

	public Vector3 start1;
	public Vector3 start2;

	bool canReset = false;


	// Use this for initialization
	void Start () 
	{
		start1 = comet1.transform.position;
		start2 = comet2.transform.position;

	}
	
	// Update is called once per frame
	void Update () 
	{
		StartCoroutine("ResetAfterTime", resetTime);

		if(canReset)
		{
			comet1.SetActive(false);
			comet2.SetActive(false);
			comet1.transform.position = start1;
			comet2.transform.position = start2;
			comet1.SetActive(true);
			comet2.SetActive(true);
			canReset = false;
		}
	
	}

	IEnumerator ResetAfterTime(float time)
	{
		while (true)
		{
			yield return new WaitForSeconds(time);
			canReset = true;
		}

	}
}

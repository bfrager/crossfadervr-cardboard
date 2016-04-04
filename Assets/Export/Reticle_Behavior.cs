using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Reticle_Behavior : MonoBehaviour {

	public bool canCount = false;
	public float countdownSeconds;
	private float timeRemaining;
	public Image reticle;

	// Use this for initialization
	void Start () 
	{
		timeRemaining = countdownSeconds;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (canCount)
		{
			Counter();
		}
		else 
		reticle.fillAmount = 0;
	}

	public void Counter()
	{

		reticle.fillAmount -= -1/timeRemaining * Time.deltaTime;;

		if(timeRemaining <= 0)
		{
			canCount = !canCount;
			timeRemaining = countdownSeconds;
			Action();
		}
	}

	void Action()
	{
		Debug.Log("This is a Boom");
	}
}

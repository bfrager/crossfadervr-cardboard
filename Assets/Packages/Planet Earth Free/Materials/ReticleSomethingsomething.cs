using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReticleSomethingsomething : MonoBehaviour {

	public Image img;
	public float slider;

	// Use this for initialization
	void Start () 
	{
		img = gameObject.GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void ReticleTiming(float value)
	{
		img.fillAmount = slider;
	}
}

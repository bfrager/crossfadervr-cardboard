using UnityEngine;
using System.Collections;

public class VeinFadeIn : MonoBehaviour {

public GameObject[] veins;

	// Use this for initialization
	void Start () {
		veins = GameObject.FindGameObjectsWithTag("Vein");
		for(int i = 0; i < veins.Length; i++)
		{
			veins[i].transform.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0);
		}
		
		//LOAD VEINS
		for(int i = 0; i < veins.Length; i++)
        {
           StartCoroutine(FadeTo(1, 1f, veins[i]));
        }
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	IEnumerator FadeTo(float aValue, float aTime, GameObject obj)
	{
		float alpha = obj.transform.GetComponent<Renderer>().material.color.a;
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
		{
			Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha,aValue,t));
			obj.transform.GetComponent<Renderer>().material.color = newColor;
			yield return null;
		}
	}
}

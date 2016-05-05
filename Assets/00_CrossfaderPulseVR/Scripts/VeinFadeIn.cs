using UnityEngine;
using System.Collections;

public class VeinFadeIn : MonoBehaviour {

public GameObject[] veins;
public ApiCall api;

	void Awake()
	{
		api = ApiCall.instance;
		api.djLoaded += djLoaded;
	}
	
	// Use this for initialization
	void Start() 
	{
		veins = GameObject.FindGameObjectsWithTag("Vein");
		
		for(int i = 0; i < veins.Length; i++)
		{
			veins[i].transform.GetComponent<Animator>().enabled = false;
			veins[i].transform.GetComponent<Renderer>().enabled = false;
			
			
			veins[i].transform.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0);
		}
	}
	
	void djLoaded(string perfId)
	{
		//LOAD VEIN
		for(int i = 0; i < veins.Length; i++)
        {
			StartCoroutine(FadeTo(1, 1f, veins[i]));
        }
	}
	
	IEnumerator FadeTo(float aValue, float aTime, GameObject obj)
	{
		// yield return new WaitForSeconds(5f);
		obj.transform.GetComponent<Animator>().enabled = true;
		obj.transform.GetComponent<Renderer>().enabled = true;
		
		
		float alpha = obj.transform.GetComponent<Renderer>().material.color.a;
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
		{
			Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha,aValue,t));
			obj.transform.GetComponent<Renderer>().material.color = newColor;
			yield return null;
		}
	}
}

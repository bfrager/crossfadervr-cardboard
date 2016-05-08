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
		
		// for(int i = 0; i < veins.Length; i++)
		// {
		// 	// veins[i].transform.GetComponent<Animator>().enabled = false;

		// 	// Color startColor = veins[i].transform.GetComponent<Renderer>().material.color;
		// 	// startColor.a = 0.0f;
		// 	// veins[i].transform.GetComponent<Renderer>().material.color = startColor;
			
		// 	veins[i].transform.GetComponent<Renderer>().enabled = false;
		// }
	}
	
	void djLoaded(string perfId)
	{
		//LOAD VEIN
		for(int i = 0; i < veins.Length; i++)
        {
			veins[i].transform.GetComponent<Renderer>().enabled = true;
			// StartCoroutine(FadeTo(1f, 1f, veins[i]));
			
			//TODO: Add alpha/opacity fade and enable renderer only after specific linked DJ node is loaded
        }
	}
	
	IEnumerator FadeTo(float aValue, float aTime, GameObject obj)
	{		
		Color color = obj.transform.GetComponent<Renderer>().material.color;
 
		// float alpha = obj.transform.GetComponent<Renderer>().material.color.a;

		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
		{
			color.a += 0.1f;
			obj.transform.GetComponent<Renderer>().material.color = color;
			
			// Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha,aValue,t));
			// obj.transform.GetComponent<Renderer>().material.SetColor("_Color", newColor);
			
			yield return null;
		}
	}
}

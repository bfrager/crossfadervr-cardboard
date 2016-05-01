using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

class PlaceNodes : MonoBehaviour {

public GameObject djNodePrefab;
public GameObject[] djNodes;

private MeshRenderer mr;
private ApiCall api;


void Start () 
{
    djNodes = GameObject.FindGameObjectsWithTag("djNode");
    for (int i = 0; i < djNodes.Length - 1; i++) 
    {
        djNodes[i].active = true;
        string perfId = djNodes[i].name;
        Debug.Log("Name = " + perfId);
        
        mr = djNodes[i].GetComponent<MeshRenderer>();
        
		StartCoroutine(LoadAvatar(perfId, mr));
             
        print("DJ Loop number " + i);
	}
}

void Update () 
{
    foreach (GameObject node in djNodes) 
    {
        node.active = true;
    }
}

IEnumerator LoadAvatar(string perfId, MeshRenderer mrender)
{
		string avatarUrl = api.performancesDict[perfId]["users"][0]["avatar"].ToString();
		string[] temp = avatarUrl.Split('\"');
		avatarUrl = temp[1];
		WWW imgUrl = new WWW(avatarUrl);
		yield return imgUrl;
		mrender.materials[1].mainTexture = imgUrl.texture;
}
}
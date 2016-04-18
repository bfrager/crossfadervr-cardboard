using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

class PlaceNodes : MonoBehaviour {

public GameObject djNodePrefab;
public GameObject[] djNodes;

void Start () 
{
    djNodes = GameObject.FindGameObjectsWithTag("djNode");
    for (int i = 0; i < djNodes.Length - 1; i++) 
    {
        // Instantiate(djNodePrefab, node.transform.position, node.transform.rotation);
        djNodes[i].active = true;
        // Debug.Log(node);
        string perfId = djNodes[i].name;
        Debug.Log("Name = " + perfId);
        
        // TODO: Set performanceId in persistent data to pass song and metadata lookup info
        // PersistentData.PD.performanceId = perfId;
        
        // add DJ canvas variables along with child gameobjects
        Transform canvas = djNodes[i].transform.Find("Dj_Info_Canvas");
        Transform djName = canvas.transform.Find("Dj_Name");
        Transform location = canvas.transform.Find("Location");
        Transform hearts = canvas.transform.Find("Hearts");
        Transform tags = canvas.transform.Find("Tags");

        // test call to Performances Dictionary containing metadata
		//var perfJson = GameObject.Find("PersistentData").GetComponent(ApiCall.cs).performancesDict[perfId];
        
		StartCoroutine(TestCall(perfId));
        
        //set dj info canvas text
        location.GetComponent<Text>().text = "USA";
        djName.GetComponent<Text>().text = "DJ Zedd";    
        hearts.GetComponent<Text>().text = "78";
        tags.GetComponent<Text>().text = "#trance #house #techno";
        
        //set canvas fonts    
        // djName.GetComponent(Text).character.font = "YOZAKURA-Regular";   
             
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

IEnumerator TestCall(string perfId)
{
	yield return new WaitForSeconds(1);
		string perfJson = GameObject.Find("PersistentData").GetComponent<ApiCall>().performancesDict[perfId].ToString();
		Debug.Log(perfJson);
}
}
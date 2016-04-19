#pragma strict
import UnityEngine.UI;

public var djNodePrefab: GameObject;
public var djNodes: GameObject[];

function Start () 
{
    djNodes = GameObject.FindGameObjectsWithTag("djNode");
    for (var node: GameObject in djNodes) 
    {
        // Instantiate(djNodePrefab, node.transform.position, node.transform.rotation);
        node.active = true;
        // Debug.Log(node);
        var perfId = node.name;
        
        // TODO: Set performanceId in persistent data to pass song and metadata lookup info
        // PersistentData.PD.performanceId = perfId;
        
        // add DJ canvas variables along with child gameobjects
        var canvas = node.transform.Find("Dj_Info_Canvas");
        var djName = canvas.transform.Find("Dj_Name");
        var location = canvas.transform.Find("Location");
        var hearts = canvas.transform.Find("Hearts");
        var tags = canvas.transform.Find("Tags");

        // test call to Performances Dictionary containing metadata
        // var perfJson = GameObject.Find("PersistentData").GetComponent(ApiCall.cs).performancesDict[perfId];
        // Debug.Log(perfJson);
        
        //set dj info canvas text
        location.GetComponent(Text).text = "USA";
        djName.GetComponent(Text).text = "DJ Zedd";    
        hearts.GetComponent(Text).text = "78";
        tags.GetComponent(Text).text = "#trance #house #techno";
        
        //set canvas fonts    
        // djName.GetComponent(Text).character.font = "YOZAKURA-Regular";        

	}
}

function Update () 
{
    for (var node: GameObject in djNodes) 
    {
        node.active = true;
    }
}
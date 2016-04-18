using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;


// based on https://github.com/andyburke/UnityHTTP

public class ApiCall : MonoBehaviour {
    
    private string url;
    private string apiEndpoint;
    // private JsonData itemData;
    public List<int> performanceIds = new List<int>();
    private List<JSONObject> performances = new List<JSONObject>();
    public Dictionary<string, JSONObject> performancesDict = new Dictionary<string, JSONObject>();


	void Start () {
        //add performance IDs of DJ Nodes to grab
        performanceIds.AddRange (new int[]{734638, 722131, 734615, 734598, 550352, 709722, 741630, 681878, 734698, 734668, 734734, 722237, 695891, 669405, 695743});
                
        // Send Request to Crossfader Server
        apiEndpoint = "http://api.crossfader.fm/v3/performances/";

	    foreach(int id in performanceIds)
        {
            string idString = id.ToString();
            url = apiEndpoint + idString;
            
            HTTP.Request crossFaderData = new HTTP.Request( "get", url );
            
            crossFaderData.Send( ( request ) => 
            {
                //add response to performances list
                performances.Add(new JSONObject( request.response.Text ));
                performancesDict.Add(idString, new JSONObject( request.response.Text ));

                //NAVIGATE JSON OBJECTS BY PERFORMANCE ID:                
                // Debug.Log(performancesDict[idString]);   
                // Debug.Log(performancesDict[idString]["performance"]["title"]);   
                             
                // // Convert response string to JSON object
                // JSONObject cfData = new JSONObject( request.response.Text );
                
                // performances.Add(cfData);
                // Debug.Log(performances);
                
                // // access data in response object
                // accessData(cfData["performances"][idString]["performance"]["title"]);
                
                // // alt syntax to access data
                // JSONObject perf = cfData["performances"][idString]["performance"];
                // Debug.Log(perf["title"]); 
            });
            
        }

        
        
        
        // LitJson
        // itemData = JsonMapper.ToObject(cfData);
        // Debug.Log(itemData["performances"][1]["title"]);
        // GetItem(783984, "performances");
        // GetItem(cfData, 783984, "performances");
        
        
        //simpleJSON             
        // TODO: PARSE JSON DATA (SIMPLEJSON FUNCTION NOT WORKING)
        //     performances = JsonConvert.DeserializeObject<List<Performance>>(cfData);
        //     cfDataParsed = JSON.Parse(cfData);
            
        //     foreach(JSONClass performance in cfData["performances"]) 
        //     {  
        //         int id = performance.AsObject["id"].Value;
        //         int duration_seconds = performance.AsObject["duration_seconds"].Value;
        //         int user_id = performance.AsObject["user_id"].Value;
        //         performances.Add(new Performance(id, duration, user_id));
        //     }
        //     Debug.Log(performances.Count);
        //     Debug.Log(performances);
        
	}

    void Update () {
	
	}
    
    //access data (and print it)
    void accessData(JSONObject obj)
    {
        switch(obj.type)
        {
            case JSONObject.Type.OBJECT:
                for(int i = 0; i < obj.list.Count; i++){
                    string key = (string)obj.keys[i];
                    JSONObject j = (JSONObject)obj.list[i];
                    Debug.Log(key);
                    accessData(j);
                }
                break;
            case JSONObject.Type.ARRAY:
                foreach(JSONObject j in obj.list){
                    accessData(j);
                }
                break;
            case JSONObject.Type.STRING:
                Debug.Log(obj.str);
                break;
            case JSONObject.Type.NUMBER:
                Debug.Log(obj.n);
                break;
            case JSONObject.Type.BOOL:
                Debug.Log(obj.b);
                break;
            case JSONObject.Type.NULL:
                Debug.Log("NULL");
                break;
        }
    }
    
    void AddToList(params int[] list) {
          for ( int i = 0 ; i < list.Length ; i++ ) {
             performanceIds.Add(list[i]);
          }
     }

//UNUSED CODE (DLL LIBRARY AS ALTERNATE JSON DECODER)
    // LitJson	
    //     JSONObject GetItem(JSONObject obj, int id, string type)
    //     {
    //         for (int i = 0; i < obj[type].Count; i++)
    //         {
    //             if (int.Parse(obj[type][i]["id"]) == id)
    //                 return obj[type][i];
    //         }
    //     }

    //     JsonData GetItem(int id, string type)
    //     {
    //         for (int i = 0; i < itemData[type].Count; i++)
    //         {
    //             if (itemData[type][i]["id"] == id)
    //                 return itemData[type][i];
    //         }
    //     }
    
	
}

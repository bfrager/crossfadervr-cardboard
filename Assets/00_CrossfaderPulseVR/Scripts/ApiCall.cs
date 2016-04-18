using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// based on https://github.com/andyburke/UnityHTTP

public class ApiCall : MonoBehaviour {
    
    public static ApiCall AC;
    private string url;
    private string apiEndpoint;
    public List<int> performanceIds = new List<int>();
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
                performancesDict.Add(idString, new JSONObject( request.response.Text ));

                //NAVIGATE JSON OBJECTS BY PERFORMANCE ID:                
                Debug.Log(performancesDict[idString]["performance"]["title"]);   
                          
                // // access data in response object
                // accessData(performancesDict[idString]["performance"]["title"]);

            });
            
        }
        
        // //test saving dictionary to PersistentData static class for access globally
        // PersistentData.PD.performancesDict = performancesDict;
        // Debug.Log(PersistentData.PD.performancesDict[performanceIds[0].ToString()]["user"]["title"]);
        
        // // Test accessing dictionary outside of foreach loop (maybe yield until server response?)
        // Debug.Log(performancesDict[performanceIds[1].ToString()]["users"]["dj_name"]);
        
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
	
}

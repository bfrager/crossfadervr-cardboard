using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using LitJson;


// based on https://github.com/andyburke/UnityHTTP

public class ApiCall : MonoBehaviour {
    
    private string cfDataParsed;
    private JsonData itemData;
    // private List<Performance> performances = new List<Performance>();

	void Start () {
	    HTTP.Request crossFaderData = new HTTP.Request( "get", "http://api.crossfader.fm/v3/performances" );
        crossFaderData.Send( ( request ) => 
        {
            JSONObject cfData = new JSONObject( request.response.Text );
            Debug.Log(cfData);
            accessData(cfData);
            
            
           // LitJson
            // itemData = JsonMapper.ToObject(cfData);
            // Debug.Log(itemData["performances"][1]["title"]);
            // GetItem(783984, "performances");
            
            
           //simpleJSON             
        // TODO: PARSE JSON DATA (SIMPLEJSON FUNCTION NOT WORKING)
            // performances = JsonConvert.DeserializeObject<List<Performance>>(cfData);
            // cfDataParsed = JSON.Parse(cfData);
            // foreach(JSONClass performance in cfDataParsed["performances"].AsArray) 
            // {  
            //     int id = performance.AsObject["id"].Value;
            //     int duration_seconds = performance.AsObject["duration_seconds"].Value;
            //     int user_id = performance.AsObject["user_id"].Value;
            //     performances.Add(new Performance(id, duration, user_id));
            // }
            // Debug.Log(performances.Count);
            // Debug.Log(performances);
        });
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

//LitJson	
    // JsonData GetItem(int id, string type)
    // {
    //     for (int i = 0; i < itemData[type].Count; i++)
    //     {
    //         if (itemData[type][i]["id"] == id)
    //             return itemData[type][i];
    //     }
    // }
    
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

// based on https://github.com/andyburke/UnityHTTP

public class ApiCall : MonoBehaviour {
    
    private string cfDataParsed;
    // private List<Performance> performances = new List<Performance>();

	void Start () {
	    HTTP.Request crossFaderData = new HTTP.Request( "get", "http://api.crossfader.fm/v3/performances" );
        crossFaderData.Send( ( request ) => 
        {
            JSONObject cfData = new JSONObject( request.response.Text );
            Debug.Log(cfData);
                        
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
	
	void Update () {
	
	}
}

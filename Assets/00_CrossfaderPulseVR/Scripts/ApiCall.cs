using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class ApiCall : MonoBehaviour {
    
    private string cfDataParsed;
    private List<Performance> performances = new List<Performance>();

	// Use this for initialization
	void Start () {
	    HTTP.Request crossFaderData = new HTTP.Request( "get", "http://api.crossfader.fm/v3/performances" );
        crossFaderData.Send( ( request ) => 
        {
            // parse some JSON, for example:
            JSONObject cfData = new JSONObject( request.response.Text );
            Debug.Log(cfData);
            // performances = JsonConvert.DeserializeObject<List<Performance>>(cfData);
            cfDataParsed = JSON.Parse(cfData);
            foreach(JSONClass performance in cfDataParsed["performances"].AsArray) 
            {  
                int id = performance.AsObject["id"].Value;
                int duration_seconds = performance.AsObject["duration_seconds"].Value;
                int user_id = performance.AsObject["user_id"].Value;
                performances.Add(new Performance(id, duration, user_id));
            }
            Debug.Log(performances.Count);
            Debug.Log(performances);
        });
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

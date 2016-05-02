using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// based on https://github.com/andyburke/UnityHTTP

public class ApiCall : MonoBehaviour {
    public static ApiCall instance;
    
    public delegate void DjLoaded(string perfId);
    public event DjLoaded djLoaded;
        
    private string url;
    private string apiEndpoint;
    public List<int> performanceIds = new List<int>();
    public Dictionary<string, JSONObject> performancesDict = new Dictionary<string, JSONObject>();

	void Awake () {
        instance = this;
        
        //add performance IDs of DJ Nodes to grab
        performanceIds.AddRange (new int[]{734638, 722131, 734615, 734598, 550352, 709722, 741630, 681878, 734698, 734668, 734734, 722237, 695891, 669405, 695743});
                
        // Send Request to Crossfader Server for each performance
        apiEndpoint = "http://api.crossfader.fm/v3/performances/";

	    foreach(int id in performanceIds)
        {
            string idString = id.ToString();
            url = apiEndpoint + idString;
            HTTP.Request crossFaderData = new HTTP.Request( "get", url );
            
            crossFaderData.Send( ( request ) => 
            {
                //add response JSON object to performances dictionary
                performancesDict.Add(idString, new JSONObject( request.response.Text ));
                
                //run delegate function to trigger event listener on corresponding djNode to load into globe
                djLoaded(idString);
            });  
        }
	}
}

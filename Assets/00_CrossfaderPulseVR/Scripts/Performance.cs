using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System; //This allows the IComparable Interface

//This is the class you will be storing
//in the different collections. In order to use
//a collection's Sort() method, this class needs to
//implement the IComparable interface.
// public class Performance : IComparable<Performance>
// {
//     public int id;
//     public int duration_seconds;
//     public int user_id;
    
//     public Performance(int newId, int newDuration_seconds, int newUser_id)
//     {
//         id = newId;
//         duration_seconds = newDuration_seconds;
//         user_id = newUser_id;
//     }
// }

public class Performance
{
    public int id { get; set; }
    public string created_at { get; set; }
    public string updated_at { get; set; }
    public int segment_count { get; set; }
    public List<string> segment_loops { get; set; }
    public int segment_loop_count { get; set; }
    public int duration { get; set; }
    public string title { get; set; }
    public string slug { get; set; }
    public string completed_at { get; set; }
    public int favorites_count { get; set; }
    public object favorited_at { get; set; }
    public int listens_count { get; set; }
    public List<string> tags { get; set; }
    public int duration_seconds { get; set; }
    public int user_id { get; set; }
    public string audio_url { get; set; }
    public int version { get; set; }
    public object location { get; set; }
    public string background_image { get; set; }
    public int reports_count { get; set; }
    public int listeners_count { get; set; }
    public int live_listeners_count { get; set; }
    public string published_at { get; set; }
    public bool is_on_main_stage { get; set; }
}

public class RootObject
{
    public List<Performance> performances { get; set; }
}
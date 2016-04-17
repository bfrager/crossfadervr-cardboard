using System;
 using UnityEngine;
 
 public class AudioList : MonoBehaviour
 {
 
 AudioSource[] sources;
             
             void Start () {
             
                  //Get every single audio sources in the scene.
                 sources = GameObject.FindSceneObjectsOfType(typeof(AudioSource)) as AudioSource[];
                 
             }
         
             void Update () {
                         
                 // When a key is pressed list all the gameobjects that are playing an audio
                 if(Input.GetKeyUp(KeyCode.A))
                 {
                     for (var i = 0; i < sources.Length; i++)
                     
                    //  foreach(AudioSource audioSource in sources)
                     {
                         if(sources[i].isPlaying) Debug.Log(i+": "+sources[i].name+" is playing "+sources[i].clip.name);
                     }
                     Debug.Log("---------------------------"); //to avoid confusion next time
                     Debug.Break(); //pause the editor
                     
                 }
             }
 }
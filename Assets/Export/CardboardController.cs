﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Reflection;
using UnityEngine.SceneManagement;
//Gavin: added for Cardboard camera fade
using VRStandardAssets.Utils;

public class CardboardController : MonoBehaviour {
    private static CardboardControl cardboard;
	public Image radialFill;
    private TextMesh textMesh;
    private TextMesh textMesh2;
    private AudioSource[] audioSources;

	// Use this for initialization
	void Start () {
        Cardboard.SDK.VRModeEnabled = true;
        AudioSource[] audioSources = Object.FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        Debug.Log(audioSources);
        textMesh = GameObject.Find("Counter").GetComponent<TextMesh>();
        textMesh2 = GameObject.Find("Message").GetComponent<TextMesh>();
        textMesh.GetComponent<Renderer>().enabled = false;
        textMesh2.GetComponent<Renderer>().enabled = false;
	    cardboard = GameObject.Find("CardboardControlManager").GetComponent<CardboardControl>();
        cardboard.trigger.OnDown += CardboardDown;  // When the trigger goes down
        cardboard.trigger.OnUp += CardboardUp;      // When the trigger comes back up

        // When the magnet or touch goes down and up within the "click threshold" time
        // That click speed threshold is configurable in the Inspector
        cardboard.trigger.OnClick += CardboardClick;

        // When the thing we're looking at changes, determined by a gaze
        // The gaze distance and layer mask are public as configurable in the Inspector
        cardboard.gaze.OnChange += CardboardGazeChange;

        // When we've been staring at an object
        cardboard.gaze.OnStare += CardboardStare;

        // When we rotate the device into portrait mode
        cardboard.box.OnTilt += CardboardMagnetReset;
    }
        
    private void CardboardDown(object sender) {
        // Debug.Log("Trigger went down");
        // ChangeObjectColor("SphereDown");
    }

    private void CardboardUp(object sender) {
        // Debug.Log("Trigger came up");
        // ChangeObjectColor("SphereUp");
    }

    private void CardboardClick(object sender) {
        // ChangeObjectColor("SphereClick");

        // TextMesh textMesh = GameObject.Find("SphereClick/Counter").GetComponent<TextMesh>();
        // int increment = int.Parse(textMesh.text) + 1;
        // textMesh.text = increment.ToString();

        // With the cardboard object, we can grab information from various controls
        // If the raycast doesn't find anything then the focused object will be null
        string name = cardboard.gaze.IsHeld() ? cardboard.gaze.Object().name : "nothing";
        float count = cardboard.gaze.SecondsHeld();
        Debug.Log("We've focused on "+name+" for "+count+" seconds.");

        // TODO: LOCK ONTO TRACK HERE INSTEAD OF ON COLLISION OBJECTS
        
        // If you need more raycast data from cardboard.gaze, the RaycastHit is exposed as gaze.Hit()
    }

    private void CardboardGazeChange(object sender) {
        // You can grab the data from the sender instead of the CardboardControl object
        CardboardControlGaze gaze = sender as CardboardControlGaze;
        // We can access to the object we're looking at
        // gaze.IsHeld will make sure the gaze.Object() isn't null
        if (gaze.IsHeld() && gaze.Object().name.Contains("Spatialized")) {
            // ChangeObjectColor(gaze.Object().name);
            if (gaze.Object().name == "Spatialized") {
                // Highlighting can help identify which objects can be interacted with
                // The reticle is hidden by default but we already toggled that in the Inspector
                cardboard.reticle.Highlight(Color.red);        
            }
        }
        // We also can access to the last object we looked at
        // gaze.WasHeld() will make sure the gaze.PreviousObject() isn't null
        if (gaze.WasHeld() && gaze.PreviousObject().name.Contains("Spatialized")) {
        ResetObjectColor(gaze.PreviousObject().name);
        // Use these to undo reticle hiding and highlighting
        cardboard.reticle.Show();
        cardboard.reticle.ClearHighlight();
        }

        // Be sure to set the Reticle Layer Mask on the CardboardControlManager
        // to grow the reticle on the objects you want. The default is everything.

        // Not used here are gaze.Forward(), gaze.Right(), and gaze.Rotation()
        // which are useful for things like checking the view angle or shooting projectiles
    }

    private void CardboardStare(object sender) {
        CardboardControlGaze gaze = sender as CardboardControlGaze;
        if (gaze.IsHeld() && gaze.Object().name.Contains("Spatialized")) {
            
            // TOGGLE ROOT/DJ LEVEL

            // if (SceneManager.GetActiveScene().buildIndex == 0) {
            //     SceneManager.LoadScene(1);
            // }
            // else if (SceneManager.GetActiveScene().buildIndex == 1) {
            //     SceneManager.LoadScene(0);
            // }
            // m_someOtherScriptOnAnotherGameObject = GameObject.FindObjectOfType(typeof(ScriptA)) as ScriptA;

            
            // Be sure to hide the cursor when it's not needed
            cardboard.reticle.Hide();
        }

    }

    private void CardboardMagnetReset(object sender) {
        // Resetting the magnet will reset the polarity if up and down are confused
        // This occasionally happens when the device is inserted into the enclosure
        // or if the magnetometer readings are weak enough to cut in and out
        Debug.Log("Device tilted");
        cardboard.trigger.ResetMagnetState();
    }

    private void ChangeObjectColor(string name) {
        GameObject obj = GameObject.Find(name);
        Color newColor = Color.blue;
        obj.GetComponent<Renderer>().material.color = newColor;
    }

    private void ResetObjectColor(string name) {
        GameObject.Find(name).GetComponent<Renderer>().material.color = Color.white;
    }
	
	// Update is called once per frame
	void Update () {
        //Countdown timer on GUI
        if (cardboard.gaze.IsHeld()) {
            Debug.Log(cardboard.gaze.Object());
            //ROOT LEVEL CONTROLS:
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                if (cardboard.gaze.Object().name.Contains("Heart"))
                {
                    textMesh2.text = "Feel the Global Pulse";
                    textMesh2.GetComponent<Renderer>().enabled = Time.time % 1 < 0.5;                  
                }
                if (cardboard.gaze.SecondsHeld() > 3 && cardboard.gaze.SecondsHeld() < 8) {
                    if (cardboard.gaze.Object().name.Contains("Diamond"))
                    {
                        textMesh2.text = "Enter DJ Room In:";
                        textMesh2.GetComponent<Renderer>().enabled = true;          
                        textMesh.GetComponent<Renderer>().enabled = true;
                        textMesh.text = (8 - cardboard.gaze.SecondsHeld()).ToString("#");
                    }
                }
                else if (8 < cardboard.gaze.SecondsHeld() && cardboard.gaze.SecondsHeld() < 10) {
                    if (cardboard.gaze.Object().name.Contains("Diamond"))
                    {
                        textMesh2.GetComponent<Renderer>().enabled = false;
                        textMesh.text = "DJ Selected";
                        textMesh.GetComponent<Renderer>().enabled = Time.time % 1 < 0.5;         
                    }
                }
                else if (cardboard.gaze.SecondsHeld() > 10) {
                    if (!cardboard.gaze.Object().name.Contains("Heart")) {

                    	//Gavin: Fade out camera before changing scenes
                    	//Send scene index to load and the fade duration
                    	StartCoroutine(FadeLevelChange(1,0.8f));
                        //SceneManager.LoadScene(1);    
                                            
                    }
                }
            }
            //DJ LEVEL CONTROLS:
            else if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                if (cardboard.gaze.SecondsHeld() > 0 && cardboard.gaze.SecondsHeld() < 5) 
                {
                    if (cardboard.gaze.Object().name.Contains("Heart"))
                    {
                        textMesh2.text = "Return to Globe In: ";
                        textMesh2.GetComponent<Renderer>().enabled = true;          
                        textMesh.GetComponent<Renderer>().enabled = true;
                        textMesh.text = (5 - cardboard.gaze.SecondsHeld()).ToString("#");
                    }         
                }
                else if (cardboard.gaze.SecondsHeld() > 5) {
                    if (cardboard.gaze.Object().name.Contains("Heart")) {

						//Gavin: Fade out camera before changing scenes
                    	//Send scene index to load and the fade duration
                    	StartCoroutine(FadeLevelChange(0,0.8f));
                        //SceneManager.LoadScene(0);
                    }
                }
            }
        }
        else {
            textMesh.GetComponent<Renderer>().enabled = false;
            textMesh2.GetComponent<Renderer>().enabled = false;

        }
        
        // // Check on gaze and add radial to reticle
        // if (cardboard.gaze.IsHeld())
		// 	cardboard.reticle.radialFill.fillAmount += (Time.deltaTime * fillSpeed);
		// else
		// 	cardboard.reticle.radialFill.fillAmount -= (Time.deltaTime * fillSpeed);

		// if (cardboard.reticle.radialFill.fillAmount >= 1f) {
		// 	cardboard.reticle.radialFill.fillAmount = 1f;

		// 	if (!cardboard.gaze.IsHeld()) { //prevent multiple triggers
		// 		_selected = true;
		// 		_OnClick.Invoke();
		// 		Debug.Log("CLICK!");
		// 	}
		// }
		// else if (cardboard.reticle.radialFill.fillAmount <= 0f) {
		// 	_selected = false;
		// 	radialFill.fillAmount = 0f;
		// }  
	}
    
    void OnDestroy() {
        cardboard.trigger.OnDown -= CardboardDown;
        cardboard.trigger.OnUp -= CardboardUp;
        cardboard.trigger.OnClick -= CardboardClick;
        cardboard.gaze.OnChange -= CardboardGazeChange;
        cardboard.gaze.OnStare -= CardboardStare;
        cardboard.box.OnTilt -= CardboardMagnetReset;
    }

    IEnumerator FadeLevelChange(int sceneIndex, float fadeDur)
    {
		transform.GetChild(0).GetChild(0).GetComponent<VRCameraFade>().FadeOut(fadeDur, false);
		yield return new WaitForSeconds(fadeDur);

		//cache current time of song
		float playHead = cardboard.gaze.Object().GetComponent<CardboardAudioSource>().audioSource.time;
		PersistentData.PD.curSongTime = playHead;

        SceneManager.LoadScene(sceneIndex); 
    }

}
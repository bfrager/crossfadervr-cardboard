using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Reflection;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
// added for Cardboard camera fade
using VRStandardAssets.Utils;

public class CardboardController : MonoBehaviour {
    public static CardboardControl cardboard;
    public static CardboardController cardboardController;
	public Image radialFill;
    private TextMesh textMesh;
    private TextMesh textMesh2;
    public GameObject planet;
    public GameObject curNode;
    public Color textColor = new Color(255/255.0f, 255/255.0f, 0/255.0f, 255/255.0f);
    enum Fade {In, Out};
    public string scene;
    public bool locked = false;
    private IEnumerator countdownTimer;
    public int countdownLength = 3;
    private int countdown;
    public UnityEngine.Audio.AudioMixerGroup mixes;
    public UnityEngine.Audio.AudioMixerGroup heartbeat;
    public UnityEngine.Audio.AudioMixerGroup sfx;
    
	// Use this for initialization
	void Awake()
    {
        textMesh = GameObject.Find("Counter").GetComponent<TextMesh>();
        textMesh2 = GameObject.Find("Message").GetComponent<TextMesh>();
    }
    
    void Start () 
    {
        cardboardController = this;
        cardboard = gameObject.GetComponent<CardboardControl>();

        // Cardboard.SDK.VRModeEnabled = true;        
        
        // AudioSource[] audioSources = Object.FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        // Debug.Log(audioSources);

		planet = GameObject.Find("Planet960tris");
        scene = SceneManager.GetActiveScene().name;
        
        textMesh.GetComponent<Renderer>().enabled = false;
        textMesh2.GetComponent<Renderer>().enabled = false;
        textMesh.color = textColor;
        textMesh2.color = textColor;
        
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
        
        if (scene == "02_Cardboard_DJLevel_v2")
        {
            cardboard.reticle.Hide();
            print("hiding reticle");
        }
    }
        
    private void CardboardDown(object sender) {
        // Debug.Log("Trigger went down");
    }

    private void CardboardUp(object sender) {
        // Debug.Log("Trigger came up");
    }

    private void CardboardClick(object sender) {
        // With the cardboard object, we can grab information from various controls
        // If the raycast doesn't find anything then the focused object will be null
        
        if (cardboard.gaze.IsHeld())
        { 
            curNode.GetComponent<InteractiveNodeCardboard>().PlaySolo();
            ToggleLock();
            Debug.Log("locked = " + locked);            
        }
        else if (cardboard.gaze.WasHeld())
        {
            cardboard.gaze.PreviousObject().GetComponent<InteractiveNodeCardboard>().PlaySolo();
            ToggleLock();
            Debug.Log("locked = " + locked);                       
        }
        else
        {
            Debug.Log("Please select a track to lock onto");    
        }
        
        // If you need more raycast data from cardboard.gaze, the RaycastHit is exposed as gaze.Hit()
    }

    private void CardboardGazeChange(object sender) {
        // You can grab the data from the sender instead of the CardboardControl object
        CardboardControlGaze gaze = sender as CardboardControlGaze;
        // We can access to the object we're looking at
        // gaze.IsHeld will make sure the gaze.Object() isn't null

        // We also can access to the last object we looked at
        // gaze.WasHeld() will make sure the gaze.PreviousObject() isn't null

		if (cardboard.gaze.Object() == null)
        {
        	if (curNode != null)
        	{
        		if (curNode.name == "Diamond")
                {
                    curNode.GetComponent<InteractiveNodeCardboard>().Reset();                    
                }
        	}
        	curNode = null;
        }
        else if (cardboard.gaze.IsHeld())
        {
	        //MOVED FROM UPDATE//
			if (scene == "01_Cardboard_RootLevel_v1")
            {
                if (cardboard.gaze.Object().name.Contains("Diamond"))
                {
                    curNode = cardboard.gaze.Object();
                    curNode.GetComponent<InteractiveNodeCardboard>().Highlight();
                    
                    //HIGHLIGHT CONTINENT BY COUNTRYID CODE
                    int countryId = cardboard.gaze.Object().GetComponentInParent<LoadingInNewFlags>().countryID;
                    Debug.Log("country id = " + countryId);
                    planet.GetComponent<CountryHighlighter>().updateCountry(countryId);
                }
                //
                //if user is staring at panel, keep active
                else if (cardboard.gaze.Object().name.Contains("HighLightCollider"))
                {
                    curNode = cardboard.gaze.Object().transform.parent.parent.Find("Diamond").gameObject;
                    curNode.GetComponent<InteractiveNodeCardboard>().Highlight();
                    
                    //HIGHLIGHT CONTINENT BY COUNTRYID CODE
                    int countryId = cardboard.gaze.Object().transform.parent.parent.GetComponent<LoadingInNewFlags>().countryID;
                    planet.GetComponent<CountryHighlighter>().updateCountry(countryId);
                }
                else if (cardboard.gaze.Object().name.Contains("ButtonCollider"))
                {
                    curNode = cardboard.gaze.Object().transform.parent.parent.Find("Diamond").gameObject;
                    curNode.GetComponent<InteractiveNodeCardboard>().Highlight();
                    curNode.GetComponent<InteractiveNodeCardboard>().IsGazedAt();
                    
                    //HIGHLIGHT CONTINENT BY COUNTRYID CODE
                    int countryId = cardboard.gaze.Object().transform.parent.parent.GetComponent<LoadingInNewFlags>().countryID;
                    planet.GetComponent<CountryHighlighter>().updateCountry(countryId);
                }
                else if (cardboard.gaze.Object().name.Contains("Slider"))
                    {
                        cardboard.gaze.Object().GetComponent<OnboardingUI>().IsGazedAt();
                    }
            }
            else if (scene == "02_Cardboard_DJLevel_v2")
            {
                if (cardboard.gaze.Object().name.Contains("Heart"))
                {
                    curNode = cardboard.gaze.Object();
                    // curNode.GetComponent<InteractiveNodeCardboard>().Highlight();
                    cardboard.reticle.Show();
                    cardboard.reticle.Highlight(textColor);
                    textMesh2.GetComponent<Renderer>().enabled = true;
                    textMesh.GetComponent<Renderer>().enabled = true;
                    countdownTimer = StartCountdown(countdownLength);
                    StartCoroutine(countdownTimer);
                }
                else if (cardboard.gaze.Object().name.Contains("Slider"))
                {
                    cardboard.gaze.Object().GetComponent<OnboardingUI>().IsGazedAt();
                }
            }
        }            
            
        if (cardboard.gaze.PreviousObject() != null && cardboard.gaze.PreviousObject().name == "ButtonCollider")
        {
            curNode.GetComponent<InteractiveNodeCardboard>().NotGazedAt();
        }
        
        else if (cardboard.gaze.PreviousObject() != null && cardboard.gaze.PreviousObject().name == "Slider")
        {
            //print("reset button");
            gaze.PreviousObject().GetComponent<OnboardingUI>().NotGazedAt();
        }
        
        else if (scene == "02_Cardboard_DJLevel_v2" && cardboard.gaze.PreviousObject() != null && cardboard.gaze.PreviousObject().name.Contains("Heart"))
        {
            cardboard.reticle.Hide();
            cardboard.reticle.ClearHighlight();
            textMesh.GetComponent<Renderer>().enabled = false;
            textMesh2.GetComponent<Renderer>().enabled = false;
            StopCoroutine(countdownTimer);  
        }            
            


        //		else if (cardboard.gaze.Object().name.Contains("Dj_Info_Canvas"))
        //		{
        //			cardboard.gaze.Object().transform.parent.GetChild(0).GetComponent<InteractiveNodeCardboard>().Highlight();
        //			//cardboard.gaze.Object().transform.parent.GetChild(0).GetComponent<InteractiveNodeCardboard>().FillButton();
        //		}
                //END MOVED FROM UPDATE//

                //print("Gaze Changed to " + cardboard.gaze.Object());
        //		else if (cardboard.gaze.Object().name.Contains("Diamond") && cardboard.gaze.Object() != curNode)
        //		{
        //			curNode.GetComponent<InteractiveNodeCardboard>().Reset();
        //			curNode = cardboard.gaze.Object();
        //		}

        // Be sure to set the Reticle Layer Mask on the CardboardControlManager
        // to grow the reticle on the objects you want. The default is everything.

        // Not used here are gaze.Forward(), gaze.Right(), and gaze.Rotation()
        // which are useful for things like checking the view angle or shooting projectiles
    }

    private void CardboardStare(object sender) {
        CardboardControlGaze gaze = sender as CardboardControlGaze;

        // TODO: TOGGLE ROOT/DJ LEVEL HERE

    }

    private void CardboardMagnetReset(object sender) {
        // Resetting the magnet will reset the polarity if up and down are confused
        // This occasionally happens when the device is inserted into the enclosure
        // or if the magnetometer readings are weak enough to cut in and out
        Debug.Log("Device tilted");
        cardboard.trigger.ResetMagnetState();
    }
	
	// Update is called once per frame
	void Update () {
        // if (cardboard.gaze.IsHeld()) {
        //     // Debug.Log(cardboard.gaze.Object());
        //     Debug.Log(cardboard.gaze.Hit());
        // }
        //Countdown timer on GUI
        // if (cardboard.gaze.IsHeld()) {
        //     //DJ LEVEL CONTROLS:
		// 	if (scene == "02_Cardboard_DJLevel_v2")
        //     {
        //         if (cardboard.gaze.Object().name.Contains("Heart"))
        //         {
        //             if (cardboard.gaze.SecondsHeld() > 0 && cardboard.gaze.SecondsHeld() < 3) 
        //             {

        //                 textMesh2.GetComponent<Renderer>().enabled = true;          
        //                 textMesh.GetComponent<Renderer>().enabled = true;
        //                 textMesh2.text = "Back to Global Beat";
        //                 textMesh.text = (3 - cardboard.gaze.SecondsHeld()).ToString("#"); 
        //             }
        //             else if (cardboard.gaze.SecondsHeld() > 3) {
        //                 //Gavin: Fade out camera before changing scenes
        //                 //Send scene name to load and the fade duration
        //                 if (!isFading)
        //                 {
        //                     StartCoroutine(FadeLevelChange(0, 2f, null));
        //                     textMesh.GetComponent<Renderer>().enabled = false;
        //                     textMesh2.GetComponent<Renderer>().enabled = false;
        //                 }
        //             }
        //         }
        //         else
        //         {
        //             cardboard.reticle.Hide();
        //             cardboard.reticle.ClearHighlight();
        //             textMesh.GetComponent<Renderer>().enabled = false;
        //             textMesh2.GetComponent<Renderer>().enabled = false;    
        //         }
                
        //     }
            
            //ROOT LEVEL CONTROLS (MOVED UP TO ONGAZECHANGE FUNCTION):
			// if (scene == "01_Cardboard_RootLevel_v1")
            // {
                // if (cardboard.gaze.Object().name.Contains("Heart"))
                // {
                    // textMesh2.text = "Feel the Global Pulse";
                //     // textMesh2.GetComponent<Renderer>().enabled = Time.time % 1 < 0.5;  
                // }

//				if (cardboard.gaze.Object().name.Contains("Diamond"))
//                {
//                	curNode = cardboard.gaze.Object();
//                	cardboard.gaze.Object().GetComponent<InteractiveNodeCardboard>().Highlight();
//                    
//                    //HIGHLIGHT CONTINENT BY COUNTRYID CODE
//                    int countryId = cardboard.gaze.Object().GetComponentInParent<LoadingInNewFlags>().countryID;
//                    planet.GetComponent<CountryHighlighter>().updateCountry(countryId);
//
//                }
//                //
//                //
//                //if user is staring at panel, keep active
//				else if(cardboard.gaze.Object().name.Contains("HighLightCollider"))
//                {
//                	curNode = cardboard.gaze.Object().transform.parent.GetChild(0).gameObject;
//                	cardboard.gaze.Object().transform.parent.GetChild(0).GetComponent<InteractiveNodeCardboard>().Highlight();
//                    
//                    //HIGHLIGHT CONTINENT BY COUNTRYID CODE
//                    int countryId = cardboard.gaze.Object().transform.parent.GetChild(0).GetComponentInParent<LoadingInNewFlags>().countryID;
//                    planet.GetComponent<CountryHighlighter>().updateCountry(countryId);
//
//                }
// 
//				else if (cardboard.gaze.Object().name.Contains("Dj_Info_Canvas"))
//				{
//					cardboard.gaze.Object().transform.parent.GetChild(0).GetComponent<InteractiveNodeCardboard>().Highlight();
//					//cardboard.gaze.Object().transform.parent.GetChild(0).GetComponent<InteractiveNodeCardboard>().FillButton();
//				}


				// if (cardboard.gaze.Object().gameObject.GetComponent<ButtonTimer>())
				// {
				// 	cardboard.gaze.Object().transform.parent.parent.GetChild(0).GetComponent<InteractiveNodeCardboard>().Highlight();
				// 	cardboard.gaze.Object().gameObject.GetComponent<ButtonTimer>().hovered = true;
				// }
				
                // if (cardboard.gaze.SecondsHeld() > 3 && cardboard.gaze.SecondsHeld() < 8) {

                //     if (cardboard.gaze.Object().name.Contains("Diamond"))
                //     {
                //         // textMesh2.text = "Enter DJ Room In:";
                //         // textMesh2.GetComponent<Renderer>().enabled = true;          
                //         // textMesh.GetComponent<Renderer>().enabled = true;
                //         // textMesh.text = (8 - cardboard.gaze.SecondsHeld()).ToString("#");
                //     }
                // }
                // else if (8 < cardboard.gaze.SecondsHeld() && cardboard.gaze.SecondsHeld() < 10) {
                //     if (cardboard.gaze.Object().name.Contains("Diamond"))
                //     {
                //         // textMesh2.GetComponent<Renderer>().enabled = false;
                //         // textMesh.text = "DJ Selected";
                //         // textMesh.GetComponent<Renderer>().enabled = Time.time % 1 < 0.5;         
                //     }
                // }
                // else if (cardboard.gaze.SecondsHeld() > 10) {
                //     if (!cardboard.gaze.Object().name.Contains("Heart")) {

                //     	//Gavin: Fade out camera before changing scenes
                //     	//Send scene name to load and the fade duration
				// 		// StartCoroutine(FadeLevelChange(1,0.8f));
                //         //SceneManager.LoadScene(1);    
                                            
                //     }
                // }
        //  }
        // }

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
    
    public void ToggleVRMode() {
        Cardboard.SDK.VRModeEnabled = !Cardboard.SDK.VRModeEnabled;
    }
    
    public void ToggleLock() {
        locked = !locked;
    }

    // MOVED TO PERSISTENTDATA
	// public void ChangeLevel (int sceneBuild, float fadeDur, GameObject node)
	// {
	// 	StartCoroutine(FadeLevelChange(sceneBuild, fadeDur, node));
	// }

    // IEnumerator FadeLevelChange(int sceneBuild, float fadeDur, GameObject node)
    // {
    // 	isFading = true;
    //     GameObject camera = GameObject.Find("Main Camera");
    //     camera.GetComponent<VRCameraFade>().FadeOut(fadeDur, false);
    //     StartCoroutine(FadeAudio(fadeDur, Fade.Out));
	// 	yield return new WaitForSeconds(fadeDur);
		
    //     //cache current time of song
	// 	if (node != null)
	// 	{
    //         if (scene == "01_Cardboard_RootLevel_v1")
    //         {
    //             float playHead = node.GetComponent<CardboardAudioSource>().audioSource.time;
    //             PersistentData.PD.curSongTime = playHead;
    //             PersistentData.PD.performanceId = node.transform.parent.name;
    //         }
    //         else if (scene == "02_Cardboard_DJLevel_v2")
    //         {
    //             float playHead = GameObject.Find("AudioSampler").GetComponent<AudioSource>().time;
    //             PersistentData.PD.curSongTime = playHead;
    //         }
    //     }
    //     camera.SetActive(false);
    //     SceneManager.LoadScene(sceneBuild); 
    //     isFading = false;
    // }
    
    IEnumerator FadeAudio (float timer, Fade fadeType) 
    {
        float start = fadeType == Fade.In? 0.0F : 1.0F;
        float end = fadeType == Fade.In? 1.0F : 0.0F;
        float i = 0.0F;
        float step = 1.0F/timer;
    
        while (i <= 1.0F) 
        {
            i += step * Time.deltaTime;
            AudioListener.volume = Mathf.Lerp(start, end, i);
            yield return new WaitForSeconds(step * Time.deltaTime);
        }  
    }
    
    IEnumerator StartCountdown(int startingNum)
    {
        countdown = startingNum;
        while (countdown > 0)
        {
            textMesh.text = countdown.ToString("#"); 
            yield return new WaitForSeconds(1.0f);
            countdown --;
        }
        textMesh.text = "0";
        PersistentData.PD.ChangeLevel(0, 1f, null);
    }
    
    

}

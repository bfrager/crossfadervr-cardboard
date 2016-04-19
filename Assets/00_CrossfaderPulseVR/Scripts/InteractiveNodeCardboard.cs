// Copyright 2014 Google Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class InteractiveNodeCardboard : MonoBehaviour {
    public float fadeTime = 3.0F;
    public float sceneStartFade = 5.0F;
    public bool locked = false;
    private GameObject djInfo;
    private GameObject visuals;
    enum Fade {In, Out};
    public float buttonFillAmount;
    public bool gazedAt;
    private Coroutine buttonFillRoutine = null;
    private Coroutine audioFadeIn = null;
    private Coroutine audioFadeOut = null;
    private Coroutine audioFade = null;

    public Image buttonFill;

	//sammoh this is where I'm gonna ping the country script
	public LoadingInNewFlags _country;


  void Start() {
    SetGazedAt(false);
    StartCoroutine(FadeAudio(sceneStartFade, Fade.In, gameObject.transform));
  }

//   void LateUpdate() {
//     Cardboard.SDK.UpdateState();
//     if (Cardboard.SDK.BackButtonPressed) {
//       Application.Quit();
//     }
//   }

//   void Update()
//   {
//   	if (visTimer > 0)
//   	{
//   		visTimer -= 0.1f;
//   	}
//   	else if (visTimer <= 0)
//   	{
//   		Reset();
//   	}
//   }

  public void SetGazedAt(bool gazed) {
  }

  public void IsGazedAt()
  {
  	gazedAt = true;
  }

  public void NotGazedAt()
  {
  	gazedAt = false;
  }

  public void Highlight() {
    GameObject.Find("EarthLow").GetComponent<SpinFree>().spin = false;
    if (!locked)
    {
    	
        foreach (Transform child in transform.parent.parent)
        {
            if (child.name != this.transform.parent.name)
            {
                // Debug.Log ("Found sibling "+child.name);
                StartCoroutine(FadeAudio(fadeTime, Fade.Out, child.transform.GetChild(0)));
            }
            else
            {
                djInfo = child.transform.GetChild(1).gameObject;
                djInfo.SetActive(true);
                visuals = child.transform.GetChild(2).gameObject;
                visuals.SetActive(true);
                child.transform.GetChild(0).GetComponent<Spin_Node>().enabled = true;
            }
        }

        gameObject.transform.localScale = 5 * Vector3.one;

    }
  }
  
  public void Reset() {
    GameObject.Find("EarthLow").GetComponent<SpinFree>().spin = true;
    if (!locked)
    {
        foreach (Transform child in transform.parent.parent)
        {
            if (child.name != this.transform.parent.name) 
            {
                Debug.Log ("Found sibling "+ child.name);
                StartCoroutine(FadeAudio(fadeTime, Fade.In, child.transform.GetChild(0)));
                // child.GetComponent<CardboardAudioSource>().volume = 1;
            }
            else 
            {
                djInfo = child.transform.GetChild(1).gameObject;
                djInfo.SetActive(false);
                visuals = child.transform.GetChild(2).gameObject;
                visuals.SetActive(false);
                child.transform.GetChild(0).GetComponent<Spin_Node>().enabled = false;
            }
        }

			gameObject.transform.localScale = 3 * Vector3.one;
    }
  }
  
  public void PlaySolo() {
      if (locked)
            {
                foreach (Transform child in transform.parent.parent)
                {
                    // GetComponent<CardboardAudioSource>().spatialize = true;
                    child.transform.GetChild(0).GetComponent<CardboardAudioSource>().UnPause();
                    child.transform.GetChild(0).GetComponent<InteractiveNodeCardboard>().locked = false;
                    if (child.transform.GetChild(0).GetComponent<CardboardAudioSource>().volume == 0) 
                    {
                        StartCoroutine(FadeAudio(fadeTime, Fade.In, child.transform.GetChild(0)));
                    }
                    else 
                    {
                        djInfo = child.transform.GetChild(1).gameObject;
                        djInfo.SetActive(false);
                        visuals = child.transform.GetChild(2).gameObject;
                        visuals.SetActive(false); 
                    }
            
                }
            }
        else if (!locked)
            {
                foreach (Transform child in transform.parent.parent)
                {
                    child.transform.GetChild(0).GetComponent<InteractiveNodeCardboard>().locked = true;
                    if (child.name != this.transform.parent.name)
                    {
                        Debug.Log ("Pausing "+ child.transform.GetChild(0).name);
                        // GetComponent<CardboardAudioSource>().spatialize = true;
                        StartCoroutine(FadeAudio(fadeTime, Fade.Out, child.transform.GetChild(0)));
                        child.transform.GetChild(0).GetComponent<CardboardAudioSource>().Pause();
                    }
                    else if (child.name == this.transform.parent.name)
                    {
                        Debug.Log ("Unpausing "+child.name);
                        // GetComponent<CardboardAudioSource>().spatialize = false;
                        child.transform.GetChild(0).GetComponent<CardboardAudioSource>().volume = 1;
                        child.transform.GetChild(0).GetComponent<CardboardAudioSource>().UnPause();
                    }
                }
            }
  }
  public void FillButton()
  {
  	StartCoroutine(IEFillButton());
  }

  IEnumerator IEFillButton()
  {
  	while (gazedAt)
  	{
	  	if (buttonFillAmount < 2)
	  	{
	  		buttonFillAmount += Time.deltaTime;
	  		buttonFill.fillAmount = buttonFillAmount/2;
	  	}
	  	else if (buttonFillAmount >= 1)
	  	{
				CardboardController.cardboardController.ChangeLevel(1,0.8f, gameObject);
	  	}
	  	yield return new WaitForSeconds(0.05f);
  	}

  }

  public void ResetButton()
  {
  	buttonFillAmount = 0;
  	buttonFill.fillAmount = buttonFillAmount;

  }


IEnumerator FadeAudio (float timer, Fade fadeType, Transform gameObject) {
    // float start = fadeType == Fade.In? 0.0F : 1.0F;
    float end = fadeType == Fade.In? 1.0F : 0.0F;
    float i = 0.0F;
    float step = 1.0F/timer;
    float currentVolume = gameObject.GetComponent<CardboardAudioSource>().volume;
 
    while (i <= 1.0F) {
        i += step * Time.deltaTime;
        gameObject.GetComponent<CardboardAudioSource>().volume = Mathf.Lerp(currentVolume, end, i);
        yield return new WaitForSeconds(step * Time.deltaTime);
    }
    
}

//   public void ToggleVRMode() {
//     Cardboard.SDK.VRModeEnabled = !Cardboard.SDK.VRModeEnabled;
//   }


// public class FadeOutAudio : MonoBehaviour {
 
//     [TooltipAttribute("The audio source")]
//     public AudioSource audioSource;
//     [TooltipAttribute("Time in seconds to fade out")]
//     public float fadeSpeed = 5f;
//     [TooltipAttribute("Toggle the fade")]
//     public bool startFade = false;
 
//     float time = 0f;
 
//     void Update(){
//         if(startFade){
//             audioSource.volume = Mathf.Lerp(1f, 0f, time);
//             time += Time.deltaTime / fadeSpeed;
//         }
//     }
 
// }

}

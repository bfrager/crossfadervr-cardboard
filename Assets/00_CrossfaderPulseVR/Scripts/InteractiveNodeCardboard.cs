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
    
    public float fadeTime = 1.0F;
    public float sceneStartFade = 5.0F;
    public bool locked = false;
    private GameObject djInfo;
    private GameObject visuals;
    enum Fade {In, Out};
    public float buttonFillAmount;
    public bool gazedAt;
    
    private Coroutine buttonFillRoutine = null;

    private Coroutine audioFade1 = null;
    private Coroutine audioFade2 = null;
    private Coroutine audioFade3 = null;



    public Slider buttonFill;

	//sammoh this is where I'm gonna ping the country script
	public LoadingInNewFlags _country;


  void Start() {
	//NotGazedAt();
    StartCoroutine(FadeAudio(sceneStartFade, Fade.In, gameObject.transform));
  }

//   void LateUpdate() {
//     Cardboard.SDK.UpdateState();
//     if (Cardboard.SDK.BackButtonPressed) {
//       Application.Quit();
//     }
//   }

  public void IsGazedAt()
  {
  	gazedAt = true;
  	StartCoroutine(IEFillButton());
  }

  public void NotGazedAt()
  {
  	gazedAt = false;
  	StopCoroutine(IEFillButton());
  	ResetButton();
  }

  public void Highlight() {
    GameObject.Find("EarthLow").GetComponent<SpinFree>().spin = false;
    if (!locked)
    {
        StopAllCoroutines();
        foreach (Transform child in transform.parent.parent) //for each DJ node in Nodes
        {
            if (child.name != this.transform.parent.name)
            {
                Debug.Log ("Fading audio for audio on "+ child.name);
                StartCoroutine(FadeAudio(fadeTime, Fade.Out, child.Find("Diamond")));
            }
            else
            {
                // TODO: FADE AUDIO TO 1
                
                child.Find("Dj_Info_Canvas").gameObject.SetActive(true);
                child.Find("Visuals").gameObject.SetActive(true);

                //activate highlight collider
                child.Find("Dj_Info_Canvas/HighLightCollider").gameObject.SetActive(true);
            }
        }
        gameObject.GetComponent<Spin_Node>().enabled = true;
        gameObject.transform.localScale = 5 * Vector3.one;

    }
  }
  
public void Reset() {
    GameObject.Find("EarthLow").GetComponent<SpinFree>().spin = true;
    if (!locked)
    {
        StopAllCoroutines();
        foreach (Transform child in transform.parent.parent)
        {
            if (child.name != this.transform.parent.name) 
            {
                Debug.Log ("Fading in audio for "+ child.name);
                StartCoroutine(FadeAudio(fadeTime, Fade.In, child.Find("Diamond")));
                // child.GetComponent<CardboardAudioSource>().volume = 1;
            }
            else 
            {
                child.Find("Dj_Info_Canvas").gameObject.SetActive(false);
                child.Find("Visuals").gameObject.SetActive(false);
                StartCoroutine(FadeAudio(fadeTime, Fade.In, child.Find("Diamond")));
                
				//deactivate highlight collider
                child.Find("Dj_Info_Canvas/HighLightCollider").gameObject.SetActive(false);
            }
        }
        gameObject.GetComponent<Spin_Node>().enabled = false;
        gameObject.transform.localScale = 3 * Vector3.one;
    }
  }
  
  public void PlaySolo() {
        if (locked)
        {
            foreach (Transform child in transform.parent.parent)
            {
                // GetComponent<CardboardAudioSource>().spatialize = true;
                child.Find("Diamond").GetComponent<CardboardAudioSource>().UnPause();
                child.Find("Diamond").GetComponent<InteractiveNodeCardboard>().locked = false;
                if (child.Find("Diamond").GetComponent<CardboardAudioSource>().volume == 0) 
                {
                    StartCoroutine(FadeAudio(fadeTime, Fade.In, child.Find("Diamond")));
                }
                else 
                {
                    child.Find("Dj_Info_Canvas").gameObject.SetActive(false);
                    child.Find("Visuals").gameObject.SetActive(false); 
                }
        
            }
        }
        else if (!locked)
        {
            foreach (Transform child in transform.parent.parent)
            {
                child.Find("Diamond").GetComponent<InteractiveNodeCardboard>().locked = true;
                if (child.name != this.transform.parent.name)
                {
                    Debug.Log ("Pausing "+ child.Find("Diamond").name);
                    // GetComponent<CardboardAudioSource>().spatialize = true;
                    StartCoroutine(FadeAudio(fadeTime, Fade.Out, child.Find("Diamond")));
                    child.Find("Diamond").GetComponent<CardboardAudioSource>().Pause();
                }
                else if (child.name == this.transform.parent.name)
                {
                    Debug.Log ("Unpausing "+child.name);
                    // GetComponent<CardboardAudioSource>().spatialize = false;
                    child.Find("Diamond").GetComponent<CardboardAudioSource>().volume = 1;
                    child.Find("Diamond").GetComponent<CardboardAudioSource>().UnPause();
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
	  		buttonFill.value = buttonFillAmount/2;
	  	}
	  	else if (buttonFillAmount >= 1)
	  	{
            CardboardController.cardboardController.ChangeLevel(1,0.8f, gameObject);
	  	}
	  	yield return new WaitForSeconds(0.02f);
  	}

  }

  public void ResetButton()
  {
  	buttonFillAmount = 0.001f;
  	buttonFill.value = buttonFillAmount;

  }

    IEnumerator FadeAudio (float timer, Fade fadeType, Transform gameObject) 
    {
        // float start = fadeType == Fade.In? 0.0F : 1.0F;
        float end = fadeType == Fade.In? 1.0F : 0.0F;
        float i = 0.0F;
        float step = 1.0F/timer;
        float currentVolume = gameObject.GetComponent<CardboardAudioSource>().volume;

        while (i <= 1.0F) 
        {
            i += step * Time.deltaTime;
            gameObject.GetComponent<CardboardAudioSource>().volume = Mathf.Lerp(currentVolume, end, i);
            yield return new WaitForSeconds(step * Time.deltaTime);
        }
    }
}

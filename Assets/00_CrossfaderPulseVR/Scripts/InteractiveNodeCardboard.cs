﻿// Copyright 2014 Google Inc. All rights reserved.
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
using System.Collections.Generic;
using AudioVisualizer;

[RequireComponent(typeof(Collider))]
public class InteractiveNodeCardboard : MonoBehaviour {
    
    public float fadeTime = 1.0F;
    public float sceneStartFade = 3.0F;
    private GameObject djInfo;
    private GameObject visuals;
    private GameObject planet;
    enum Fade {In, Out};
    public bool gazedAt;
    private GameObject earth;
    private bool heart = false;
    public Component[] nodeVisuals;
    public List<AudioSource> audioSources;
    public GameObject[] djNodes;
    public int audioSourceIndex;
    
    public Slider buttonFill;
    public float buttonFillTime = 1.0F;
    public float buttonFillAmount;
    
    private float audioDivisor = 2.0F;

	//sammoh this is where I'm gonna ping the country script
	public LoadingInNewFlags _country;


  void Start() 
  {
	//NotGazedAt();
    // StartCoroutine(FadeAudio(sceneStartFade, 1.0F / audioDivisor, gameObject.transform));
    gameObject.GetComponent<CardboardAudioSource>().volume = 1.0F / audioDivisor;
    
    Debug.Log(gameObject.GetComponent<CardboardAudioSource>().volume);
    
    planet = GameObject.Find("Planet960tris");
    earth = GameObject.Find("EarthLow");
    // djNodes = GameObject.FindGameObjectsWithTag("djNode");
    
    if (gameObject.name.Contains("Heart"))
    {
        heart = true;
    }
    
    // DJ Node only Start functions
    if (!heart)
    {
        // StartCoroutine(SetAudioVisualizerIndexes());

        buttonFill = gameObject.transform.parent.Find("Dj_Info_Canvas/Slider").GetComponent<Slider>();
        
        Debug.Log("CURSONGTIME = " + PersistentData.PD.curSongTime);
        
        if (PersistentData.PD.curSongTime != null && PersistentData.PD.curSongTime != 0)
        {
            Debug.Log("Setting audio playhead to " + PersistentData.PD.curSongTime);
            gameObject.GetComponent<CardboardAudioSource>().audioSource.time = PersistentData.PD.curSongTime;
        } 
    }
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
    if (!heart)
    {
        // earth.GetComponent<SpinFree>().spin = false;
        
        if (!(CardboardController.cardboardController.locked))
        {
            StopAllCoroutines();
            foreach (Transform child in transform.parent.parent) //for each DJ node in Nodes
            {
                if (child.name != this.transform.parent.name)
                {
                    StartCoroutine(FadeAudio(fadeTime, 0.0F, child.Find("Diamond")));
                }
                else
                {
                    StartCoroutine(FadeAudio(fadeTime, 1.0F, child.Find("Diamond")));
                    child.Find("Dj_Info_Canvas").gameObject.SetActive(true);
                    child.Find("Visuals").gameObject.SetActive(true);

                    //activate highlight collider
                    child.Find("Dj_Info_Canvas/HighLightCollider").gameObject.SetActive(true);
                }
            }
            gameObject.GetComponent<Spin_Node>().enabled = true;
            gameObject.transform.localScale = 5 * Vector3.one;

            // If we want to move country highlighting here...
//            int countryID = gameObject.GetComponentInParent<LoadingInNewFlags>().countryID;
//            planet.GetComponent<CountryHighlighter>().updateCountry(countryID);
        }
    }
    
  }
  
public void Reset() {
    if (!heart)
    {
        // earth.GetComponent<SpinFree>().spin = true;
        if (!(CardboardController.cardboardController.locked))
        {
            StopAllCoroutines();
            foreach (Transform child in transform.parent.parent)
            {
                if (child.name != this.transform.parent.name) 
                {
                    StartCoroutine(FadeAudio(fadeTime, 1.0F / audioDivisor, child.Find("Diamond")));
                }
                else 
                {
                    child.Find("Dj_Info_Canvas").gameObject.SetActive(false);
                    child.Find("Visuals").gameObject.SetActive(false);
                    StartCoroutine(FadeAudio(fadeTime, 1.0F / audioDivisor, child.Find("Diamond")));
                    //deactivate highlight collider
                    child.Find("Dj_Info_Canvas/HighLightCollider").gameObject.SetActive(false);
                }
            }
            gameObject.GetComponent<Spin_Node>().enabled = false;
            gameObject.transform.localScale = 3 * Vector3.one;
            planet.GetComponent<CountryHighlighter>().updateCountry(0);
        }
    }
  }
  
  public void PlaySolo() {
        StopAllCoroutines();
        if (CardboardController.cardboardController.locked)
        {
            foreach (Transform child in transform.parent.parent)
            {
                // GetComponent<CardboardAudioSource>().spatialize = true;
                // child.Find("Diamond").GetComponent<CardboardAudioSource>().UnPause();
                // child.Find("Diamond").GetComponent<InteractiveNodeCardboard>().locked = false;
                if (child.Find("Diamond").GetComponent<CardboardAudioSource>().volume == 0)
                {
                    StartCoroutine(FadeAudio(fadeTime, 1.0F / audioDivisor, child.Find("Diamond")));
                }
                else 
                {
                    child.Find("Dj_Info_Canvas").gameObject.SetActive(false);
                    child.Find("Visuals").gameObject.SetActive(false); 
                }
        
            }
        }
        else if (!(CardboardController.cardboardController.locked))
        {
            foreach (Transform child in transform.parent.parent)
            {
                // child.Find("Diamond").GetComponent<InteractiveNodeCardboard>().locked = true;
                if (child.name != this.transform.parent.name)
                {
                    // GetComponent<CardboardAudioSource>().spatialize = true;
                    StartCoroutine(FadeAudio(fadeTime, 0.0F, child.Find("Diamond")));
                    // child.Find("Diamond").GetComponent<CardboardAudioSource>().Pause();
                }
                else if (child.name == this.transform.parent.name)
                {
                    // Debug.Log ("Unpausing "+child.name);
                    // GetComponent<CardboardAudioSource>().spatialize = false;
                    StartCoroutine(FadeAudio(fadeTime, 1.0F, child.Find("Diamond")));
                    // child.Find("Diamond").GetComponent<CardboardAudioSource>().UnPause();
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
	  		buttonFillAmount += Time.deltaTime / buttonFillTime;
	  		buttonFill.value = buttonFillAmount / 2;
	  	}
	  	else if (buttonFillAmount >= 1)
	  	{
            CardboardController.cardboardController.ChangeLevel(1,0.8f, gameObject);
	  	}
	  	yield return new WaitForSeconds(0.01f);
  	}

  }

  public void ResetButton()
  {
  	buttonFillAmount = 0.001f;
  	buttonFill.value = buttonFillAmount;

  }
  
  IEnumerator SetAudioVisualizerIndexes ()
  {
    yield return new WaitForSeconds(2);
    // select audio source index from audiosampler array based on dj node name
    audioSources = AudioVisualizer.AudioSampler.instance.audioSources;
    for (int i = 0; i < audioSources.Count; i++)
    {
        string clipName = audioSources[i].clip.name.ToString();
        string nodeName = gameObject.transform.parent.name.ToString();
        if (clipName == nodeName)
        {
            audioSourceIndex = i;
            Debug.Log("Audio source index for " + gameObject.transform.parent.name + " = " + i);
        }
    }
    
    // set sphere waveform visuals to audio source index
    nodeVisuals = gameObject.transform.parent.Find("Visuals").GetComponentsInChildren<SphereWaveform>();
    foreach (SphereWaveform sphereWaveform in nodeVisuals) {
        sphereWaveform.audioSource = audioSourceIndex;
    }
  }

    IEnumerator FadeAudio (float timer, float fadeVol, Transform djNode) 
    {
        float end = fadeVol;
        float i = 0.0F;
        float step = 1.0F/timer;
        float currentVolume = djNode.GetComponent<CardboardAudioSource>().volume;

        while (i <= 1.0F) 
        {
            i += step * Time.deltaTime;
            djNode.GetComponent<CardboardAudioSource>().volume = Mathf.Lerp(currentVolume, end, i);
            yield return new WaitForSeconds(step * Time.deltaTime);
        }
    }
}

﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OnboardingUI : MonoBehaviour {

	public bool gazedAt;
	public float buttonFillAmount;
	public Slider buttonFill;
	public CanvasGroup introCanvas;


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

	IEnumerator IEFillButton()
  {
  	while (gazedAt)
  	{
	  	if (buttonFillAmount < 1)
	  	{
	  		buttonFillAmount += Time.deltaTime / 2;
	  		buttonFill.value = buttonFillAmount;
	  	}
	  	else if (buttonFillAmount >= 1)
	  	{
				//CardboardController.cardboardController.ChangeLevel(1,0.8f, gameObject);
				//Fade Panel
				StartCoroutine(FadeCanvas());
	  	}
	  	yield return new WaitForSeconds(0.02f);
  	}

  }

  IEnumerator FadeCanvas()
  {
  	while (introCanvas.alpha > 0)
  	{
  		introCanvas.alpha -= Time.deltaTime * 0.3f;
  		yield return new WaitForSeconds(0.02f);
  	}
  	introCanvas.gameObject.SetActive(false);
  	//LOAD NODES HERE//
  }

  public void ResetButton()
  {
  	buttonFillAmount = 0.001f;
  	buttonFill.value = buttonFillAmount;

  }
}
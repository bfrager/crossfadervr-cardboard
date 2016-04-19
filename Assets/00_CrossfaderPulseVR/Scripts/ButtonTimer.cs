using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonTimer : MonoBehaviour {

	public bool hovered;
	public string buttonText;
	private float timer;
	// Use this for initialization
	void Start () {
	buttonText = "Enter DJ Room";
	transform.GetChild(0).gameObject.GetComponent<Text>().text = buttonText;

	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (hovered)
		{
			timer -= 0.1f;
			if (timer > 0)
			{
				Countdown();
			}
			else{
				CardboardController.cardboardController.ChangeLevel("02_Cardboard_DJLevel_v2",0.8f, transform.parent.parent.GetChild(0).gameObject);

			}
		}
		else{	
			timer = 3f;
		}
	}

	public void IsHovered()
	{	
		hovered = true;
	}

	public void NotHovered()
	{
		hovered = false;
		timer = 3;
		buttonText = "Enter DJ Room";
		transform.GetChild(0).gameObject.GetComponent<Text>().text = buttonText;


	}

	void Countdown()
	{
		buttonText = "Enter room in: " + timer.ToString("F0");
		transform.GetChild(0).gameObject.GetComponent<Text>().text = buttonText;
	}

}

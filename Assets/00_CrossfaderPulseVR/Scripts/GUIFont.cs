using UnityEngine;
using System.Collections;

public class GUIFont : MonoBehaviour {

	public Font Gravity;
	public int fontSize;
	private GUIStyle style;

	void Start(){
		style.font = Gravity;
		style.fontSize = fontSize;
		style.normal.textColor = new Color(255/255.0F,255/255.0F,0/255.0F,255/255.0F);
	}
}

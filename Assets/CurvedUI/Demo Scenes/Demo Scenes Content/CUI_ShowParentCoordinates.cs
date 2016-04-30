<<<<<<< HEAD
﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CUI_ShowParentCoordinates : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.GetComponent<Text>().text = transform.parent.GetComponent<RectTransform>().anchoredPosition.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
=======
﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CUI_ShowParentCoordinates : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.GetComponent<Text>().text = transform.parent.GetComponent<RectTransform>().anchoredPosition.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
>>>>>>> 51c1004b72761e2dcd146bcc2a90f917ee9aece9

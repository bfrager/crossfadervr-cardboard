<<<<<<< HEAD
﻿using UnityEngine;
using System.Collections;

public class CUI_MoveHeartbeat : MonoBehaviour {

	public float speed;
	public bool wrapAroundParent = true;


	RectTransform rectie;
	RectTransform parentRectie;

	void Start(){
		rectie = (transform as RectTransform);
		parentRectie = transform.parent as RectTransform;

	}

	// Update is called once per frame
	void Update () {
	
		rectie.anchoredPosition = new Vector2(rectie.anchoredPosition.x - speed * Time.deltaTime,
			rectie.anchoredPosition.y);

		if(wrapAroundParent){
				if(rectie.anchoredPosition.x + rectie.rect.width < 0 )
					rectie.anchoredPosition = new Vector2(parentRectie.rect.width, rectie.anchoredPosition.y);

			}
	}
}
=======
﻿using UnityEngine;
using System.Collections;

public class CUI_MoveHeartbeat : MonoBehaviour {

	public float speed;
	public bool wrapAroundParent = true;


	RectTransform rectie;
	RectTransform parentRectie;

	void Start(){
		rectie = (transform as RectTransform);
		parentRectie = transform.parent as RectTransform;

	}

	// Update is called once per frame
	void Update () {
	
		rectie.anchoredPosition = new Vector2(rectie.anchoredPosition.x - speed * Time.deltaTime,
			rectie.anchoredPosition.y);

		if(wrapAroundParent){
				if(rectie.anchoredPosition.x + rectie.rect.width < 0 )
					rectie.anchoredPosition = new Vector2(parentRectie.rect.width, rectie.anchoredPosition.y);

			}
	}
}
>>>>>>> 51c1004b72761e2dcd146bcc2a90f917ee9aece9

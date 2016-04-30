<<<<<<< HEAD
﻿using UnityEngine;
using System.Collections;

namespace CurvedUI
{
    public class CUI_RaycastToCanvas : MonoBehaviour
    {

        CurvedUISettings mySettings;

        // Use this for initialization
        void Start()
        {
            mySettings = GetComponentInParent<CurvedUISettings>();
        }

        // Update is called once per frame
        void Update()
        {

            Vector2 pos = Vector2.zero;
            mySettings.RaycastToCanvasSpace(Camera.main.ScreenPointToRay(Input.mousePosition), out pos);
            this.transform.localPosition = pos;

        }
    }
}
=======
﻿using UnityEngine;
using System.Collections;

namespace CurvedUI
{
    public class CUI_RaycastToCanvas : MonoBehaviour
    {

        CurvedUISettings mySettings;

        // Use this for initialization
        void Start()
        {
            mySettings = GetComponentInParent<CurvedUISettings>();
        }

        // Update is called once per frame
        void Update()
        {

            Vector2 pos = Vector2.zero;
            mySettings.RaycastToCanvasSpace(Camera.main.ScreenPointToRay(Input.mousePosition), out pos);
            this.transform.localPosition = pos;

        }
    }
}
>>>>>>> 51c1004b72761e2dcd146bcc2a90f917ee9aece9

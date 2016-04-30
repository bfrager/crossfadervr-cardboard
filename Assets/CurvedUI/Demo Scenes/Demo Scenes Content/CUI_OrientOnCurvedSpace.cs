<<<<<<< HEAD
﻿using UnityEngine;
using System.Collections;

namespace CurvedUI
{
    public class CUI_OrientOnCurvedSpace : MonoBehaviour
    {

        CurvedUISettings mySettings;


        // Use this for initialization
        void Awake()
        {

            mySettings = GetComponentInParent<CurvedUISettings>();


        }

        // Update is called once per frame
        void Update()
        {

            transform.position = mySettings.CanvasToCurvedCanvas(this.transform.parent.localPosition);
            transform.rotation = Quaternion.LookRotation(mySettings.CanvasToCurvedCanvasNormal(this.transform.parent.localPosition));

        }
    }
}
=======
﻿using UnityEngine;
using System.Collections;

namespace CurvedUI
{
    public class CUI_OrientOnCurvedSpace : MonoBehaviour
    {

        CurvedUISettings mySettings;


        // Use this for initialization
        void Awake()
        {

            mySettings = GetComponentInParent<CurvedUISettings>();


        }

        // Update is called once per frame
        void Update()
        {

            transform.position = mySettings.CanvasToCurvedCanvas(this.transform.parent.localPosition);
            transform.rotation = Quaternion.LookRotation(mySettings.CanvasToCurvedCanvasNormal(this.transform.parent.localPosition));

        }
    }
}
>>>>>>> 51c1004b72761e2dcd146bcc2a90f917ee9aece9

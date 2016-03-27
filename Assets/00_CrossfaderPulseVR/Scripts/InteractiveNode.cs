using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;

    // interactive item can be used to change things on gameobjects by handling events.
    public class InteractiveNode : MonoBehaviour
    {
        [SerializeField] private Material m_NormalMaterial;                
        [SerializeField] private Material m_OverMaterial;                  
        [SerializeField] private Material m_ClickedMaterial;               
        [SerializeField] private Material m_DoubleClickedMaterial;         
        [SerializeField] private VRInteractiveItem m_InteractiveItem;
        [SerializeField] private Renderer m_Renderer;
        public float audioFadeTime = 3.0F;
        public bool locked = false;

        private void Awake ()
        {
            m_Renderer.material = m_NormalMaterial;
        }


        private void OnEnable()
        {
            m_InteractiveItem.OnOver += HandleOver;
            m_InteractiveItem.OnOut += HandleOut;
            m_InteractiveItem.OnClick += HandleClick;
            m_InteractiveItem.OnDoubleClick += HandleDoubleClick;
        }


        private void OnDisable()
        {
            m_InteractiveItem.OnOver -= HandleOver;
            m_InteractiveItem.OnOut -= HandleOut;
            m_InteractiveItem.OnClick -= HandleClick;
            m_InteractiveItem.OnDoubleClick -= HandleDoubleClick;
        }


        //Handle the Over event
        private void HandleOver()
        {
            Debug.Log("Show over state");
            m_Renderer.material = m_OverMaterial;
            StartCoroutine(Highlight());   
        }
        
        IEnumerator Highlight() {
            if (!locked)
            {
                foreach (Transform child in transform.parent)
                {
                    if (child.name != this.name)
                    {
                        Debug.Log ("Found sibling "+child.name);
                        float startVolume = GetComponent<AudioSource>().volume;
                        
                        while (GetComponent<AudioSource>().volume > 0) 
                        {
                            GetComponent<AudioSource>().volume -= startVolume * Time.deltaTime / audioFadeTime;
                            yield return null;
                        }
                        GetComponent<AudioSource>().volume = startVolume;
                        GetComponent<AudioSource>().mute = true;
                    }
                }
            }
        }


        //Handle the Out event
        private void HandleOut()
        {
            Debug.Log("Show out state");
            m_Renderer.material = m_NormalMaterial;
            if (!locked)
            {
                foreach (Transform child in transform.parent)
                {
                    if (child.name != this.name)
                    {
                        Debug.Log ("Found sibling "+child.name);
                        GetComponent<AudioSource>().mute = false;
                    }
                }
            }
        }


        //Handle the Click event
        private void HandleClick()
        {
            Debug.Log("Show click state");
            m_Renderer.material = m_ClickedMaterial;
            
            if (locked)
            {
                locked = false;
                foreach (Transform child in transform.parent)
                {
                    GetComponent<AudioSource>().spatialize = true;
                    GetComponent<AudioSource>().UnPause();
                }
            }
            else if (!locked)
            {
                locked = true;
                foreach (Transform child in transform.parent)
                {
                    if (child.name != this.name)
                    {
                        Debug.Log ("Pausing "+child.name);
                        GetComponent<AudioSource>().spatialize = true;
                        GetComponent<AudioSource>().Pause();
                    }
                    else if (child.name == this.name)
                    {
                        Debug.Log ("Unpausing "+child.name);
                        GetComponent<AudioSource>().spatialize = false;
                        GetComponent<AudioSource>().UnPause(); 
                    }
                }
            }
        }


        //Handle the DoubleClick event
        private void HandleDoubleClick()
        {
            Debug.Log("Show double click");
            m_Renderer.material = m_DoubleClickedMaterial;
        }
    }
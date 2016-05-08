using UnityEngine;
using System.Collections;

public class CameraFacingBillboard : MonoBehaviour
{
	public Camera m_Camera;

	void Start()
	{
		m_Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
		StartCoroutine(InitialRotation());
	}

	void Update()
	{
		/*transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
		                 m_Camera.transform.rotation * Vector3.up);*/

		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.position - m_Camera.transform.position),0.001f);
	}


	IEnumerator InitialRotation()
	{
		//initial delay
		yield return new WaitForSeconds(0.0f);
		transform.rotation = Quaternion.LookRotation(transform.position - m_Camera.transform.position);
		// gameObject.SetActive(false);
	}
}
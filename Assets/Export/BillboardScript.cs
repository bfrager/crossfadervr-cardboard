using UnityEngine;
using System.Collections;

public class BillboardScript : MonoBehaviour
{
	public Camera m_Camera;

	void Update()
	{
		transform.LookAt(m_Camera.transform.position, Vector3.up);
	}
}
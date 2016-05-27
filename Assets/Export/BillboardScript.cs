using UnityEngine;
using System.Collections;

public class BillboardScript : MonoBehaviour
{
	public GameObject target;

	void Update()
	{
		transform.LookAt(target.transform.position, Vector3.up);
	}
}
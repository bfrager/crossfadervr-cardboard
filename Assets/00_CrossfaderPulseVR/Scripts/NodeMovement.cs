using UnityEngine;
using System.Collections;

public class NodeMovement : MonoBehaviour {

	public GameObject[] m_nodeList;
	public GameObject nextNode;
	public GameObject current_node;
	private Vector3 m_forward;
	public float buffer;
	public float speed = 5;



	// Use this for initialization
	void Start () 
	{
		nextNode = m_nodeList[0];

	}
	
	// Update is called once per frame
	void Update () 
	{
		Move();

	}


	void Move()
	{
		m_forward = nextNode.transform.position - transform.position;
		m_forward.y = 0f;
		m_forward.Normalize();

		transform.position += m_forward * speed * Time.deltaTime;

		if (transform.position.x >= nextNode.transform.position.x - buffer);
		{
			current_node = nextNode;
		}

		if(current_node == m_nodeList[m_nodeList.Length])
		{
			nextNode = m_nodeList[0];
			transform.position = nextNode.transform.position;
		}


	}
		
}

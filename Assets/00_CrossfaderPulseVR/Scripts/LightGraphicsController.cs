using UnityEngine;
using System.Collections;

public class LightGraphicsController : MonoBehaviour {

	public GameObject[] lights;
	public int BehaviorNumber;

	private bool canSwitch = true;
	private float rand;

	private Animator anim;


	public enum Behaviors{Falldown, RiseUp, RLPingpong}
	Behaviors currentState;

	//gather all lights in the child list.
	//apply animations to each light. Add a n second delay to have effects
	//add enum variations base on random int.

	// Use this for initialization
	void Start () 
	{
		
		currentState = Behaviors.RLPingpong;
	}

	IEnumerator RiseUp(float waitTime, GameObject go)
	{
		yield return new WaitForSeconds(waitTime);
		canSwitch = true;
	}
	IEnumerator PingPong(float waitTime, GameObject go)
	{
		yield return new WaitForSeconds(waitTime);
		canSwitch = true;
	}






	// Update is called once per frame
	void Update () 
	{
		rand = Random.Range(3,5);

		for(int i = 0; i < lights.Length; i++)
		{

			anim = lights[i].GetComponent<Animator>();
			anim.SetInteger("Switch", BehaviorNumber);
			//print("Make Lights fall down");
//			switch(currentState)
//			{
//			case Behaviors.Falldown:
//				canSwitch = false;
//				anim = lights[i].GetComponent<Animator>();
//				anim.SetInteger("Switch", 0);
//				print("Make Lights fall down");
//				break;
//			case Behaviors.RiseUp:
//				canSwitch = false;
//				print("Make Lights rise up");
//				break;
//			case Behaviors.RLPingpong:
//				anim = lights[i].GetComponent<Animator>();
//				anim.SetInteger("Switch", 1);
//				canSwitch = false;
//				print("Side to Side Motion");
//				break;
//			default:
//				canSwitch = false;
//				print("Default State");
//				break;
//			}

		}

	}

}

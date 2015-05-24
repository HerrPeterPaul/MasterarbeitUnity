using UnityEngine;
using System.Collections;
//using Parameters;

public class SphereMovement : MonoBehaviour {

	Vector3 startPosition = Vector3.up;
	public GameObject sphere;
	public GameObject helper;
	public GameObject arrow;
	public WindSpeed windSpeed;
	public Trials trials;
	public enum sphereStates
	{
		HIDDEN,
		MOVING,
		DROPPING,
		COLLIDED,
	}

	public sphereStates state = sphereStates.HIDDEN;


	// Use this for initialization
	void Start () 
	{
		windSpeed = WindSpeed.Instance;
		trials = Trials.Instance;
	}

	void FixedUpdate()
	{
		if (state == sphereStates.DROPPING)
		{
			Vector2 force = windSpeed.ComputeWindForce(sphere.transform.position);
			sphere.rigidbody.AddForce(new Vector3(force.x, 0, force.y));
		}
	}

	
	// Update is called once per frame
	void Update () 
	{
		Debug.Log (state);
		switch (state)
		{
		case sphereStates.MOVING:
			float x = Input.GetAxis ("L_XAxis_1"); 
			float z = -Input.GetAxis ("L_YAxis_1");
			Vector3 v = sphere.transform.position;
			v.x = v.x + x * Parameters.moveSpeed;
			if ( v.x < -Parameters.fieldSizeX )
			{
				v.x = -Parameters.fieldSizeX;
			}
			else if ( v.x > Parameters.fieldSizeX)
			{
				v.x = Parameters.fieldSizeX;
			}
			v.z = v.z + z * Parameters.moveSpeed;
			if ( v.z < -Parameters.fieldSizeZ )
			{
				v.z = -Parameters.fieldSizeZ;
			}
			else if ( v.z > Parameters.fieldSizeZ)
			{
				v.z = Parameters.fieldSizeZ;
			}
			sphere.transform.position = v;
			if (Input.GetButtonDown("A_1"))
			{
				SwitchState(sphereStates.DROPPING);
			}

			break;
		}
	}

	public void SwitchState(sphereStates newState)
	{
		this.state = newState;
		switch (newState)
		{
			case sphereStates.DROPPING:
				arrow.renderer.enabled = false;
				sphere.rigidbody.useGravity = true;
				sphere.renderer.enabled = true;
				break;
			case sphereStates.HIDDEN:
				arrow.renderer.enabled = false;
				sphere.renderer.enabled = false;
				sphere.rigidbody.useGravity = false;
				break;
			case sphereStates.MOVING:
				sphere.transform.position = startPosition;
				sphere.renderer.enabled = true;
				sphere.rigidbody.useGravity = false;
				if (trials.currentTrial.type == Trials.typeOfTrial.INTRO || trials.currentTrial.type == Trials.typeOfTrial.TRAINING)
				{
					arrow.renderer.enabled = true;
				}
				break;
		}
	}

	IEnumerator OnCollisionEnter(Collision col)
	{			
		sphere.rigidbody.useGravity = false;
		sphere.rigidbody.velocity = Vector3.zero;
		sphere.rigidbody.angularVelocity = Vector3.zero;
		SwitchState(sphereStates.COLLIDED);
		yield return new WaitForSeconds(Parameters.dispayOfHit);
		sphere.renderer.enabled = false;
		helper.SendMessage("newTrial");
	}
}

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
	
	Vector3 velocity;
	Rigidbody myRigidbody;

	void Start () {

		myRigidbody = GetComponent<Rigidbody> ();
	
	}
	
	public void Move(Vector3 _velocity) {				//we want to move our player object by that velocity. we want to use a rigid body to move it so that it contrains to collisions
		velocity = _velocity;
	
	}

	public void LookAt(Vector3 lookPoint){
		Vector3 heightCorrectedPoint = new Vector3 (lookPoint.x, transform.position.y, lookPoint.z);
		transform.LookAt (heightCorrectedPoint);
	}

	void FixedUpdate() {							//an update method specifically for the rigid body. you want it to execute it in small regular steps (not affected my frame rate)
		myRigidbody.MovePosition(myRigidbody.position + velocity * Time.fixedDeltaTime);
	}
}

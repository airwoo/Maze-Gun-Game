using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerController))]	//makes sure playercontroller is attached to the same game object. so if we add this player script to an object it will force it to add the playercontroller script with it
[RequireComponent(typeof(GunController))]
public class Player : LivingEntity {

	public float moveSpeed = 5;

	Camera viewCamera;
	PlayerController controller;				//reference to player controller so we can pass the movevelocity into the player controller script so it can handle all of the physics
	GunController gunController;


	protected override void Start () {
		base.Start();
		controller = GetComponent<PlayerController> ();				//we assume that the playercontroller is attached to the same game object as the player script
		gunController = GetComponent<GunController>();
		viewCamera = Camera.main;
	}
	

	void Update () {
		//Movement Input
		Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical"));
		Vector3 direction = moveInput.normalized;
		Vector3 moveVelocity = direction * moveSpeed;
		controller.Move (moveVelocity);

		//Look Input
		Ray ray = viewCamera.ScreenPointToRay (Input.mousePosition);					//return ray from the camera through the position into infinite . we must intersect the ray
		Plane groundPlane = new Plane (Vector3.up, Vector3.zero);		//generate a plane perpendicular to what we want and end point
		float rayDistance;

		if(groundPlane.Raycast(ray, out rayDistance)){		//takes in ray and takes out float enter (out means give a variable and assigns it a value whih is our rayDistance)
			Vector3 point = ray.GetPoint(rayDistance);
			//Debug.DrawLine (ray.origin, point, Color.red);
			controller.LookAt(point);
		}

		//Weapon Input
		if (Input.GetMouseButton (0)) {					//equivalent to if left button on mouse is being held down
			gunController.Shoot();
			
		}
	}
}

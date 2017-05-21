using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public LayerMask collisionMask;			//determine which objects/layers this project tile will collide with
	float speed = 10;
	float damage = 1;

	float lifetime = 3;
	float skinWidth = .1f;

	void Start(){
		Destroy (gameObject, lifetime);

		Collider[] initialCollisions = Physics.OverlapSphere (transform.position, .1f, collisionMask);		//array of all the colliders that our projectiles are intersecting with
		if (initialCollisions.Length > 0) {
			OnHitObject (initialCollisions [0]);
		}
	}

	public void SetSpeed(float newSpeed){
		speed = newSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		float moveDistance = speed * Time.deltaTime;
		CheckCollisions (moveDistance);
		transform.Translate (Vector3.forward * moveDistance);			//projectile move forward
	}

	void CheckCollisions(float moveDistance){
		Ray ray = new Ray (transform.position, transform.forward);		
		RaycastHit hit;

		if(Physics.Raycast(ray, out hit, moveDistance + skinWidth, collisionMask, QueryTriggerInteraction.Collide)){	//perform actual ray cast	
			//QueryTriggerInteraction which allows us to set whether or not this will collide with trigger colliders
			OnHitObject(hit);			//if we hit something we can call this onhitobject and pass in the hit variable
		}

	}

	void OnHitObject(RaycastHit hit){
		IDamageable damageableObject = hit.collider.GetComponent<IDamageable> ();
		if (damageableObject != null) {
			damageableObject.TakeHit (damage, hit);
		}
		GameObject.Destroy (gameObject);			//destroy project tile once it hits object

		}

	void OnHitObject(Collider c){

		IDamageable damageableObject = c.GetComponent<IDamageable> ();
		if (damageableObject != null) {
			damageableObject.TakeDamage (damage);
		}
		GameObject.Destroy (gameObject);			//destroy project tile once it hits object
	}
}

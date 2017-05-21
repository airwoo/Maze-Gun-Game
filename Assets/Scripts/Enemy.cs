using UnityEngine;
using System.Collections;

[RequireComponent (typeof (NavMeshAgent))]
public class Enemy : LivingEntity {

	public enum State {Idle, Chasing, Attacking};
	State currentState;

	NavMeshAgent pathfinder;		//get reference to NavMeshAgent component and path finder will handle all our path finding
	Transform target;
	LivingEntity targetEntity;
	Material skinMaterial;

	Color originalColour;

	float attackDistanceThreshold = .5f;
	float timeBetweenAttacks = 1;			//we dont want to attack every frame when this is the case, we need timer
	float damage = 1;

	float nextAttackTime;
	float myCollisionRadius;
	float targetCollisionRadius;

	bool hasTarget;


	// Use this for initialization
	protected override void Start () {
		base.Start ();
		pathfinder = GetComponent<NavMeshAgent> ();
		skinMaterial = GetComponent<Renderer>().material;
		originalColour = skinMaterial.color;

		if (GameObject.FindGameObjectWithTag ("Player") != null) {

			currentState = State.Chasing;
			hasTarget = true;

			target = GameObject.FindGameObjectWithTag ("Player").transform;			//assume our player will have player tag so the enemy knows what to chase
			targetEntity = target.GetComponent<LivingEntity> ();
			targetEntity.OnDeath += OnTargetDeath; 

			myCollisionRadius = GetComponent<CapsuleCollider> ().radius;
			targetCollisionRadius = target.GetComponent<CapsuleCollider> ().radius;

			StartCoroutine (UpdatePath ());
		}
	}

	void OnTargetDeath(){
		hasTarget = false;
		currentState = State.Idle;
	}

	// Update is called once per frame
	void Update () {

		//We could use Vector3.Distance but it requires sqrt operation which is expensive so when we dont need to know the actual distance 
		//and when we are just comparing distances, we can take their distances in their sq form to avoid sqrt operation

		if(hasTarget) {
			if (Time.time > nextAttackTime) {
				float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
				if (sqrDstToTarget < Mathf.Pow (attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2)) {				//if the distance is close enough to attack	
					nextAttackTime = Time.time + timeBetweenAttacks;
					StartCoroutine (Attack ());
				}
			}
		}
	}


	IEnumerator Attack(){				//we want to animate our lunge, so we need to store starting and targeting position

		currentState = State.Attacking;
		pathfinder.enabled = false;

		Vector3 originalPosition = transform.position;
		Vector3 dirToTarget = (target.position - transform.position).normalized;
		Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius);


		float attackSpeed = 3;
		float percent = 0;							//determine how far into the lunge animation we are

		skinMaterial.color = Color.red;
		bool hasAppliedDamage = false;

		while (percent <= 1) {					//while the percent is less than or equal to 1 then we will animate our lunge

			if (percent >= .5f && !hasAppliedDamage) {
				hasAppliedDamage = true;
				targetEntity.TakeDamage (damage);
			}

			percent += Time.deltaTime * attackSpeed;
			float interpolation = (-Mathf.Pow (percent, 2) + percent) * 4;
			transform.position = Vector3.Lerp (originalPosition, attackPosition, interpolation);

			yield return null;					//since this is a coroutine we say this to skip a frame between each step of the while loop
		}

		skinMaterial.color = originalColour;
		currentState = State.Chasing;
		pathfinder.enabled = true;
	}

	IEnumerator UpdatePath(){						//changes update from very frame to a fixed timer

		float refreshRate = .25f;

		while (hasTarget) {					//while there is a target
			if (currentState == State.Chasing) {					//only update the patch if the current state is chasing
				Vector3 dirToTarget = (target.position - transform.position).normalized;
				Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold/2);		//get target's position
				if (!dead) {
					pathfinder.SetDestination (targetPosition);
				}
			}
			yield return new WaitForSeconds (refreshRate);		//repeat this loop every refresh rate

		}

	}

}
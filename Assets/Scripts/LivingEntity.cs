using UnityEngine;
using System.Collections;

public class LivingEntity : MonoBehaviour, IDamageable {

	public float startingHealth;
	protected float health;			//protected means that it will not be available to other classes except the derived classes and it wont appear in the inspector
	protected bool dead;

	public event System.Action OnDeath;			
	//we create an event because we dont want the living entity have anything to do with the spawner //spawner should subscribe to event and be notified //method that is void and takes no parameters

	protected virtual void Start(){		
		health = startingHealth;
	}

	public void TakeHit(float damage, RaycastHit hit){
		//Do Some stuff here with hit var
		TakeDamage (damage);
	}


	public void TakeDamage(float damage){

		health -= damage;

		if (health <= 0 && !dead) {
			Die ();
		}
	}

	protected void Die(){
		dead = true;
		if (OnDeath != null) {
			OnDeath ();
		}
		GameObject.Destroy (gameObject);
	}
}

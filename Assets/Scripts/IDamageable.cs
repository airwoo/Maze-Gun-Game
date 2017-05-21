using UnityEngine;

public interface IDamageable{

	void TakeHit(float damage, RaycastHit hit);				// damage it need to take and where it was hit

	void TakeDamage(float damage);


}
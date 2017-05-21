using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour {

	public Transform weaponHold;
	public Gun startingGun;
	Gun equippedGun;			//stores our currently equipped gun in a variable called equipped gun

	void Start(){
		if (startingGun != null) {
			EquipGun (startingGun);
		}
	}

	public void EquipGun(Gun gunToEquip){		//takes in a gun called gunToEquip
		if (equippedGun != null){
			Destroy (equippedGun.gameObject);		//if there is already an equipped gun then destroy the current gun
		}
			equippedGun = Instantiate (gunToEquip, weaponHold.position, weaponHold.rotation) as Gun;		//when we want to equip a new gun 
			equippedGun.transform.parent = weaponHold;
		}

	public void Shoot(){
		if (equippedGun != null) { 				//checks if there is a weapon currently equipped
			equippedGun.Shoot ();
		}
	}
}

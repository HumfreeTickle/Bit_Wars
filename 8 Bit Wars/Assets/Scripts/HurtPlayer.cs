using UnityEngine;
using System.Collections;

public class HurtPlayer : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D col){

		if(col.gameObject.layer == LayerMask.NameToLayer("Player")){
			Health playerHealth = col.gameObject.GetComponent<Health>();
			playerHealth.currentHealth -= 5;
			Destroy(this.gameObject);
		}else{
			Destroy(this.gameObject);
		}
	}
}

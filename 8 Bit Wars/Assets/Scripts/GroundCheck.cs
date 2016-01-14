using UnityEngine;
using System.Collections;

[RequireComponent(typeof (CircleCollider2D))]

public class GroundCheck : MonoBehaviour
{
	private Health playerHealth;
	private LayerMask mask;
	private bool inTheAir = false;
	public bool damagePlayer;

	private Rigidbody2D rb;

//	void OnValidate(){
//		CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
//
//		circleCollider.isTrigger = false;
//		circleCollider.offset = new Vector2(0, -0.45f);
//		circleCollider.radius = 0.16f;
//	}

	void Start ()
	{
		rb = GetComponent<Rigidbody2D>();
		playerHealth = GetComponent<Health>();
		mask = LayerMask.NameToLayer ("Ground");
	
	}

	void FixedUpdate(){
		GetComponent<Animator>().SetBool("Jumping", inTheAir);

		// If the character is falling take damage
		if (rb.velocity.y < -10){
			damagePlayer = true;
		}else{
			damagePlayer = false;
		}
	}

	void OnCollisionStay2D (Collision2D col)
	{
		if (col.gameObject.layer == mask) {
			inTheAir = false;

			if(damagePlayer){
				playerHealth.currentHealth -= 3;
			}
		} 
	}

	void OnCollisionExit2D (Collision2D col)
	{
		if (col.gameObject.layer == mask) {
			inTheAir = true;
		} 
	}
}

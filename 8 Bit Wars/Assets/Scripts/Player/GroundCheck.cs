using UnityEngine;
using System.Collections;

[RequireComponent(typeof (CircleCollider2D))]

public class GroundCheck : MonoBehaviour
{
	private Health playerHealth;
	private LayerMask mask;
	private bool inTheAir = false;
	private bool damagePlayer;

	public float fallDamage = 10;
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
		if (rb.velocity.y < -fallDamage){
			damagePlayer = true;
		}

		if (transform.position.y < -10) {
			transform.parent.GetComponent<TeamManager>().players.Remove(gameObject.name);
			Destroy(gameObject);
		}

	}

	void OnCollisionStay2D (Collision2D col)
	{
		if (col.gameObject.layer == mask) {
			inTheAir = false;

			if(damagePlayer){
				playerHealth.currentHealth -= 3;
				damagePlayer = false;

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

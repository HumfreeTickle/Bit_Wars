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

	private GameObject hurtPlayer;

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
		hurtPlayer = (GameObject)Resources.Load ("HurtPlayerParticles");
	}

	void FixedUpdate(){
		GetComponent<Animator>().SetBool("Jumping", inTheAir);

		// If the character is falling take damage
		if (rb.velocity.y < -fallDamage){
			damagePlayer = true;
			Turns.turnOver = 0f;
			// This somehow has to transfer to the turns coroutine and end the turn
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
				Instantiate(hurtPlayer, transform.position, Quaternion.identity);
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

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CircleCollider2D))]

public class Test_explosion : MonoBehaviour
{
	private bool hit;
	private float r = 0.5f;
	public GameObject energyParticles;

	 
	void FixedUpdate ()
	{
		if (hit) { 
			// Increases the radius of the collider

			if (GetComponent<CircleCollider2D> ().radius <= r * 2) {
				GetComponent<CircleCollider2D> ().radius *= 1.2f;
			} else if (GetComponent<CircleCollider2D> ().radius > r * 2) {
				// causing an error when it gets destroyed too soon.
				Destroy (this.gameObject, 0.1f);
			}
		} else {
			r = GetComponent<CircleCollider2D> ().radius;
		}
	}
	
	void OnCollisionEnter2D (Collision2D col)
	{
		if (col.gameObject.layer == LayerMask.NameToLayer ("Ground")) {

			GetComponent<Renderer> ().enabled = false;

			// Calls the destroyBlock function
			GetComponent<DestroyBlock>().OnBlockDestroyed (col.gameObject.GetComponent<Surroundings> ().surroundingBlocks, col.gameObject);
			hit = true;
		}

		if (col.gameObject.layer == LayerMask.NameToLayer ("Player")) {

			// pushes the hit player backwards 
			col.gameObject.GetComponent<Rigidbody2D> ().AddForce ((Vector2.up + (-1 * Vector2.right * col.gameObject.transform.localScale.x)) * 100);
			Health playerHealth = col.gameObject.GetComponent<Health> ();
			playerHealth.currentHealth -= 5;

			// Creates a small particle effect 
			ContactPoint2D contact = col.contacts[0];
			Quaternion particleRotation = Quaternion.FromToRotation(Vector3.right * col.transform.localScale.x, contact.normal);
			Vector3 particlePosition = contact.point;
			
			GameObject particles = Instantiate(energyParticles, particlePosition, particleRotation) as GameObject;
			particles.transform.parent = col.transform;
		}
	}
}

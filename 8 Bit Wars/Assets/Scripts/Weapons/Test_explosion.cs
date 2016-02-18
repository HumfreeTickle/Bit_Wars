using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CircleCollider2D))]

public class Test_explosion : MonoBehaviour
{
	public GameObject energyParticles;
	public GameObject explosionParticles;

	[Range (0, 100)]
	public float explostionSize;
// = Mathf.Clamp(3, 0, 100);

	void OnCollisionEnter2D (Collision2D col)
	{
		if (GetComponent<Rigidbody2D> ()) {
			if (col.gameObject.layer == LayerMask.NameToLayer ("Ground")) {
				Explosion (col);

				// Calls the destroyBlock function
				GetComponent<DestroyBlock> ().OnBlockDestroyed (col.gameObject.GetComponent<Surroundings> ().surroundingBlocks, col.gameObject);
			}


			if (col.gameObject.layer == LayerMask.NameToLayer ("Player")) {
				Explosion (col);

				// pushes the hit player backwards 
				col.gameObject.GetComponent<Rigidbody2D> ().AddForce ((Vector2.up + (-1 * Vector2.right * col.gameObject.transform.localScale.x)) * 100);
				Health playerHealth = col.gameObject.GetComponent<Health> ();
				playerHealth.currentHealth -= 5;

				// Creates a small particle effect 
				ContactPoint2D contact = col.contacts [0];
				Quaternion particleRotation = Quaternion.FromToRotation (Vector3.right * col.transform.localScale.x, contact.normal);
				Vector3 particlePosition = contact.point;
			
				GameObject particles = Instantiate (energyParticles, particlePosition, particleRotation) as GameObject;
				particles.transform.parent = col.transform;

			}
		}
	}

	void Explosion (Collision2D col)
	{
		GetComponent<Renderer> ().enabled = false;

		GetComponent<CircleCollider2D> ().radius = Mathf.Clamp (GetComponent<CircleCollider2D> ().radius * explostionSize, 0, 5);
		ContactPoint2D contact = col.contacts [0];
		Quaternion particleRotation = Quaternion.FromToRotation (Vector3.right * col.transform.localScale.x, contact.normal);
		Vector3 particlePosition = contact.point;
		Instantiate (explosionParticles, particlePosition, particleRotation);
			
		Destroy (this.gameObject, 0.3f);
	}
}

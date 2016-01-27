using UnityEngine;
using System.Collections;

public class HurtPlayer : MonoBehaviour
{
	public GameObject energyParticles;

	void OnCollisionEnter2D (Collision2D col)
	{
		if (col.gameObject.layer == LayerMask.NameToLayer ("Player")) {
			Health playerHealth = col.gameObject.GetComponent<Health> ();
			playerHealth.currentHealth -= 5;

			ContactPoint2D contact = col.contacts[0];
			Quaternion particleRotation = Quaternion.FromToRotation(Vector3.right * col.transform.localScale.x, contact.normal);
			Vector3 particlePosition = contact.point;

			GameObject particles = Instantiate(energyParticles, particlePosition, particleRotation) as GameObject;
			particles.transform.parent = col.transform;
			Destroy (this.gameObject);

		} else if (col.gameObject.tag == "Ground") {
			GetComponent<DestroyBlock>().OnBlockDestroyed (col.gameObject.GetComponent<Surroundings> ().surroundingBlocks, col.gameObject);
			Destroy (this.gameObject);
		} else {
			Destroy (this.gameObject);
		}
	}
}

using UnityEngine;
using System.Collections;

[RequireComponent(typeof (CircleCollider2D))]

public class Test_explosion : MonoBehaviour
{

	void Update ()
	{
		if (GetComponent<CircleCollider2D> ().radius >= 5) {
			Destroy (this.gameObject);
		}
	}
	
	void OnCollisionEnter2D (Collision2D col)
	{
		if (col.gameObject.layer == LayerMask.NameToLayer ("Ground") || col.gameObject.layer == LayerMask.NameToLayer ("Player")) {
			GetComponent<Renderer> ().enabled = false;

			if (col.gameObject.layer == LayerMask.NameToLayer ("Ground")) {
				GetComponent<DestroyBlock> ().OnBlockDestroyed (col.gameObject.GetComponent<Level_Colliders> ().edgeIntersects, col.gameObject);
			}
			if (GetComponent<CircleCollider2D> ().radius <= 5) {
				GetComponent<CircleCollider2D> ().radius *= 2f;
			}
		}
	}

//	void OnCollisionStay2D (Collision2D col)
//	{
//		if (col.gameObject.layer == LayerMask.NameToLayer ("Ground") || col.gameObject.layer == LayerMask.NameToLayer ("Player")) {
//			if (GetComponent<CircleCollider2D> ().radius <= 10) {
//				transform.localScale *= 2f;
//
//				GetComponent<CircleCollider2D> ().radius *= 2f;
//			} else {
//				Destroy (this.gameObject);
//			}
//		}
//	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestroyBlock : MonoBehaviour
{
	public GameObject destroyParticleEffect;

	public void OnBlockDestroyed (List<Transform> surroundings, GameObject block)
	{
		if(destroyParticleEffect){
			Vector3 placement = new Vector3(block.transform.position.x, block.transform.position.y - block.transform.lossyScale.y/2,  block.transform.position.z);
			Instantiate(destroyParticleEffect, placement , Quaternion.identity);
		}
		Destroy (block);
		ReCheckBlocks(surroundings);
	}

	/// <summary>
	/// Rechecks surrounding on all blocks near destroyed block
	/// </summary>
	/// <param name="edgesList">List of all ground blocks touch the destroyed block.</param>
	void ReCheckBlocks(List<Transform> surroundings){

		foreach (Transform intersect in surroundings) {
			if (intersect != null) {
				intersect.gameObject.layer = LayerMask.NameToLayer ("Ground");
				intersect.gameObject.GetComponent<Surroundings> ().surroundingBlocks.Clear ();
				GameObject.Find("Level").GetComponent<Level_Colliders> ().Direction (surroundings);
			}else{
				surroundings.Remove(intersect);
				ReCheckBlocks(surroundings);
			}
		}
	}
}






//		Destroy (GetComponent<BoxCollider2D> ());

//			edgesList [intersect].GetComponent<Level_Colliders> ().edgeIntersects.Remove (this.gameObject);



// Destroys the current set of edge colliders
//			if (edgesList [intersect].GetComponent<EdgeCollider2D> ()) {
//				EdgeCollider2D[] edges = edgesList [intersect].GetComponents<EdgeCollider2D> ();
//				foreach (EdgeCollider2D edge in edges) {
//					
//					Destroy (edge);
//				}
//			}
//			edgesList[intersect].GetComponent<Level_Colliders>().colliders_Edge.Clear();
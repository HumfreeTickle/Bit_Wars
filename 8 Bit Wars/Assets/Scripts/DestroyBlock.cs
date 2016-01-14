using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestroyBlock : MonoBehaviour
{
	public GameObject destroyParticleEffect;

	public void OnBlockDestroyed (List<GameObject> edgesList, GameObject block)
	{
		if(destroyParticleEffect){
			Vector3 placement = new Vector3(block.transform.position.x, block.transform.position.y - block.transform.lossyScale.y/2,  block.transform.position.z);
			Instantiate(destroyParticleEffect, placement , Quaternion.identity);
		}
//		ReCheckBlocks(edgesList);
		Destroy (block);
	}

	/// <summary>
	/// Rechecks surrounding on all blocks near destroyed block
	/// </summary>
	/// <param name="edgesList">List of all ground blocks touch the destroyed block.</param>
	void ReCheckBlocks(List<GameObject> edgesList){

		for (int intersect = 0; intersect < edgesList.Count; intersect++) {
			if (edgesList [intersect] != null) {
				edgesList [intersect].layer = LayerMask.NameToLayer ("Ground");
				edgesList [intersect].GetComponent<Level_Colliders> ().edgeIntersects.Clear ();
				edgesList [intersect].GetComponent<Level_Colliders> ().Direction ();
			}else{
				edgesList.RemoveAt(intersect);
				ReCheckBlocks(edgesList);
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
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DestroyBlock : MonoBehaviour{
	public void OnBlockDestroyed (List<GameObject> edgesList, GameObject block)
	{
		Destroy (block);
		for (int intersect = 0; intersect < edgesList.Count; intersect++) {
			edgesList[intersect].layer = LayerMask.NameToLayer("Ground");
			edgesList [intersect].GetComponent<Level_Colliders> ().edgeIntersects.Clear ();
			edgesList [intersect].GetComponent<Level_Colliders> ().Direction ();
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
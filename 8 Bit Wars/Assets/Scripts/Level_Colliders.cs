using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level_Colliders : MonoBehaviour
{
//	private Vector2 col_Offset_Pos = new Vector2 (0, 0.5f);
//	private Vector2 col_Offset_Neg = new Vector2 (0, -0.5f);

	/// <summary>
	/// 0 - right
	/// 1 - left
	/// 2 - down
	/// 3 - up
	/// </summary>
//	private Dictionary<string, EdgeCollider2D> colliders_Edge{ get; set; }

//	private Dictionary<string, bool> colliders_Edge{ get; set; }
	
	public List<GameObject> edgeIntersects;//{get; private set;}

	void Awake ()
	{
		if(transform.position.y < 6){
			Direction ();
		}
	}

	void Start ()
	{
		if (edgeIntersects.Count >= 4) {
			EdgeColliders ();
		}
		float roundedX = Mathf.RoundToInt (transform.position.x);
		float roundedY = Mathf.RoundToInt (transform.position.y);
		transform.position = new Vector3 (roundedX, roundedY, transform.position.z);
	}

	public void Direction ()
	{
		Vector2 directionOfRay = Vector2.up;

		for (int direction = 0; direction < 4; direction++) {
			switch (direction) {
			case 0:
				directionOfRay = Vector2.up;
				RayCastSurroundings (directionOfRay);
				break;
			case 1:
				directionOfRay = Vector2.right;
				RayCastSurroundings (directionOfRay);
				break;
			case 2:
				directionOfRay = Vector2.right * -1;
				RayCastSurroundings (directionOfRay);
				break;
			case 3:
				directionOfRay = Vector2.up * -1;
				RayCastSurroundings (directionOfRay);
				break;
			default:
				Debug.LogError ("Ray went wrong");
				break;
			}
		}

		CheckSurroundings ();
	}

	void RayCastSurroundings (Vector2 directionOfRay)
	{
		RaycastHit2D[] hitInfo = Physics2D.RaycastAll (transform.position, directionOfRay, 1);

		foreach (RaycastHit2D hit in hitInfo) {
			if (hit.collider.gameObject != this.gameObject) {
				if (hit.collider != null) {
//					Debug.Log (this.gameObject.name + " : " + hit.collider.gameObject.name + directionOfRay);
					edgeIntersects.Add (hit.collider.gameObject);
				} else {
					Debug.LogWarning ("Raycast failed");
				}
			}
		}
	}

	public void CheckSurroundings ()
	{
//		colliders_Edge = new Dictionary<string, EdgeCollider2D> (edgeIntersects.Count);
//		colliders_Edge = new Dictionary<string, bool> (edgeIntersects.Count);

		
		
//		Bounds boxBounds = this.GetComponent<Renderer> ().bounds;
//

//			if (boxBounds.Intersects (edgeIntersects [intersect].GetComponent<Renderer> ().bounds)) {
//
//				// ------- RIGHT ------- //
//				if (edgeIntersects [intersect].transform.position.x > transform.position.x &&
//					edgeIntersects [intersect].transform.position.y == transform.position.y) {
////					print(this.gameObject.name);
////					print (edgeIntersects [intersect].name + " right");
////					if (colliders_Edge.ContainsKey ("right")) {
		//////						break;
////					} 
////				} else {
////					if (!colliders_Edge.ContainsKey ("right")) {
//////						EdgeCollider2D edge = gameObject.AddComponent<EdgeCollider2D> ();
//						colliders_Edge.Add ("right", true);
////					} 
//				}
//
//				// ------- LEFT ------- //
//				if (edgeIntersects [intersect].transform.position.x < transform.position.x &&
//					edgeIntersects [intersect].transform.position.y == transform.position.y) {
////					print(this.gameObject.name);
////					print (edgeIntersects [intersect].name + " left");
//				
////					if (colliders_Edge.ContainsKey ("left")) {
		//////						break;
////					} 
////				} else {
////					if (!colliders_Edge.ContainsKey ("left")) {
////						EdgeCollider2D edge = gameObject.AddComponent<EdgeCollider2D> ();
//						colliders_Edge.Add ("left", true);
////					} 
//
//				} 
//
//				// ------- DOWN ------- //
//				if (edgeIntersects [intersect].transform.position.y < transform.position.y &&
//					edgeIntersects [intersect].transform.position.x == transform.position.x) {
////					print (this.gameObject.name + " INTERSECTS " + edgeIntersects [intersect].name + ": DOWN");
////					if (colliders_Edge.ContainsKey ("down")) {
		//////						break;
////					} 
////				} else {
////					if (!colliders_Edge.ContainsKey ("down")) {
////						EdgeCollider2D edge = gameObject.AddComponent<EdgeCollider2D> ();
//						colliders_Edge.Add ("down", true);
////					}
//				}
//
		
//			}
//		}
	


		
		
		//			if (GetComponent<EdgeCollider2D> ()) {
//				EdgeCollider2D[] edges = GetComponents<EdgeCollider2D> ();
//				foreach (EdgeCollider2D edge in edges) {
//					Destroy (edge);
//				}
//			}

		if (!gameObject.GetComponent<BoxCollider2D> ()) {
			gameObject.AddComponent<BoxCollider2D> ();
//				colliders_Edge.Clear ();

		}

//			edgeIntersects.Clear();
//			print(this.gameObject);
		if (edgeIntersects.Count < 4) {

			gameObject.tag = "Ground";

			for (int intersect = 0; intersect < edgeIntersects.Count; intersect++) {
				// ------- UP ------- //
				if (edgeIntersects [intersect].transform.position.y > transform.position.y) {
					if (gameObject.layer != LayerMask.NameToLayer ("Ground")) {
						gameObject.layer = LayerMask.NameToLayer ("UnLayered");
						break;
					}
				} else {
					gameObject.layer = LayerMask.NameToLayer ("Ground");
				}

//				// ------- DOWN ------- //
//				if (edgeIntersects [intersect].transform.position.y < transform.position.y) {
//					gameObject.layer = LayerMask.NameToLayer ("UnLayered");
//					break;
//				} else {
//					gameObject.layer = LayerMask.NameToLayer ("Ground");
//				}
//
//				// ------- RIGHT ------- //
//				if (edgeIntersects [intersect].transform.position.x > transform.position.x){
//					gameObject.layer = LayerMask.NameToLayer ("UnLayered");
//					break;
//				} else {
//					gameObject.layer = LayerMask.NameToLayer ("Ground");
//				}
//
//				// ------- LEFT ------- //
//				if (edgeIntersects [intersect].transform.position.x < transform.position.x){
//					gameObject.layer = LayerMask.NameToLayer ("UnLayered");
//					break;
//				} else {
//					gameObject.layer = LayerMask.NameToLayer ("Ground");
//				}

			}
		}
	}

	void EdgeColliders ()
	{	
//		if (colliders_Edge.ContainsKey ("down")) {
//			colliders_Edge ["down"].offset = col_Offset_Neg;
//		}
//
//		if (colliders_Edge.ContainsKey ("right")) {
//			Vector2[] tempArray = colliders_Edge ["right"].points;
//			colliders_Edge ["right"].offset = col_Offset_Pos;
//			tempArray [0].x = 0.5f;
//			tempArray [0].y = -1;
//			colliders_Edge ["right"].points = tempArray;
//		}
//
//		if (colliders_Edge.ContainsKey ("left")) {
//			Vector2[] tempArray = colliders_Edge ["left"].points;
//			colliders_Edge ["left"].offset = col_Offset_Pos;
//			tempArray [1].x = -0.5f;
//			tempArray [1].y = -1;
//			colliders_Edge ["left"].points = tempArray;
//		}
//
//		if (colliders_Edge.ContainsKey ("up")) {
//			colliders_Edge ["up"].offset = col_Offset_Pos;
//		}

//		colliders_Edge.Clear ();
		edgeIntersects.Clear ();
		Destroy (GetComponent<BoxCollider2D> ());
	}
}
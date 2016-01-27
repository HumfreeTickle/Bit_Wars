using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Level_Colliders : MonoBehaviour
{
	/// <summary>
	/// Idea : Keep all the box colliders on the ground boxes
	/// But add ridgidbodies to thw ones on top
	/// Wouldn't really matter then if there is a slight delay changing the blocks around
	/// </summary>

	private GameManager gameManager;
	private List<Transform> ground;
	public List<Transform> groundBlockList;
	public List<GameObject> edgeIntersects;//{get; private set;}

	public bool levelTesting = false;

	public Sprite dirt;
	public Sprite dirt_Grass;

	void Awake ()
	{
		gameManager = GameObject.Find("GameManger").GetComponent<GameManager>();
		ground = new List<Transform> (transform.childCount);


		// aligns all the blocks to nearest int
		for (int i = 0; i < transform.childCount; i++) {
			ground.Add (transform.GetChild (i).transform);
			
			float roundedX = Mathf.RoundToInt (transform.GetChild (i).transform.position.x * 100);
			float roundedY = Mathf.RoundToInt (transform.GetChild (i).transform.position.y * 100);

			transform.GetChild (i).transform.position = new Vector3 (roundedX/100, roundedY/100, transform.GetChild (i).transform.position.z);
		}

		if (Application.loadedLevelName == "Level Creation Testing") {
			levelTesting = true;
		} else {
			levelTesting = false;
		}

		if (!levelTesting) {
			Direction (ground);
		}

	}

	public void Direction (List <Transform> GroundBlocks = null)
	{
		foreach (Transform block in GroundBlocks) {

			Vector2 directionOfRay = Vector2.up;

			for (int direction = 0; direction < 4; direction++) {
				switch (direction) {
				case 0:
					directionOfRay = Vector2.up;
					RayCastSurroundings (directionOfRay, block);
					break;
				case 1:
					directionOfRay = Vector2.right;
					RayCastSurroundings (directionOfRay, block);
					break;
				case 2:
					directionOfRay = Vector2.right * -1;
					RayCastSurroundings (directionOfRay, block);
					break;
				case 3:
					directionOfRay = Vector2.up * -1;
					RayCastSurroundings (directionOfRay, block);
					break;
				default:
					Debug.LogError ("Ray went wrong");
					break;
				}
			}

//			print (edgeIntersects.Count);
			if (edgeIntersects.Count < 4) {
				CheckSurroundings (block);
			}else{
				block.gameObject.tag = "Ground";
				block.gameObject.layer = LayerMask.NameToLayer ("UnLayered");
				foreach (GameObject edge in edgeIntersects) {
					block.GetComponent<Surroundings> ().surroundingBlocks.Add (edge.transform);
				}
				edgeIntersects.Clear();
			}
		}
	}

	void RayCastSurroundings (Vector2 directionOfRay, Transform currentBlock)
	{
		RaycastHit2D[] hitInfo = Physics2D.RaycastAll (currentBlock.position, directionOfRay, currentBlock.localScale.x);

		if (hitInfo.Length == 0) {
			Debug.LogError ("No info: " + currentBlock);
		}

		foreach (RaycastHit2D hit in hitInfo) {
			if (hit.collider != null) {
				if (hit.collider.gameObject != currentBlock.gameObject) {
					if (hit.collider.tag == "Ground") {
						edgeIntersects.Add (hit.collider.gameObject);
					}
//					else {
//						Debug.LogWarning ("Raycast failed: tag");
//					}
				} 
			} else {
				Debug.LogWarning ("Raycast failed: null");
			}
		}
	}

	public void CheckSurroundings (Transform currentBlock)
	{
		// Reorders list to the order they appear in Hierarchy
		edgeIntersects.OrderBy (go => go.name).ToList ();

		// if there is an open edge to the block
		if (edgeIntersects.Count > 0) {

			currentBlock.gameObject.tag = "Ground";

			// checks to see if there is a block above 
			foreach (GameObject intersect in edgeIntersects) {

				// ------- UP ------- //
				if (intersect.transform.position.y > currentBlock.position.y || currentBlock.position.y >= gameManager.maxHeight) {
					if (currentBlock.gameObject.layer != LayerMask.NameToLayer ("Ground")) {
						currentBlock.gameObject.layer = LayerMask.NameToLayer ("UnLayered");
						currentBlock.GetComponent<SpriteRenderer>().sprite = dirt;

					}
				} 
			}

			if (currentBlock.gameObject.layer != LayerMask.NameToLayer ("UnLayered")) {
				currentBlock.gameObject.layer = LayerMask.NameToLayer ("Ground");
				currentBlock.GetComponent<SpriteRenderer>().sprite = dirt_Grass;
//				currentBlock.gameObject.AddComponent<Rigidbody2D>();

				groundBlockList.Add (currentBlock);

			}

		} else if (edgeIntersects.Count <= 0) {
			currentBlock.gameObject.tag = "Ground";
			currentBlock.gameObject.layer = LayerMask.NameToLayer ("Ground");
			currentBlock.GetComponent<SpriteRenderer>().sprite = dirt_Grass;
			groundBlockList.Add (currentBlock);
		}

		// Adds list of surrounding blocks into that block's surrounding's list
		foreach (GameObject edge in edgeIntersects) {
			currentBlock.GetComponent<Surroundings>().CullBlocks(edge.transform);
		}


		edgeIntersects.Clear ();
	}

}// End of script



//-------------------------------------------Direction of intersects------------------------------------------------//
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


//------------------------------------ Bounds Checks -----------------------------------------------//		
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


//	void EdgeColliders ()
//	{	
////		if (colliders_Edge.ContainsKey ("down")) {
////			colliders_Edge ["down"].offset = col_Offset_Neg;
////		}
////
////		if (colliders_Edge.ContainsKey ("right")) {
////			Vector2[] tempArray = colliders_Edge ["right"].points;
////			colliders_Edge ["right"].offset = col_Offset_Pos;
////			tempArray [0].x = 0.5f;
////			tempArray [0].y = -1;
////			colliders_Edge ["right"].points = tempArray;
////		}
////
////		if (colliders_Edge.ContainsKey ("left")) {
////			Vector2[] tempArray = colliders_Edge ["left"].points;
////			colliders_Edge ["left"].offset = col_Offset_Pos;
////			tempArray [1].x = -0.5f;
////			tempArray [1].y = -1;
////			colliders_Edge ["left"].points = tempArray;
////		}
////
////		if (colliders_Edge.ContainsKey ("up")) {
////			colliders_Edge ["up"].offset = col_Offset_Pos;
////		}
//
////		colliders_Edge.Clear ();
//
////		print (this.name);
//		edgeIntersects.Clear ();
////		Destroy (GetComponent<BoxCollider2D> ());
//	}

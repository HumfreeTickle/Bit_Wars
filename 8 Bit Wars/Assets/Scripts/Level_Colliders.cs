using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level_Colliders : MonoBehaviour
{
	private Vector2 col_Offset_Pos = new Vector2 (0, 0.5f);
	private Vector2 col_Offset_Neg = new Vector2 (0, -0.5f);

	/// <summary>
	/// 0 - right
	/// 1 - left
	/// 2 - down
	/// 3 - up
	/// </summary>
	private Dictionary<string, EdgeCollider2D> colliders_Edge{get; set;} 
	public GameObject box2;
	public GameObject box3;
	public GameObject box4;
	public GameObject box5;
	public List<GameObject> edgeIntersects;//{get; private set;}

	void Awake ()
	{
		colliders_Edge = new Dictionary<string, EdgeCollider2D> (4);

		if (box2 && box3 && box4 && box5) {

			edgeIntersects = new List<GameObject> (4);
			edgeIntersects.Add (box2);
			edgeIntersects.Add (box3);
			edgeIntersects.Add (box4);
			edgeIntersects.Add (box5);

			CheckSurroundings ();
		}
	}

	public void CheckSurroundings ()
	{
		Bounds boxBounds = this.GetComponent<Renderer> ().bounds;

		for (int intersect = 0; intersect < edgeIntersects.Count; intersect++) {

			if (boxBounds.Intersects (edgeIntersects [intersect].GetComponent<Renderer> ().bounds)) {

				// ------- RIGHT ------- //
				if (edgeIntersects [intersect].transform.position.x > transform.position.x &&
				    edgeIntersects [intersect].transform.position.y == transform.position.y) {
//					print(this.gameObject.name);
//					print (edgeIntersects [intersect].name + " right");
					if (colliders_Edge.ContainsKey ("right")) {
						Destroy (colliders_Edge ["right"]);
					} 
				} else {
					if (!colliders_Edge.ContainsKey ("right")) {
						EdgeCollider2D edge = gameObject.AddComponent<EdgeCollider2D> ();
						colliders_Edge.Add ("right", edge);
					} 
				}

				// ------- LEFT ------- //
				if (edgeIntersects [intersect].transform.position.x < transform.position.x &&
				    edgeIntersects [intersect].transform.position.y == transform.position.y) {
//					print(this.gameObject.name);
//					print (edgeIntersects [intersect].name + " left");
				
					if (colliders_Edge.ContainsKey ("left")) {
						Destroy (colliders_Edge ["left"]);
					} 
				} else {
					if (!colliders_Edge.ContainsKey ("left")) {
						EdgeCollider2D edge = gameObject.AddComponent<EdgeCollider2D> ();
						colliders_Edge.Add ("left", edge);
					} 

				} 

				// ------- DOWN ------- //
				if (edgeIntersects [intersect].transform.position.y < transform.position.y &&
				    edgeIntersects [intersect].transform.position.x == transform.position.x) {
//					print(this.gameObject.name);
//
//					print (edgeIntersects [intersect].name + " down");
					if (colliders_Edge.ContainsKey ("down")) {
						Destroy (colliders_Edge ["down"]);
					} 
				} else {
					if (!colliders_Edge.ContainsKey ("down")) {
						EdgeCollider2D edge = gameObject.AddComponent<EdgeCollider2D> ();
						colliders_Edge.Add ("down", edge);
					}
				}

				// ------- UP ------- //
				if (edgeIntersects [intersect].transform.position.y > transform.position.y &&
				    edgeIntersects [intersect].transform.position.x == transform.position.x) {
//					print(this.gameObject.name);
//
//					print (edgeIntersects [intersect].name + " up");
					if (colliders_Edge.ContainsKey ("up")) {
						Destroy (colliders_Edge ["up"]);
					} 
				} else {
					if (!colliders_Edge.ContainsKey ("up")) {
						EdgeCollider2D edge = gameObject.AddComponent<EdgeCollider2D> ();
						colliders_Edge.Add ("up", edge);
					}
				} 
			}
		}

		if (colliders_Edge.Count > 0) {
			EdgeColliders ();
		}else{
			gameObject.AddComponent<BoxCollider2D>();
		}
	}

	void EdgeColliders ()
	{
		if (colliders_Edge.ContainsKey ("down")) {
			colliders_Edge ["down"].offset = col_Offset_Neg;
		}

		if (colliders_Edge.ContainsKey ("right")) {
			Vector2[] tempArray = colliders_Edge ["right"].points;
			colliders_Edge ["right"].offset = col_Offset_Pos;
			tempArray [0].x = 0.5f;
			tempArray [0].y = -1;
			colliders_Edge ["right"].points = tempArray;
		}

		if (colliders_Edge.ContainsKey ("left")) {
			Vector2[] tempArray = colliders_Edge ["left"].points;
			colliders_Edge ["left"].offset = col_Offset_Pos;
			tempArray [1].x = -0.5f;
			tempArray [1].y = -1;
			colliders_Edge ["left"].points = tempArray;
		}

		if (colliders_Edge.ContainsKey ("up")) {
			colliders_Edge ["up"].offset = col_Offset_Pos;
		}

		colliders_Edge.Clear();

		gameObject.tag = "Ground";
	}

	public void OnBlockDestroyed(List<GameObject> edgesList){
		for (int intersect = 0; intersect < edgesList.Count; intersect++) {
			edgesList[intersect].GetComponent<Level_Colliders>().edgeIntersects.Remove(this.gameObject);
//			edgesList[intersect].GetComponent<Level_Colliders>().colliders_Edge.Clear();
			edgesList[intersect].GetComponent<Level_Colliders>().CheckSurroundings();
		}

		Destroy(this.gameObject);
	}
}





//EdgeCollider2D edge1 = gameObject.AddComponent<EdgeCollider2D> ();
//					EdgeCollider2D edge2 = gameObject.AddComponent<EdgeCollider2D> ();
//					EdgeCollider2D edge4 = gameObject.AddComponent<EdgeCollider2D> ();
//
//					if (!colliders_Edge.ContainsKey ("right")) {
//						colliders_Edge.Add ("right", edge1);
//					} else {
//						colliders_Edge.Remove ("right");
//						Destroy(edge1);
//					}
//				
//					if (!colliders_Edge.ContainsKey ("left")) {
//						colliders_Edge.Add ("left", edge2);
//					} else {
//						colliders_Edge.Remove ("left");
//						Destroy(edge2);
//					}
//
//					if (!colliders_Edge.ContainsKey ("up")) {
//						colliders_Edge.Add ("up", edge4);
//					} else {
//						colliders_Edge.Remove ("up");
//						Destroy(edge4);
//					}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainManager : MonoBehaviour
{

	public List<Transform> ground;
	public GameObject levelBlock;
	//--------------------//
	public float maxHeight = 6;
	public float minHeight = 0;
	public float maxWidth = 14;
	public float minWidth = -19;
	//--------------------//

	public int blockCount = 100;

	void Awake ()
	{
		TerrainGenerator ();
	}

//	void Start ()
//	{
//		for (int block = 0; block < blockCount; block++) {
//			foreach (Transform overlap in ground) {
//				if (ground [block].position.x == overlap.position.x &&
//					ground [block].position.y == overlap.position.y &&
//					ground [block].name != overlap.name) {
//					print (ground [block].name + " : " + overlap.name);
//				}
//			}
//		}
//	}

	public void TerrainGenerator ()
	{
		int column = 0;
		int row = 0;

		float rangeX = minWidth;
		float rangeY = minHeight;

		for (int block = 0; block < blockCount; block++) {

//			float randomY = Mathf.RoundToInt (Random.Range (minHeight, maxHeight));
//			float randomX = Mathf.RoundToInt (Random.Range (minWidth, maxWidth));

			float randomX = Mathf.RoundToInt (rangeX + block);
			float randomY = Mathf.RoundToInt (rangeY);

			if (randomX == maxWidth) {
				rangeY += 1 + column;
				rangeX = -(block - row - Random.Range (minWidth, maxWidth)); 
//				maxWidth -= 1;
				row += 1;
			} 

			Vector3 randomPlacement = new Vector3 (Mathf.Clamp(randomX, minWidth, maxWidth) ,Mathf.Clamp(randomY,minHeight,maxHeight), 10);

			GameObject newBlock = Instantiate (levelBlock, randomPlacement, Quaternion.identity) as GameObject;
			newBlock.transform.parent = this.transform;
			newBlock.name = "Cube " + "(" + block + ")";
			ground.Add (newBlock.transform);
		}
		GetComponent<Level_Colliders>().Direction(ground);
	}
}

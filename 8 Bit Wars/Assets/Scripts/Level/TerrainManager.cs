using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TerrainManager : MonoBehaviour
{
	private Level_Colliders levelColliders;
	private List<Transform> ground;
	private GameObject levelBlock;

	//--------------------//
	public float maxHeight = 6;
	public float minHeight = 0;
	public float maxWidth = 14;
	public float minWidth = -19;
	//--------------------//

	public int blockCount = 100;

	private Vector2 parentVector;

	void Awake ()
	{
		parentVector = new Vector2(transform.position.x, transform.position.y );

		levelBlock = (GameObject)Resources.Load ("Cube");
		ground = new List<Transform> (blockCount);
		levelColliders = GetComponent<Level_Colliders> ();
		TerrainGenerator ();

	}

	public void TerrainGenerator ()
	{
		//--------- Temp ----------//
		float t_maxWidth = maxWidth; //temporary max width
		float t_minWidth = minWidth; //temporary max width
		float t_maxHeight = maxHeight; //temporary max width
		float t_minHeight = minHeight; //temporary max width
		//-------------------------//
		float random_X = 0;
		float random_Y;

		Vector2 placement = Vector2.zero; //where the block should be placed within the scene

		float blockCounter = 0;
		Random.seed = Mathf.RoundToInt(blockCount * Time.time);

		// spawns all the blocks
		for (float block = 0; block < blockCount; block++) {

			//---- Create a random placement on the map ----//
			float temp_X = (Mathf.Clamp(levelBlock.transform.localScale.x , minWidth, maxWidth)); //width of the block
			float temp_Y = (Mathf.Clamp(levelBlock.transform.localScale.y , minHeight, maxHeight)); //height of the block
			float noiseNum = Mathf.PerlinNoise(temp_X/10, temp_Y/10);

			random_Y = t_minHeight;

			Vector2 randomPlacement = new Vector2 (temp_X + random_X, temp_Y + random_Y);
			placement = new Vector2 ( Mathf.Clamp(randomPlacement.x, -t_minWidth, t_maxWidth) ,randomPlacement.y); //new placement


			if (t_minHeight <= t_maxHeight) {
				//---- Spawn block there (First block is placed at (0,0) ----//
				GameObject newBlock = Instantiate (levelBlock, placement + parentVector , Quaternion.identity) as GameObject;

				//-------------- Places them correctly in Hierarchy ----------------//
				newBlock.transform.parent = this.transform;
				newBlock.name = "Cube " + "(" + block + ")";
				ground.Add (newBlock.transform);

				blockCounter += 1;
				if(blockCounter >= t_maxWidth){
					//					
					t_maxWidth += (levelBlock.transform.localScale.x * Mathf.RoundToInt(Random.Range(-1, 1)));
					t_minWidth += (levelBlock.transform.localScale.x * Mathf.RoundToInt(Random.Range(-1, 1)));

					t_minHeight += levelBlock.transform.localScale.y;

					blockCounter = 0;
					random_X = (Mathf.Round( (( blockCounter * noiseNum)* Mathf.RoundToInt(Random.Range(1, 4) ))*2 ))/2;

					placement = Vector2.zero;
				}else{
					random_X = (Mathf.Round( (blockCounter  * (noiseNum))*2 ))/2;
				}
			} else {
				print ("Max reached - " + block);
				break;
			}
		}

//		levelColliders.Direction(ground);
	}


	void Update(){
		if(Input.GetKeyDown(KeyCode.R)){
			SceneManager.LoadScene("Level Creation Testing");
		}

	}
}

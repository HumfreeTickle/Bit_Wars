using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// The current gameState
/// </summary>
public enum GameState
{
	Start,
	Game,
	ChangeTurn,
	Menu,
	GameOver
}

/// <summary>
/// The current players turn
/// </summary>
public enum CurrentGo
{
	Player1,
	Player2
}

public class GameManager : MonoBehaviour
{
	//------------- Inherited Classes ---------------------//

	public float startTime{ get; private set; }

//	private TerrainManager terrainManager = new TerrainManager();

	private Turns turns;
	private Move moving;
	//-------------------------------//
	public GameObject currentPlayer{ get; set; }

	public List<string> player1{ get; private set; }

	private Transform Team1;

	public List<string> player2{ get; private set; }

	private Transform Team2;
	private List<Transform> ground;
	//-------------------------------//

	public GameState currentGameState{ get; set; }

	public GameState displayedGameState;

	public CurrentGo currentPlayersTurn{ get; set; }
	//--------------------//
	public float characterSpeed = 10;
	public float cameraSpeed = 2;
	//--------------------//
	public float setThreshold = 10.105f;
	public float setTurnTime = 60;

	public float threshold{ get; private set; }

	public float turnTime{ get; private set; }
	//--------------------//
	private int i = 0;

	public int nextPlayer {
		get{ return i;}
		set{ i = value;}
	}

	private float health;
	private Transform centreOfTheMap;
	private RectTransform fight;
	private bool startCoroutine;

	//---------------------//
	private GameObject weaponsMenu;
	private Image weaponsMenuImage;
	private float menuDefaultX;

	//--------------------//
	public float maxHeight = 6;
	public float minHeight = 0;
	public float maxWidth = 14;
	public float minWidth = -19;
	//--------------------//


	void Awake ()
	{
		startTime = Time.time;

		turns = GetComponent<Turns> ();
		moving = GetComponent<Move> ();
		fight = GameObject.Find ("Fight").GetComponent<RectTransform> ();
		centreOfTheMap = GameObject.Find ("CentreOfTheMap").transform;
		weaponsMenu = GameObject.Find ("Weapons Menu");
		weaponsMenuImage = weaponsMenu.GetComponent<Image> ();
		currentGameState = GameState.Start;


		//----------------- Setting up the teams----------------------//

		Team1 = GameObject.Find ("Team 1").transform;
		Team2 = GameObject.Find ("Team 2").transform;

		player1 = new List<string> (Team1.childCount);
		player2 = new List<string> (Team2.childCount);

		//------------------------------------------------------------//
		threshold = setThreshold;
		turnTime = setTurnTime;

	}

	void SpawnCharacters (Transform currentCharacter)
	{
		// Randomly spawns the teams around the level

		int randomBlock = Random.Range (0, ground.Count);
		float spawnX = ground [randomBlock].position.x + Random.Range (-ground [randomBlock].lossyScale.x, ground [randomBlock].lossyScale.x) / 2;
		float spawnY = ground [randomBlock].position.y + currentCharacter.lossyScale.y;

		currentCharacter.position = new Vector2 (Mathf.Clamp (spawnX, minWidth, maxWidth), Mathf.Clamp (spawnY, minHeight, maxHeight));
	}

	void Start ()
	{
		//----------------- Setting up the ground --------------------//
		Transform level = GameObject.Find ("Level").transform;
		ground = new List<Transform> (level.childCount);

		for (int groundChild = 0; groundChild < level.childCount; groundChild++) {
			// Only adds ground blocks that are in the right layer
			// Layer "Ground" relates to the top most layer that the player could be on
			if (level.GetChild (groundChild).gameObject.layer == LayerMask.NameToLayer ("Ground"))
				ground.Add (level.GetChild (groundChild).transform);
		}

		//----------------- Spawning Teams --------------------//

		for (int character = 0; character < Team1.childCount; character++) {
			if (Team1.GetChild (character).position.x > centreOfTheMap.position.x) {
				moving.FlipIt (Team1.GetChild (character).gameObject);
			}
			player1.Add (Team1.GetChild (character).name);
			
			// Move into Terrain Manager
			SpawnCharacters (Team1.GetChild (character).transform);
		}
		
		for (int character = 0; character < Team2.childCount; character++) {
			if (Team2.GetChild (character).position.x > centreOfTheMap.position.x) {
				moving.FlipIt (Team2.GetChild (character).gameObject);
			} 
			player2.Add (Team2.GetChild (character).name);
			
			// Move into Terrain Manager
			SpawnCharacters (Team2.GetChild (character).transform);
		}


		StartCoroutine (StartGame ());
	}

	void Update ()
	{		
		// Resets level
		if (Input.GetKeyDown (KeyCode.R)) {
			Application.LoadLevel (0);
		}

		displayedGameState = currentGameState;

		if (currentGameState != GameState.Start) {

			if (player1.Count > 0 || player2.Count > 0) {
				if (currentPlayer != null) {
					health = currentPlayer.GetComponent<Health> ().currentHealth;
					
					// Removes the player from the Team list and destroys the gameObject
					if (currentPlayer.transform.position.y < -3 || health <= 0) {
						switch (currentPlayersTurn) {
						case CurrentGo.Player1:
							player1.Remove (currentPlayer.name);
							Destroy (currentPlayer);
							break;
						case CurrentGo.Player2:
							player2.Remove (currentPlayer.name);
							Destroy (currentPlayer);
							break;
						default:
							break;
						}
					}
					
				} else if (currentPlayer == null) {
					turns.TurnUpdateInitialise ();
				}
				
				
			} else if(player1.Count == 0 || player2.Count == 0) {
				currentGameState = GameState.GameOver;
			} 

			if (Input.GetKeyUp (KeyCode.M) || currentGameState == GameState.ChangeTurn) {
				StartCoroutine (Menu ());
			}
		}
	}

	/// <summary>
	/// Activates the menu
	/// At some point the menu will move in from the side of the screen
	/// Hence the coroutine
	/// </summary>
	public IEnumerator Menu ()
	{
		if (startCoroutine) {
			if (currentGameState != GameState.ChangeTurn) {
				Debug.LogError ("Already Running");
			}
			yield break;
		}

		startCoroutine = true;

		//----- Close -----//
		if (weaponsMenu.GetComponent<Image>().isActiveAndEnabled || currentGameState == GameState.ChangeTurn) {

			if (currentGameState == GameState.Menu) {
				currentGameState = GameState.Game;
			}

			weaponsMenu.GetComponent<Animator> ().SetBool ("Open", false);
			weaponsMenu.GetComponent<Animator> ().SetBool ("Close", true);

			yield return new WaitForSeconds (0.3f);

			startCoroutine = false;

			for(int child = 0; child < weaponsMenu.transform.childCount; child ++){
				weaponsMenu.transform.GetChild(child).gameObject.SetActive(false);
			}
			weaponsMenu.GetComponent<Image>().enabled = false;
			
			yield return null;

			//----- Open -----//
		} else if (!weaponsMenu.GetComponent<Image>().isActiveAndEnabled) {

			for(int child = 0; child < weaponsMenu.transform.childCount; child ++){
				weaponsMenu.transform.GetChild(child).gameObject.SetActive(true);
			}
			weaponsMenu.GetComponent<Image>().enabled = true;

			if (currentGameState == GameState.Game) {
				currentGameState = GameState.Menu;
			}
			startCoroutine = false;

			weaponsMenu.GetComponent<Animator> ().SetBool ("Close", false);
			weaponsMenu.GetComponent<Animator> ().SetBool ("Open", true);

			yield return null;

		} else {

			yield break;
		}

		yield  break;
	}

	/// <summary>
	/// Starts the game.
	/// </summary>
	/// <returns>The game.</returns>
	IEnumerator StartGame ()
	{
		//-- Stops multiple coroutines from running --//
		if (startCoroutine) {
			Debug.LogError ("Already Running");
			yield break;
		}
		
		startCoroutine = true;

		//--------------- FIGHT MESSAGE --------------//
		while (fight.localPosition.x < 0) {
			fight.position += new Vector3 (20, 0, 0);
			yield return null;
		}
		yield return new WaitForSeconds (1);

		while (fight.localPosition.x < 700) {
			fight.position += new Vector3 (20, 0, 0);
			yield return null;
		}
		yield return new WaitForSeconds (1);
		//-------------------------------------------//

		currentGameState = GameState.Game;

		turns.TurnUpdateInitialise ();
		startCoroutine = false;
		yield break;
	}

	/// <summary>
	/// Death animation for which ever team mate has died
	/// Ideally there will be 3 or 4 separate animations
	/// that are picked at random
	/// </summary>
	/// <returns>The animation.</returns>
	IEnumerator DeathAnim ()
	{
		yield break;
	}
}

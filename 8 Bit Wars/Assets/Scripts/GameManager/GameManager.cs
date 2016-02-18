using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
/// The current players turn. Probably not the best
/// Honestly a list might be better
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

	private Turns turns;
	private Move moving;

	//-------------------------------//

	public List<string> teams { get; set; }

	public GameObject currentPlayer{ get; set; }

	public List<string> player1;
	//{ get; private set; }

	public TeamManager Team1_Manager;

	public List<string> player2;
	//{ get; private set; }

	public TeamManager Team2_Manager;

	public List<Color> teamColor;

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

	static public float threshold{ get; private set; }

	static public float turnTime{ get; private set; }

	private Image timerIm;
	//--------------------//
	private int i = 0;
	private int j = 0;

	public int nextPlayer {
		get { 
			if (currentPlayersTurn == CurrentGo.Player1) {
				return i; 
			} else {
				return j;
			}
		}
		set {
			if (currentPlayersTurn == CurrentGo.Player1) {
				//Checks to make sure the count doesn't go above the current amount of players
				if (i < player1.Count - 1) {	
					i = value; 
				} else {
					i = 0;
				}

			} else {
				if (j < player2.Count - 1) {	
					j = value; 
				} else {
					j = 0;
				}
			}
		}
	}

	//--------------------//
	private Transform centreOfTheMap;
	private bool startCoroutine;

	//---------------------//
	private GameObject weaponsMenu;
	private float menuDefaultX;

	static public bool canFire{ get; set; }

	//--------------------//
	public float maxHeight = 6;
	public float minHeight = 0;
	public float maxWidth = 14;
	public float minWidth = -19;
	//--------------------//

	private Text playerDisplay;

	void Awake ()
	{
		startTime = Time.time;
		DontDestroyOnLoad (this.gameObject);
		teamColor = new List<Color> (2);
		teamColor.Add (Color.white);
		teamColor.Add (Color.white);

		teams = new List<string> (2);
	}

	void OnLevelWasLoaded (int level)
	{
		//--------------- Character Select Screen -------------------//
		if (level == 1) {
			turns = GetComponent<Turns> ();
			moving = GetComponent<Move> ();
			
			if (turns.isActiveAndEnabled) {
				turns.enabled = false;
			}
			
			if (moving.isActiveAndEnabled) {
				moving.enabled = false;
			}
		}

		//--------------- Main Game Screen -------------------//
		if (level == 2) {
			turns = GetComponent<Turns> ();
			moving = GetComponent<Move> ();

			if (!turns.isActiveAndEnabled) {
				turns.enabled = true;
			}

			if (!moving.isActiveAndEnabled) {
				moving.enabled = true;
			}

			centreOfTheMap = GameObject.Find ("CentreOfTheMap").transform;
			weaponsMenu = GameObject.Find ("Weapons Menu");
			currentGameState = GameState.Start;
			timerIm = GameObject.Find ("Timer_Object").GetComponent<Image> ();
			playerDisplay = GameObject.Find ("Player Display").GetComponent<Text> ();

			//----------------- Setting up the teams----------------------//

			GameObject team1 = (GameObject)Instantiate (Resources.Load (teams [0]), Vector3.zero, Quaternion.identity);
			GameObject team2 = (GameObject)Instantiate (Resources.Load (teams [1]), Vector3.zero, Quaternion.identity);
		
			team1.name = "Team 1"; //teams[0];
			team2.name = "Team 2"; //teams[1];


			// Needs to 
			Team1_Manager = GameObject.Find (team1.name).GetComponent<TeamManager> ();
			Team2_Manager = GameObject.Find (team2.name).GetComponent<TeamManager> ();

			//------------------------------------------------------------//
			threshold = setThreshold;
			turnTime = setTurnTime;

			NewGame ();
		}
	}

	public void SpawnCharacters (Transform currentCharacter)
	{
		// Randomly spawns the teams around the level

		int randomBlock = Random.Range (0, ground.Count);

		if (ground [randomBlock].gameObject.layer == LayerMask.NameToLayer ("Ground")) {

			float spawnX = ground [randomBlock].position.x; //+ Random.Range (-ground [randomBlock].lossyScale.x,
			//              ground [randomBlock].lossyScale.x);
			float spawnY = ground [randomBlock].position.y + currentCharacter.lossyScale.y;
			currentCharacter.position = new Vector2 (Mathf.Clamp (spawnX, minWidth, maxWidth), 
				Mathf.Clamp (spawnY, minHeight, maxHeight));
		
		} else {
			SpawnCharacters (currentCharacter);
		}
	}

	void NewGame ()
	{
		//----------------- Setting up the level --------------------//
		GameObject level = GameObject.Find ("Level");

		ground = new List<Transform> (level.GetComponent<Level_Colliders> ().groundBlockList);

		foreach (Transform groundBlock in level.GetComponent<Level_Colliders> ().groundBlockList) {
			if (groundBlock.gameObject.layer == LayerMask.NameToLayer ("Ground")) {
				ground.Add (groundBlock);
			}	
		}

		//-------------------------------------------------------------//

		// Places the teams randomly around the map
		Team1_Manager.InitialiseTeams (moving, centreOfTheMap);
		Team2_Manager.InitialiseTeams (moving, centreOfTheMap);
		//------------------------------------------------------------//

		// Assigns the players
		player1 = Team1_Manager.players;
		player2 = Team2_Manager.players;

		moving.CameraSetup ();
	}

	void Update ()
	{		
		// Resets Game
		if (Input.GetKeyDown (KeyCode.R)) {
			SceneManager.LoadScene (0);
		}

		// Resets mainLevel
		if (Input.GetKeyDown (KeyCode.Y)) {
			int currentSceneIndex = SceneManager.GetActiveScene ().buildIndex;
			SceneManager.LoadScene (currentSceneIndex);
		}

		// Character slect screen manager
		if (SceneManager.GetActiveScene ().buildIndex == 1) {
			if (GameObject.Find ("Buttons").GetComponent<CharacterSelect> ().teamNumber >= 2) {
				int currentSceneIndex = SceneManager.GetActiveScene ().buildIndex;
				SceneManager.LoadScene (currentSceneIndex += 1);
			}
		}

		displayedGameState = currentGameState;
		if (playerDisplay) {
			playerDisplay.text = currentPlayersTurn.ToString ();
		}
		// This doesn't always trigger
		if (currentGameState != GameState.Start) {
			if (Team1_Manager.players.Count == 0 || Team2_Manager.players.Count == 0) {
				currentGameState = GameState.GameOver;
				print ("GameOver");
				print ("Play Time: " + (Time.time - startTime));
				if (!IsInvoking ("RestartGame")) {
					Invoke ("RestartGame", 1);
				}
			} 

			if (Input.GetKeyUp (KeyCode.M) || currentGameState == GameState.ChangeTurn) {
				StartCoroutine (Menu ());
			}
		}
	}

	/// <summary>
	/// Activates the menu
	/// </summary>
	public IEnumerator Menu ()
	{
		if (startCoroutine || turns.endTurn) {
			if (currentGameState != GameState.ChangeTurn) {
				Debug.LogError ("Already Running");
			}
			yield break;
		}

		startCoroutine = true;

		//----- Close -----//
		if (weaponsMenu.GetComponent<Image> ().isActiveAndEnabled || currentGameState == GameState.ChangeTurn) {

			if (currentGameState == GameState.Menu) {
				currentGameState = GameState.Game;
			}

			weaponsMenu.GetComponent<Animator> ().SetBool ("Open", false);
			weaponsMenu.GetComponent<Animator> ().SetBool ("Close", true);

			yield return new WaitForSeconds (0.3f);

			startCoroutine = false;

			for (int child = 0; child < weaponsMenu.transform.childCount; child++) {
				weaponsMenu.transform.GetChild (child).gameObject.SetActive (false);
			}
			weaponsMenu.GetComponent<Image> ().enabled = false;
			
			yield return null;

			//----- Open -----//
		} else if (!weaponsMenu.GetComponent<Image> ().isActiveAndEnabled) {

			for (int child = 0; child < weaponsMenu.transform.childCount; child++) {
				weaponsMenu.transform.GetChild (child).gameObject.SetActive (true);
			}
			weaponsMenu.GetComponent<Image> ().enabled = true;

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
	/// Calls all the functions that need to happen at the start of the game
	/// Main level only
	/// </summary>
	public void StartGame ()
	{
		currentGameState = GameState.Game;
		timerIm.enabled = true;
		currentPlayersTurn = CurrentGo.Player2;
		turns.ChangePlayer ();
		turns.TurnUpdateInitialise ();
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

	void RestartGame ()
	{
		SceneManager.LoadScene (0);

	}
}

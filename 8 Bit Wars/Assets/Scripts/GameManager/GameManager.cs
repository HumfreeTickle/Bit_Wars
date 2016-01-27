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

	private Turns turns;
	private Move moving;

	//-------------------------------//

	public GameObject currentPlayer{ get; set; }

	public List<string> player1{ get; private set; }
	private TeamManager Team1;

	public List<string> player2{ get; private set; }
	private TeamManager Team2;

	public List<Color> teamColor;//{get;set;}

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

	private Image timerIm;
	//--------------------//
	private int i = 0;

	public int nextPlayer {
		get{ return i;}
		set{ i = value;}
	}

	private Transform centreOfTheMap;
	private bool startCoroutine;

	//---------------------//
	private GameObject weaponsMenu;
	private float menuDefaultX;

	public bool canFire{ get; set; }

	//--------------------//
	public float maxHeight = 6;
	public float minHeight = 0;
	public float maxWidth = 14;
	public float minWidth = -19;
	//--------------------//


	void Awake ()
	{
		startTime = Time.time;
		DontDestroyOnLoad (this.gameObject);
		teamColor = new List<Color>(2);
		teamColor.Add(Color.white);
		teamColor.Add(Color.white);
	}

	void OnLevelWasLoaded (int level)
	{
	//--------------- Character Select Screen -------------------//
		if(level == 1){
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

			//----------------- Setting up the teams----------------------//

			Team1 = GameObject.Find ("Team 1").GetComponent<TeamManager> ();
			Team2 = GameObject.Find ("Team 2").GetComponent<TeamManager> ();

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
		//----------------- Setting up the ground --------------------//
		GameObject level = GameObject.Find ("Level");

		ground = new List<Transform> (level.GetComponent<Level_Colliders> ().groundBlockList);

		foreach (Transform groundBlock in level.GetComponent<Level_Colliders> ().groundBlockList) {
			if (groundBlock.gameObject.layer == LayerMask.NameToLayer ("Ground")) {
				ground.Add (groundBlock);
			}	
		}

		Team1.InitialiseTeams (moving, centreOfTheMap);
		Team2.InitialiseTeams (moving, centreOfTheMap);

		player1 = Team1.players;
		player2 = Team2.players;

	}

	void Update ()
	{		
		// Resets Game
		if (Input.GetKeyDown (KeyCode.Y)) {
			Application.LoadLevel (0);
		}

		// Resets mainLevel
		if (Input.GetKeyDown (KeyCode.R)) {
			Application.LoadLevel (2);
		}

		if (Application.loadedLevel == 1) {
			if (Input.GetKeyDown(KeyCode.I)) {
				Application.LoadLevel (2);
			}
		}

		displayedGameState = currentGameState;

		if (currentGameState != GameState.Start) {

			if (Team1.players.Count == 0 || Team2.players.Count == 0) {
				currentGameState = GameState.GameOver;
				print ("GameOver");
				print ("Play Time: " + (Time.time - startTime));
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

			for (int child = 0; child < weaponsMenu.transform.childCount; child ++) {
				weaponsMenu.transform.GetChild (child).gameObject.SetActive (false);
			}
			weaponsMenu.GetComponent<Image> ().enabled = false;
			
			yield return null;

			//----- Open -----//
		} else if (!weaponsMenu.GetComponent<Image> ().isActiveAndEnabled) {

			for (int child = 0; child < weaponsMenu.transform.childCount; child ++) {
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

	public void StartGame ()
	{
		currentGameState = GameState.Game;
		timerIm.enabled = true;
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
}

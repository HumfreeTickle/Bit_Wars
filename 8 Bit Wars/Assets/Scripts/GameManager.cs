using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum GameState
{
	Start,
	Game,
	ChangeTurn,
	Menu,
	GameOver
}

public enum CurrentGo
{
	Player1,
	Player2
}

public class GameManager : MonoBehaviour
{
	public float startTime{ get; private set; }

	private Turns turns;
	private Move moving;
	//-------------------------------//
	public GameObject currentPlayer{ get; set; }

	public List<string> player1{ get; private set; }

	private Transform Team1;

	public List<string> player2{ get; private set; }

	private Transform Team2;
	public List<Transform> ground;
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
	private GameObject weaponsMenu;
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
		menuDefaultX = weaponsMenu.GetComponent<RectTransform> ().localPosition.x;

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
		// Probably needs to be a Transform[] parameter
		// with a foreach loop

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
		if (Input.GetKeyDown (KeyCode.R)) {
			Application.LoadLevel (0);
		}

		displayedGameState = currentGameState;
		if (currentGameState != GameState.Start) {


			if (Input.GetKeyUp (KeyCode.Return) || currentGameState == GameState.ChangeTurn) {
				StartCoroutine (Menu ());
			}

			if (player1.Count > 0 || player2.Count > 0) {
				if (currentPlayer != null) {
					health = currentPlayer.GetComponent<Health> ().currentHealth;

					if (Input.GetKeyDown (KeyCode.R) || currentPlayer.transform.position.y < -3 || health <= 0) {
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



			} else {
				currentGameState = GameState.GameOver;
			} 
		}
	}

	IEnumerator Menu ()
	{
		if (startCoroutine) {
			if (currentGameState != GameState.ChangeTurn) {
				Debug.LogError ("Already Running");
			}
			yield break;
		}

		startCoroutine = true;

		if (weaponsMenu.activeSelf || currentGameState == GameState.ChangeTurn) {
			while (weaponsMenu.GetComponent<RectTransform>().localPosition.x < menuDefaultX / 2) {
				weaponsMenu.GetComponent<RectTransform> ().Translate (new Vector3 (menuDefaultX / 2,
					                                           weaponsMenu.GetComponent<RectTransform> ().localPosition.y
				                                               , 0));
				yield return new WaitForSeconds (Time.deltaTime);
			}    

			weaponsMenu.SetActive (false);
			if (currentGameState == GameState.Menu) {
				currentGameState = GameState.Game;
			}
			yield return null;

		} else if (!weaponsMenu.activeSelf) {
			weaponsMenu.SetActive (true);
			if (currentGameState == GameState.Game) {
				currentGameState = GameState.Menu;
			}

			while (weaponsMenu.GetComponent<RectTransform>().localPosition.x > -menuDefaultX) {
				weaponsMenu.GetComponent<RectTransform> ().Translate (new Vector3 (-menuDefaultX,
					                                           weaponsMenu.GetComponent<RectTransform> ().localPosition.y
				                                               , 0));
				yield return new WaitForSeconds (Time.deltaTime);
			}  
			yield return null;
		}

		startCoroutine = false;
		yield  break;
	}
	
	IEnumerator StartGame ()
	{
		//-- Stops multiple coroutines from running --//
		if (startCoroutine) {
			Debug.LogError ("Already Running");
			yield break;
		}
		
		startCoroutine = true;

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

		currentGameState = GameState.Game;

		turns.TurnUpdateInitialise ();
		startCoroutine = false;
		yield break;
	}

	IEnumerator DeathAnim ()
	{
		yield break;
	}
}

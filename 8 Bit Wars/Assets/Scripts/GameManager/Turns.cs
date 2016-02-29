using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Turns : MonoBehaviour
{
	//------ Components ----------//
	private GameManager gameManager;
	private Move movementManager;
	private Move direction;

	//------ Timer ---------------//
	public float _timer{ get; private set; }

	private Text TimerDisplay;
	private GameObject Ready;
	public bool startCoroutine{ get; set; }

	private Transform followCamera;
	public GameObject arrow;

	public bool endTurn{ get; set; }
	static public float turnOver{private get; set;}

	void Start ()
	{
		gameManager = GetComponent<GameManager> ();
		movementManager = GetComponent<Move> ();

		_timer = GameManager.turnTime;
		followCamera = GameObject.Find ("Main Camera").transform;

		direction = GetComponent<Move> ();
		TimerDisplay = GameObject.Find ("Timer").GetComponent<Text> ();
		Ready = GameObject.Find("Ready");
		if(Ready.activeSelf){
			Ready.SetActive(false);
		}

		turnOver = 5f;
	}

	public void TurnUpdateInitialise ()
	{
		if (GameManager.currentGameState != GameState.GameOver) {
			
			// sets whether the next player is facing right or left
			if (GameManager.currentPlayer.transform.localScale.x > 0) {
				Move.facingRight = true;
			} else if (GameManager.currentPlayer.transform.localScale.x < 0) {
				Move.facingRight = false;
			} else {
				Debug.LogError ("Can't tell which way!!!");
			}

			if (GameManager.currentPlayer != null) {

				movementManager.rb = GameManager.currentPlayer.GetComponent<Rigidbody2D> ();
				movementManager.playerAnimation = GameManager.currentPlayer.GetComponent<Animator> ();
				movementManager.offTheGround = GameManager.currentPlayer.GetComponent<CircleCollider2D> ();


				Vector3 arrowLocation = new Vector3 (
					GameManager.currentPlayer.transform.position.x,
					GameManager.currentPlayer.transform.position.y +
					GameManager.currentPlayer.transform.lossyScale.y + 1,
					GameManager.currentPlayer.transform.position.z
				                        );

				GameObject currentArrow = Instantiate (arrow, arrowLocation, Quaternion.identity) as GameObject;
				currentArrow.transform.parent = GameManager.currentPlayer.transform;

				GameManager.canFire = true;
			}

			//-- Changes to next player --//
			gameManager.nextPlayer += 1;				

			//-- Starts turn countdown --//
			if (!startCoroutine) {
				StartCoroutine (TurnsCoroutine ());
			}

		} else {
			print ("GameOver");
			print ("Play Time: " + (Time.time - gameManager.startTime));
		}
	}

	public IEnumerator TurnsCoroutine ()
	{

		//-- Stops multiple coroutines from running --//
		if (startCoroutine) {
			Debug.LogError ("Already Running: Turns Coroutine");
			yield break;
		}

		startCoroutine = true;
		TimerDisplay.GetComponent<Animator>().SetInteger("Timer", 45);

		TimerDisplay.text = Mathf.Round (_timer).ToString ();

		//-- Counts down till end of turn --//
		while (_timer >= 0 && GameManager.currentPlayer != null && !endTurn) {

			TimerDisplay.text = Mathf.Round (_timer).ToString ();
			TimerDisplay.GetComponent<Animator>().SetInteger("Timer",(int)_timer);

			_timer -= Time.deltaTime;

			yield return null;


		}

		// Used to allow the player time to move away from their current postion
		TimerDisplay.text = "";
		TimerDisplay.GetComponent<Animator>().SetInteger("Timer", 45);

		yield return new WaitForSeconds (turnOver);

		endTurn = false;
	
		if (GameManager.currentGameState != GameState.GameOver) {
			GameManager.currentGameState = GameState.ChangeTurn;


			// Destroys Weapon at the end of the turn
			if (GameManager.currentPlayer != null) {
				if (GameManager.currentPlayer.transform.childCount > 2) {
					for (int child = 2; child < GameManager.currentPlayer.transform.childCount; child++) {
						Destroy (GameManager.currentPlayer.transform.GetChild (child).gameObject);
//						gameManager.currentPlayer.transform.GetChild (child).gameObject.SetActive (false);
					}
				}
			}

			// Change over of teams.
			ChangePlayer ();
		
			yield return new WaitForSeconds (1);
//			print ("Turn for: " + gameManager.currentPlayer.name + "(" + gameManager.currentPlayersTurn + ")");

			// Back up incase something goes wrong
			if (GameManager.currentPlayer == null) {
				ChangePlayer ();
			}
			yield return null;


			//-- Moves camera to next player --//
			while (Vector2.Distance (followCamera.position, GameManager.currentPlayer.transform.position) > GameManager.threshold) {

				float cameraX = Mathf.Lerp (followCamera.position.x, GameManager.currentPlayer.transform.position.x, Time.fixedDeltaTime * gameManager.cameraSpeed);
				float cameraY = Mathf.Lerp (followCamera.position.y, GameManager.currentPlayer.transform.position.y, Time.fixedDeltaTime * gameManager.cameraSpeed);

				Vector3 moveCameraPosition = new Vector3 (cameraX, Mathf.Clamp (cameraY, 0.8f, Mathf.Infinity), -10);
				followCamera.position = moveCameraPosition;
			
				yield return null;
			}
		
			yield return null;
		
			_timer = GameManager.turnTime;

			bool startTurn = false;
			float startTurnTimer = 5;


			while (!startTurn) {
				
				TimerDisplay.GetComponent<Animator>().SetInteger("Timer",(int)startTurnTimer);
				TimerDisplay.text = Mathf.RoundToInt (startTurnTimer).ToString ();
				if(!Ready.activeSelf){
					Ready.SetActive(true);
				}
				// Five second timer in between turns
				// Any button can be used to start the turn before then

				if (Input.anyKeyDown || startTurnTimer <= 0) {
					startTurn = true;
					startCoroutine = false;
					if(Ready.activeSelf){
						Ready.SetActive(false);
					}
					TurnUpdateInitialise ();
				}
				startTurnTimer -= Time.deltaTime;
				
				yield return null;
			}
			turnOver = 5f;
			GameManager.currentGameState = GameState.Game;

			yield break;

		} 
//		else {
//			print ("GameOver");
//			print ("Play Time: " + (Time.time - gameManager.startTime));
//			yield break;
//		}
	}

	public void ChangePlayer ()
	{
		// Assigns the new character to control
		switch (gameManager.currentPlayersTurn) {

		case CurrentGo.Player1:
			gameManager.currentPlayersTurn = CurrentGo.Player2;
			GameManager.currentPlayer = GameObject.Find (gameManager.player2 [gameManager.nextPlayer]);
			break;
		case CurrentGo.Player2:
			gameManager.currentPlayersTurn = CurrentGo.Player1;
			GameManager.currentPlayer = GameObject.Find (gameManager.player1 [gameManager.nextPlayer]);
			break;

		default:
			Debug.LogError ("No player");
			break;
		}

//		print ("Turn for: " + gameManager.currentPlayer.name + " (" + gameManager.currentPlayersTurn + ")");

	}
}

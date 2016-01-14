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
	private bool startCoroutine;
	private Transform followCamera;
	public GameObject arrow;

	void Start ()
	{
		gameManager = GetComponent<GameManager> ();
		movementManager = GetComponent<Move> ();

		_timer = gameManager.turnTime;
		followCamera = GameObject.Find ("Main Camera").transform;

		direction = GetComponent<Move> ();
		TimerDisplay = GameObject.Find ("Timer").GetComponent<Text> ();
	}
	
	public void TurnUpdateInitialise ()
	{
		if (gameManager.currentGameState != GameState.GameOver) {
			if (!startCoroutine) {

				//-- Finds the new player at the start of the turn --//
				//-- Assigns whether that player is facing left or right --//

				if (gameManager.currentPlayersTurn == CurrentGo.Player1) {
					gameManager.currentPlayer = GameObject.Find (gameManager.player1 [gameManager.nextPlayer]);
				} else if (gameManager.currentPlayersTurn == CurrentGo.Player2) {
					gameManager.currentPlayer = GameObject.Find (gameManager.player2 [gameManager.nextPlayer]);
				}

				// sets whether the next player is facing right or left
				if (gameManager.currentPlayer.transform.localScale.x > 0) {
					direction.facingRight = true;
				} else if (gameManager.currentPlayer.transform.localScale.x < 0) {
					direction.facingRight = false;
				}

			}

			if (gameManager.currentPlayer != null) {
//				print ("Turn for: " + gameManager.currentPlayer.name + "(" + gameManager.currentPlayersTurn + ")");
				movementManager.rb = gameManager.currentPlayer.GetComponent<Rigidbody2D> ();
				movementManager.playerAnimation = gameManager.currentPlayer.GetComponent<Animator> ();
				movementManager.offTheGround = gameManager.currentPlayer.GetComponent<CircleCollider2D> ();


				Vector3 arrowLocation = new Vector3 (gameManager.currentPlayer.transform.position.x,
				                                    gameManager.currentPlayer.transform.position.y +
					gameManager.currentPlayer.transform.lossyScale.y + 1,
				                                    gameManager.currentPlayer.transform.position.z
				);
				GameObject currentArrow = Instantiate (arrow, arrowLocation, Quaternion.identity) as GameObject;
				currentArrow.transform.parent = gameManager.currentPlayer.transform;
			}

			//-- Decideds on the player to use --//

			if (gameManager.currentPlayersTurn == CurrentGo.Player2) {
				if (gameManager.nextPlayer >= gameManager.player1.Count - 1 || gameManager.nextPlayer >= gameManager.player2.Count - 1) {
					gameManager.nextPlayer = 0;
				} else {
					gameManager.nextPlayer += 1;
				}
			}

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
			Debug.LogError ("Already Running");
			yield break;
		}

		startCoroutine = true;
		
		//-- Counts down till end of turn --//
		while (_timer >= 0 && gameManager.currentPlayer != null) {
			TimerDisplay.text = _timer.ToString ();
			yield return new WaitForSeconds (1);
			_timer -= 1;
		}

		if (gameManager.currentGameState != GameState.GameOver) {
			gameManager.currentGameState = GameState.ChangeTurn;

			if (gameManager.currentPlayer != null) {
				if (gameManager.currentPlayer.transform.childCount > 1) {
					for (int child = 1; child < gameManager.currentPlayer.transform.childCount; child ++) {
						Destroy (gameManager.currentPlayer.transform.GetChild (child).gameObject);
					}
				}
			}

			ChangePlayer();

			print(gameManager.currentPlayer);

			yield return new WaitForSeconds (1);
			if(gameManager.currentPlayer == null){
				ChangePlayer();
			}

			//-- Moves camera to next player --//
			while (Vector2.Distance(followCamera.position, gameManager.currentPlayer.transform.position) > gameManager.threshold) {

				float cameraX = Mathf.Lerp (followCamera.position.x, gameManager.currentPlayer.transform.position.x, Time.fixedDeltaTime * gameManager.cameraSpeed);
				float cameraY = Mathf.Lerp (followCamera.position.y, gameManager.currentPlayer.transform.position.y, Time.fixedDeltaTime * gameManager.cameraSpeed);

				Vector3 moveCameraPosition = new Vector3 (cameraX, Mathf.Clamp (cameraY, 0.8f, Mathf.Infinity), -10);
				followCamera.position = moveCameraPosition;
			
				yield return null;
			}
		
			yield return null;
		
			_timer = gameManager.turnTime;

			startCoroutine = false;
			yield return null;

			TurnUpdateInitialise ();
			gameManager.currentGameState = GameState.Game;
			yield break;

		} else {
			print ("GameOver");
			print ("Play Time: " + (Time.time - gameManager.startTime));
			yield break;
		}
	}

	void ChangePlayer(){

		switch (gameManager.currentPlayersTurn) {
		case CurrentGo.Player1:
			gameManager.currentPlayersTurn = CurrentGo.Player2;
			gameManager.currentPlayer = GameObject.Find (gameManager.player2 [gameManager.nextPlayer]);
			break;
		case CurrentGo.Player2:
			gameManager.currentPlayersTurn = CurrentGo.Player1;
			gameManager.currentPlayer = GameObject.Find (gameManager.player1 [gameManager.nextPlayer]);
			break;
		default:
			
			break;
		}
	}
}

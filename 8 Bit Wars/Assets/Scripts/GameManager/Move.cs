using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;
using UnityEngine.UI;

public class Move : MonoBehaviour
{
	private GameManager gameManager;

	public Rigidbody2D rb { private get; set; }

	public Vector3 startingPosition{ private get; set; }

	public Vector2 jumpingForce = new Vector2 (60, 120);

	static public bool facingRight{ get; set; }
// = true;
	private float jumpForce;

	//----- Camera Stuff -----//
	private Transform followCamera;
	public float CameraSize = 3;
	public float cameraBaseRestriction = 0f;
	//-----------------------//

	public CircleCollider2D offTheGround{ private get; set; }

	public Animator playerAnimation{ private get; set; }

	public void CameraSetup ()
	{
		gameManager = GetComponent<GameManager> ();
		followCamera = GameObject.Find ("Main Camera").transform;
	}

	void Update ()
	{
		if (GameManager.currentGameState != GameState.ChangeTurn && GameManager.currentPlayer != null) {

			MoveCamera ();
			if (GameManager.currentGameState != GameState.Menu) {
				MoveHorizontal ();

				if (Input.GetButton ("Jump")) {
					jumpForce += 0.1f;
				} else if (Input.GetButtonUp ("Jump")) {

					if (rb.velocity == Vector2.zero) {
						rb.AddForce (Jumping (Mathf.Clamp (jumpForce, 1, 2)));
						jumpForce = 0;
					}
				}
			}
			playerAnimation.SetFloat ("vSpeed", rb.velocity.y);

		} else {
			if (playerAnimation != null) {
				playerAnimation.SetBool ("Moving", false);
			}
		}
	}

	void MoveHorizontal ()
	{
		//might be better to see if the local scale x is positive or negative and adjust accordingly
		if (Input.GetAxisRaw ("Horizontal") > 0 && !facingRight) {
			FlipIt (GameManager.currentPlayer);				

		} else if (Input.GetAxisRaw ("Horizontal") < 0 && facingRight) {
			FlipIt (GameManager.currentPlayer);
		}

		if (Mathf.Abs (Input.GetAxisRaw ("Horizontal")) > 0) {
			GameManager.currentPlayer.transform.Translate (Vector3.right * gameManager.characterSpeed * Time.deltaTime * Input.GetAxis ("Horizontal"));
			playerAnimation.SetBool ("Moving", true);
		} else {
			playerAnimation.SetBool ("Moving", false);
		}
	}

	/// <summary>
	/// Flips the player based on which direction you are moving.
	/// </summary>
	/// <param name="player">Player to flip.</param>
	static public void FlipIt (GameObject player)
	{
		facingRight = !facingRight;

		Vector3 theScale = player.transform.localScale;
		theScale.x *= -1;
		player.transform.localScale = theScale;

		//Flips children of the object
		for (int child = 0; child < 2; child++) {
			Vector3 canvasScale = player.transform.GetChild (child).localScale;
			canvasScale.x *= -1;
			player.transform.GetChild (child).localScale = canvasScale;
		}

	}

	Vector2 Jumping (float addedForce = 1)
	{
		Vector2 jump = new Vector2 (jumpingForce.x * Input.GetAxisRaw ("Horizontal"), jumpingForce.y * addedForce);
		return jump;
	}

	void MoveCamera ()
	{
		if (followCamera.GetComponent<Camera> ().orthographicSize > CameraSize) {
			followCamera.GetComponent<Camera> ().orthographicSize = Mathf.Lerp (followCamera.GetComponent<Camera> ().orthographicSize, CameraSize, Time.deltaTime * 2);
		}

		Vector3 playerPosition = GameManager.currentPlayer.transform.position;
		Vector3 moveCameraPosition = new Vector3 (playerPosition.x, Mathf.Clamp (playerPosition.y, cameraBaseRestriction, Mathf.Infinity), -10);
		followCamera.position = moveCameraPosition;
	}
}

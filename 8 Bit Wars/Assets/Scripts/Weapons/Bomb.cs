using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour
{

	private GameManager gameManager;
	private Turns turns;
	public float throwingSpeed;
	public float distanceFromPlayer = 0.1f;

	void Start ()
	{
		gameManager = GameObject.Find("GameManger").GetComponent<GameManager> ();
		turns = gameManager.GetComponent<Turns>();
	}

	void Update ()
	{
		if (GameManager.canFire) {
			if (Input.GetKeyUp (KeyCode.F)) {
				if (!IsInvoking ("ThrowBomb")) {
					Invoke ("ThrowBomb", 0.1f);
				}
				GameManager.canFire = false;
			}
		} 
	}

	void ThrowBomb ()
	{
		GameObject thrownBomb = gameManager.currentPlayer.transform.GetChild (2).gameObject;
		Vector2 direction = new Vector2 (throwingSpeed * gameManager.currentPlayer.transform.localScale.x, 10);
		thrownBomb.AddComponent<Rigidbody2D> ();

		thrownBomb.transform.parent = null;
//		thrownBomb.GetComponent<Test_explosion> ().enabled = true;
		thrownBomb.GetComponent<Rigidbody2D> ().AddForce (direction);
		
		turns.endTurn = true;
	}
}

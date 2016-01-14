using UnityEngine;
using System.Collections;

public class FireTesting : MonoBehaviour
{	
	private GameManager gameManager;
	public GameObject bullet;
	public GameObject bomb;
	public float bulletSpeed;
	public float distanceFromPlayer = 0.1f;

	public string weaponName;//{ private get; set; }

	void Start ()
	{
		gameManager = GetComponent<GameManager> ();
	}

	void Update ()
	{
		if (gameManager.currentGameState == GameState.Game) {

			// Calls different funtions depending on selected weapon
			switch (weaponName) {
			case "Tommy Gun":
				if (bullet) {
					if (Input.GetKeyUp (KeyCode.F)) {
						if (!IsInvoking ("FireBullet")) {
							Invoke ("FireBullet", 0.1f);
						}
					}
				} else {
					Debug.LogError ("Out of Ammo " + gameObject.name); 
				}
				break;

			case "Bomb":
				if (bomb) {
					if (Input.GetKeyUp (KeyCode.F)) {
						if (!IsInvoking ("ThrowBomb")) {
							Invoke ("ThrowBomb", 0.1f);
						}
					}
				} else {
					Debug.LogError ("Out of Ammo " + gameObject.name); 
				}
				break;

			default:

				break;
			}
		}
	}

	void FireBullet ()
	{
		Vector3 spawnPoint = new Vector3 (gameManager.currentPlayer.transform.position.x + (distanceFromPlayer * gameManager.currentPlayer.transform.localScale.x), 
		                                  gameManager.currentPlayer.transform.position.y, 
		                                  gameManager.currentPlayer.transform.position.z);
		GameObject firedBullet = Instantiate (bullet, spawnPoint, Quaternion.identity) as GameObject;
		firedBullet.transform.localScale = new Vector3 (gameManager.currentPlayer.transform.localScale.x * firedBullet.transform.localScale.x,
		                                               firedBullet.transform.localScale.y,
		                                               firedBullet.transform.localScale.z);
		Vector2 direction = new Vector2 (bulletSpeed * gameManager.currentPlayer.transform.localScale.x, 0);
		firedBullet.GetComponent<Rigidbody2D> ().AddForce (direction);
	}

	void ThrowBomb ()
	{
		GameObject thrownBomb = gameManager.currentPlayer.transform.GetChild(2).gameObject;
		Vector2 direction = new Vector2(bulletSpeed * gameManager.currentPlayer.transform.localScale.x,10);
		thrownBomb.AddComponent<Rigidbody2D>();
		transform.parent = null;
		thrownBomb.GetComponent<Rigidbody2D>().AddForce(direction);
	}
}

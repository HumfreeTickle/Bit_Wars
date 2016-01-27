using UnityEngine;
using System.Collections;

public class MachineGun : MonoBehaviour
{

	private GameManager gameManager;
	private Turns turns;
	public GameObject bullet;
	public float bulletSpeed;
	public float distanceFromPlayer = 0.1f;
	public float delayTime = 0.05f;
	private int maxFire = 0;

	void Start ()
	{
		gameManager = GameObject.Find ("GameManger").GetComponent<GameManager> ();
		turns = gameManager.GetComponent<Turns> ();
	}

	void Update ()
	{
		if (gameManager.canFire) {
			if (bullet) {
				if (Input.GetKeyDown (KeyCode.F)) {
					if (!IsInvoking ("FireBullet")) {
						InvokeRepeating ("FireBullet", 0, delayTime);
					}
					gameManager.canFire = false;
				}

			} else {
				Debug.LogError ("Out of Ammo " + gameObject.name); 
			}
		}
	}

	void FireBullet ()
	{
		maxFire++;
		if (maxFire <= 8) {
			Vector3 spawnPoint = new Vector3 (gameManager.currentPlayer.transform.position.x + (distanceFromPlayer * gameManager.currentPlayer.transform.localScale.x), 
		                                  gameManager.currentPlayer.transform.position.y, 
		                                  gameManager.currentPlayer.transform.position.z);
			GameObject firedBullet = Instantiate (bullet, spawnPoint, Quaternion.identity) as GameObject;
			firedBullet.transform.localScale = new Vector3 (gameManager.currentPlayer.transform.localScale.x * firedBullet.transform.localScale.x,
		                                                firedBullet.transform.localScale.y,
		                                                firedBullet.transform.localScale.z);
			Vector2 direction = new Vector2 (bulletSpeed * gameManager.currentPlayer.transform.localScale.x, 0);
			firedBullet.GetComponent<Rigidbody2D> ().AddForce (direction);
		} else {
			CancelInvoke("FireBullet");
			turns.endTurn = true;
			maxFire = 0;
			Destroy(this.gameObject);
		}

	}
}

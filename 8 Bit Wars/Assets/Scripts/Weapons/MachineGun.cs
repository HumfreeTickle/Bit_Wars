using UnityEngine;
using System.Collections;

public class MachineGun : MonoBehaviour
{
	private static GameManager gameManager = new GameManager ();
	private static Turns turns = new Turns ();
	private GameObject bullet;
	private GameObject[] bulletPool = new GameObject[8];

	public float bulletSpeed;
	public float distanceFromPlayer = 0.1f;
	public float delayTime = 0.05f;
	private int maxFire = 0;

	// There needs to be a object pool for all the different bullets that need to be spawned

	void Start ()
	{
		bullet = (GameObject)Resources.Load ("Bullet"); // Loads the bullet prefab from the project folder

//		for (int i = 0; i < bulletPool.Length; i++) {
//			GameObject obj = (GameObject)Instantiate (bullet);
//			obj.SetActive (false);
//			bulletPool [i] = obj;
//		}

		gameManager = GameObject.Find ("GameManger").GetComponent<GameManager> ();
		turns = gameManager.GetComponent<Turns> ();

	}

	void Update ()
	{
		if (GameManager.canFire) {
			if (bullet) {
				if (Input.GetKeyDown (KeyCode.F)) {
					if (!IsInvoking ("FireBullet")) {
						InvokeRepeating ("FireBullet", 0, delayTime);
					}
					GameManager.canFire = false;
				}

			} else {
				Debug.LogError ("Out of Ammo " + gameObject.name); 
			}
		}
	}

	void FireBullet ()
	{
		// object pooling will probably help here 
		// it would mean that when the array is over. End
		// Rather than using the I have here loop

		maxFire++;

		if (maxFire <= bulletPool.Length) {
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
			CancelInvoke ("FireBullet");
			turns.endTurn = true;
			maxFire = 0;
//			this.gameObject.SetActive(false);
		}
	}
}

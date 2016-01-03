using UnityEngine;
using System.Collections;

public class FireTesting : MonoBehaviour
{	
	private GameManager gameManager;

	public GameObject bullet;
	private GameObject firedBullet;
	public float bulletSpeed;
	public float distanceFromPlayer = 0.1f;

	void Start(){
		gameManager = GetComponent<GameManager>();
	}

	void Update ()
	{
		if (bullet) {
			if (Input.GetKeyUp (KeyCode.F)) {
				if (!IsInvoking ("Fire")) {
					Invoke ("Fire", 0.1f);
				}
			}
		} else {
			Debug.LogError ("Out of Ammo " + gameObject.name); 
		}
	}

	void Fire ()
	{
		Vector3 spawnPoint = new Vector3 (gameManager.currentPlayer.transform.position.x + (distanceFromPlayer * gameManager.currentPlayer.transform.localScale.x), 
		                                  gameManager.currentPlayer.transform.position.y, 
		                                  gameManager.currentPlayer.transform.position.z);
		firedBullet = Instantiate (bullet, spawnPoint, Quaternion.identity) as GameObject;
		Vector2 direction = new Vector2 (bulletSpeed * gameManager.currentPlayer.transform.localScale.x, 0);
		firedBullet.GetComponent<Rigidbody2D> ().AddForce (direction);
	}
}

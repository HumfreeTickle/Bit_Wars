using UnityEngine;
using System.Collections;

public class MachineGun : MonoBehaviour
{
	private static GameManager gameManager = new GameManager ();
	private static Turns turns = new Turns ();
	private GameObject bullet;
	private GameObject[] bulletObjectPool = new GameObject[8];
	// bullet object pool

	public float bulletSpeed;
	public float distanceFromPlayer = 0.1f;
	public float delayTime = 0.05f;
	private int maxFire = 0;

	public float rotationSpeed = 1;

	public LayerMask whatToHit;
	private Transform firePoint;
	private Vector2 fireRotation;

	void Start ()
	{
		bullet = (GameObject)Resources.Load ("Bullet"); // Loads the bullet prefab from the project folder

		firePoint = transform.FindChild ("FirePoint");

//		for (int i = 0; i < bulletObjectPool.Length; i++) {
//			GameObject obj = (GameObject)Instantiate (bullet);
//			obj.SetActive (false);
//			bulletObjectPool [i] = obj;
//		}

		gameManager = GameObject.Find ("GameManger").GetComponent<GameManager> ();
		turns = gameManager.GetComponent<Turns> ();

	}

	void Update ()
	{
		if (GameManager.canFire && GameManager.currentGameState == GameState.Game) {
			if (bullet) {
				if (Input.GetMouseButtonDown (0)) {
					if (!IsInvoking ("FireBullet")) {
						InvokeRepeating ("FireBullet", 0, delayTime);
					}
					GameManager.canFire = false;
				}

			} else {
				Debug.LogError ("Out of Ammo " + gameObject.name); 
			}
		}
		RotateWeapon ();

	}

	void RotateWeapon ()
	{
		Vector2 mousePos = new Vector2 (
			                   Camera.main.ScreenToWorldPoint (Input.mousePosition).x, 
			                   Camera.main.ScreenToWorldPoint (Input.mousePosition).y
		                   );

		Vector2 firePointPos = new Vector2 (firePoint.position.x, firePoint.position.y);
//		RaycastHit2D hit = Physics2D.Raycast (firePointPos, mousePos - firePointPos, 100, whatToHit);
		Debug.DrawLine (firePointPos, mousePos);

		fireRotation = mousePos - firePointPos;
		fireRotation.Normalize ();

		// figure out maths behind this
		float rot_z = Mathf.Atan2 (fireRotation.y, fireRotation.x) * Mathf.Rad2Deg;
		transform.localRotation = Quaternion.Euler (
			0f, 
			0f, 
			rot_z * GameManager.currentPlayer.transform.localScale.x
//			Mathf.Clamp (rot_z* GameManager.currentPlayer.transform.localScale.x, -45, 45)
		);

		// still a glitch if you are facing right when you select the machine gun... but getting there

		if (GameManager.currentPlayer.transform.localScale.x > 0) {
			if (rot_z > 90 || rot_z < -90) {
				bool rotate = true;
				if (rotate) {
					Move.FlipIt (GameManager.currentPlayer);
					transform.localScale = transform.localScale*-1;
					rotate = false;
				}
			}
		}
		else if (GameManager.currentPlayer.transform.localScale.x < 0) {
			if (rot_z < 90 && rot_z > -90) {
				bool rotate = true;
				if (rotate) {
					Move.FlipIt (GameManager.currentPlayer);
					transform.localScale = transform.localScale*-1;
					rotate = false;
				}
			}
		}
	}

	void FireBullet ()
	{
		// object pooling will probably help here 
		// it would mean that when the array is over. End
		// Rather than using the I have here loop

		maxFire++;

		if (maxFire <= bulletObjectPool.Length) {
			
			Quaternion gunrotation = transform.localRotation;
			print (gunrotation);
			// instead of instatiating them I just need to grab them from the object pool
			// Also need to change the destroy function on the bullets to just a disable command

//			GameObject firedBullet = bulletObjectPool[maxFire];
//			firedBullet.transform.position = firePoint.position;
//			firedBullet.transform.rotation = gunrotation;

			GameObject firedBullet = Instantiate (bullet, firePoint.position, gunrotation) as GameObject;

			firedBullet.transform.localScale = new Vector3 (
				GameManager.currentPlayer.transform.localScale.x * firedBullet.transform.localScale.x,
				firedBullet.transform.localScale.y,
				GameManager.currentPlayer.transform.localScale.x * firedBullet.transform.localScale.z
			);
			
//			Vector2 direction = new Vector2 (bulletSpeed * gameManager.currentPlayer.transform.localScale.x, 0);
			fireRotation.Normalize ();
			firedBullet.GetComponent<Rigidbody2D> ().AddForce (bulletSpeed * fireRotation);
		} else {
			CancelInvoke ("FireBullet");
			turns.endTurn = true;
			maxFire = 0;
//			this.gameObject.SetActive(false);
		}
	}
}

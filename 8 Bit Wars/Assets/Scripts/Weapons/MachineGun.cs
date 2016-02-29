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

	public float rotationSpeed = 1;

	public LayerMask whatToHit;
	private Transform firePoint;
	private Vector2 fireRotation;
	// There needs to be a object pool for all the different bullets that need to be spawned

	void Start ()
	{
		bullet = (GameObject)Resources.Load ("Bullet"); // Loads the bullet prefab from the project folder

		firePoint = transform.FindChild ("FirePoint");

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
		RotateWeapon ();

	}

	void RotateWeapon ()
	{
		Vector2 mousePos = new Vector2 (
			                   Camera.main.ScreenToWorldPoint (Input.mousePosition).x, 
			                   Camera.main.ScreenToWorldPoint (Input.mousePosition).y
		                   );

		Vector2 firePointPos = new Vector2 (firePoint.position.x, firePoint.position.y);
		RaycastHit2D hit = Physics2D.Raycast (firePointPos, mousePos - firePointPos, 100, whatToHit);
		Debug.DrawLine (firePointPos, mousePos);

		fireRotation = mousePos - firePointPos;
		fireRotation.Normalize ();

		float rot_z = Mathf.Atan2 (fireRotation.y, fireRotation.x) * Mathf.Rad2Deg;
		print(rot_z);
		transform.localRotation = Quaternion.Euler (
			0f, 
			0f, 
			Mathf.Clamp (rot_z, -45, 45)// * GameManager.currentPlayer.transform.localScale.x
		);

		if (rot_z > 90 || rot_z < -90) {
			Move.FlipIt(GameManager.currentPlayer, false);
		}else{
			Move.FlipIt(GameManager.currentPlayer, true);

		}
//		transform.localRotation = Quaternion.LookRotation(new Vector3(fireRotation.x, fireRotation.y, 0));
	}

	void FireBullet ()
	{
		// object pooling will probably help here 
		// it would mean that when the array is over. End
		// Rather than using the I have here loop

		maxFire++;

		if (maxFire <= bulletPool.Length) {
			
			Quaternion gunrotation = transform.localRotation;
			print (gunrotation);
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

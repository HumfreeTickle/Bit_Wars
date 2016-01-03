using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
	private GameManager gameManager;
	private GameObject healthBar;

	public float currentHealth { get; set; }

	private float _timer;
	private TextMesh healthDisplay;
	private SpriteRenderer healthRenderer;

	void Start ()
	{
		currentHealth = 100;
		gameManager = GameObject.Find ("GameManger").GetComponent<GameManager> ();
		healthBar = transform.GetChild (0).gameObject;
		healthRenderer = healthBar.GetComponent<SpriteRenderer> ();
		healthDisplay = healthBar.transform.GetChild (0).GetComponent<TextMesh> ();
	}
	
	void Update ()
	{
		DamageColourChange ();
		DisplayHealth ();

		if (currentHealth <= 0 || transform.position.y < -3) {
			gameManager.player1.Remove (this.gameObject.name);
			Destroy (this.gameObject);
		}
	}

	void DisplayHealth ()
	{
		healthDisplay.text = currentHealth.ToString ();
	}

	void DamageColourChange ()
	{
		healthRenderer.color = Color.Lerp (Color.red, Color.green, currentHealth / 100);
	}
}

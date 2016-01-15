using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
	private GameManager gameManager;
	private GameObject healthBar;

	public float currentHealth { get; set; }

	private float savedHealth;
	private float _timer;
	private TextMesh healthDisplay;
	private SpriteRenderer healthRenderer;
	private Turns turns;

	void Start ()
	{
		currentHealth = 100;
		savedHealth = currentHealth;

		gameManager = GameObject.Find ("GameManger").GetComponent<GameManager> ();
		turns = GameObject.Find ("GameManger").GetComponent<Turns> ();

		healthBar = transform.GetChild (0).gameObject;
		healthRenderer = healthBar.GetComponent<SpriteRenderer> ();
		healthDisplay = healthBar.transform.GetChild (0).GetComponent<TextMesh> ();
	}
	
	void Update ()
	{
		DamageColourChange ();
		DisplayHealth ();

		if (savedHealth > currentHealth) {
			savedHealth = currentHealth;
//			gameManager.currentGameState = GameState.ChangeTurn;

			StartCoroutine (turns.TurnsCoroutine ());
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

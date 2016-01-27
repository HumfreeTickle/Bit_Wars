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

		if (currentHealth <= 0) {
			switch (gameManager.currentPlayersTurn) {
			case CurrentGo.Player1:
				gameManager.player1.Remove (this.name);
				Destroy (this.gameObject);
				break;
			case CurrentGo.Player2:
				gameManager.player2.Remove (this.name);
				Destroy (this.gameObject);
				break;
			default:
				break;
			}
		}

		if (savedHealth > currentHealth) {
			savedHealth = currentHealth;

			if(!turns.endTurn){
			turns.endTurn = true;
			}
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

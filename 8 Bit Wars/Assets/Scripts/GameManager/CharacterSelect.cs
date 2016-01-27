using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{

	private GameManager gameManager;
	private int teamNumber = 0;

	void Start ()
	{
		gameManager = GameObject.Find ("GameManger").GetComponent<GameManager> ();

		for (int child = 0; child < transform.childCount; child ++) {
			Transform childButton = transform.GetChild (child);

			childButton.GetComponent<Button> ().onClick.AddListener (delegate { 
				CharacterSelection (childButton.GetComponent<Image> ().color, childButton.GetChild (0).GetComponent<Text> ());
				//				WeaponSelected (transform.FindChild (childButton.name));
			});
		}
	}

	void CharacterSelection (Color buttonColour, Text selection)
	{
		if (teamNumber < 2) {
			selection.text = (teamNumber + 1).ToString ();
			gameManager.teamColor [teamNumber] = buttonColour;
			teamNumber ++;

		} 
//		else {
//			teamNumber = 0;
//			selection.text = (teamNumber + 1).ToString ();
//			gameManager.teamColor [teamNumber] = buttonColour;
//			teamNumber ++;
//
//		}
	}
}

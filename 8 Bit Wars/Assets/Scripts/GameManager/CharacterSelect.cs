using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{

	private GameManager gameManager;
	public int teamNumber{get; private set;}

	public List<string> teams = new List<string> (6);

	void Start ()
	{
		gameManager = GameObject.Find ("GameManger").GetComponent<GameManager> ();

		for (int child = 0; child < transform.childCount; child++) {
			Transform childButton = transform.GetChild (child);
			childButton.GetComponent<Button> ().onClick.AddListener (delegate {
				print (childButton.GetSiblingIndex ());

				CharacterSelection (childButton.GetComponent<Image> ().color, 
					childButton.GetChild (0).GetComponent<Text> (), 
					childButton.GetSiblingIndex ());
			});
		}
	}

	void CharacterSelection (Color buttonColour, Text selection, int choosenTeam)
	{
		if (teamNumber < 2) {
			selection.text = (teamNumber + 1).ToString ();
			gameManager.teams.Add (teams [choosenTeam]);
			gameManager.teamColor [teamNumber] = buttonColour;
			teamNumber++;
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

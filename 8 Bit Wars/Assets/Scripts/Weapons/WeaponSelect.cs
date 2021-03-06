﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WeaponSelect : MonoBehaviour
{
	// Might instatiate all the weapons at the start (well the ones you can use)
	// Then just activate them/ deactivate them


	public GameObject weapon1;
	private GameManager gameManager;
	public List<GameObject> weaponsList;
	public Vector3 offSetX;
	private TeamManager Team1;
	private TeamManager Team2;

	void Start ()
	{
		gameManager = GameObject.Find ("GameManger").GetComponent<GameManager> ();
		GetComponent<Image> ().enabled = false;


		Team1 = GameObject.Find ("Team 1").GetComponent<TeamManager> ();
		Team2 = GameObject.Find ("Team 2").GetComponent<TeamManager> ();

		//-- Crates the weapon select buttons --//
		for (int child = 0; child < transform.childCount; child ++) {
			Transform childButton = transform.GetChild (child);

			// adds listener to each button that calls the CheckAmmo function
			childButton.GetComponent<Button> ().onClick.AddListener (delegate { 
				CheckAmmo (transform.FindChild (childButton.name));
			});
			transform.GetChild (child).gameObject.SetActive (false);
		}
	}

	void CheckAmmo (Transform childButton)
	{
		if (gameManager.currentPlayersTurn == CurrentGo.Player1) {
			if (Team1.weaponsList [childButton.name] > 0) {
				WeaponSelected (childButton, Team1);
			}
		} else if (gameManager.currentPlayersTurn == CurrentGo.Player2) {
			if (Team2.weaponsList [childButton.name] > 0) {
				WeaponSelected (childButton, Team2);
			}
		}
	}

	void WeaponSelected (Transform buttonPressed, TeamManager currentTeam)
	{
	// --- Now all there needs to be is a check to see if you've actually used the weapon
	// --- Or are just switching.

		if (GameManager.currentGameState == GameState.Menu) {
			// Destroys previous weapon
			if (GameManager.currentPlayer.transform.childCount > 2) {
				Destroy (GameManager.currentPlayer.transform.GetChild (2).gameObject);
			}

			int buttonPressedIndex = transform.FindChild (buttonPressed.name).GetSiblingIndex ();

			GameObject weaponSelected = weaponsList [buttonPressedIndex];
			GameObject weapon = Instantiate (weaponSelected, 
				GameManager.currentPlayer.transform.position + (
				offSetX * GameManager.currentPlayer.transform.localScale.x),
				Quaternion.identity
			) as GameObject; 
			weapon.transform.localScale = new Vector3 (weapon.transform.localScale.x * GameManager.currentPlayer.transform.localScale.x,
		                                          weapon.transform.localScale.y,
		                                          weapon.transform.localScale.z);
			weapon.transform.parent = GameManager.currentPlayer.transform;
//			fireWeapon.weaponName = buttonPressed.name;

//			print (buttonPressed.name);
			currentTeam.weaponsList [buttonPressed.name] -= 1;
			currentTeam.UpdateAmmoDisplay(buttonPressed);
//			print ("Ammo left: " + currentTeam.weaponsList [buttonPressed.name]);

			// turns off menu once weapon has been selected
			StartCoroutine (gameManager.Menu ());
		}
	}
}

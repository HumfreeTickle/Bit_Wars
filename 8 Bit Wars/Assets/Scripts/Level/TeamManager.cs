using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TeamManager : MonoBehaviour {

	private GameManager gameManager;

	public List<string> players{ get; private set; }
	private Transform Team;

	public Dictionary<string, int> weaponsList {get;set;}
	private List<Text> Ammo = new List<Text>();

	public Color teamColour{get; private set;}

	public void InitialiseTeams(Move moving, Transform centreOfTheMap){

		gameManager = GameObject.Find("GameManger").GetComponent<GameManager>();

		Team = GameObject.Find (this.gameObject.name).transform;
		int teamNumber = System.Int32.Parse(this.name.Substring(this.name.Length - 1, 1));
		teamColour = gameManager.teamColor[teamNumber - 1];

		players = new List<string> (Team.childCount);

		//----------------- Spawning Teams --------------------//
		
		for (int character = 0; character < Team.childCount; character++) {
			Transform currentCharacter = transform.GetChild (character);

			if (currentCharacter.position.x > centreOfTheMap.position.x) {
				moving.FlipIt (currentCharacter.gameObject);
			}
			currentCharacter.GetComponent<SpriteRenderer>().color = teamColour;
			players.Add (currentCharacter.name);
			
			// Move into Terrain Manager
			gameManager.SpawnCharacters (currentCharacter.transform);
		}

		weaponsList = new Dictionary<string, int>();

		weaponsList.Add("Tommy Gun", 100000);
		weaponsList.Add("Bomb", 5);

		Transform WeaponsMenu = GameObject.Find("Weapons Menu").transform;


		for(int child = 0; child < WeaponsMenu.childCount; child++){
			if(WeaponsMenu.GetChild(child).childCount > 0){
				Transform weaponType = WeaponsMenu.GetChild(child);
				Text ammoDisplay = weaponType.GetChild(0).GetComponent<Text>();
				ammoDisplay.text = weaponsList[weaponType.name].ToString();
				Ammo.Add(ammoDisplay);
			}
		}
	}

	public void UpdateAmmoDisplay(Transform weaponType){
		Text ammoDisplay = weaponType.GetChild(0).GetComponent<Text>();
		ammoDisplay.text = weaponsList[weaponType.name].ToString();
	}
}

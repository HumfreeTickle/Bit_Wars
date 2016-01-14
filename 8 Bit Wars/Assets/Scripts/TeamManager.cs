using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TeamManager : MonoBehaviour {

	public Dictionary<string, int> weaponsList {get;set;}
	private List<Text> Ammo = new List<Text>();

	 void Start(){
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

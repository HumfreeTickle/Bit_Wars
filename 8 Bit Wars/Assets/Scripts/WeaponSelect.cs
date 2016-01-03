using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WeaponSelect : MonoBehaviour
{
	public GameObject weapon1;
	private GameManager gameManager;

	public List<GameObject> weaponsList;

	void Start ()
	{
		gameManager = GameObject.Find("GameManger").GetComponent<GameManager>();

		for (int child = 0; child < transform.childCount; child ++) {
			Transform childButton = transform.GetChild (child);
			childButton.GetComponent<Button> ().onClick.AddListener (delegate { 
				WeaponSelected (transform.FindChild(childButton.name).GetSiblingIndex()); 
			});
		}
		this.gameObject.SetActive (false);
	}

	void WeaponSelected (int buttonPressed)
	{
		if(gameManager.currentPlayer.transform.childCount > 2){
			Destroy(gameManager.currentPlayer.transform.GetChild(2).gameObject);
		}

		GameObject weaponSelected = weaponsList[buttonPressed];
		GameObject weapon = Instantiate(weaponSelected, gameManager.currentPlayer.transform.position, Quaternion.identity) as GameObject; 
		weapon.transform.parent = gameManager.currentPlayer.transform;
	}
}

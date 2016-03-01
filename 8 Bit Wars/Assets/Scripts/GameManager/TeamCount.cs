using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TeamCount : MonoBehaviour {

	private Transform Team;
	private int teamCount;

	void Start () {
		Team = GameObject.Find(transform.GetChild(0).name.Substring(0, 6)).transform; //Finds the correct Team based off the first 6 characters of the gameObject
																					 // ie. Team 1 Status ==> Finds "Team 1"
		teamCount = Team.childCount;
		transform.GetChild(0).GetComponent<Image>().color = Team.GetComponent<TeamManager>().teamColour;
	}
	
	void Update () {
		GetComponent<Text>().text = teamCount.ToString();
		if(teamCount > Team.childCount){
			teamCount = Team.childCount;
			BarSize(transform.GetChild(0).GetComponent<RectTransform>());
		}
	}

	void BarSize(RectTransform teamProgressBar){
		float changedWidth = Mathf.Lerp(0, 1, (float)teamCount/4);
		Vector3 newScale = new Vector3(changedWidth, teamProgressBar.localScale.y, teamProgressBar.localScale.z);
		teamProgressBar.localScale = newScale;
	}
}

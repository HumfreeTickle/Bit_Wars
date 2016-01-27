using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TeamCount : MonoBehaviour {

	private Transform Team;
	private int teamCount;


	// Use this for initialization
	void Start () {
		Team = GameObject.Find(transform.parent.name.Substring(0, 6)).transform	;
		teamCount = Team.childCount;
		transform.parent.GetComponent<Image>().color = Team.GetComponent<TeamManager>().teamColour;
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Text>().text = Team.childCount.ToString();
//		if(teamCount > Team.childCount){
//			BarSize(transform.parent.GetComponent<RectTransform>());
//			teamCount = Team.childCount;
//		}
	}

	void BarSize(RectTransform teamProgressBar){
		float width = teamProgressBar.localScale.x;
		float changedWidth = Mathf.Lerp(width, 0, teamCount/4);

		Vector3 newScale = new Vector3(changedWidth, teamProgressBar.localScale.y, teamProgressBar.localScale.z);
		teamProgressBar.localScale = newScale;
	}
}

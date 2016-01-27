using UnityEngine;
using System.Collections;

public class AnimationEventCall : MonoBehaviour {

	private GameManager gameManager;

	void Start () {
		gameManager = GameObject.Find("GameManger").GetComponent<GameManager>();
	}

	public void _AnimStartGameEvent(){
		gameManager.StartGame();
	}

}

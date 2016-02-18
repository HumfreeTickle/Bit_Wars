using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Start_Game : MonoBehaviour {

	void Awake(){
		if(GameObject.Find("GameManger")){
			Destroy(GameObject.Find("GameManger"));
		}
	}

	// Update is called once per frame
	void Update () {
	if(Input.anyKeyDown){
			int currentSceneIndex = SceneManager.GetActiveScene ().buildIndex;
			SceneManager.LoadScene (currentSceneIndex += 1);
		}
	}
}

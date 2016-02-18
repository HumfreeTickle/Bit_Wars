 using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Name : MonoBehaviour {

	private Transform nameTag;
	private TextMesh nameTag_Text;

	// Use this for initialization
	void Start () {
		nameTag = transform.FindChild("NameTag");
		nameTag.localPosition = new Vector3(0, 1.5f, -0.15f);
		nameTag.localScale = new Vector3(nameTag.parent.localScale.x * 1.5f, 1.5f, 1);

		nameTag_Text = transform.FindChild("NameTag").GetComponent<TextMesh>();
		nameTag_Text.characterSize = 0.15f;
		DisplayName ();
	}
	
	// Update is called once per frame
	void DisplayName () {
		nameTag_Text.text = this.gameObject.name;
	}
}

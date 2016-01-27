using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Name : MonoBehaviour {

	private Transform nameTag;
	private TextMesh nameTag_Text;

	// Use this for initialization
	void Start () {
		nameTag = transform.FindChild("NameTag");
		nameTag.localPosition = new Vector3(0, 1.4f, 0);
		nameTag.localScale = nameTag.parent.localScale;

		nameTag_Text = transform.FindChild("NameTag").GetComponent<TextMesh>();
		DisplayName ();
	}
	
	// Update is called once per frame
	void DisplayName () {
		nameTag_Text.text = this.gameObject.name;
	}
}

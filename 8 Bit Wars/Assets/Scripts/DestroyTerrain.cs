using UnityEngine;
using System.Collections;

public class DestroyTerrain : MonoBehaviour
{

	public GameObject box;

	void Update ()
	{
		if (box != null) {
			if (!IsInvoking ("ByeBye")) {
				Invoke ("ByeBye", 5f);
			}
		}

	}

	void ByeBye ()
	{
		box.GetComponent<Level_Colliders> ().OnBlockDestroyed (box.GetComponent<Level_Colliders> ().edgeIntersects);
		box = null;
	}
}

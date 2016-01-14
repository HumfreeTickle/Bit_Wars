using UnityEngine;
using System.Collections;

public class DestroyParticles : MonoBehaviour {

	public float _timer = 1;

	void Update () {
		Destroy(this.gameObject, _timer);
	}
}

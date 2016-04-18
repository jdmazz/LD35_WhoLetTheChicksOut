using UnityEngine;
using System.Collections;

public class TimedDestruction : MonoBehaviour {
	public float timeOut = 1.0f;
	public bool detachChildren = false;

	void Awake () {
		Invoke ("Destroy", timeOut);
	}

	void Destroy () {
		if (detachChildren) {
			transform.DetachChildren ();
		}
		Destroy(gameObject);
	}
}

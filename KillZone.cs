using UnityEngine;
using System.Collections;

public class KillZone : MonoBehaviour {
	public bool destroyNpos = true;

	void OnCollisionEnter2D (Collision2D other) {
		if (other.gameObject.tag == "Player")
			other.gameObject.GetComponent<PlayerController2d> ().FallKill ();
		else if (destroyNpos)
			DestroyObject (other.gameObject);
	}
}

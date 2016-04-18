using UnityEngine;
using System.Collections;

public class Knockout : MonoBehaviour {
	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Player") {
			if (other.gameObject.GetComponent<PlayerController2d> ().isChicken)
				this.GetComponentInParent<Stalker> ().KnockedOut ();
		}
	}
}

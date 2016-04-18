using UnityEngine;
using System.Collections;

public class Winner : MonoBehaviour {
	public bool won = false;
	public GameObject explosion;

	void OnTriggerEnter2D (Collider2D other) {
		if ((other.tag == "Player") && (!won) && (other.gameObject.GetComponent<PlayerController2d> ().playerCanMove)) {
			won = true;
			if (explosion)
				Instantiate (explosion, transform.position, transform.rotation);
			other.gameObject.GetComponent<PlayerController2d> ().Winner ();
		}
	}
}

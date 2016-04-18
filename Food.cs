using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour {
	public int foodVal = 1;
	public bool eaten = false;
	public GameObject juice;

	void OnTriggerEnter2D(Collider2D other) {
		if ((other.tag == "Player") && (!eaten) && other.gameObject.GetComponent<PlayerController2d>().playerCanMove) {
			eaten = true;
			if (juice)
				Instantiate (juice, transform.position, transform.rotation);
			other.gameObject.GetComponent<PlayerController2d>().CollectFood (foodVal);
			DestroyObject (this.gameObject);
		}
	}
}

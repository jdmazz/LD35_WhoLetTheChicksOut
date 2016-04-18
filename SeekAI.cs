using UnityEngine;
using System.Collections;

public class SeekAI : MonoBehaviour {
	public Transform target;
	public float moveSpeed;
	public float maxDist;
	public AudioClip hawkSFX;

	Transform mTransform;
	Rigidbody2D mRigidbody;
	AudioSource mAudio;
	bool facingLeft = true;

	void Awake() {
		mTransform = GetComponent<Transform> ();
		mAudio = GetComponent<AudioSource> ();
		mRigidbody = GetComponent<Rigidbody2D> ();
		if (target == null)
			target = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	void Update() {
		if (target == null)
			target = GameObject.FindGameObjectWithTag ("Player").transform;

		if (facingLeft && target.position.x > mTransform.position.x) {
			Vector3 localScale = mTransform.localScale;
			mTransform.localScale = new Vector3 (-1 * localScale.x, localScale.y, 0f);
			facingLeft = false;
		} else if (!facingLeft && target.position.x < mTransform.position.x) {
			Vector3 localScale = mTransform.localScale;
			mTransform.localScale = new Vector3 (-1 * localScale.x, localScale.y, 0f);
			facingLeft = true;
		}
		Seek ();
	}

	void Seek() {
		if (Vector3.Distance (target.position, mTransform.position) > maxDist) {
			Vector3 dir = target.position - mTransform.position;
			dir.Normalize ();
			mTransform.position += dir * moveSpeed * Time.deltaTime;
		}
	}

	void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Player") {
			PlayerController2d player = collision.gameObject.GetComponent<PlayerController2d>();
			mAudio.PlayOneShot(hawkSFX);
			mRigidbody.velocity = new Vector2 (0, 0);
			player.ApplyDamage (1);
		}
	}
}

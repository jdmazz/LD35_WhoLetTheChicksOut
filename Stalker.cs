using UnityEngine;
using System.Collections;

public class Stalker : MonoBehaviour {
	public float moveSpeed = 4f;
	public int damageAmount = 1;
	public GameObject stunnedCheck;
	public float stunnedTime = 3f;
	public string stunnedLayer = "StunnedEnemy";
	public string playerLayer = "Player";
	public bool isStunned = false;
	public GameObject[] mWaypoints;
	public float waitAtWaypointTime = 1f;
	public bool loopWaypoints = true;
	public AudioClip stunnedSFX;
	public AudioClip attackSFX;

	Transform mTransform;
	Rigidbody2D mRigidbody;
	Animator mAnimator;
	AudioSource mAudio;

	int mWaypointIndex = 0;
	float moveTime; 
	float vx = 0f;
	bool isMoving = true;
	int mEnemyLayer, mStunnedLayer;

	void Awake() {
		mTransform = GetComponent<Transform> ();
		mRigidbody = GetComponent<Rigidbody2D> ();
		mAnimator = GetComponent<Animator> ();
		mAudio = GetComponent<AudioSource> ();
		moveTime = 0f;
		isMoving = true;
		mEnemyLayer = this.gameObject.layer;
		mStunnedLayer = LayerMask.NameToLayer (stunnedLayer);
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer (playerLayer), mStunnedLayer, true);
	}

	void Update() {
		if (!isStunned) {
			if (Time.time >= moveTime)
				EnemyMovement ();
			else
				mAnimator.SetBool ("Moving", false);
		}
	}

	void EnemyMovement() {
		if ((mWaypoints.Length != 0) && (isMoving)) {
			Flip (vx);
			vx = mWaypoints [mWaypointIndex].transform.position.x - mTransform.position.x;
			if (Mathf.Abs (vx) <= 0.05f) {
				mRigidbody.velocity = new Vector2 (0, 0);
				++mWaypointIndex;
				if (mWaypointIndex >= mWaypoints.Length) {
					if (loopWaypoints)
						mWaypointIndex = 0;
					else
						isMoving = false;
				}
				moveTime = Time.time + waitAtWaypointTime;
			} else {
				mAnimator.SetBool ("Moving", true);
				mRigidbody.velocity = new Vector2 (mTransform.localScale.x * moveSpeed, mRigidbody.velocity.y);
			}
		}
	}

	void Flip(float _vx) {
		Vector3 localScale = mTransform.localScale;
		if ((vx > 0f) && (localScale.x < 0f))
			localScale.x *= -1;
		else if ((vx < 0f) && (localScale.x > 0f))
			localScale.x *= -1;
		mTransform.localScale = localScale;
	}

	void OnTriggerEnter2D(Collider2D collision) {
		if ((collision.tag == "Player") && !isStunned) {
			PlayerController2d player = collision.gameObject.GetComponent<PlayerController2d> ();
			if (player.playerCanMove) {
				Flip (collision.transform.position.x - mTransform.position.x);
				mAudio.PlayOneShot(attackSFX);
				mRigidbody.velocity = new Vector2 (0, 0);
				player.ApplyDamage (damageAmount);
				moveTime = Time.time + stunnedTime;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag=="MovingPlatform") {
			this.transform.parent = other.transform;
		}
	}

	void OnCollisionExit2D(Collision2D other) {
		if (other.gameObject.tag=="MovingPlatform") {
			this.transform.parent = null;
		}
	}

	public void KnockedOut() {
		if (!isStunned) {
			isStunned = true;
			mAudio.PlayOneShot (stunnedSFX);
			mAnimator.SetTrigger ("Knockedout");
			mRigidbody.velocity = new Vector2 (0, 0);
			this.gameObject.layer = mStunnedLayer;
			stunnedCheck.layer = mStunnedLayer;
			StartCoroutine (Stand());
		}
	}

	IEnumerator Stand() {
		yield return new WaitForSeconds(stunnedTime);
		isStunned = false;
		this.gameObject.layer = mEnemyLayer;
		stunnedCheck.layer = mEnemyLayer;
		mAnimator.SetTrigger ("Stand");
	}
}

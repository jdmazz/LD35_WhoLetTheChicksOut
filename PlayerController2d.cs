using UnityEngine;
using System.Collections;

public class PlayerController2d : MonoBehaviour {
	[RangeAttribute(0.0f, 10.0f)]
	public float moveSpeed = 3f;
	public float jumpForce = 600f;
	public Transform groundCheck;
	public LayerMask whatIsGround;
	public int playerHealth = 1;
	public int powerUpInt = 3;
	public GameObject spawnPrefab;

	[HideInInspector]
	public bool isChicken = false;

	[HideInInspector]
	public bool playerCanMove = true;

	public AudioClip foodSFX;
	public AudioClip dinnerSFX;
	public AudioClip fallSFX;
	public AudioClip jumpSFX;
	public AudioClip winnerSFX;

	Transform mTransform;
	Rigidbody2D mRigidbody;
	Animator mAnimator;
	AudioSource mAudio;
	float vx, vy;

	bool facingRight = true;
	bool isGrounded = false;
	bool isWalking = false;
	bool canDblJump = false;
	int playerLayer, platformLayer;

	void Awake() {
		mTransform = GetComponent<Transform> ();
		mRigidbody = GetComponent<Rigidbody2D> ();
		mAnimator = GetComponent<Animator> ();
		mAudio = GetComponent<AudioSource> ();
		playerLayer = this.gameObject.layer;
		platformLayer = LayerMask.NameToLayer ("Platform");
	}

	void Update () {
		if (!playerCanMove || (Time.timeScale == 0f))
			return;
		vx = Input.GetAxisRaw ("Horizontal");
		if (vx != 0) isWalking = true;
		else isWalking = false;
		mAnimator.SetBool ("Walking", isWalking);
		vy = mRigidbody.velocity.y;
		isGrounded = Physics2D.Linecast (mTransform.position, groundCheck.position, whatIsGround);
		if (isChicken && isGrounded)
			canDblJump = true;
		mAnimator.SetBool ("Grounded", isGrounded);
		if (isGrounded && Input.GetButtonDown ("Jump"))
			Jump ();
		else if (canDblJump && Input.GetButtonDown ("Jump")) {
			Jump (); // dblJump
			canDblJump = false;
		}
		if (Input.GetButtonUp ("Jump") && vy > 0f)
			vy = 0f;
		mRigidbody.velocity = new Vector2 (vx * moveSpeed, vy);
		Physics2D.IgnoreLayerCollision (playerLayer, platformLayer, vy > 0f);
	}

	void Jump() {
		vy = 0f;
		mRigidbody.AddForce (new Vector2 (0, jumpForce));
		mAudio.PlayOneShot (jumpSFX);
	}

	void LateUpdate() {
		Vector3 localeScale = mTransform.localScale;
		if (vx > 0)
			facingRight = true;
		else if (vx < 0)
			facingRight = false;
		if (((facingRight) && (localeScale.x < 0)) || ((!facingRight) && (localeScale.x > 0)))
			localeScale.x *= -1;
		mTransform.localScale = localeScale;
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "MovingPlatform")
			this.transform.parent = other.transform;
	}

	void OnCollisionExit2D(Collision2D other) {
		if (other.gameObject.tag == "MovingPlatform") {
			this.transform.parent = null;
		}
	}

	void FreezeMotion() {
		playerCanMove = false;
		mRigidbody.isKinematic = true;
	}

	void UnFreezeMotion() {
		playerCanMove = true;
		mRigidbody.isKinematic = false;
	}

	public void ApplyDamage (int damage) {
		if (playerCanMove) {
			playerHealth -= damage;
			if (playerHealth <= 0) {
				mAudio.PlayOneShot (dinnerSFX);
				StartCoroutine (KillPlayer ());
			}
		}
	}

	public void FallKill() {
		if (playerCanMove) {
			playerHealth = 0;
			mAudio.PlayOneShot (fallSFX);
			StartCoroutine (KillPlayer ());
		}
	}

	IEnumerator KillPlayer() {
		if (playerCanMove) {
			FreezeMotion ();
			mAnimator.SetTrigger ("Dinner");
			yield return new WaitForSeconds (2f);
			if (GameManager.gm)
				GameManager.gm.ResetGame ();
		}
	}

	public void CollectFood(int amount) {
		mAudio.PlayOneShot (foodSFX);
		if (GameManager.gm)
			GameManager.gm.AddWorms(amount);
		if (!isChicken && (spawnPrefab != null) && GameManager.gm.wormCount >= powerUpInt) {
			Instantiate (spawnPrefab, transform.position, transform.rotation);
			spawnPrefab.GetComponent<PlayerController2d> ().isChicken = true;
			Destroy (this.gameObject);
		}
	}

	public void Winner() {
		mAudio.PlayOneShot(winnerSFX);
		FreezeMotion ();
		mAnimator.SetTrigger ("Winner");
		if (GameManager.gm)
			GameManager.gm.LevelComplete ();
	}

	public void Respawn (Vector3 location) {
		UnFreezeMotion ();
		playerHealth = 1;
		mTransform.parent = null;
		mTransform.position = location;
		mAnimator.SetTrigger ("Respawn");
	}
}

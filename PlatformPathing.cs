using UnityEngine;
using System.Collections;

public class PlatformPathing : MonoBehaviour {
	public GameObject platform;
	public GameObject[] mWaypoints;
	[Range(0f, 10f)]
	public float moveSpeed = 5f;
	public float waitTime = 1f;
	public bool loop = true;

	Transform mTransform;
	int mWaypointI = 0;
	float moveTime;
	bool isMoving = true;

	void Start() {
		mTransform = platform.transform;
		moveTime = 0f;
		isMoving = true;
	}

	void Update() {
		if (Time.time >= moveTime)
			Movement ();
	}

	void Movement() {
		if ((mWaypoints.Length != 0) && (isMoving)) {
			mTransform.position = Vector3.MoveTowards (mTransform.position, mWaypoints [mWaypointI].transform.position, moveSpeed * Time.deltaTime);
			if (Vector3.Distance (mWaypoints [mWaypointI].transform.position, mTransform.position) <= 0) {
				++mWaypointI;
				moveTime = Time.time + waitTime;
			}
			if (mWaypointI >= mWaypoints.Length) {
				if (loop)
					mWaypointI = 0;
				else
					isMoving = false;
			}
		}
	}
}

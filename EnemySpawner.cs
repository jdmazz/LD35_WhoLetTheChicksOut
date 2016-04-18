using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
	public GameObject spawnPrefab;
	public float minSecondsBetweenSpawning = 3.0f;
	public float maxSecondsBetweenSpawning = 6.0f;

	float savedTime;
	float secondsBetweenSpawning;

	void Start () {
		savedTime = Time.time;
		secondsBetweenSpawning = Random.Range (minSecondsBetweenSpawning, maxSecondsBetweenSpawning);
	}
		
	void Update () {
		if (Time.time - savedTime >= secondsBetweenSpawning) {
			Instantiate(spawnPrefab, transform.position, transform.rotation);
			savedTime = Time.time;
			secondsBetweenSpawning = Random.Range (minSecondsBetweenSpawning, maxSecondsBetweenSpawning);
		}	
	}
}

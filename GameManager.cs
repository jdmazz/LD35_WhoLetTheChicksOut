using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	public static GameManager gm;

	public string levelAfterVictory;
	public string levelAfterGameOver;
	public int powerWormNum = 3;
	public int startLives = 1;
	public int lives = 3;

	[HideInInspector]
	public int wormCount = 0;
	[HideInInspector]
	public int highscore = 0;

	public Text UIWorms;
	public Text UILevel;
	public GameObject UIGamePaused;

	GameObject mPlayer;
	Vector3 mSpawnLocation;

	void Awake() {
		if (gm == null)
			gm = this.GetComponent<GameManager> ();
		SetupDefaults ();
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (Time.timeScale > 0f) {
				UIGamePaused.SetActive (true);
				Time.timeScale = 0f;
			} else {
				Time.timeScale = 1f;
				UIGamePaused.SetActive (false);
			}
		}
	}

	void SetupDefaults() {
		if (mPlayer == null)
			mPlayer = GameObject.FindGameObjectWithTag ("Player");
		if (UIWorms==null)
			Debug.LogError ("Need to set UIScore on Game Manager.");
		mSpawnLocation = mPlayer.transform.position;
		if (levelAfterGameOver == "")
			levelAfterGameOver = SceneManager.GetActiveScene ().name;
		RefreshPlayerState ();
		RefreshGUI ();
	}

	void RefreshPlayerState() {
		lives = PlayerPrefManager.GetLives();
		if (lives <= 0) {
			PlayerPrefManager.ResetPlayerState (startLives, false);
			lives = PlayerPrefManager.GetLives();
		}
		wormCount = PlayerPrefManager.GetWormCount ();
		highscore = PlayerPrefManager.GetHighscore ();
		PlayerPrefManager.UnlockLevel ();
	}

	void RefreshGUI() {
		UIWorms.text = wormCount.ToString ();
		UILevel.text = SceneManager.GetActiveScene ().name;
	}

	public void AddWorms(int amount) {
		wormCount += amount;
		UIWorms.text = wormCount.ToString ();
		if (wormCount % powerWormNum == 0) {
			// Turn into player into chicken
		}
	}

	public void ResetGame() {
		--lives;
		RefreshGUI ();
		if (lives <= 0) {
			PlayerPrefManager.SavePlayerState (wormCount, highscore, lives);
			SceneManager.LoadScene (levelAfterGameOver);
		} else
			mPlayer.GetComponent<PlayerController2d> ().Respawn (mSpawnLocation);
	}

	public void LevelComplete() {
		PlayerPrefManager.SavePlayerState (wormCount, highscore, lives);
		StartCoroutine (LoadNextLevel ());
	}

	IEnumerator LoadNextLevel() {
		yield return new WaitForSeconds (3f);
		SceneManager.LoadScene (levelAfterVictory);
	}
}

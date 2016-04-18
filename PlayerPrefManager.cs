using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public static class PlayerPrefManager  {
	public static int GetLives() {
		if (PlayerPrefs.HasKey("Lives")) {
			return PlayerPrefs.GetInt("Lives");
		} else {
			return 0;
		}
	}

	public static void SetLives(int lives) {
		PlayerPrefs.SetInt("Lives",lives);
	}

	public static int GetWormCount() {
		if (PlayerPrefs.HasKey ("WormCount"))
			return PlayerPrefs.GetInt ("WormCount");
		else return 0;
	}

	public static void SetWormCount(int numWorms) {
		PlayerPrefs.SetInt ("WormCount", numWorms);
	}

	public static int GetHighscore() {
		if (PlayerPrefs.HasKey("Highscore")) {
			return PlayerPrefs.GetInt("Highscore");
		} else {
			return 0;
		}
	}

	public static void SetHighscore(int highscore) {
		PlayerPrefs.SetInt("Highscore",highscore);
	}

	public static void SavePlayerState(int wormCount, int highScore, int lives) {
		PlayerPrefs.SetInt ("WormCount", 0);
		PlayerPrefs.SetInt("Lives",lives);
		PlayerPrefs.SetInt("Highscore",highScore);
	}

	public static void ResetPlayerState(int startLives, bool resetHighscore) {
		PlayerPrefs.SetInt("Lives",startLives);
		PlayerPrefs.SetInt ("WormCount", 0);
		if (resetHighscore)
			PlayerPrefs.SetInt("Highscore", 0);
	}

	public static void UnlockLevel() {
		PlayerPrefs.SetInt (SceneManager.GetActiveScene ().name, 1);
	}

	public static bool LevelIsUnlocked(string levelName) {
		return (PlayerPrefs.HasKey (levelName));
	}

	public static void ShowPlayerPrefs() {
		string[] values = {"WormCount","Highscore","Lives"};
		foreach(string value in values) {
			if (PlayerPrefs.HasKey(value))
				Debug.Log (value+" = "+PlayerPrefs.GetInt(value));
			else
				Debug.Log (value+" is not set.");
		}
	}
}

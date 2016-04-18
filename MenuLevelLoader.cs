using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuLevelLoader : MonoBehaviour {
	public string levelToLoad;
	public float delay = 2f;

	void Update () {
		Invoke("LoadLevel",delay);
	}
		
	void LoadLevel() {
		SceneManager.LoadScene (levelToLoad);
	}
}

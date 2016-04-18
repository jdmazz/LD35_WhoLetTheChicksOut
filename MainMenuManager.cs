using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
	public int startLives = 3;
	public GameObject mainMenu;
	public GameObject levelsMenu;
	public GameObject aboutMenu;

	public GameObject menuDefaultButton;
	public GameObject aboutDefaultButton;
	public GameObject levelSelectDefaultButton;
	public GameObject quitButton;

	public string[] levelNames;
	public GameObject levelsPanel;
	public GameObject levelButtonPrefab;
	public Text titleText;

	private string mainTitle;

	void Awake() {
		mainTitle = titleText.text;
		SetLevelSelect ();
		DisplayQuitWhenAppropriate ();
		ShowMenu ("MainMenu");
	}

	void SetLevelSelect() {
		levelsMenu.SetActive (true);
		for (int i = 0; i < levelNames.Length; i++) {
			string levelName = levelNames[i];
			GameObject levelButton = Instantiate(levelButtonPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			levelButton.name = levelName + " Button";
			levelButton.transform.SetParent(levelsPanel.transform, false);
			Button levelButtonScript = levelButton.GetComponent<Button>();
			levelButtonScript.onClick.RemoveAllListeners();
			levelButtonScript.onClick.AddListener(() => LoadLevel(levelName));
			Text levelButtonLabel = levelButton.GetComponentInChildren<Text>();
			levelButtonLabel.text = levelName;
			if (PlayerPrefManager.LevelIsUnlocked (levelName))
				levelButtonScript.interactable = true;
			else
				levelButtonScript.interactable = false;
		}
	}

	void DisplayQuitWhenAppropriate() {
		switch (Application.platform) {
		case RuntimePlatform.WindowsPlayer:
		case RuntimePlatform.OSXPlayer:
		case RuntimePlatform.LinuxPlayer:
			quitButton.SetActive (true);
			return;
		default:
			quitButton.SetActive (false);
			return;
		}
	}

	public void ShowMenu(string name) {
		mainMenu.SetActive (false);
		aboutMenu.SetActive (false);
		levelsMenu.SetActive (false);
		switch (name) {
		case "MainMenu":
			mainMenu.SetActive (true);
			EventSystem.current.SetSelectedGameObject (menuDefaultButton);
			titleText.text = mainTitle;
			return;
		case "LevelSelect":
			levelsMenu.SetActive(true);
			EventSystem.current.SetSelectedGameObject (levelSelectDefaultButton);
			titleText.text = "Level Select";
			return;
		case "About":
			aboutMenu.SetActive(true);
			EventSystem.current.SetSelectedGameObject (aboutDefaultButton);
			titleText.text = "About";
			return;
		}
	}

	public void LoadLevel(string leveltoLoad) {
		PlayerPrefManager.ResetPlayerState(startLives,false);
		SceneManager.LoadScene (leveltoLoad);
	}

	public void QuitGame() {
		Application.Quit ();
	}
}

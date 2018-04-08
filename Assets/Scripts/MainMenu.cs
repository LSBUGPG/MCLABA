using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class MainMenu : MonoBehaviour {

	public SaveGame saveGamePrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartGame (){
		SceneManager.LoadSceneAsync (1);
	}

	public void TitleScreen (){
		SceneManager.LoadSceneAsync (0); 
	}

	public void GameBegin(){
		SceneManager.LoadSceneAsync (2);
	}

	public void Settings (){
		SceneManager.LoadSceneAsync (0);
	}	

	public void ExitGame (){
		Application.Quit();
	}

	public void LoadGame (int slot){
		SaveGame saveGame = FindObjectOfType<SaveGame> ();
		if (saveGame == null) {
			saveGame = Instantiate (saveGamePrefab);
			GameObject.DontDestroyOnLoad (saveGame.gameObject);
		}
		saveGame.LoadFromSlot (slot);
		SceneManager.LoadScene ("Test");
	}
}

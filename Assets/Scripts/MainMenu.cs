using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class MainMenu : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartGame (){
		SceneManager.LoadSceneAsync (1);
	}

	public void Settings (){
		SceneManager.LoadSceneAsync (0);
	}	

	public void ExitGame (){
		Application.Quit();
	}
}

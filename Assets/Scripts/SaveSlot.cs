using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSlot : MonoBehaviour {

	public int slot;

	public void Save()
	{
		SaveGame saveGame = FindObjectOfType<SaveGame> ();
		if (saveGame != null) {
			saveGame.SaveToSlot (slot);
		}
		SceneManager.LoadScene ("Test");
	}
}

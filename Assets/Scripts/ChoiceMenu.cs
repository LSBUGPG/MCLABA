using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ChoiceMenu : MonoBehaviour
{
    public Button choicePrefab;
	public SaveGame saveGamePrefab;
	public Story story;

	public void SetChoices(List<DialogueOption> choices, Dictionary<string, Character> characters, System.Action<string> jumpTo)
    {
        foreach (DialogueOption choice in choices)
        {
			if (string.IsNullOrEmpty (choice.choiceText)) {
				Character character = null;
				if (string.IsNullOrEmpty (choice.requiredCharacter)) {
					jumpTo (choice.jumpTo);
					break;
				}
				else if (characters.TryGetValue (choice.requiredCharacter, out character)) {
					if (character.affection >= choice.requiredAffection) {
						jumpTo (choice.jumpTo);
						break;
					}
				}

			} else {
				Button button = Instantiate (choicePrefab, transform);
				Text text = button.GetComponentInChildren<Text> ();
				text.text = choice.choiceText;
				if (string.IsNullOrEmpty (choice.specialAction)) {
					button.onClick.AddListener (() => {
						ClearChoices ();
						jumpTo (choice.jumpTo);
					});
				} else if (choice.specialAction == "saveGame") {
					button.onClick.AddListener (() => {
						SaveGame(choice.jumpTo);
					});
				}
			}
        }
    }

	void SaveGame(string jumpTo)
	{
		SaveGame saveGame = FindObjectOfType<SaveGame> ();
		if (saveGame == null) {
			saveGame = Instantiate (saveGamePrefab);
			GameObject.DontDestroyOnLoad (saveGame.gameObject);
		}

		saveGame.saveData = story.CreateSaveData (jumpTo);
		SceneManager.LoadScene ("SaveScene");
	}

    void ClearChoices()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ChoiceMenu : MonoBehaviour
{
    public Button choicePrefab;

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
				button.onClick.AddListener (() => {
					ClearChoices ();
					jumpTo (choice.jumpTo);
				});
			}
        }
    }

    void ClearChoices()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}

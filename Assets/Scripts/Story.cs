using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Story : MonoBehaviour
{
	public Background background;
	public Image portrait;
	public Text speakerName;
	public Text dialogueBox;
	public ChoiceMenu menu;
	public bool debugLoad = false;

	Dictionary<string, NarrativeEvent> narrative = new Dictionary<string, NarrativeEvent> ();
	Dictionary<string, Sprite> images = new Dictionary<string, Sprite> ();
	Dictionary<string, Character> characters = new Dictionary<string, Character>();

	void Start ()
	{
		foreach (TextAsset asset in Resources.LoadAll<TextAsset>("Story")) {
			Debug.Log (asset.name);
			NarrativeEvent passage = JsonUtility.FromJson<NarrativeEvent> (asset.text);
			if (passage != null) {
				if (debugLoad) {
					Debug.LogFormat ("Loaded story passage {0}", asset.name);
				}
				narrative [asset.name] = passage;
			} else {
				Debug.LogWarningFormat ("Could not load story source text '{0}'", asset.name);
			}
		}

		foreach (Sprite sprite in Resources.LoadAll<Sprite>("Images")) {
			if (debugLoad) {
				Debug.LogFormat ("Loaded image {0}", sprite.name);
			}
			images [sprite.name] = sprite;
		}

		StartCoroutine (Play ("Start"));
	}

	IEnumerator Play (string passageName)
	{
		while (passageName != null) {
			NarrativeEvent passage = narrative [passageName];

			Sprite backgroundImage = null;
			if (!string.IsNullOrEmpty (passage.background) && images.TryGetValue (passage.background, out backgroundImage)) {
				background.SetBackground (backgroundImage);
			}

			foreach (Dialogue dialogue in passage.dialogue) {
				Sprite image = null;
				if (string.IsNullOrEmpty (dialogue.portrait)) {
					image = portrait.sprite;
				} else if (dialogue.portrait == "none") {
					image = null;
				} else if (images.TryGetValue (dialogue.portrait, out image)) {
				}

				portrait.sprite = image;
				portrait.enabled = image != null;

				dialogueBox.text = string.Empty;

				speakerName.text = dialogue.name;

				if (dialogue.affection != 0) {
					Character character = null;
					if (!characters.TryGetValue (dialogue.name, out character)) {
						character = new Character ();
						character.name = dialogue.name;
						characters.Add (character.name, character);
						Debug.LogFormat ("Adding character {0}", character.name);
					}

					character.affection += dialogue.affection;
				}

				foreach (char letter in dialogue.dialogueText) {
					dialogueBox.text += letter;
					yield return null;
				}

				yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
	
				yield return null;
			}

			passageName = null;

			if (passage.options != null && passage.options.Count > 0) {
				bool selected = false;
				menu.SetChoices (passage.options, characters, (nextPassage) => {
					passageName = nextPassage;
					selected = true;
				});
				yield return new WaitUntil (() => selected);
			}
		}
	}
}

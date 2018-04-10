using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class Story : MonoBehaviour
{
	public Background background;
	public Image portrait;
	public Text speakerName;
	public Text dialogueBox;
	public Text date;
	public ChoiceMenu menu;
	public bool debugLoad = false;
	public int time = 0;
	public Slider affectionSlider;
	public AudioSource musicPlayer;

	Dictionary<string, NarrativeEvent> narrative = new Dictionary<string, NarrativeEvent> ();
	Dictionary<string, Sprite> images = new Dictionary<string, Sprite> ();
	Dictionary<string, AudioClip> music = new Dictionary<string, AudioClip>();
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

		foreach (AudioClip track in Resources.LoadAll<AudioClip>("Sounds")) {
			if (debugLoad) {
				Debug.LogFormat ("Loaded music {0}", track.name);
			}
			music [track.name] = track;
		}

		StartCoroutine (Play ("FacultyOffice"));
	}

	string LoadGame()
	{
		string loadPosition = null;
		SaveGame game = FindObjectOfType<SaveGame> ();
		if (game != null) {
			SaveData data = game.saveData;
			loadPosition = data.position;
			time = data.time;
			foreach (Character character in data.characters) {
				characters.Add (character.name, new Character(character));
			}
		}
		return loadPosition;
	}

	public SaveData CreateSaveData(string jumpTo)
	{
		SaveData gameData = new SaveData ();
		gameData.position = jumpTo;
		gameData.time = time;
		foreach (var entry in characters) {
			gameData.characters.Add (new Character(entry.Value));
		}
		return gameData;
	}

	IEnumerator Play (string passageName)
	{
		string loadPosition = LoadGame();
		if (loadPosition != null) {
			passageName = loadPosition;
		}

		while (passageName != null) {
			NarrativeEvent passage = narrative [passageName];

			Sprite backgroundImage = null;
			if (!string.IsNullOrEmpty (passage.background) && images.TryGetValue (passage.background, out backgroundImage)) {
				background.SetBackground (backgroundImage);
			}

			AudioClip track = null;
			if (!string.IsNullOrEmpty (passage.music) && music.TryGetValue (passage.music, out track)) {
				musicPlayer.Stop ();
				musicPlayer.clip = track;
				musicPlayer.Play ();
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

				Character character = null;
				if (!characters.TryGetValue (dialogue.name, out character)) {
					character = new Character (dialogue.name);
					characters.Add (character.name, character);
					Debug.LogFormat ("Adding character {0}", character.name);
				}

				if (dialogue.affection != 0) {
					character.affection += dialogue.affection;
				}

				affectionSlider.value = character.affection;

				time += dialogue.time;

				DateTime date = new DateTime (2017, 4, 1)  + new TimeSpan (6 * time, 0, 0);
				string timeOfDay = "Morning";
				switch (time % 4) {
				case 0: 
					timeOfDay = "Morning";
					break;
				case 1:
					timeOfDay = "Afternoon";
					break;
				case 2:
					timeOfDay = "Evening";
					break;
				case 3:
					timeOfDay = "Night";
					break;
				}
				this.date.text = string.Format ("{0}/{1} {2}", date.Day, date.Month, timeOfDay);

				bool skip = false;
				foreach (char letter in dialogue.dialogueText) {
					dialogueBox.text += letter;
					if (!skip) {
						yield return null;
						if (Input.GetMouseButtonUp (0)) {
							skip = true;
						}
					}
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

		SceneManager.LoadScene ("Credits");
	}
}

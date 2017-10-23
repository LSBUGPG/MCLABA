using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelConfig : MonoBehaviour {

	public bool CharacterIsTalking;

	public Image AvatarImage;
	public Image TextBox;
	public Text characterName;
	public Text dialogue;

	private Color maskActiveColor = new Color (103.0f/255.0f, 101.0f/255.0f, 101.0f/255.0f);

	public void ToggleCharacterMask(){
		if (CharacterIsTalking) {
			AvatarImage.color = Color.white;
			TextBox.color = Color.white;
		} else {
			AvatarImage.color = maskActiveColor;
			TextBox.color = maskActiveColor;
		}

	}

	public void Configure (Dialogue currentDialogue){
		ToggleCharacterMask ();

		AvatarImage.sprite = MasterManager.atlasManager.loadSprite (currentDialogue.atlasImageName);
		characterName.text = currentDialogue.name;

		if (CharacterIsTalking) {
			StartCoroutine (AnimateText (currentDialogue.dialogueText));
		} else {
			dialogue.text = "";
		}	
	}

	IEnumerator AnimateText(string dialogueText) {

		foreach (char letter in dialogueText) {
			dialogue.text += letter;
			yield return new WaitForSeconds (0.05f);
		}
	
	}
}

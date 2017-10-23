using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NarrativeEvent {
	public List<Dialogue> dialogues;
}

[System.Serializable]
public struct Dialogue{

	public CharacterType characterType;
	public string name;
	public string atlasImageName;
	public string dialogueText;
}

[System.Serializable]
public enum CharacterType{
	Takeshi, Lucci, Rob
}





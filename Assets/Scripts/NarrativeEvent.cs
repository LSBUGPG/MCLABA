using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NarrativeEvent
{
    public string background;
	public string music;
    public List<Dialogue> dialogue;
    public List<DialogueOption> options;
}

[System.Serializable]
public struct DialogueOption
{
    public string choiceText;
	public string requiredCharacter;
	public int requiredAffection;
    public string jumpTo;
	public string specialAction;
}

[System.Serializable]
public struct Dialogue
{
    public string name;
    public string portrait;
	public int affection;
	public int time;
    public string dialogueText;
}

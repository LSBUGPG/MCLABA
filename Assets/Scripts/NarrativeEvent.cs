using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NarrativeEvent
{
    public string background;
    public List<Dialogue> dialogue;
    public List<DialogueOption> options;
}

[System.Serializable]
public struct DialogueOption
{
    public string choiceText;
    public string jumpTo;
}

[System.Serializable]
public struct Dialogue
{
    public string name;
    public string panel;
    public string atlasImageName;
    public string dialogueText;
}

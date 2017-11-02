using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story : MonoBehaviour
{
    public Background background;
    public CharacterPanel left;
    public CharacterPanel right;
    public ChoiceMenu menu;
    public bool debugLoad = false;

    Dictionary<string, NarrativeEvent> narrative = new Dictionary<string, NarrativeEvent>();
    Dictionary<string, Sprite> images = new Dictionary<string, Sprite>();

    void Start()
    {
        foreach (TextAsset asset in Resources.LoadAll<TextAsset>("Story"))
        {
            NarrativeEvent passage = JsonUtility.FromJson<NarrativeEvent>(asset.text);
            if (passage != null)
            {
                if (debugLoad)
                {
                    Debug.LogFormat("Loaded story passage {0}", asset.name);
                }
                narrative[asset.name] = passage;
            }
            else
            {
                Debug.LogWarningFormat("Could not load story source text '{0}'", asset.name);
            }
        }

        foreach (Sprite sprite in Resources.LoadAll<Sprite>("Images"))
        {
            if (debugLoad)
            {
                Debug.LogFormat("Loaded image {0}", sprite.name);
            }
            images[sprite.name] = sprite;
        }

        StartCoroutine(Play("Start"));
    }

    IEnumerator Play(string passageName)
    {
        while (passageName != null)
        {
            NarrativeEvent passage = narrative[passageName];

            Sprite backgroundImage = null;
            if (!string.IsNullOrEmpty(passage.background) && images.TryGetValue(passage.background, out backgroundImage))
            {
                background.SetBackground(backgroundImage);
            }

            CharacterPanel panel = left;
            foreach (Dialogue dialogue in passage.dialogue)
            {
                if (dialogue.panel == "left")
                {
                    panel = left;
                }
                else if (dialogue.panel == "right")
                {
                    panel = right;
                }

                Sprite avatarImage = null;
                if (!string.IsNullOrEmpty(dialogue.atlasImageName))
                {
                    images.TryGetValue(dialogue.atlasImageName, out avatarImage);
                }

                yield return panel.Dialogue(avatarImage, dialogue.name, dialogue.dialogueText);

                yield return null;
            }

            passageName = null;

            if (passage.options != null && passage.options.Count > 0)
            {
                bool selected = false;
                menu.SetChoices(passage.options, (nextPassage) => { passageName = nextPassage; selected = true; } );
                yield return new WaitUntil(() => selected);
            }
        }
    }
}

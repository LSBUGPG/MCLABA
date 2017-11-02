using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ChoiceMenu : MonoBehaviour
{
    public Button choicePrefab;

    public void SetChoices(List<DialogueOption> choices, System.Action<string> jumpTo)
    {
        foreach (DialogueOption choice in choices)
        {
            Button button = Instantiate(choicePrefab, transform);
            Text text = button.GetComponentInChildren<Text>();
            text.text = choice.choiceText;
            button.onClick.AddListener(() => { ClearChoices(); jumpTo(choice.jumpTo); });
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

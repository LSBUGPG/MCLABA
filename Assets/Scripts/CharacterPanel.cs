using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour
{
    public Animator animator;
    public Image profile;
    public Image box;
    public Text characterName;
    public Text dialogue;
    Color inactiveColor = new Color (103.0f/255.0f, 101.0f/255.0f, 101.0f/255.0f);

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public Coroutine Dialogue(Sprite avatar, string name, string dialogue)
    {
        return StartCoroutine(PlayDialogue(avatar, name, dialogue));
    }

    IEnumerator PlayDialogue(Sprite avatar, string name, string dialogue)
    {
        this.dialogue.text = string.Empty;

        if (!string.IsNullOrEmpty(characterName.text) && characterName.text != name)
        {
            animator.SetBool("IntroAnimationIn", false);
            yield return new WaitForSeconds(1);
            characterName.text = null;
        }

        if (avatar != null)
        {
            profile.sprite = avatar;
        }

        box.color = Color.white;
        profile.color = Color.white;

        if (string.IsNullOrEmpty(characterName.text) && !string.IsNullOrEmpty(name))
        {
            characterName.text = name;
            animator.SetBool("IntroAnimationIn", true);
            yield return new WaitForSeconds(1);
        }

        foreach (char letter in dialogue) 
        {
            this.dialogue.text += letter;
            yield return null;
        }


        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));

        box.color = inactiveColor;
        profile.color = inactiveColor;
    }
}

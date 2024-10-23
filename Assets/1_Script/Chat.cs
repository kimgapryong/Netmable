using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Chat : MonoBehaviour
{
    public Dialogue dialogue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            DialogueManager.Instance.StartDialogue(dialogue, GetComponent<Collider2D>());
        }
    }
}


[System.Serializable]
public class DialogueCharacter
{
    public string name;
    public Sprite icon;
}

[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(3, 10)]
    public string line;
    public bool isEvent = false;
    public UnityEvent<DialogueLine> onEvent;
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}


using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    //aaaaa
    public DialogueNode startNode; // The starting node for this trigger

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<DialogueManager>().StartDialogue(startNode);
        }
    }
}
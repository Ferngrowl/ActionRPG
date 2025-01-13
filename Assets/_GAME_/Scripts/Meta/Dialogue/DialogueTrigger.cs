using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueNode startNode; // The starting node for this trigger
    private DialogueManager dialogueManager;
    private bool playerInRange = false; // Tracks if the player is in range of the trigger
    private bool canStartDialogue = true; // Prevents immediate re-triggering after dialogue ends

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    private void Update()
    {
        // Start dialogue if player presses space, is in range, and dialogue is not already active
        if (playerInRange && Input.GetKeyDown(KeyCode.Space) && !dialogueManager.isDialogueActive && canStartDialogue)
        {
            dialogueManager.StartDialogue(startNode);
            canStartDialogue = false; // Prevent immediate re-triggering
        }

        // Reset the ability to start dialogue after it ends
        if (!dialogueManager.isDialogueActive && !canStartDialogue)
        {
            canStartDialogue = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            // End the dialogue if active when the player leaves the trigger
            if (dialogueManager.isDialogueActive)
            {
                dialogueManager.EndDialogue();
            }
        }
    }
}

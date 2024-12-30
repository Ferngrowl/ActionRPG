using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    //aaa
    public GameObject dialogueBox; // The dialogue UI panel
    public TextMeshProUGUI dialogueText; // Text UI for displaying the dialogue
    public GameObject optionsContainer; // Container for option buttons
    public GameObject optionButtonPrefab; // Prefab for option buttons

    private DialogueNode currentNode; // The active dialogue node
    private bool isDialogueActive = false;

    // Start the dialogue with a given node
    public void StartDialogue(DialogueNode startNode)
    {
        currentNode = startNode;
        isDialogueActive = true;
        dialogueBox.SetActive(true);
        DisplayNode(currentNode);

        // Check if the node has options
        if (currentNode.options != null && currentNode.options.Count > 0)
        {
            CreateOptions(currentNode);
        }
        else
        {
            // If no options, wait for player input to close dialogue
            StartCoroutine(WaitForPlayerToCloseDialogue());
        }
    }

    // Stop the dialogue
    public void EndDialogue()
    {
        isDialogueActive = false;
        dialogueBox.SetActive(false);
        ClearOptions();
    }

    // Display the current node
    private void DisplayNode(DialogueNode node)
    {
        dialogueText.text = node.dialogueText;
        ClearOptions();
        CreateOptions(node);
    }

    // Create buttons for the node's options
    private void CreateOptions(DialogueNode node)
    {
        for (int i = 0; i < node.options.Count; i++)
        {
            int optionIndex = i; // Capture the index for the button
            GameObject buttonObj = Instantiate(optionButtonPrefab, optionsContainer.transform);
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = node.options[i];
            buttonObj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => OnOptionSelected(optionIndex));
        }
    }

    // Handle option selection
    private void OnOptionSelected(int index)
    {
        if (currentNode.nextNodes[index] != null)
        {
            StartDialogue(currentNode.nextNodes[index]);
        }
        else
        {
            EndDialogue(); // End the dialogue if no next node exists
        }
    }

    // Clear all options from the container
    private void ClearOptions()
    {
        foreach (Transform child in optionsContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private IEnumerator WaitForPlayerToCloseDialogue()
    {
        // Wait for the player to press Space
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }

        EndDialogue(); // Close the dialogue
    }
}

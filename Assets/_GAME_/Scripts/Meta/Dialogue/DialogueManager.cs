using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject dialogueBox; // The dialogue UI element
    public TextMeshProUGUI dialogueText; // Text UI for displaying dialogue
    public GameObject optionsContainer; // Container for option buttons
    public GameObject optionButtonPrefab; // Prefab for option buttons

    [Header("Typing Effect")]
    public float typewriterSpeed = 0.005f; // Speed for typewriter effect

    private DialogueNode currentNode; // The active dialogue node
    private List<string> dialoguePages = new List<string>(); // Pages of dialogue text for the current node
    private int currentPageIndex; // Tracks the current page index
    private Coroutine typingCoroutine; // Reference to the active typing coroutine
    private bool isTyping; // Tracks if the typewriter effect is active
    public bool isDialogueActive; // Tracks if dialogue is active

    // Start the dialogue with a given node
    public void StartDialogue(DialogueNode startNode)
    {
        currentNode = startNode;
        isDialogueActive = true;
        dialogueBox.SetActive(true);
        PrepareDialoguePages(currentNode.dialogueText); // Split the node's text into pages
        DisplayPage();
    }

    // End the dialogue
    public void EndDialogue()
    {
        isDialogueActive = false;
        dialogueBox.SetActive(false);
        ClearOptions();
    }

    // Prepare dialogue pages by splitting text into manageable chunks
    private void PrepareDialoguePages(string text)
    {
        dialoguePages.Clear();
        currentPageIndex = 0;

        int characterLimit = 99; // Character limit per page
        while (text.Length > 0)
        {
            int length = Mathf.Min(characterLimit, text.Length);
            int lastSpace = text.LastIndexOf(' ', length - 1);

            // Avoid splitting words midway
            if (lastSpace > 0 && length < text.Length)
                length = lastSpace;

            dialoguePages.Add(text.Substring(0, length).Trim());
            text = text.Substring(length).TrimStart();
        }
    }

    // Display the current page of dialogue with typewriter effect
    private void DisplayPage()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(dialoguePages[currentPageIndex]));
    }

    // Typewriter effect coroutine
    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = ""; // Clear text box

        foreach (char c in text)
        {
            dialogueText.text += c; // Add one character at a time
            yield return new WaitForSeconds(typewriterSpeed);
        }

        isTyping = false;
    }

    // Advance through the pages of the current node
    private void AdvanceDialogue()
    {
        if (isTyping)
        {
            // Skip the typewriter effect if it's active
            StopCoroutine(typingCoroutine);
            dialogueText.text = dialoguePages[currentPageIndex];
            isTyping = false;
            return;
        }

        currentPageIndex++;

        if (currentPageIndex < dialoguePages.Count)
        {
            DisplayPage();
        }
        else if (currentNode.options != null && currentNode.options.Count > 0)
        {
            CreateOptions();
        }
        else
        {
            EndDialogue();
        }
    }

    // Create buttons for the node's options
    private void CreateOptions()
    {
        ClearOptions(); // Ensure no leftover buttons

        for (int i = 0; i < currentNode.options.Count; i++)
        {
            int optionIndex = i; // Capture the index for the button
            GameObject buttonObj = Instantiate(optionButtonPrefab, optionsContainer.transform);

            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentNode.options[i];

            Button button = buttonObj.GetComponent<Button>();
            button.onClick.AddListener(() => OnOptionSelected(optionIndex));
        }
    }

    // Handle option selection
    private void OnOptionSelected(int index)
    {
        ClearOptions();

        if (currentNode.nextNodes != null && index < currentNode.nextNodes.Count && currentNode.nextNodes[index] != null)
        {
            StartDialogue(currentNode.nextNodes[index]);
        }
        else
        {
            EndDialogue();
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

    // Update for advancing dialogue with space key
    private void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space) && optionsContainer.transform.childCount == 0)
        {
            AdvanceDialogue();
        }
    }
}

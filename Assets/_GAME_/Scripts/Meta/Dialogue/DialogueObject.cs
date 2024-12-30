using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueObject : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox; // The dialogue UI element
    [SerializeField] private TextMeshProUGUI dialogueTextUI; // The TextMeshPro UI element for displaying dialogue
    [TextArea(3, 10)] [SerializeField] private string fullDialogue; // The full dialogue text entered in the Inspector

    private int characterLimit = 99; // Maximum number of characters per page
    private List<string> dialoguePages = new List<string>(); // Holds split dialogue pages
    private int currentPageIndex = 0; // Tracks the current page
    private bool showDialogue = false; // Tracks if player is in range

    void Start()
    {
        dialogueBox.SetActive(false); // Ensure dialogue box starts hidden
        PrepareDialogue(fullDialogue); // Split the full dialogue into pages
    }

    void Update()
    {
        if (showDialogue && Input.GetKeyDown(KeyCode.Space))
        {
            if (dialogueBox.activeInHierarchy)
            {
                // Advance to next page or close dialogue
                AdvanceDialogue();
            }
            else
            {
                // Open dialogue box and start dialogue
                dialogueBox.SetActive(true);
                currentPageIndex = 0;
                DisplayPage();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered range");
            showDialogue = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left range");
            showDialogue = false;
            dialogueBox.SetActive(false);
        }
    }

    private void PrepareDialogue(string fullDialogue)
    {
        dialoguePages.Clear();
        
        while (fullDialogue.Length > 0)
        {
            int length = Mathf.Min(characterLimit, fullDialogue.Length);
            string chunk = fullDialogue.Substring(0, length);

            // Avoid splitting words mid-way
            int lastSpace = chunk.LastIndexOf(' ');
            if (lastSpace > 0 && length < fullDialogue.Length)
            {
                chunk = fullDialogue.Substring(0, lastSpace);
                length = lastSpace + 1; // Include the space
            }

            dialoguePages.Add(chunk.Trim());
            fullDialogue = fullDialogue.Substring(length).TrimStart();
        }
    }

    private void DisplayPage()
    {
        // Display the full page instantly
        dialogueTextUI.text = dialoguePages[currentPageIndex];
    }

    private void AdvanceDialogue()
    {
        currentPageIndex++;
        if (currentPageIndex < dialoguePages.Count)
        {
            DisplayPage();
        }
        else
        {
            CloseDialogue();
        }
    }

    private void CloseDialogue()
    {
        dialogueBox.SetActive(false);
    }
}

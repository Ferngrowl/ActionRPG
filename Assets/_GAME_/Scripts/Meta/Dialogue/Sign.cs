using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Sign : MonoBehaviour
{
    public GameObject dialogueBox; // The dialogue UI element
    public TextMeshProUGUI dialogueTextUI; // The TextMeshPro UI element for displaying dialogue
    [TextArea(3, 10)] public string fullDialogue; // The full dialogue text entered in the Inspector
    
    
    private int characterLimit = 100; // Maximum number of characters per page
    private float typewriterSpeed = 0.005f; // Speed for typewriter effect
    private List<string> dialoguePages = new List<string>(); // Holds split dialogue pages
    private int currentPageIndex = 0; // Tracks the current page
    private bool showDialogue = false; // Tracks if player is in range
    private bool isTyping = false; // Tracks if typewriter effect is active
    private Coroutine typingCoroutine; // Reference to the active typing coroutine

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
                if (isTyping)
                {
                    // Skip typewriter effect
                    StopCoroutine(typingCoroutine);
                    dialogueTextUI.text = dialoguePages[currentPageIndex];
                    isTyping = false;
                }
                else
                {
                    // Advance to next page or close dialogue
                    AdvanceDialogue();
                }
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
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(dialoguePages[currentPageIndex]));
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueTextUI.text = ""; // Clear text box
        foreach (char c in text)
        {
            dialogueTextUI.text += c; // Add one character at a time
            yield return new WaitForSeconds(typewriterSpeed);
        }
        isTyping = false;
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

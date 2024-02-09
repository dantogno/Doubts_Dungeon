using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Character { Player, Depression, DepressionBoss }

public class DialogueManager : MonoBehaviour
{
    //These are where in the UI the text values sit
    public TextMeshProUGUI NPCDialogue;
    public TextMeshProUGUI PlayerDialogue;

    //Ideally only one dialogue tree per level
    public DialogueTree DT;

    private int currentDialogueIndex = 0;
    private Coroutine currentTypewriterCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTypewriterCoroutine != null && Input.GetKeyDown(KeyCode.Space))
        {
            // Stop the typewriter effect coroutine and display the entire text immediately
            StopCurrentTypewriterCoroutine();
            DisplayFullText();
        }
    }

    public void RunDialogue()
    {
        currentDialogueIndex = 0;
        DisplayNextDialogue();
    }

    // Method to display the entire text immediately
    private void DisplayFullText()
    {
        // Get the current dialogue
        Dialogue currentDialogue = DT.dialogueLines[currentDialogueIndex];

        // Display the entire text immediately based on the character type
        switch (currentDialogue.Person)
        {
            case Character.Player:
                PlayerDialogue.text = currentDialogue.Text;
                break;
            case Character.Depression:
            case Character.DepressionBoss:
                NPCDialogue.text = currentDialogue.Text;
                break;
        }

        // Proceed to the next dialogue
        NextDialogue();
    }

    public void DisplayNextDialogue()
    {
        StopCurrentTypewriterCoroutine();

        Dialogue currentDialogue = DT.dialogueLines[currentDialogueIndex];
        switch (currentDialogue.Person)
        {
            case Character.Player:
                PlayerDialogue.text = "";
                currentTypewriterCoroutine = StartCoroutine(TypewriterEffect(PlayerDialogue, currentDialogue.Text));
                break;
            case Character.Depression:
            case Character.DepressionBoss:
                NPCDialogue.text = "";
                currentTypewriterCoroutine = StartCoroutine(TypewriterEffect(NPCDialogue, currentDialogue.Text));
                break;
        }
    }

    private IEnumerator TypewriterEffect(TextMeshProUGUI textMeshPro, string text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            textMeshPro.text += text[i];
            yield return new WaitForSeconds(0.05f); // Adjust the speed of typing here
        }

        // Optionally, you can wait for the player to press a key before proceeding to the next dialogue
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        NextDialogue();
    }

    private void NextDialogue()
    {
        currentDialogueIndex++;
        if (currentDialogueIndex < DT.dialogueLines.Count)
        {
            DisplayNextDialogue();
        }
        else
        {
            // Dialogue ends here
            Debug.Log("End of dialogue.");
        }
    }

    private void StopCurrentTypewriterCoroutine()
    {
        if (currentTypewriterCoroutine != null)
        {
            StopCoroutine(currentTypewriterCoroutine);
            currentTypewriterCoroutine = null;
        }
    }
}

public class DialogueTree
{
    public List<Dialogue> dialogueLines = new List<Dialogue>();

    //Has a limit
    public List<string> dialogueResponses1 = new List<string>();
    public List<string> dialogueResponses2 = new List<string>();
    public List<string> dialogueResponses3 = new List<string>();
}

public class Dialogue
{
    public string Text;
    public Character Person;
    public bool respondable;
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public GameObject dialoguePanel;

    public float typingSpeed = 0.05f;

    private List<DialogueLine> dialogLines;
    private int currentLineIndex;
    private bool isTyping;
    private Coroutine typingCoroutine;
    private Action onDialogEnd;

    private PlayerController player;

    private bool isActive = false;

    void Update()
    {
        if (!isActive) return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                dialogueText.text = dialogLines[currentLineIndex].text;
                isTyping = false;
            }
            else
            {
                NextLine();
            }
        }
    }

    public void SetupDialogue(List<DialogueLine> lines, PlayerController playerRef, Action onEnd = null)
    {
        dialogLines = lines;
        player = playerRef;
        onDialogEnd = onEnd;

        currentLineIndex = 0;
        isActive = true;

        if (player != null)
            player.canMove = false;

        dialoguePanel.SetActive(true);
        ShowLine();
    }

    private void ShowLine()
    {
        if (currentLineIndex >= dialogLines.Count)
        {
            EndDialog();
            return;
        }

        nameText.text = dialogLines[currentLineIndex].actorName;
        typingCoroutine = StartCoroutine(TypeText(dialogLines[currentLineIndex].text));
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in text)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void NextLine()
    {
        currentLineIndex++;
        ShowLine();
    }

    private void EndDialog()
    {
        dialoguePanel.SetActive(false);
        isActive = false;

        if (player != null)
            player.canMove = true;

        onDialogEnd?.Invoke();
    }
}

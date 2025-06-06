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

    [Header("Typing Settings")]
    public float typingSpeed = 0.03f;

    private List<DialogueLine> currentLines;
    private int currentLineIndex;
    private bool isTyping;
    private Coroutine typingCoroutine;

    private PlayerController player;
    private Action onDialogEnd;
    private bool isActive;

    void Update()
    {
        if (!isActive) return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                dialogueText.text = currentLines[currentLineIndex].text;
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
        if (lines == null || lines.Count == 0)
        {
            Debug.LogWarning("No dialogue lines provided.");
            return;
        }

        currentLines = lines;
        currentLineIndex = 0;
        player = playerRef;
        onDialogEnd = onEnd;
        isActive = true;

        if (player != null)
            player.canMove = false;

        dialoguePanel.SetActive(true);
        ShowLine();
    }

    private void ShowLine()
    {
        dialoguePanel.SetActive(true); // Garante visibilidade mesmo se desativado antes

        nameText.text = currentLines[currentLineIndex].actorName;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(currentLines[currentLineIndex].text));
    }

    private IEnumerator TypeText(string text)
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

        if (currentLineIndex >= currentLines.Count)
        {
            EndDialog();
            return;
        }

        ShowLine();
    }

    private void EndDialog()
    {
        isActive = false;
        dialoguePanel.SetActive(false);

        if (player != null)
            player.canMove = true;

        onDialogEnd?.Invoke();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueIntroManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public GameObject dialoguePanel;

    public List<DialogueLine> dialogLines;

    public float typingSpeed;

    public PlayerController player;

    private int currentLineIndex;
    private bool isTyping;
    private Coroutine typingCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        dialoguePanel.SetActive(true);
        StartDialog();
    }

    // Update is called once per frame
    void Update()
    {
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

    public void StartDialog()
    {
        currentLineIndex = 0;

        if(player != null)
        {
            player.canMove = false;
        }

        ShowLine();
    }

    private void ShowLine()
    {
        if(currentLineIndex >= dialogLines.Count)
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

        foreach(char letter in text)
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

        if(player != null)
        {
            player.canMove = true;
        }
    }
}

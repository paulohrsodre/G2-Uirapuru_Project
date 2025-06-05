using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalDialogTirgger : MonoBehaviour
{
    public DialogueManager dialogManager;
    public List<DialogueLine> finalLines;
    public PlayerController player;

    private bool triggered = false;

    private void Start()
    {
        if (dialogManager == null)
        {
            dialogManager = FindObjectOfType<DialogueManager>();
        }

        if (player == null)
        {
            player = FindObjectOfType<PlayerController>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered || !collision.CompareTag("Player"))
        {
            return;
        }

        triggered = true;

        dialogManager.SetupDialogue(finalLines, player, () => StartCoroutine(LoadNextSceneAfterDelay()));
    }

    private IEnumerator LoadNextSceneAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalDialogTirgger : MonoBehaviour
{
    public List<DialogueLine> finalLines;

    public DialogueManager dialogManager;
    public PlayerController player;
    private bool triggered = false;

    private void Awake()
    {
        // Tenta encontrar automaticamente o DialogueManager e o PlayerController na cena
        dialogManager = FindObjectOfType<DialogueManager>();
        player = FindObjectOfType<PlayerController>();

        if (dialogManager == null)
            Debug.LogError("DialogueManager não encontrado na cena.");

        if (player == null)
            Debug.LogError("PlayerController não encontrado na cena.");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (triggered || dialogManager == null || player == null)
            return;

        if (collision.CompareTag("Player"))
        {
            triggered = true;

            // Inicia o diálogo final e chama a troca de cena ao fim
            dialogManager.SetupDialogue(finalLines, player, () => StartCoroutine(LoadNextSceneAfterDelay()));
        }
    }

    private IEnumerator LoadNextSceneAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0); // Mude para o nome ou índice da próxima cena
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
  
    public void EndGame()
    {

            Debug.Log("Exit");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
        Aplication.OpenURL("https://paulohrsodre.itch.io/a-lenda-do-uirapuru");
#else
            Application.Quit();
#endif
        


    }
}

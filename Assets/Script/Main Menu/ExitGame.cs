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
        Aplication.OpenURL("https://google.com.br/");
#else
            Application.Quit();
#endif
        


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Dialog", menuName = "Dialog/New Dialog", order = 1)]
public class DialogueLine : ScriptableObject
{
    public string actorName;

    [TextArea(2, 5)]
    public string text;   
}

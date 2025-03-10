using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "UI/DIalogue")]
public class Dialogue : ScriptableObject
{
    [SerializeField][TextArea] private string[] dialogue;

    public string[] dialogueSO => dialogue;
}

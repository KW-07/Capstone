using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "DialogueSO/Dialogue")]
public class DialogueSO : ScriptableObject
{
    [SerializeField][TextArea] private string[] dialogue;
    //[SerializeField] private string otherName;
    //[SerializeField] private Sprite otherSprite;
    //bool isTurnPlayer;

    public string[] Dialogue => dialogue;
    //public string OtherName => otherName;
    //public Sprite OtherSprite => otherSprite;
    //public bool IsTurnPlayer => isTurnPlayer;
}

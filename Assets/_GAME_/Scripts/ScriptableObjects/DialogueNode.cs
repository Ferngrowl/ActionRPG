using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueNode", menuName = "Dialogue System/Dialogue Node")]
public class DialogueNode : ScriptableObject
{
    //aaa
    [TextArea(3, 10)] public string dialogueText; // Text displayed in this node
    public List<string> options; // The text for each option
    public List<DialogueNode> nextNodes; // The nodes corresponding to each option

}
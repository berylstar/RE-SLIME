using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Dialogue")]
public class DialogueData : ScriptableObject
{
    [Serializable]
    public struct DialogueStruct
    {
        public Sprite img;
        public string talker;
        [Multiline(3)]
        public string talk;
    }

    public List<DialogueStruct> dialogues = new List<DialogueStruct>();
}

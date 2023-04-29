using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RPG.Dialogue
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] string text;
        [SerializeField] List<string> children = new List<string>();
        [SerializeField] Rect rect = new Rect(0, 0, 200, 200);

        // Getters 
        public string GetText()
        {
            return text;
        }
        public List<string> GetChildren()
        {
            return children;
        }
        public Rect GetRect()
        {
            return rect;
        }
        // Setters
        public void UpdateText(string newText)
        {
            if (text != newText)
            {
                Undo.RecordObject(this, "Update Dialogue Text");
                text = newText;
            }
        }
        public void AddChild(string nodeName)
        {
            Undo.RecordObject(this, "Add Dialogue Link");
            children.Add(nodeName);
        }
        public void RemoveChild(string nodeName)
        {
            Undo.RecordObject(this, "Remove Dialogue Link");
            children.Remove(nodeName);
        }
#if UNITY_EDITOR
        public void SetRectPosition(Vector2 newPosition)
        {
            Undo.RecordObject(this, "Move Dialogue Node");
            rect.position = newPosition;
        }
#endif
    }
}
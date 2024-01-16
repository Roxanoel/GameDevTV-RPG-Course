using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using RPG.Core;

namespace RPG.Dialogue
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] bool isPlayerSpeaking = false; // If there was more than 2 possibilities, use enum
        [SerializeField] string text;
        [SerializeField] List<string> children = new List<string>();
        [SerializeField] Rect rect = new Rect(0, 0, 200, 200);
        [SerializeField] DialogueTriggerType onEnterAction;
        [SerializeField] DialogueTriggerType onExitAction; // Could be extended to an array if I wanted many actions
        [SerializeField] Condition condition;

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

        public bool IsPlayerSpeaking()
        {
            return isPlayerSpeaking;
        }

        public DialogueTriggerType GetOnEnterAction()
        {
            return onEnterAction;
        }

        public DialogueTriggerType GetOnExitAction()
        {
            return onExitAction;
        }
        // Setters
        public void UpdateText(string newText)
        {
            if (text != newText)
            {
                Undo.RecordObject(this, "Update Dialogue Text");
                text = newText;
                EditorUtility.SetDirty(this);
            }
        }
        public void AddChild(string nodeName)
        {
            Undo.RecordObject(this, "Add Dialogue Link");
            children.Add(nodeName);
            EditorUtility.SetDirty(this);
        }
        public void RemoveChild(string nodeName)
        {
            Undo.RecordObject(this, "Remove Dialogue Link");
            children.Remove(nodeName);
            EditorUtility.SetDirty(this);
        }
        public void SetIsPlayerSpeaking(bool value)
        {
            Undo.RecordObject(this, "Change dialogue speaker");
            isPlayerSpeaking = value;
            EditorUtility.SetDirty(this);
        }
        public bool CheckCondition(IEnumerable<IPredicateEvaluator> evaluators)
        {
            return condition.Check(evaluators);
        }

#if UNITY_EDITOR
        public void SetRectPosition(Vector2 newPosition)
        {
            Undo.RecordObject(this, "Move Dialogue Node");
            rect.position = newPosition;
            EditorUtility.SetDirty(this);
        }
#endif
    }
}
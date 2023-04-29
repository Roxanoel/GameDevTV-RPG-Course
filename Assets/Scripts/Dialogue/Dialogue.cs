using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] List<DialogueNode> nodes = new List<DialogueNode>();

        Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();


#if UNITY_EDITOR        
        // Called whenever the SO is loaded, in the Editor only 
        private void Awake()
        {

        }
#endif
        // For it to work in a fully exported game, OnValidate() will need to be called from Awake()
        private void OnValidate()
        {
            nodeLookup.Clear();
            foreach (DialogueNode node in GetAllNodes() )
            {
                nodeLookup[node.name] = node;
            }
        }

        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }

        public DialogueNode GetRootNode()
        {
            return nodes[0];
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
        {
            foreach (string childID in parentNode.children)
            {
                if (nodeLookup.ContainsKey(childID))
                {
                    yield return nodeLookup[childID];
                }
            }
        }

        public void CreateNode(DialogueNode parent)
        {
            DialogueNode newNode = CreateInstance<DialogueNode>();
            newNode.name = Guid.NewGuid().ToString();

            // Registering for undo
            Undo.RegisterCreatedObjectUndo(newNode, "Created new dialogue node");

            if (parent != null)
            {
                parent.children.Add(newNode.name);
            }
            Undo.RecordObject(this, "Added Dialogue Node");
            nodes.Add(newNode);
            OnValidate();
        }

        public void DeleteNode(DialogueNode nodeToDelete)
        {
            Undo.RecordObject(this, "Deleted Dialogue Node");
            nodes.Remove(nodeToDelete);
            OnValidate();

            // Removes dangling children
            foreach(DialogueNode node in GetAllNodes())
            {
                node.children.Remove(nodeToDelete.name);
            }
            // For undo
            Undo.DestroyObjectImmediate(nodeToDelete);

        }

        public void OnBeforeSerialize()
        {
            // Before saving, ensure there is always at least one node
            if (nodes.Count < 1)
            {
                CreateNode(null);
            }

            // Check if this dialogue exists as an asset yet
            if (AssetDatabase.GetAssetPath(this) != "")
            {
                foreach (DialogueNode node in GetAllNodes())
                {
                    if (AssetDatabase.GetAssetPath(node) == "")
                    {
                        // Register new node in the asset database (for saving properly)
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
        }

        public void OnAfterDeserialize()
        {

        }
    }
}

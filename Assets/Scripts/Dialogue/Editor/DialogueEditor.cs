using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPG.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        Dialogue selectedDialogue = null;

        Vector2 scrollPosition;
        const float canvasSize = 4000;
        const float backgroundSize = 50;

        [NonSerialized] DialogueNode draggingNode = null;
        [NonSerialized] Vector2 draggingOffset;
        [NonSerialized] DialogueNode creatingNode = null;
        [NonSerialized] DialogueNode deletingNode = null;
        [NonSerialized] DialogueNode linkingParentNode = null;
        [NonSerialized] bool draggingCanvas = false;
        [NonSerialized] Vector2 draggingCanvasOffset;

        [NonSerialized] GUIStyle nodeStyle;
        [NonSerialized] GUIStyle playerNodeStyle;
        
        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }
        
        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;

            if(dialogue != null)
            {
                ShowEditorWindow();
                return true;
            }
            return false;
        }
        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;

            // Style for the NPC nodes
            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            nodeStyle.padding = new RectOffset(24, 24, 24, 24);
            nodeStyle.border = new RectOffset(12, 12, 12, 12);
            // Style for the player nodes
            playerNodeStyle = new GUIStyle();
            playerNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
            playerNodeStyle.padding = new RectOffset(24, 24, 24, 24);
            playerNodeStyle.border = new RectOffset(12, 12, 12, 12);
        }

        private void OnSelectionChanged()
        {
            Dialogue newSelectedDialogue = Selection.activeObject as Dialogue;
            if (newSelectedDialogue != null)
            {
                selectedDialogue = newSelectedDialogue;
                Repaint();
            }
        }

        private void OnGUI()
        {
            if (selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No dialogue selected");
            }
            else
            {
                ProcessEvents();

                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

                // Gives us a scrollable area even if nodes are not auto-layout
                Rect canvas = GUILayoutUtility.GetRect(canvasSize, canvasSize);
                // Background texture
                Texture2D backgroundTexture = Resources.Load("background") as Texture2D;
                Rect textureCoords = new Rect(0, 0, canvasSize / backgroundSize, canvasSize / backgroundSize);
                GUI.DrawTextureWithTexCoords(canvas, backgroundTexture, textureCoords);

                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawNode(node);
                }
                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawConnections(node);
                }

                EditorGUILayout.EndScrollView();

                if (creatingNode != null)
                {
                    selectedDialogue.CreateNode(creatingNode);
                    creatingNode = null;
                }
                if (deletingNode != null)
                {
                    selectedDialogue.DeleteNode(deletingNode);
                    deletingNode = null;
                }
            }

        }

        private void ProcessEvents()
        {
            if (Event.current.type == EventType.MouseDown && draggingNode == null)
            {
                // Dragging a node
                draggingNode = GetNodeAtPoint(Event.current.mousePosition);
                if (draggingNode != null)
                {
                    draggingOffset = draggingNode.GetRect().position - Event.current.mousePosition;
                    // Changing the selection
                    Selection.activeObject = draggingNode;
                }
                // Dragging the canvas 
                else
                {
                    draggingCanvas = true;
                    draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
                    // Changing the selection
                    Selection.activeObject = selectedDialogue;
                }
            }
            else if (Event.current.type == EventType.MouseDrag && draggingNode != null)
            {
                draggingNode.SetRectPosition(Event.current.mousePosition + draggingOffset);

                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseDrag && draggingCanvas)
            {
                scrollPosition = draggingCanvasOffset - Event.current.mousePosition;

                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseUp && draggingNode != null)
            {
                draggingNode = null;
            }
            else if (Event.current.type == EventType.MouseUp && draggingCanvas)
            {
                draggingCanvas = false;
            }
        }

        private void DrawNode(DialogueNode node)
        {
            // Determining style to use 
            GUIStyle style = nodeStyle; // defaults to NPC node style
            if (node.IsPlayerSpeaking() == true)
            {
                style = playerNodeStyle;
            }
            // Apply correct style
            GUILayout.BeginArea(node.GetRect(), style);
            
            EditorGUILayout.LabelField("ID");
            EditorGUILayout.TextField($"{node.name}");
            EditorGUILayout.LabelField("Text");
            node.UpdateText(EditorGUILayout.TextField($"{node.GetText()}"));

            if (GUILayout.Button("Add Child Node"))
            {
                creatingNode = node;
            }

            if (linkingParentNode == null)
            {
                if (GUILayout.Button("Link to Child"))
                {
                    linkingParentNode = node;
                }
            } 
            else
            {
                if (linkingParentNode == node)
                {
                    if (GUILayout.Button("Cancel"))
                    {
                        linkingParentNode = null;
                    }
                }
                else if  (linkingParentNode.GetChildren().Contains(node.name))
                {
                    if (GUILayout.Button("Unlink Child"))
                    {
                        linkingParentNode.RemoveChild(node.name);
                        linkingParentNode = null;
                    }
                }
                else if
                (GUILayout.Button("Select as Child"))
                {
                    linkingParentNode.AddChild(node.name);
                    linkingParentNode = null;
                }
            }


            if (GUILayout.Button("Delete"))
            {
                deletingNode = node;
            }

            GUILayout.EndArea();
        }

        private void DrawConnections(DialogueNode node)
        {
            Vector3 startPosition = new Vector3(node.GetRect().xMax, node.GetRect().center.y, 0);

            foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node))
            {
                Vector3 endPosition = new Vector3(childNode.GetRect().xMin, childNode.GetRect().center.y, 0);
                Vector3 controlPointOffset = endPosition - startPosition;
                controlPointOffset.y = 0;
                controlPointOffset.x *= 0.8f;
                Handles.DrawBezier(
                    startPosition, 
                    endPosition, 
                    startPosition + controlPointOffset, 
                    endPosition-controlPointOffset, 
                    Color.white, null, 4f);
            }
        }
        private DialogueNode GetNodeAtPoint(Vector2 point)
        {
            DialogueNode foundNode = null;

            Vector2 adjustedPoint = new Vector2();
            adjustedPoint = point + scrollPosition;

            foreach (DialogueNode node in selectedDialogue.GetAllNodes())
            {
                if (node.GetRect().Contains(adjustedPoint))
                {
                    // Ensures the node that will be returned is the top-most one
                    foundNode = node;
                }
            }
            return foundNode;
        }
    }
}


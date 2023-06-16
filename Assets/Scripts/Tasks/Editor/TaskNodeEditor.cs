using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace Tasks.Editor
{
    [CustomNodeEditor(typeof(TaskSegment))]
    public class TaskNodeEditor : NodeEditor
    {
        public override void OnBodyGUI()
        {
            serializedObject.Update();

            TaskSegment currentSegment = serializedObject.targetObject as TaskSegment;
            NodeEditorGUILayout.PortField(currentSegment.GetPort("input"));

            GUILayout.Label("ID");
            currentSegment.id = EditorGUILayout.IntField(currentSegment.id);

            GUILayout.Label("Description");
            currentSegment.description = GUILayout.TextArea(currentSegment.description, new GUILayoutOption[]
            {
                GUILayout.MinHeight(50),
            });

            NodeEditorGUILayout.DynamicPortList(
                "CompletionPaths",
                typeof(TaskGoal),
                serializedObject,
                NodePort.IO.Output,
                Node.ConnectionType.Override,
                Node.TypeConstraint.None,
                OnCreateReorderableList
            );

            foreach (NodePort dynamicPort in target.DynamicPorts)
            {
                if(NodeEditorGUILayout.IsDynamicPortListPort(dynamicPort)) continue;
                NodeEditorGUILayout.PortField(dynamicPort);
            }

            serializedObject.ApplyModifiedProperties();
        }
        internal void OnCreateReorderableList(ReorderableList list)
        {
            list.elementHeightCallback = (int index) => { return 200; };

            // Override drawHeaderCallback to display node's name instead
            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                TaskSegment segment = serializedObject.targetObject as TaskSegment;
                NodePort port = segment.GetPort("CompletionPaths " + index);

                segment.CompletionPaths[index] = (segment.CompletionPaths[index] == null) ? new TaskGoal() : segment.CompletionPaths[index];
                TaskGoal currentGoal = segment.CompletionPaths[index];

                int guiSpace = 20; // spacer for height
                int guiWidth = 150;

                GUI.Label(
                    new Rect(
                        rect.x, 
                        rect.y, 
                        guiWidth, 
                        EditorGUIUtility.singleLineHeight
                    ), 
                    "Goal Type"
                );
                currentGoal.goalType = (GoalType)EditorGUI.EnumPopup(
                    new Rect(
                        rect.x, 
                        rect.y + guiSpace, 
                        guiWidth, 
                        EditorGUIUtility.singleLineHeight
                    ), 
                    currentGoal.goalType
                );
                GUI.Label(
                    new Rect(
                        rect.x,
                        rect.y + guiSpace * 2,
                        guiWidth,
                        EditorGUIUtility.singleLineHeight
                    ), 
                    "Target"
                );
                currentGoal.target = GUI.TextField(
                    new Rect(
                        rect.x,
                        rect.y + guiSpace * 3,
                        guiWidth,
                        EditorGUIUtility.singleLineHeight
                    ), 
                    currentGoal.target
                );
                GUI.Label(
                    new Rect(
                        rect.x,
                        rect.y + guiSpace * 4,
                        guiWidth,
                        EditorGUIUtility.singleLineHeight
                    ), 
                    "Required Amount"
                );
                currentGoal.requiredAmount = EditorGUI.IntField(
                    new Rect(
                        rect.x,
                        rect.y + guiSpace * 5,
                        guiWidth,
                        EditorGUIUtility.singleLineHeight
                    ),
                    currentGoal.requiredAmount
                );
                if (currentGoal.goalType != GoalType.Hidden)
                {
                    GUI.Label(
                        new Rect(
                            rect.x,
                            rect.y + guiSpace * 6,
                            guiWidth,
                            EditorGUIUtility.singleLineHeight
                        ), 
                        "Accept Text"
                    );
                    currentGoal.acceptText = GUI.TextArea(
                        new Rect(
                            rect.x,
                            rect.y + guiSpace * 7,
                            guiWidth,
                            EditorGUIUtility.singleLineHeight * 3
                        ), 
                        currentGoal.acceptText
                    );
                }
                else
                {
                    GUI.Label(
                        new Rect(
                            rect.x,
                            rect.y + guiSpace * 6,
                            guiWidth,
                            EditorGUIUtility.singleLineHeight
                        ), 
                        "Hidden Text"
                    );
                    currentGoal.hiddenText = GUI.TextArea(
                        new Rect(
                            rect.x,
                            rect.y + guiSpace * 7,
                            guiWidth,
                            EditorGUIUtility.singleLineHeight * 3
                        ), 
                        currentGoal.hiddenText
                    );
                }

                if(port != null)
                {
                    Vector2 pos = rect.position + 
                                    (port.IsOutput ? new Vector2(rect.width + 6, 0) : new Vector2 (-36, 0));
                                    
                    NodeEditorGUILayout.PortField(pos, port);
                }
            };
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Tasks;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace Dialog.Editor
{
    [CustomNodeEditor(typeof(DialogSegment))]
    public class DialogNodeEditor : NodeEditor
    {
        public override void OnBodyGUI()
        {
            serializedObject.Update();

            DialogSegment currentSegment = serializedObject.targetObject as DialogSegment;
            
            NodeEditorGUILayout.PortField(currentSegment.GetPort("input"));

            GUILayout.Label("Dialog Text");
            currentSegment.DialogText = GUILayout.TextArea(currentSegment.DialogText, new GUILayoutOption[]
            {
                GUILayout.MinHeight(50),
            });

            NodeEditorGUILayout.DynamicPortList(
                "Answers",
                typeof(string),
                serializedObject,
                NodePort.IO.Input,
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
            list.elementHeightCallback = (int index) => { return 60; };

            // Override drawHeaderCallback to display node's name instead
            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                DialogSegment segment = serializedObject.targetObject as DialogSegment;

                NodePort port = segment.GetPort("Answers " + index);

                segment.Answers[index] = GUI.TextArea(rect, segment.Answers[index]);

                if(port != null)
                {
                    Vector2 pos = rect.position + 
                                    (port.IsOutput ? new Vector2(rect.width + 6, 0) : new Vector2 (-36, 0));
                                    
                    NodeEditorGUILayout.PortField(pos, port);
                }
            };
        }
    }
    [CustomNodeEditor(typeof(ReceiveTaskSegment))]
    public class ReceiveTaskNodeEditor : DialogNodeEditor
    {
        public override void OnBodyGUI()
        {
            serializedObject.Update();

            ReceiveTaskSegment currentSegment = serializedObject.targetObject as ReceiveTaskSegment;
            NodeEditorGUILayout.PortField(currentSegment.GetPort("input"));
            GUILayout.Label("Task");
            currentSegment.task = EditorGUILayout.ObjectField(currentSegment.task, typeof(TaskSegment), false) as TaskSegment;

            GUILayout.Label("Rejection Text");
            currentSegment.rejectText = GUILayout.TextArea(currentSegment.rejectText, new GUILayoutOption[]
            {
                GUILayout.MinHeight(50),
            });

            NodeEditorGUILayout.DynamicPortList(
                "Answers",
                typeof(string),
                serializedObject,
                NodePort.IO.Input,
                Node.ConnectionType.Override,
                Node.TypeConstraint.None,
                OnCreateReorderableList
            );

            foreach (NodePort dynamicPort in target.DynamicPorts)
            {
                if(NodeEditorGUILayout.IsDynamicPortListPort(dynamicPort)) continue;
                NodeEditorGUILayout.PortField(dynamicPort);
            }
        }
    }
}

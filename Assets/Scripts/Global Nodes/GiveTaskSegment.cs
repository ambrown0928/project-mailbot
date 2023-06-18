using System.Collections;
using System.Collections.Generic;
using Tasks;
using UnityEngine;
using XNode;

namespace GlobalNodes
{
	[System.Serializable]
	public struct Connection { }
	public class GiveTaskSegment : Node 
	{
		[Input] public Connection input;

		public TaskGraph task;

		[Output] public Connection output;
		// Return the correct value of an output port when requested
		public override object GetValue(NodePort port) {
			return null; // Replace this
		}
		public Node AttemptToAddToLog()
		{
			TaskController.AddToLog(task);
			return GetPort("output").Connection.node;
		}
	}
}
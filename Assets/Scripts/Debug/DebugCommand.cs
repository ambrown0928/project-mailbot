using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Debugging
{
    public class DebugCommandBase
    {
        private string _commandId;
        private string _commandDescription;
        private string _commandFormat;

        public DebugCommandBase(string commandId, string commandDescription, string commandFormat)
        {
            this.commandId = commandId;
            this.commandDescription = commandDescription;
            this.commandFormat = commandFormat;
        }

        public string commandId { get => _commandId; set => _commandId = value; }
        public string commandDescription { get => _commandDescription; set => _commandDescription = value; }
        public string commandFormat { get => _commandFormat; set => _commandFormat = value; }
    }
    public class DebugCommand : DebugCommandBase
    {
        private Action command;

        public DebugCommand(string id, string description, string format, Action command) : base(id, description, format)
        {
            this.command = command;
        }

        public void Invoke()
        {
            command.Invoke();
        }
    }
    public class DebugCommand<T> : DebugCommandBase
    {
        private Action<T> command;

        public DebugCommand(string id, string description, string format, Action<T> command) : base(id, description, format)
        {
            this.command = command;
        }

        public void Invoke(T value)
        {
            command.Invoke(value);
        }
    }
    public class DebugCommand<T, U> : DebugCommandBase
    {
        private Action<T, U> command;

        public DebugCommand(string id, string description, string format, Action<T, U> command) : base(id, description, format)
        {
            this.command = command;
        }

        public void Invoke(T val1, U val2)
        {
            command.Invoke(val1, val2);
        }
    }
}
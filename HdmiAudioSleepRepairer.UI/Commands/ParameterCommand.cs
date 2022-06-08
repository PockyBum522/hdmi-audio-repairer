using System;
using System.Windows.Input;
using JetBrains.Annotations;

namespace HdmiAudioSleepRepairer.UI.Commands
{
    public class ParameterCommand : ICommand
    {
        private readonly Action<object> _exec;

        public ParameterCommand(Action<object> exec)
        {
            _exec = exec;
        }
        
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (parameter is not null)
                _exec.Invoke(parameter);
        }
        
        
        public event EventHandler? CanExecuteChanged { add{} remove{} }
    }
}
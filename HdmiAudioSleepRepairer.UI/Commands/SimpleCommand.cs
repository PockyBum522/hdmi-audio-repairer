using System;
using System.Windows.Input;
using JetBrains.Annotations;

namespace HdmiAudioSleepRepairer.UI.Commands
{
    [PublicAPI]
    public class SimpleCommand : ICommand
    {
        private readonly Action _exec;

        public SimpleCommand(Action exec)
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
                _exec.Invoke();
        }
        
        public event EventHandler? CanExecuteChanged { add{} remove{} }
    }
}
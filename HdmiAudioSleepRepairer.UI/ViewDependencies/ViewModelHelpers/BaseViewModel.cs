using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace HdmiAudioSleepRepairer.UI.ViewDependencies.ViewModelHelpers
{
    /// <summary>
    /// All view models should inherit from this, helps with MVVM
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Fires when Property Changed, for MVVM
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Called from properties that need to notify that they have changed
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Helper for setting backing fields of properties and also notifying that property has change at the same time
        /// </summary>
        /// <param name="backingField">Backing field to update</param>
        /// <param name="value">New value to set backing field to</param>
        /// <param name="propertyName">Exposed property that is updating</param>
        /// <typeparam name="T">Type of backing field and property</typeparam>
        protected void SetValue<T>(ref T backingField, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value))
                return;

            backingField = value;
            
            OnPropertyChanged(propertyName);
        }
    }
}
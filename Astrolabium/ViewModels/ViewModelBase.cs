using System;
using System.ComponentModel;

namespace Astrolabium.ViewModels
{
    /// <summary>
    /// Base of ViewModel
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {

        /// <summary>
        /// Raise a PropertyChanged event
        /// </summary>
        /// <param name="propertyName">Name of the property changed, or empty if all properties are changed</param>
        protected virtual void OnPropertyChanged(String propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Event raised when a property changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

    }

}

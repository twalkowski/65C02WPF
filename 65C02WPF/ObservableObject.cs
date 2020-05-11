using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace _65C02WPF
{
    /// <summary>
    /// A class to simplify the setters for data context proerties bound to UI properties. It implements the INotifyProertyChagned inteface 
    /// </summary>
    class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// method to raise the PropertChanged event
        /// </summary>
        /// <param name="propertyName">the propert needindg to raise the event</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); 
        }

        /// <summary>
        /// Method to set the backing field of a property - if the value has changed - and raise the ProertyChanged event
        /// </summary>
        /// <typeparam name="T">the property type</typeparam>
        /// <param name="field">the property's backing field</param>
        /// <param name="newValue">the new value</param>
        /// <param name="propertyName">the name of the property.  Of omitted, it is the name of the calling member</param>
        /// <returns></returns>
        protected bool Set<T>(
            ref T field,
            T newValue,
            [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }

            field = newValue;
            RaisePropertyChanged(propertyName);
            return true;
        }
    }
}

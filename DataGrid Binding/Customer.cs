using System.ComponentModel;

namespace BindingBasics
{
    /// <summary>
    /// Sample model
    /// </summary>
    public class Customer : INotifyPropertyChanged
    {
        /// <summary>
        /// The event that is fired when any child property changes its value
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// Call this to fire a <see cref="PropertyChanged"/> event
        /// </summary>
        /// <param name="name">The name of the property</param>
        public void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Sample property
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Sample property
        /// </summary>
        public string MoneySpent { get; set; }

        /// <summary>
        /// Indicates if this class instance is selected
        /// </summary>
        public bool IsItemSelected { get; set; }
    }
}

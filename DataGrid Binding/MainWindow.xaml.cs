using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace BindingBasics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
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

        #region Public Properties

        /// <summary>
        /// Data collection
        /// </summary>
        public ObservableCollection<Customer> Customers { get; set; } = new ObservableCollection<Customer>();

        /// <summary>
        /// Currently selected cell (preserved despite lost focus)
        /// </summary>
        public DataGridCellInfo DataGridSelectedCell { get; set; }

        /// <summary>
        /// The name of the property of a class instance inside a selected cell
        /// </summary>
        public string CurrentCustomerSelectedPropertyName => DataGridSelectedCell.Column != null ? (string)DataGridSelectedCell.Column?.Header : string.Empty;

        /// <summary>
        /// The property of a class instance inside a selected cell
        /// </summary>
        public PropertyInfo CustomerPropertyInfo => mCustomerType.GetProperty(CurrentCustomerSelectedPropertyName);

        /// <summary>
        /// The class instance inside a selected cell
        /// </summary>
        public Customer CurrentCustomer
        {
            get
            {
                // Loop through all Customers...
                foreach (var item in Customers)
                    // deselecting each of them
                    item.IsItemSelected = false;

                // Get a Customer class instance from the selected cell
                Customer customer = DataGridSelectedCell.Item as Customer;

                // If the class instance is obtained...
                if (customer != null)
                    // select it
                    customer.IsItemSelected = true;

                // Return the class instance
                return customer;
            }
        }

        /// <summary>
        /// The value of the property of a class instance inside a selected cell
        /// </summary>
        public object CurrentCustomerSelectedPropertyValue
        {
            get { return CustomerPropertyInfo?.GetValue(CurrentCustomer); }
            set { CustomerPropertyInfo?.SetValue(CurrentCustomer, value); }
        }

        /// <summary>
        /// Indicates whether the textbox has a negative value
        /// </summary>
        public bool IsCellTextNegative => CurrentCustomerSelectedPropertyValue != null && CurrentCustomerSelectedPropertyValue.ToString().Contains("-");

        #endregion

        /// <summary>
        /// <see cref="Customer"/> as <see cref="Type"/>
        /// </summary>
        private readonly Type mCustomerType = typeof(Customer);

        public MainWindow()
        {
            InitializeComponent();

            // Fill data collection with sample data
            Random rnd = new Random();

            for (int i = 0; i < 5; i++)
            {
                Customers.Add(new Customer { Name = $"Customer{i}", MoneySpent = $"{ rnd.Next(-5, 5) }$" });
            }

            // Set Data Context to this Window, so that its properties could be bound to
            DataContext = this;
        }

        private void DataGrid_SelectedCellsChanged(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e)
        {
            // Get the sender as a DataGrid
            DataGrid dataGrid = sender as DataGrid;

            // If at least 1 cell is selected...
            if (dataGrid.SelectedCells.Count > 0)
                // fill the appropriate property with the first selected cell
                DataGridSelectedCell = dataGrid.SelectedCells[0];
        }
    }
}

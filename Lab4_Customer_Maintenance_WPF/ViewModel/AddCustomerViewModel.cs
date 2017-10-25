using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Lab4_Customer_Maintenance_WPF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Lab4_Customer_Maintenance_WPF.ViewModel
{
    public class AddCustomerViewModel: ViewModelBase
    {
        bool validFlag = false;
        private string customerNameTextBox;
        private string customerAddressTextBox;
        private string customerCityTextBox;
        private ObservableCollection<State> stateList;
        private State selectedState;
        private string customerZipTextBox;
        private Customer customer;

        public RelayCommand AcceptCommand { get; set; }
        public RelayCommand<Window> AcceptCloseCommand { get; private set; }
        public RelayCommand<Window> CancelCommand { get; private set; }
        public AddCustomerViewModel()
        {
            validFlag = false;
            // Use a Linq query to get all the states from the state table.
            try
            {
                // Code a query to retrieve the required information from
                // the States table, and sort the results by state name.
                var states = (from state in MMABooksEntity.MMABooks.States
                              orderby state.StateName
                              select state).ToList();
                
                // Bind the State combo box to the query results.
                StateList = new ObservableCollection<State>(states);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().ToString());
            }
            AcceptCommand = new RelayCommand(AcceptCommandAction);
            AcceptCloseCommand = new RelayCommand<Window>(AcceptCloseCommandAction);
            CancelCommand = new RelayCommand<Window>(CancelCommandAction);
        }

        private void AcceptCommandAction()
        {
            if (IsValidData())
            {
                validFlag = true;
                customer = new Customer
                {
                    Name = customerNameTextBox,
                    Address = customerAddressTextBox,
                    City = customerCityTextBox,
                    State = selectedState.StateCode,
                    ZipCode = customerZipTextBox
                };
                // Add the new vendor to the collection of vendors in the DBSET
                // DBSET is the Database: MMABooks / Collection object: Customers
                MMABooksEntity.MMABooks.Customers.Add(customer);
                try
                {
                    // Doing something...
                    // Try and save to the database (write).
                    MMABooksEntity.MMABooks.SaveChanges();
                    MessageBox.Show("Customer ID: " + customer.CustomerID + " has been added");
                    var newMessage = new AddToHomeMessage();
                    newMessage.CustomerContext = customer;
                    Messenger.Default.Send<AddToHomeMessage>(newMessage);
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                                ve.PropertyName,
                                eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                                ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            }
        }

        private void AcceptCloseCommandAction(Window obj)
        {
            if (validFlag && null != obj)
            {
                obj.Close();
                ClearControls();
            }
        }

        private void CancelCommandAction(Window window)
        {
            if (null != window)
            {
                window.Close();
                ClearControls();
            }
        }

        private void ClearControls()
        {
            CustomerNameTextBox = "";
            CustomerAddressTextBox = "";
            CustomerCityTextBox = "";
            selectedState = null;
            CustomerZipTextBox = "";
        }

        private bool IsValidData()
        {
            return  Validator.IsPresent(customerNameTextBox, "Name") &&
                    Validator.IsPresent(customerAddressTextBox, "Address") &&
                    Validator.IsPresent(customerCityTextBox, "City") &&
                    Validator.IsPresent(selectedState.StateName, "State") &&
                    Validator.IsPresent(customerZipTextBox, "Zip") &&
                    Validator.IsInt32(customerZipTextBox);
        }

        public string CustomerNameTextBox
        {
            get { return customerNameTextBox; }
            set
            {
                customerNameTextBox = value;
                RaisePropertyChanged("CustomerNameTextBox");
            }
        }
        public string CustomerAddressTextBox
        {
            get { return customerAddressTextBox; }
            set
            {
                customerAddressTextBox = value;
                RaisePropertyChanged("CustomerAddressTextBox");
            }
        }
        public string CustomerCityTextBox
        {
            get { return customerCityTextBox; }
            set
            {
                customerCityTextBox = value;
                RaisePropertyChanged("CustomerCityTextBox");
            }
        }
        public ObservableCollection<State> StateList
        {
            get { return stateList; }
            set
            {
                stateList = value;
                RaisePropertyChanged("StateList");
            }
        }
        public State SelectedState
        {
            get { return selectedState; }
            set
            {
                selectedState = value;
                RaisePropertyChanged("SelectedState");
            }
        }
        public string CustomerZipTextBox
        {
            get { return customerZipTextBox; }
            set
            {
                customerZipTextBox = value;
                RaisePropertyChanged("CustomerZipTextBox");
            }
        }
        
    }
}

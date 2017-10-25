using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Lab4_Customer_Maintenance_WPF.Model;
using Lab4_Customer_Maintenance_WPF.View;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab4_Customer_Maintenance_WPF.ViewModel
{
    public class CustomerMaintenanceViewModel: ViewModelBase
    {
        private Customer selectedCustomer;
        private int customerIDTextBox;
        private string customerNameTextBox;
        private string customerAddressTextBox;
        private string customerCityTextBox;
        private string customerStateTextBox;
        private string customerZipTextBox;
        private bool turnOn;
        private bool deleteOption;

        public RelayCommand GetCustomerCommand { get; set; }
        public RelayCommand AddCommand { get; set; }
        public RelayCommand ModifyCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand<Window> DeleteYesCommand { get; set; }
        public RelayCommand<Window> DeleteNoCommand { get; set; }
        public RelayCommand<Window> ExitCommand { get; private set; }

        public CustomerMaintenanceViewModel()
        {
            GetCustomerCommand = new RelayCommand(GetCustomerCommandAction);
            AddCommand = new RelayCommand(AddCommandAction);
            ModifyCommand = new RelayCommand(ModifyCommandAction);
            DeleteCommand = new RelayCommand(DeleteCommandAction);
            DeleteYesCommand = new RelayCommand<Window>(DeleteYesCommandAction);
            DeleteNoCommand = new RelayCommand<Window>(DeleteNoCommandAction);
            ExitCommand = new RelayCommand<Window>(ExitCommandAction);
            Messenger.Default.Register<AddToHomeMessage>(this, onReceiveAddMessage);
            TurnOn = false;
            Messenger.Default.Register<Customer>(this, "modifyReload", (customer) =>
            {
                GetCustomer(customer.CustomerID);
            });
            Messenger.Default.Register<Customer>(this, "deleteReload", (customer) =>
            {
                ClearControls();
            });

        }

        private void GetCustomerCommandAction()
        {
            if (Validator.IsPresent(CustomerIDTextBox.ToString(), "Missing ID") && Validator.IsInt32(CustomerIDTextBox.ToString()))
            {
                GetCustomer(customerIDTextBox);
            }
        }

        private void GetCustomer(int CustomerID)
        {
            try
            {
                // Code a query to retrieve the selected customer
                // and store the Customer object in the class variable.
                // SingleorDefault allows for a null value
                SelectedCustomer = (from customer in MMABooksEntity.MMABooks.Customers
                                    where customer.CustomerID == CustomerID
                                    select customer).SingleOrDefault();

                // Check if we pulled a null value from the query.
                if (SelectedCustomer == null)
                {
                    MessageBox.Show("No customer found with this ID. " +
                        "Please try again.", "Customer Not Found");
                    this.ClearControls();
                }
                else
                {
                    //  If the customer is found, add code to the GetCustomer method that checks if the State object
                    // has been loaded and that loads if it hasn't.
                    if (!MMABooksEntity.MMABooks.Entry(
                        SelectedCustomer).Reference("State1").IsLoaded)
                        MMABooksEntity.MMABooks.Entry(
                            SelectedCustomer).Reference("State1").Load();
                    this.DisplayCustomer();
                    TurnOn = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().ToString());
            }

        }

        private void AddCommandAction()
        {
            var newView = new AddCustomerView();
            newView.ShowDialog();
        }

        private void onReceiveAddMessage(AddToHomeMessage message)
        {
            if (null != message)
            {
                SelectedCustomer = message.CustomerContext;
                CustomerIDTextBox = SelectedCustomer.CustomerID;
                CustomerNameTextBox = SelectedCustomer.Name;
                CustomerAddressTextBox = SelectedCustomer.Address;
                CustomerCityTextBox = SelectedCustomer.City;
                CustomerStateTextBox = SelectedCustomer.State;
                CustomerZipTextBox = SelectedCustomer.ZipCode;
                TurnOn = true;
            }
        }

        private void DeleteCommandAction()
        {
            // Delete the entry here.
            if(null != SelectedCustomer)
            {
                try
                {
                    var deleteView = new DeleteCustomerView();
                    deleteView.ShowDialog();
                    Console.WriteLine(deleteOption);
                    if (deleteOption)
                    {
                        //Mark the row for deletion
                        MMABooksEntity.MMABooks.Customers.Remove(SelectedCustomer);
                        MMABooksEntity.MMABooks.SaveChanges();
                        MessageBox.Show("Customer ID: " + SelectedCustomer.CustomerID 
                            + " was Removed!", "Removal Success");
                        ClearControls();
                    }
                }
                
                catch (DbUpdateConcurrencyException ex)
                {
                    ex.Entries.Single().Reload();
                    // You have been deleted.
                    if (MMABooksEntity.MMABooks.Entry(SelectedCustomer).State == EntityState.Detached)
                    {
                        MessageBox.Show("Your entry for ID: " + selectedCustomer.CustomerID +
                            " has already been deleted by a different user.", "Concurrency Error");
                        this.ClearControls();

                    }
                    // You have been modified.
                    else
                    {
                        MessageBox.Show("Your entry for ID: " + selectedCustomer.CustomerID +
                            " has been modified by a different user.", "Concurrency Error");
                        this.DisplayCustomer();
                    }
                }
                
                catch(DbUpdateException ex)
                {
                    MessageBox.Show(ex.Message, "Update Exception");
                }
                
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "General Exception");
                }
            }
            else
            {
                MessageBox.Show("Please search for a customer before you " +
                    "attempt to delete.", "No Selected Customer");
            }
        }

        private void ModifyCommandAction()
        {
            if(null != selectedCustomer)
            {
                var newView = new ModifyCustomerView();
                Messenger.Default.Send(selectedCustomer, "HometoModify");
                newView.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please search for a customer before you " +
                    "attempt to modify.", "No Selected Customer");
            }
        }

        private void ClearControls()
        {
            CustomerIDTextBox = 0;
            CustomerNameTextBox = "";
            CustomerAddressTextBox = "";
            CustomerCityTextBox = "";
            CustomerStateTextBox = "";
            CustomerZipTextBox = "";
            TurnOn = false;
        }

        private void DisplayCustomer()
        {
            // take all of the values from the selected customer and display them
            CustomerNameTextBox = selectedCustomer.Name;
            CustomerAddressTextBox = selectedCustomer.Address;
            CustomerCityTextBox = selectedCustomer.City;
            CustomerStateTextBox = selectedCustomer.State1.StateCode;
            CustomerZipTextBox = selectedCustomer.ZipCode;
            Console.WriteLine(customerNameTextBox);
            Console.WriteLine(customerAddressTextBox);
            Console.WriteLine(customerCityTextBox);
            Console.WriteLine(customerStateTextBox);
            Console.WriteLine(customerZipTextBox);
        }

        private void ExitCommandAction(Window window)
        {
            if (null != window)
            {
                window.Close();
            }
        }

        private void DeleteNoCommandAction(Window window)
        {
            if (null != window)
            {
                deleteOption = false;
                window.Close();
            }

        }

        private void DeleteYesCommandAction(Window window)
        {
            if (null != window)
            {
                deleteOption = true;
                window.Close();
            }
        }

        public Customer SelectedCustomer
        {
            get { return selectedCustomer; }
            set
            {
                selectedCustomer = value;
                RaisePropertyChanged("SelectedCustomer");
            }
        }

        public int CustomerIDTextBox
        {
            get { return customerIDTextBox; }
            set
            {
                customerIDTextBox = value;
                RaisePropertyChanged("CustomerIDTextBox");
            }
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
        public string CustomerStateTextBox
        {
            get { return customerStateTextBox; }
            set
            {
                customerStateTextBox = value;
                RaisePropertyChanged("CustomerStateTextBox");
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
        public bool TurnOn
        {
            get { return turnOn; }
            set
            {
                turnOn = value;
                RaisePropertyChanged("TurnOn");
            }
        }
    }
}

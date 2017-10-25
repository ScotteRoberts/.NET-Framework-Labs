using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Lab4_Test.Model;
using Lab4_Test.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab4_Test.ViewModel
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
        public RelayCommand GetCustomerCommand { get; set; }
        public RelayCommand AddCommand { get; set; }
        public RelayCommand ModifyCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand<Window> ExitCommand { get; private set; }

        public CustomerMaintenanceViewModel()
        {
            GetCustomerCommand = new RelayCommand(GetCustomerCommandAction);
            AddCommand = new RelayCommand(AddCommandAction);
            ModifyCommand = new RelayCommand(ModifyCommandAction);
            DeleteCommand = new RelayCommand(DeleteCommandAction);
            ExitCommand = new RelayCommand<Window>(ExitCommandAction);

        }

        private void ExitCommandAction(Window window)
        {
            if (null != window)
            {
                window.Close();
            }
        }

        private void DeleteCommandAction()
        {
            // Delete the entry here.
            /*
             try
             {
                //ark the row for deletion
                MMABooksEntity.mmaBooks.Customers.Remove(selectedCustomer);
                MMABooksEntity.mmaBooks.SaveChanges();
                Messenger.Default.Send(new NotificationMessage("Customer Removed!");

                customerID = "";

                catch(DBConcurrencyException)
                {
                }
                catch()
                {
                }


             */ 
        }

        private void ModifyCommandAction()
        {
            var newView = new ModifyCustomerView();
            // Messenger.Default.Send(selectedCustomer, "CustomerToEdit");
            // CustomerToEdit 
            newView.ShowDialog();
        }

        private void AddCommandAction()
        {
            var newView = new AddCustomerView();
            newView.ShowDialog();
        }

        private void GetCustomerCommandAction()
        {
           // if (validator.ispresent(customerID) && validator.isInt32(customerID))
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
                selectedCustomer = (from customer in MMABooksEntity.MMABooks.Customers
                                    where customer.CustomerID == CustomerID
                                    select customer).SingleOrDefault();

                // Check if we pulled a null value from the query.
                if (selectedCustomer == null)
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
                        selectedCustomer).Reference("State").IsLoaded)
                        MMABooksEntity.MMABooks.Entry(
                            selectedCustomer).Reference("State").Load();
                    this.DisplayCustomer();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().ToString());
            }

        }

        private void ClearControls()
        {
            throw new NotImplementedException();
        }

        private void DisplayCustomer()
        {
            // take all of the values from the selected customer and display them
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
                RaisePropertyChanged("CustomerNameTextBox");
            }
        }
        public string CustomerCityTextBox
        {
            get { return customerCityTextBox; }
            set
            {
                customerCityTextBox = value;
                RaisePropertyChanged("CustomerNameTextBox");
            }
        }
        public string CustomerStateTextBox
        {
            get { return customerStateTextBox; }
            set
            {
                customerStateTextBox = value;
                RaisePropertyChanged("CustomerNameTextBox");
            }
        }
        public string CustomerZipTextBox
        {
            get { return customerZipTextBox; }
            set
            {
                customerZipTextBox = value;
                RaisePropertyChanged("CustomerNameTextBox");
            }
        }
    }
}

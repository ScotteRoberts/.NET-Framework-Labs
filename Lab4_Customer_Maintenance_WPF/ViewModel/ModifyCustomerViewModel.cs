using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Lab4_Customer_Maintenance_WPF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Lab4_Customer_Maintenance_WPF.ViewModel
{
    public class ModifyCustomerViewModel : ViewModelBase
    {
        bool validFlag = false;
        private string customerNameTextBox;
        private string customerAddressTextBox;
        private string customerCityTextBox;
        private ObservableCollection<State> stateList;
        private State selectedState;
        private string customerZipTextBox;
        private Customer selectedCustomer;

        public RelayCommand AcceptCommand { get; set; }
        public RelayCommand<Window> AcceptCloseCommand { get; private set; }
        public RelayCommand<Window> CancelCommand { get; private set; }
        public ModifyCustomerViewModel()
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

            // In Constructor Message Declaration.
            Messenger.Default.Register<Customer>(this, "HometoModify", (customer) =>
            {
                try
                {
                    selectedCustomer = customer;
                    DisplayCustomer();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "General Exception");
                }
            });
        }

        private void AcceptCommandAction()
        {
            if (IsValidData())
            {
                validFlag = true;
                PutCustomerData(selectedCustomer);
               
                try
                {
                    //Try and save to the database (write)
                    MMABooksEntity.MMABooks.SaveChanges();
                    MessageBox.Show("Customer ID: " + selectedCustomer.CustomerID + " has been modified.");
                    var newMessage = new AddToHomeMessage();
                    newMessage.CustomerContext = selectedCustomer;
                    Messenger.Default.Send<AddToHomeMessage>(newMessage);
                }
                catch(DbUpdateConcurrencyException ex)
                {
                    ex.Entries.Single().Reload();
                    if (MMABooksEntity.MMABooks.Entry(selectedCustomer).State == EntityState.Detached)
                    {
                        MessageBox.Show("Your entry for ID: " + selectedCustomer.CustomerID +
                            " has already been deleted by a different user.", "Concurrency Error");
                        this.ClearControls();
                        Messenger.Default.Send(selectedCustomer, "deleteReload");

                    }
                    // You have been modified.
                    else
                    {
                        Console.WriteLine("We got here baby!");
                        MessageBox.Show("Your entry for ID: " + selectedCustomer.CustomerID +
                            " has been modified by a different user.", "Concurrency Error");
                        Messenger.Default.Send(selectedCustomer, "modifyReload");
                    }
                }
                catch(DbUpdateException ex)
                {
                    MessageBox.Show("Modify Customer DbUpdateException???");
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
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "General Exception");
                }
            }
        }

        private void DisplayCustomer()
        {
            // take all of the values from the selected customer and display them
            CustomerNameTextBox = selectedCustomer.Name;
            CustomerAddressTextBox = selectedCustomer.Address;
            CustomerCityTextBox = selectedCustomer.City;
            SelectedState = (from state in MMABooksEntity.MMABooks.States
                             where selectedCustomer.State == state.StateCode
                             select state).SingleOrDefault();
            CustomerZipTextBox = selectedCustomer.ZipCode;
            Console.WriteLine(customerNameTextBox);
            Console.WriteLine(customerAddressTextBox);
            Console.WriteLine(customerCityTextBox);
            //Console.WriteLine(customerStateTextBox);
            Console.WriteLine(customerZipTextBox);
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

        private void PutCustomerData(Customer customer)
        {

            customer.Name = customerNameTextBox;
            customer.Address = customerAddressTextBox;
            customer.City = customerCityTextBox;
            customer.State = selectedState.StateCode;
            customer.ZipCode = customerZipTextBox;
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
        public Customer SelectedCustomer
        {
            get { return selectedCustomer; }
            set
            {
                selectedCustomer = value;
                RaisePropertyChanged("SelectedCustomer");
            }
        }
    }
}

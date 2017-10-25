using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Lab4_Test.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Lab4_Test.ViewModel
{
    public class AddCustomerViewModel: ViewModelBase
    {
        bool validFlag = false;
        private string customerNameTextBox;
        private string customerAddressTextBox;
        private string customerCityTextBox;
        private ObservableCollection<State> stateList;
        private string selectedState;
        private string customerZipTextBox;
        private Customer customer;

        public RelayCommand AcceptCommand { get; set; }
        public RelayCommand<Window> AcceptCloseCommand { get; private set; }
        public RelayCommand<Window> CancelCommand { get; private set; }
        public AddCustomerViewModel()
        {
            // Use a Linq query to get all the states from the state table.
            try
            {
                // Code a query to retrieve the required information from
                // the States table, and sort the results by state name.
                var states = (from state in MMABooksEntity.MMABooks.States
                              orderby state.StateName
                              select state).ToList();
                
                foreach (var state in states)
                {
                    Console.WriteLine(state);
                }
                
                
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

        private void AcceptCloseCommandAction(Window obj)
        {
            if(validFlag && null != obj)
            {
                obj.Close();
            }
        }

        private void CancelCommandAction(Window window)
        {
            if (null != window)
            {
                window.Close();
            }
        }

        private void AcceptCommandAction()
        {
            if (IsValidData())
            {

                customer = new Customer
                {
                    Name = customerNameTextBox,
                    Address = customerAddressTextBox,
                    City = customerCityTextBox,
                    State = SelectedState.ToString(),
                    ZipCode = customerZipTextBox
                };

                // Add the new vendor to the collection of vendors.
                MMABooksEntity.MMABooks.Customers.Add(customer);
                try
                {
                    //Try and save to the database (write)
                    MMABooksEntity.MMABooks.SaveChanges();
                    //this.DialogResult = DialogResult.OK;
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

        private void ClearControls()
        {
            throw new NotImplementedException();
        }

        private bool IsValidData()
        {
            return true;
        }
        private void PutCustomerData(Customer customer)
        {
            
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
        public string SelectedState
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

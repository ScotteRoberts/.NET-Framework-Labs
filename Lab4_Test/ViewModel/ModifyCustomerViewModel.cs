using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Lab4_Test.ViewModel
{
    public class ModifyCustomerViewModel : ViewModelBase
    {
        bool validFlag = false;
        private string customerNameTextBox;
        private string customerAddressTextBox;
        private string customerCityTextBox;
        private readonly CollectionView stateList;
        private string selectedState;
        private string customerZipTextBox;
        public RelayCommand AcceptCommand { get; set; }
        public RelayCommand<Window> AcceptCloseCommand { get; private set; }
        public RelayCommand<Window> CancelCommand { get; private set; }
        public ModifyCustomerViewModel()
        {
            // Use a Linq query to get all the states from the state table.
            // stateList = ;
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
            //Accept button logic
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
        public CollectionView StateList
        {
            get { return stateList; }
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

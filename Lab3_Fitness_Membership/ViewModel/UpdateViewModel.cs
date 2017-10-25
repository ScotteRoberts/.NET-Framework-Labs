using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Lab3_Fitness_Membership.Messages;
using Lab3_Fitness_Membership.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab3_Fitness_Membership.ViewModel
{
    public class UpdateViewModel : ViewModelBase
    {
        private string firstTextBox;
        private string lastTextBox;
        private string emailTextBox;
        private bool validFlag = false;
        public RelayCommand UpdateCommand { get; set; }
        public RelayCommand<Window> CloseCommand { get; private set; }
        public RelayCommand<Window> UpdateCloseCommand { get; private set; }

        public UpdateViewModel()
        {
            // Initialize all properties/methods needed.
            UpdateCommand = new RelayCommand(UpdateCommandAction);
            CloseCommand = new RelayCommand<Window>(CloseCommandAction);
            UpdateCloseCommand = new RelayCommand<Window>(UpdateCloseCommandAction);
            // Register messages from MainViewModel.
            Messenger.Default.Register<MainToUpdateMessage>(this, OnReceiveNewData);
        }

        // Get/Set
        public string FirstTextBox
        {
            get { return firstTextBox; }
            set
            {
                firstTextBox = value;
                RaisePropertyChanged("FirstTextBox");
                //Tells the view's textbox to change values when set.
            }
        }

        // Get/Set
        public string LastTextBox
        {
            get
            {
                return lastTextBox;
            }
            set
            {
                lastTextBox = value;
                RaisePropertyChanged("LastTextBox"); 
            }
        }

        // Get/Set
        public string EmailTextBox
        {
            get
            {
                return emailTextBox;
            }
            set
            {
                emailTextBox = value;
                RaisePropertyChanged("EmailTextBox");
            }
        }

        // Received message method from MainViewModel.
        private void OnReceiveNewData(MainToUpdateMessage obj)
        {
            if(obj != null)
            {
                FirstTextBox = obj.FirstText;
                LastTextBox = obj.LastText;
                EmailTextBox = obj.EmailText;
            }
        }

        // Local validate method using Validator class.
        public bool ValidateData()
        {
            validFlag = false;
            if (!Validator.isPresent(firstTextBox, "First Name")) validFlag = true;
            if (!Validator.isPresent(lastTextBox, "Last Name")) validFlag = true;
            if (!Validator.isPresent(emailTextBox, "Email")) validFlag = true;
            else
            {
                if (!Validator.IsValidEmail(emailTextBox)) validFlag = true;
            }

            if (!validFlag) return true;
            else return false;
        }

        // Method used when updatecommand is used.
        private void UpdateCommandAction()
        {
            if (ValidateData())
            {
                var messageViewModel = new MessageViewModel()//use message class
                {
                    FirstText = FirstTextBox,
                    LastText = LastTextBox,
                    EmailText = EmailTextBox
                };
                Messenger.Default.Send(messageViewModel);
            }
        }

        private void CloseCommandAction(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }

        private void UpdateCloseCommandAction(Window window)
        {
            if (!validFlag && window != null)
            {
                window.Close();
                firstTextBox = null;
                lastTextBox = null;
                emailTextBox = null;
            }

        }
    }
}

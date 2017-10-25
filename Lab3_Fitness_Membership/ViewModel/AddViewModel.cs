using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Lab3_Fitness_Membership.Messages;
using Lab3_Fitness_Membership.Model;
using Lab3_Fitness_Membership.View;
using System.Collections.ObjectModel;
using System.Windows;
using System;

namespace Lab3_Fitness_Membership.ViewModel
{
    public class AddViewModel: ViewModelBase
    {
        //Properties and Methods
        private string firstTextBox;
        private string lastTextBox;
        private string emailTextBox;
        bool validFlag = false;
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand<Window> CloseCommand { get; private set; }
        public RelayCommand<Window> SaveCloseCommand { get; set; }

        // Constructor
        public AddViewModel()
        {
            SaveCommand = new RelayCommand(SaveCommandAction);
            CloseCommand = new RelayCommand<Window>(CloseCommandAction);
            SaveCloseCommand = new RelayCommand<Window>(SaveCloseCommandAction);
        }

        // Local Validation Method that uses the Validator class.
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

        // Method Action called by SaveCommand RelayCommand.
        private void SaveCommandAction()
        {
            if (ValidateData())
            {
                // Messanger class used to transfer data from viewmodel to viewmodel.
                var messageViewModel = new MessageViewModel()
                {
                    FirstText = FirstTextBox,
                    LastText = LastTextBox,
                    EmailText = EmailTextBox
                };
                // Send data out by default... (to the mainviewmodel).
                Messenger.Default.Send(messageViewModel);
                firstTextBox = null;
                lastTextBox = null;
                emailTextBox = null;
            }
        }

        // Method Action called by CloseCommand RelayCommand.
        private void CloseCommandAction(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
            firstTextBox = null;
            lastTextBox = null;
            emailTextBox = null;
        }

        // Method Action called by SaveCloseCommand RelayCommand.
        private void SaveCloseCommandAction(Window window)
        {
            if(!validFlag && window != null)
            {
                window.Close();
                firstTextBox = null;
                lastTextBox = null;
                emailTextBox = null;
            }
            
        }

        // Get/Set
        public string FirstTextBox
        {
            get { return firstTextBox; }
            set
            {
                firstTextBox = value;
                RaisePropertyChanged("FirstTextBox");
                // Tells the textbox in the view to change values.
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
        
    }
    
}

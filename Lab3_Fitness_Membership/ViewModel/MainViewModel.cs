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
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        Member selectedMember;
        ObservableCollection<Member> memberList;
        string firstContext;
        string lastContext;
        string emailContext;
        
        public RelayCommand OnClickAddCommand { get; set; }
        public RelayCommand DeleteMemberCommand { get; set; }
        public RelayCommand EditCommand { get; set; }
        public RelayCommand<Window> CloseWindowCommand { get; private set; }

        public MainViewModel()
        {
            // Initialize all property variables.
            selectedMember = new Member();
            memberList = new ObservableCollection<Member>();
            OnClickAddCommand = new RelayCommand(AddMethod);
            DeleteMemberCommand = new RelayCommand(DeleteMethod);
            EditCommand = new RelayCommand(EditCommandAction);
            CloseWindowCommand = new RelayCommand<Window>(CloseWindow);
            memberList = MemberDB.LoadMembership();
            // Register the message coming from the AddViewModel and call "OnReceiveNewData"
            // when a message is received.
            Messenger.Default.Register<MessageViewModel>(this, OnReceiveNewData);
        }

        private void EditCommandAction()
        {
            UpdateView newView = new UpdateView();
            var message = new MainToUpdateMessage()
            {
                FirstText = selectedMember.FirstName,
                LastText = selectedMember.LastName,
                EmailText = selectedMember.Email
            };
            Messenger.Default.Send(message);
            newView.ShowDialog();

            // Make sure that all fields were entered and received from the UpdateViewModel
            if (null != firstContext && null != lastContext && null != emailContext)
            {
                // Work around code for not having the correct interaction event handler in the view.
                // If the list has a selected member, then update it.
                if(memberList.Contains(selectedMember))
                {
                    var i = memberList.IndexOf(selectedMember);
                    memberList[i] = new Member(firstContext, lastContext, emailContext);
                }
                //If the list does not have your selected member, then make a new one.
                else
                {
                    MemberList.Add(new Member(firstContext, lastContext, emailContext));
                }
                // Save to the text file.
                MemberDB.SaveMembership(memberList);
                firstContext = null;
                lastContext = null;
                emailContext = null;
            }
        }
        // Property get/set
        public Member SelectedMember
        {
            get { return selectedMember; }
            set
            {
                selectedMember = value;
                RaisePropertyChanged("SelectedMember");
            }
        }
        // Property get/set
        public ObservableCollection<Member> MemberList
        {
            get { return memberList; }
            set
            {
                memberList = value;
                RaisePropertyChanged("MemberList");
            }
        }
        // Property get/set
        public string FirstContext
        {
            get { return firstContext; }
            set
            {
                firstContext = value;
                RaisePropertyChanged("FirstContext");
            }
        }
        // Property get/set
        public string LastContext
        {
            get { return lastContext; }
            set
            {
                lastContext = value;
                RaisePropertyChanged("LastContext");
            }
        }
        // Property get/set
        public string EmailContext
        {
            get { return lastContext; }
            set
            {
                emailContext = value;
                RaisePropertyChanged("EmailContext");
            }
        }
        // Action method attached to the "OnClickAddCommand" relay command
        public void AddMethod()
        {
            // Create your new view.
            AddView newView = new AddView();
            // Show your view.
            newView.ShowDialog();

            // Make sure that all fields were entered and received from the AddViewModel.
            // If not null, then add a new member to the list and save them to the Database.
            if (null != firstContext && null != lastContext && null != emailContext)
            {
                MemberList.Add(new Member(firstContext, lastContext, emailContext));
                MemberDB.SaveMembership(memberList);
                firstContext = null;
                lastContext = null;
                emailContext = null;
            }
        }

        // Action method attached to the "DeleteMemberCommand" relay command
        public void DeleteMethod()
        {
            if(selectedMember != null)
            {
                memberList.Remove(selectedMember);
                MemberDB.SaveMembership(memberList);
                RaisePropertyChanged("MemberList");
            }
        }

        //Receiving data
        void OnReceiveNewData(MessageViewModel obj)
        {
            if(obj != null)
            {
                firstContext = obj.FirstText;
                lastContext = obj.LastText;
                emailContext = obj.EmailText;
            }
        }

        private void CloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }

    }
}
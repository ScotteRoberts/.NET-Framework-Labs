using GalaSoft.MvvmLight.Messaging;
using Lab3_Fitness_Membership.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3_Fitness_Membership.Messages
{
    class MessageViewModel : MessageBase
    {
        public string FirstText {get; set;}
        public string LastText { get; set;}
        public string EmailText { get; set;}
    }
    class MainToUpdateMessage: MessageBase
    {
        public string FirstText { get; set; }
        public string LastText { get; set; }
        public string EmailText { get; set; }
    }
    class MessageData : MessageBase
    {
        public ObservableCollection<Member> MemberList { get; set; }
    }
}

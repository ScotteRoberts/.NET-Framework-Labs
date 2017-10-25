using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lab3_Fitness_Membership.Model
{
    public class MemberDB
    {
        // Saving mechanism that saves the membership list to a textfile in local storage.
        // Requires a ObservableCollection object parameter.
        public static void SaveMembership(ObservableCollection<Member> members)
        {
            // Creates a new StreamWriter whenever opened.
            // Closes the file after finishing.
            using (StreamWriter sm = new StreamWriter("saved_members.txt"))
            {
                // Each member's properties are added in-line in the file.
                foreach(Member m in members)
                {
                    // Use a delimeter "|"
                    sm.Write(m.FirstName + "|"); 
                    sm.Write(m.LastName + "|");
                    sm.Write(m.Email + "|");
                    sm.WriteLine();
                    //Do some saving stuff
                }
            }
        }
        // Loading mechanism that loads the membership list from textfile in local storage.
        // Returns an ObservableCollection object to the caller.
        public static ObservableCollection<Member> LoadMembership()
        {
            ObservableCollection<Member> memberList = new ObservableCollection<Member>();
            // String array of all different types of seperating characters.
            string[] separatingChar = { "|" };
            // Creates a new StreamReader whenever opened.
            // Closes the file after finishing.
            using (StreamReader sr = new StreamReader("saved_members.txt"))
            {
                //Checking the end of the file
                while (sr.Peek()!= -1) 
                {
                    var line = sr.ReadLine();
                    // Detecting each delimeter, removing each delimeter, and adding
                    // each string between the delimeter as objects in the indexer.
                    string[] info = line.Split(separatingChar, System.StringSplitOptions.RemoveEmptyEntries);
                    memberList.Add(new Member(info[0], info[1], info[2]));
                }
            }
            return memberList;
        }
    }
}

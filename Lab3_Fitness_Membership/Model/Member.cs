using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3_Fitness_Membership.Model
{
    /**
     * Extends ObservableObject from MVVM Light toolkit to allow for binding to other views and viewModels
     * Member class: Model object that holds three string properties:
     *      firstName
     *      lastName
     *      email
     * Contains constructors and property get/set methods
     */
    public class Member : ObservableObject
    {

        string firstName;
        string lastName;
        string email;

        public Member(){}
        public Member(string firstName, string lastName, string email)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
        }

        public string FirstName
        {
            get { return firstName; }
            set
            {
                // Sets the property and raises the PropertyChanged event to alert bound objects
                // of the change.
                Set<string>(() => this.firstName, ref firstName, value);
            }
        }

        public string LastName
        {
            get { return lastName; }
            set
            {
                Set<string>(() => this.lastName, ref lastName, value);
            }
        }

        public string Email
        {
            get { return email; }
            set
            {
                Set<string>(() => this.email, ref email, value);
            }
        }
    }
}

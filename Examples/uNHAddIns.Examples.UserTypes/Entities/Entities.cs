using System;
using System.Collections.Generic;
using System.Text;

namespace NH
{
    public class Customer
    {
        private int id;

        public virtual int Id
        {
            get { return id; }
            set { id = value; }
        }
        
        private string firstName;
        
        public virtual string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
        
        private string lastName;

        public virtual string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }    
    }
}

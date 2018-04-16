using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ComputerizedLibrarySystem
{
    public class EmailAlert
    {
        private long Id;
        private string borrowId;
        private string libraryId;
        private string dateDue;
        private string emailStatus;
        private string emailDate;
        private string email;
        private string emailSender;
        private int trials;
        private string remarks;

        public EmailAlert()
        {
            this.EmailDate = DateTime.Today.ToShortDateString();
            this.Trials = 0;
        }

        public EmailAlert(DataRow dataRow)
        {
            this.BorrowId = (dataRow[6].ToString()); //populate the Borrow ID
            this.LibraryId = (dataRow[0].ToString()); //populate the Library ID
            this.DateDue = (dataRow[4].ToString()); //populate the Due Date
            this.Email = (dataRow[5].ToString()); //populate the User Phone No
            this.EmailDate = DateTime.Today.ToShortDateString();
            this.Trials = 0;
            this.EmailSender = "Admin";
        }
        
        public string EmailSender
        {
            get { return emailSender; }
            set { emailSender = value; }
        }


        public string Email
        {
            get { return email; }
            set { email = value; }
        }


        public string EmailDate
        {
            get { return emailDate; }
            set { emailDate = value; }
        }



        public string EmailStatus
        {
            get { return emailStatus; }
            set { emailStatus = value; }
        }



        public string DateDue
        {
            get { return dateDue; }
            set { dateDue = value; }
        }

        public string LibraryId
        {
            get { return libraryId; }
            set { libraryId = value; }
        }

        public string BorrowId
        {
            get { return borrowId; }
            set { borrowId = value; }
        }
        
        
        public int Trials
        {
            get { return trials; }
            set { trials = value; }
        }
        

        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }
    }
}

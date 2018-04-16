using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ComputerizedLibrarySystem
{
    public class SMSAlert
    {
        private long Id;
        private string borrowId;
        private string libraryId;
        private string dateDue;
        private string smsStatus;
        private string smsDate;
        private string phoneNo;
        private string smsSender;
        private int trials;
        private string remarks;


        public SMSAlert()
        {
            this.SmsDate = DateTime.Today.ToShortDateString();
            this.Trials = 0;
        }

        public SMSAlert(DataRow dataRow)
        {
            this.BorrowId = (dataRow[6].ToString()); //populate the Borrow ID
            this.LibraryId = (dataRow[0].ToString()); //populate the Library ID
            this.DateDue = (dataRow[4].ToString()); //populate the Due Date
            this.PhoneNo = (dataRow[5].ToString()); //populate the User Phone No
            this.SmsDate = DateTime.Today.ToShortDateString();
            this.Trials = 0;
            this.SmsSender = "Admin";
        }

        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }

        public int Trials
        {
            get { return trials; }
            set { trials = value; }
        }



        public string SmsSender
        {
            get { return smsSender; }
            set { smsSender = value; }
        }


        public string PhoneNo
        {
            get { return phoneNo; }
            set { phoneNo = value; }
        }


        public string SmsDate
        {
            get { return smsDate; }
            set { smsDate = value; }
        }



        public string SmsStatus
        {
            get { return smsStatus; }
            set { smsStatus = value; }
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

    }
}

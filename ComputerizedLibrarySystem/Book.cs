using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComputerizedLibrarySystem
{
    public class Book
    {
        private string accessionNo;
        private string authorName;
        private string bookTitle;
        private string publisher;
        private string placeOfPublication;
        private string noOfCopies;
        private string isbn;
        private string dateOfRegistration;
        private string dateOfPublished;
        private string status;

        public Book() {
            this.accessionNo = "N/A";
            this.dateOfRegistration = DateTime.Today.ToShortDateString();
            this.status = "AVAILABLE";
        }

        public string AccessionNo
        {
            set{ accessionNo = value;}
            get{return accessionNo; }
        }

        public string AuthorName
        {
            get { return authorName; }
            set { authorName = value; }
        }

        public string BookTitle
        {
            get { return bookTitle; }
            set { bookTitle = value; }
        }

        public string Publisher
        {
            get { return publisher; }
            set { publisher = value; }
        }

        public string PlaceOfPublication
        {
            get { return placeOfPublication; }
            set { placeOfPublication = value; }
        }

        public string NoOfCopies
        {
            get { return noOfCopies; }
            set { noOfCopies = value; }
        }

        public string Isbn
        {
            get { return isbn; }
            set { isbn = value; }
        }

        public string DateOfRegistration
        {
            get { return dateOfRegistration; }
            set { dateOfRegistration = value; }
        }

        public string DateOfPublished
        {
            get { return dateOfPublished; }
            set { dateOfPublished = value; }
        }

        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        

    }
}

using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Linq;

namespace JoinQueriesWPF.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        string textBlock;
        public MainViewModel()
        {
            QueryCommandAction();
            // Entity Framework DBContext
            
        }

        public string TextBlock
        {
            get { return textBlock; }
            set
            {
                textBlock = value;
                RaisePropertyChanged("TextBlock");
            }
        }
        private void QueryCommandAction()
        {
            BooksEntities dbContext = new BooksEntities();

            // get authors and ISBNs of each book they co-authored
            var authorsAndISBNs =
               (from author in dbContext.Authors
                from book in author.Titles
                orderby author.LastName, author.FirstName
                select new { author.FirstName, author.LastName, book.ISBN });

            TextBlock+=("Authors and ISBNs:");
            Console.WriteLine(TextBlock);

            // display authors and ISBNs in tabular format
            foreach (var element in authorsAndISBNs)
            {
                TextBlock+=(
                   String.Format("\r\n\t{0,-10} {1,-10} {2,-10}",
                      element.FirstName, element.LastName, element.ISBN));
                Console.WriteLine(TextBlock);
            } // end foreach

            // get authors and titles of each book they co-authored
            var authorsAndTitles =
               (from book in dbContext.Titles
               from author in book.Authors
               orderby author.LastName, author.FirstName, book.Title1
               select new
               {
                   author.FirstName,
                   author.LastName,
                   book.Title1
               });

            TextBlock+="\r\n\r\nAuthors and titles:";

            // display authors and titles in tabular format
            foreach (var element in authorsAndTitles)
            {
                TextBlock+=(
                   String.Format("\r\n\t{0,-10} {1,-10} {2}",
                      element.FirstName, element.LastName, element.Title1));
            } // end foreach

            // get authors and titles of each book 
            // they co-authored; group by author
            var titlesByAuthor =
               from author in dbContext.Authors
               orderby author.LastName, author.FirstName
               select new
               {
                   Name = author.FirstName + " " + author.LastName,
                   Titles =
                     from book in author.Titles
                     orderby book.Title1
                     select book.Title1
               };

            TextBlock+="\r\n\r\nTitles grouped by author:";

            // display titles written by each author, grouped by author
            foreach (var author in titlesByAuthor)
            {
                // display author's name
                TextBlock+=("\r\n\t" + author.Name + ":");

                // display titles written by that author
                foreach (var title in author.Titles)
                {
                    TextBlock+=("\r\n\t\t" + title);
                } // end inner foreach
            } // end outer foreach
            TextBlock = TextBlock;
            Console.WriteLine(TextBlock);
            


        } // end method JoiningTableData_Load
    }
}
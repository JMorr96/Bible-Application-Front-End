using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BibleApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {


            InitializeComponent();
            this.DataContext = this;
            List<string> testBooks = new List<string>(new string[] { "Genesis", "Exodus", "Leviticus" });
            List<int> testChapters = new List<int>(new int[] { 1, 2, 3, 4, 5 });
            List<int> testVerses = new List<int>(new int[] { 1, 2, 3, 4, 5,6,7,8,9 });
            Books = testBooks;
            Chapters = testChapters;
            Verses = testVerses;
        }
        private List<int> verses;

        public List<int> Verses
        {
            get { return verses; }
            set { verses = value;
                OnPropertyChanged("Verses");
            }
        }

        private List<int> chapters;

        public List<int> Chapters
        {
            get { return chapters; }
            set
            {
                chapters = value;
                OnPropertyChanged("Chapters");
            }
        }

        private List<string> books;

        public List<string> Books
        {
            get { return books; }
            set
            {
                books = value;
                OnPropertyChanged("Books");
            }
        }


        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cbBook_Loaded(object sender, RoutedEventArgs e)
        {

        }
        #region INotifyPropertyChanged Members



        public event PropertyChangedEventHandler PropertyChanged;



        //Create OnPropertyChanged method to raise event

        protected void OnPropertyChanged(string PropertyName)

        {

            if (PropertyChanged != null)

                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));

        }



        #endregion
    }
}

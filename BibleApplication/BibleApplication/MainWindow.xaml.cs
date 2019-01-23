using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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

        /// <summary>
        /// Sets the textblocks with the requested verses.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            // Test URL.
            // TODO: Get the book version, book name, chapter, and verse and then make the call.
            var URL = @"http://profo.pythonanywhere.com/bible/api/v1.0/KJV/Ex/40/2";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                    var test = reader.ReadToEnd();
                    var result = JsonConvert.DeserializeObject<Verse>(test);
                    setTextBoxValue(result.prev.text, result.curr.text, result.next.text);
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }
        }
        /// <summary>
        /// Sets the text of each of the textblocks.
        /// </summary>
        /// <param name="prev">Previous text box.</param>
        /// <param name="curr">Current text box.</param>
        /// <param name="next">Next text box.</param>
        private void setTextBoxValue(string prevText, string currText, string nextText)
        {
            //TODO: null case for prev and next.
            prev.Text = prevText;
            curr.Text = currText;
            next.Text = nextText;
     
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
    public class Verse
    {
        public VerseDetails prev { get; set; }
        public VerseDetails curr { get; set; }
        public VerseDetails next { get; set; }
    }
    public class VerseDetails
    {
        public string book { get; set; }
        public string chapter { get; set; }
        public string verse { get; set; }
        public string text { get; set; }
    }
}

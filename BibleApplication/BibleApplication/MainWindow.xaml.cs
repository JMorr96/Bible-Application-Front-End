using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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
            setDefault = true;
            this.DataContext = this;
            BooksDict = new Dictionary<string, string>();

            //Grab Bible books
            var booksURL = @"http://profo.pythonanywhere.com/bible/api/v1.0/KJV/books";
            setDefault = false;
            GetNumberOf(booksURL, true, false, false);


            //Grab Bible chapters.
            var chaptersURL = @"http://profo.pythonanywhere.com/bible/api/v1.0/KJV/Ge/chapters";
            GetNumberOf(chaptersURL, false, true, false);

            //Grab Bible verses.
            var versesURL = @"http://profo.pythonanywhere.com/bible/api/v1.0/KJV/Ge/1/verses";
            GetNumberOf(versesURL, false, false, true);
            GoTo("Ge", 1, 1);



        }
        private Book _selectedBook;

        public Book SelectedBook
        {
            get { return _selectedBook; }
            set
            {
                _selectedBook = value;
                OnPropertyChanged();
            }
        }
        private int _selectedChapter;

        public int SelectedChapter
        {
            get { return _selectedChapter; }
            set
            {
                _selectedChapter = value;
                OnPropertyChanged();
            }
        }
        private int _selectedVerse;

        public int SelectedVerse
        {
            get { return _selectedVerse; }
            set
            {
                _selectedVerse = value;
                OnPropertyChanged();
            }
        }



        private List<int> _verses;

        public List<int> Verses
        {
            get { return _verses; }
            set
            {
                _verses = value;
                OnPropertyChanged();
            }
        }
        private List<int> _chapters;

        public List<int> Chapters
        {
            get { return _chapters; }
            set
            {
                _chapters = value;
                OnPropertyChanged();
            }
        }

        private List<Book> _books;

        public List<Book> Books
        {
            get { return _books; }
            set
            {
                _books = value;
                OnPropertyChanged();
            }
        }
        private Verse _currentVerseSelection;

        public Verse CurrentVerseSelection
        {
            get { return _currentVerseSelection; }
            set
            {
                _currentVerseSelection = value;
                OnPropertyChanged();
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
            GoTo(SelectedBook.abbrev, SelectedChapter, SelectedVerse);
        }
        /// <summary>
        /// Sets the text of each of the textblocks.
        /// </summary>
        /// <param name="prev">Previous text box.</param>
        /// <param name="curr">Current text box.</param>
        /// <param name="next">Next text box.</param>
        private void SetFormat()
        {

            SelectedBook = Books.FirstOrDefault(x => x.abbrev == CurrentVerseSelection.curr.book);
            SelectedChapter = CurrentVerseSelection.curr.chapter;
            SelectedVerse = CurrentVerseSelection.curr.verse;
            if (CurrentVerseSelection.prev == null)
            {
                tbPrev.Text = "There is no verse before this.";
            }
            else
            {
                tbPrev.Text = CurrentVerseSelection.prev.text;
            }
            if (CurrentVerseSelection.curr == null)
            {
                tbCurr.Text = "";
            }
            else
            {
                tbCurr.Text = CurrentVerseSelection.curr.text;
            }
            if (CurrentVerseSelection.next == null)
            {
                tbNext.Text = "The end.";

            }
            else
            {
                tbNext.Text = CurrentVerseSelection.next.text;

            }
            //If there is no previous verse, it will say. If there is none after then the text block is hidden.

            //nextText = null;
            //prev.Text = prevText == "" || prevText == null ? prev.Text = "There is no verse before this." : prev.Text = prevText;
            //curr.Text = currText;

            //next.Text = nextText;
            //if (next.Text == "" || next.Text == null)
            //{
            //    next.Visibility = Visibility.Hidden;
            //}

        }
        public bool setDefault { get; set; }

        #region INotifyPropertyChanged Members



        public event PropertyChangedEventHandler PropertyChanged;



        //Create OnPropertyChanged method to raise event

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {

            if (PropertyChanged != null)

                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
        public Dictionary<string, string> BooksDict { get; set; }
        private void CbBook_SelectionChanged(object sender, SelectionChangedEventArgs e)

        {
            string bookabbrev = SelectedBook.abbrev;
            var str = @"http://profo.pythonanywhere.com/bible/api/v1.0/KJV/{0}/chapters";
            var URL = string.Format(str, bookabbrev);


            GetNumberOf(URL, false, true, false);

        }
        private void CbChapter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var str = @"http://profo.pythonanywhere.com/bible/api/v1.0/KJV/{0}/{1}/verses";
            var URL = string.Format(str, SelectedBook.abbrev, SelectedChapter);

            GetNumberOf(URL, false, false, true);

            //if (!setDefault)
            //{
            //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            //    try
            //    {
            //        WebResponse response = request.GetResponse();
            //        using (Stream responseStream = response.GetResponseStream())
            //        {
            //            StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
            //            var number = int.Parse(reader.ReadToEnd());
            //            var numberList = Enumerable.Range(1, number).ToList();

            //            Chapters = numberList;
            //            cbChapter.ItemsSource = Chapters;
            //            cbChapter.SelectedItem = Chapters.First();
            //        }
            //    }
            //    catch (WebException ex)
            //    {
            //        WebResponse errorResponse = ex.Response;
            //        using (Stream responseStream = errorResponse.GetResponseStream())
            //        {
            //            StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.GetEncoding("utf-8"));
            //            String errorText = reader.ReadToEnd();
            //            // log errorText
            //        }
            //        throw;
            //    }
            //}
            //else
            //    setDefault = false;
        }


        private void GetNumberOf(string URL, bool books, bool chapters, bool verses)
        {
            if (!setDefault)
            {
                if (books)
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                    try
                    {
                        WebResponse response = request.GetResponse();
                        using (Stream responseStream = response.GetResponseStream())
                        {
                            StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                            var array = reader.ReadToEnd();
                            var result = JsonConvert.DeserializeObject<dynamic[][]>(array);

                            var list = new List<Book>();
                            foreach (var item in result)
                            {
                                Book temp = new Book();
                                temp.abbrev = item[0];
                                temp.name = item[1];
                                BooksDict.Add(item[0], item[1]);
                                list.Add(temp);
                            }
                            Books = list;
                            //Books = list.Select(x => x.name).ToList();
                            //cbBook.ItemsSource = Books;
                            //cbBook.SelectedItem = Books.First();
                            setDefault = false;

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

                else if (chapters)
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                    try
                    {
                        WebResponse response = request.GetResponse();
                        using (Stream responseStream = response.GetResponseStream())
                        {
                            StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                            var number = int.Parse(reader.ReadToEnd());
                            var numberList = Enumerable.Range(1, number).ToList();

                            Chapters = numberList;
                            //cbChapter.ItemsSource = Chapters;
                            //cbChapter.SelectedItem = Chapters.First();
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

                else if (verses)
                {
                    HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(URL);
                    try
                    {
                        WebResponse response = request2.GetResponse();
                        using (Stream responseStream = response.GetResponseStream())
                        {
                            StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                            var number = int.Parse(reader.ReadToEnd());
                            Verses = Enumerable.Range(1, number).ToList();

                            //cbVerse.ItemsSource = Verses;
                            //cbVerse.SelectedItem = Verses.First();

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

            }

            else
                setDefault = false;
        }

        private void GoTo(string bookAbbrev, int chapter, int verse)
        {
            var str = @"http://profo.pythonanywhere.com/bible/api/v1.0/KJV/{0}/{1}/{2}";
            var URL = string.Format(str, bookAbbrev, chapter, verse);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                    var test = reader.ReadToEnd();
                    CurrentVerseSelection = JsonConvert.DeserializeObject<Verse>(test);

                    SetFormat();

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
            SetFormat();
        }



        private void grdmain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                //check # of chapters in book
                //if last chapter get next book
                //back one chapter
                if (SelectedBook.abbrev == CurrentVerseSelection.prev.book)
                {
                    if (Chapters.Contains(CurrentVerseSelection.prev.chapter))
                    {
                        GoTo(CurrentVerseSelection.curr.book, CurrentVerseSelection.prev.chapter, 1);
                    }
                }



            }
            if (e.Key == Key.Right)
            {
                if (SelectedBook.abbrev == CurrentVerseSelection.prev.book)
                {
                    if (Chapters.Contains(CurrentVerseSelection.prev.chapter))
                    {
                        GoTo(CurrentVerseSelection.curr.book, CurrentVerseSelection.prev.chapter, 1);
                    }
                    else
                    {
                        GoTo(CurrentVerseSelection.curr.book, CurrentVerseSelection.curr.chapter, 1);
                    }
                }

                // forward one chapter

            }
            if (e.Key == Key.Up)
            {
                // back one verse
                if (CurrentVerseSelection.prev == null) return;
                GoTo(CurrentVerseSelection.curr.book, CurrentVerseSelection.curr.chapter, CurrentVerseSelection.prev.verse);
                SelectedChapter = CurrentVerseSelection.curr.chapter;
                SelectedVerse = CurrentVerseSelection.curr.verse;

            }
            if (e.Key == Key.Down)
            {
                if (CurrentVerseSelection.next == null) return;
                GoTo(CurrentVerseSelection.curr.book, CurrentVerseSelection.curr.chapter, CurrentVerseSelection.next.verse);

                SelectedChapter = CurrentVerseSelection.curr.chapter;
                SelectedVerse = CurrentVerseSelection.curr.verse;
            }



        }

        private void winMain_Loaded(object sender, RoutedEventArgs e)
        {
            winMain.Focus();
        }
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
        public int chapter { get; set; }
        public int verse { get; set; }
        public string text { get; set; }
    }
    public class Book
    {
        public string abbrev { get; set; }
        public string name { get; set; }
    }

}


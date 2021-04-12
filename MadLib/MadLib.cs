using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace MadLib
{
    /// <summary>
    /// Mainform
    /// </summary>
    public partial class MadLib : Form
    {
        // List of words to skip when creating mad-lib
        string[] skipList = new string[] { "to", "too", "is", "was", "a", "of", "if", "the", "and", "but", "or", "nor", "for", "only", "in"};
        private readonly int maxSpacing = 7; // Default Max Spacing for mad lib creation, I might make something to save a value somewhere
        private readonly int minWords = 10;

        /// <summary>Default Constructor</summary>
        public MadLib()
        {
            
            InitializeComponent();
        }

        /// <summary>
        /// Button click event for opening a mad lib file and kick off the dialogs to read it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpen_Click(object sender, EventArgs e)
        {
            DialogResult theResult = DialogOpen.ShowDialog();
            if (theResult == DialogResult.OK && File.Exists(DialogOpen.FileName))
            {
                string MadLibText = File.ReadAllText(DialogOpen.FileName);
                    
                WordSelect theWordSelect = new WordSelect(MadLibText);
                theResult = theWordSelect.ShowDialog();
                if (theResult == DialogResult.OK)
                {
                    StoryBox.Text = theWordSelect.retString;
                }

            }
        }

        /// <summary>
        /// Creates mad lib string and fills it in the main text box. 
        /// </summary>
        /// <param name="theText"></param>
        private void create(string theText)
        {
            if (theText.Contains('<') || theText.Contains('>'))
            {
                MessageBox.Show("Contains illegal Character < or > Either you have already processed this into a madlib or you will need to remove the characters to proceed.");
                return;
            }

            // Turns the text into a string of words to process.
            string[] theWords = theText.Split(new char[] { ' ', '\n' },StringSplitOptions.RemoveEmptyEntries);
            string[] original = (string[])theWords.Clone();
            // Make sure there are at least 10 words. This could probably be larger.
            if (theWords.Length < minWords)
            {
                MessageBox.Show("Not enough words to make a mad lib.");
                return;
            }

            // Now let's pick some random spacing for word replacement
            var random = new Random();

            var i = 0 + random.Next(maxSpacing);
            while (i < theWords.Length)
            {
                // Hold any punctuation on the word
                var pre = "";
                var post = "";
                var theWord = theWords[i];
                while (char.IsPunctuation(theWord.First()))
                {
                    pre += theWord.First();
                    theWord = theWord.Substring(1, theWord.Length - 1);
                    
                }
                while (char.IsPunctuation(theWord.Last()))
                {
                    post = theWord.Last() + post;
                    theWord = theWord.Substring(0, theWord.Length - 1);
                }

                // Skip words that don't make sense to madlib
                if (skipList.Contains(theWord.ToLower()))
                {
                    i += random.Next(1, 3); // When Skipping don't always use the next word to make it a little more random
                    continue;
                }

                // get Context and Madlibify the word and replace it in the list
                var context = getContext(original, i);
                var madlibification = madlibify(theWord, context);
                // If we skip or have an unknown dialog return 
                if (madlibification == "skip" || madlibification == theWord)
                {
                    i += random.Next(1, 3); // When Skipping don't always use the next word to make it a little more random
                    continue;
                }
                else if (madlibification == "cancel")
                {
                    return;
                }

                theWords[i] = pre + madlibification + post;

                i += random.Next(1, 7);
            }

            // Re-add spaces
            StoryBox.Text = "";
            foreach (string word in theWords)
            {
                StoryBox.Text += word + " ";
            }
        }

        /// <summary>
        /// Grabs a few words before or after the word to provide context for user to assign it's part of speech
        /// </summary>
        /// <param name="theWords">The list of words</param>
        /// <param name="i">The index of the word that needs context</param>
        /// <returns></returns>
        private string getContext(string[] theWords, int i)
        {
            string context = "";
            int minWordIndex = (i < 3) ? 0 : i - 3; // Get beginning of context
            int maxWordIndex = (i + 3 >= theWords.Length) ? theWords.Length - 1 : i + 3; // Get end of context
            for (int j = minWordIndex; j <= maxWordIndex; ++j)
            {
                context += theWords[j] + ' ';
            }
            return context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="madWord"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private string madlibify(string madWord, string context)
        {
            string res = madWord;
            MadPicker picker = new MadPicker(madWord, context);
            picker.ShowDialog();
            if (picker.DialogResult == DialogResult.OK)
                res = "<" + picker.POS + ">";
            else if (picker.DialogResult == DialogResult.Retry)
                res = "skip";
            else if (picker.DialogResult == DialogResult.Cancel)
                res = "cancel";
            return res;
        }

        private void btnCreateText_Click(object sender, EventArgs e)
        {
            create(StoryBox.Text);
        }

        private void btnCreateFile_Click(object sender, EventArgs e)
        {

            DialogResult theResult = DialogOpen.ShowDialog();
            if (theResult == DialogResult.OK)
            {
                if (File.Exists(DialogOpen.FileName))
                {
                    create(File.ReadAllText(DialogOpen.FileName));
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog theDlg = new SaveFileDialog();
            theDlg.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            theDlg.ShowDialog();

            File.WriteAllText(theDlg.FileName, StoryBox.Text);
        }

    }
}

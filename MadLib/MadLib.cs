using System;
using System.Linq;
using System.Windows.Forms;

namespace MadLib
{
    enum Error
    {
        None,
        UnknownException,
        Abort,
        Create_IllegalTags,
        Create_TooSmall,
    }
    class MadLib
    {
        // List of words to skip when creating mad-lib
        string[] skipList = new string[] { "to", "too", "is", "was", "a", "of", "if", "the", "and", "but", "or", "nor", "for", "only", "in" };
        private readonly int maxSpacing = 7; // Default Max Spacing for mad lib creation, I might make something to save a value somewhere
        private readonly int minWords = 10;

        private string _baseText;
        private string _editedText;
        public string MadLibText { get {return _editedText;} }

        /// <summary>
        /// Gets the error message 
        /// </summary>
        /// <param name="aErr">the Error code</param>
        /// <returns>the Error Message</returns>
        public static string getError(Error aErr)
        {
            string message = "";
            switch (aErr)
            {
                case Error.UnknownException:
                    message = "An Unexpected error has occurred";
                    break;
                case Error.Abort:
                    message = "Create Madlib Aborted";
                    break;
                case Error.Create_IllegalTags:
                    message = "Contains illegal Tags \"<*\" or \"*\"> Either you have already processed this into a madlib or you will need to remove the characters to proceed.";
                    break;
                case Error.Create_TooSmall:
                    message = "Not enough words to make a mad lib.";
                    break;
                default:
                    break;

            }
            if (!string.IsNullOrEmpty(message))
            {
                Console.WriteLine(message);
            }
            return message;
        }
        
        public MadLib(string MadLibText)
        {
            _baseText = MadLibText;
        }

        /// <summary>
        /// Creates mad lib string 
        /// </summary>
        /// <param name="_baseText"></param>
        public Error create()
        {
            try
            {
                if (_baseText.Contains("<*") || _baseText.Contains("*>"))
                {
                    return Error.Create_IllegalTags;
                }

                // Turns the text into a string of words to process.
                string[] theWords = _baseText.Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                string[] original = (string[])theWords.Clone();
                // Make sure there are at least 10 words. This could probably be larger.
                if (theWords.Length < minWords)
                {
                    return Error.Create_TooSmall;
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
                        return Error.Abort;
                    }

                    theWords[i] = pre + madlibification + post;

                    i += random.Next(1, 7);
                }

                // Re-add spaces
                _editedText = "";
                foreach (string word in theWords)
                {
                    _editedText += word + " ";
                }
            }
            catch
            {
                return Error.UnknownException;
            }
            return Error.None;
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
        /// Uses part of speech picker to turn the word into a MadLib tag
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
                res = "<*" + picker.POS + "*>";
            else if (picker.DialogResult == DialogResult.Retry)
                res = "skip";
            else if (picker.DialogResult == DialogResult.Cancel)
                res = "cancel";
            return res;
        }
    }
}

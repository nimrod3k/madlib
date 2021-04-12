using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MadLib
{
    partial class WordSelect
    {
        List<string> mStrings = new List<string>();
        List<int> mIndicies;
        List<Label> mLabels = new List<Label>();
        List<TextBox> mTexts = new List<TextBox>();
        public String retString = null;

        // Number of Columns to start with defines the initial size of the form
        private int _numCols = 2;

        // Put row size in one place for easier editing
        private Size _rowSize = new Size(300, 35);

        /// <summary>
        /// initializes the form and sets up the madlib fields
        /// </summary>
        /// <param name="MadLibText">The text to extract the tags from</param>
        public WordSelect(string MadLibText)
        {
            InitializeComponent();
            Width = _numCols * _rowSize.Width + 20;
            panelScroll.Height = (btnOK.Top - 20);
            btnOK.Top = Height - btnOK.Height - 50;
            btnOK.Left = (Width / 2) - (btnOK.Width / 2);
            SetupFields(MadLibText);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void SetupFields(string madLibText)
        {
            string[] theStringList;
            string openTag = "<*";
            string closeTag = "*>";
            if (!(madLibText.Contains(openTag) && madLibText.Contains(closeTag))) // if does not contain new tags try simple tags
            {
                openTag = "<";
                closeTag = ">";
            }
            theStringList = madLibText.Split(new string[] { openTag }, StringSplitOptions.None);

            // Split the madlib text on tag beginnings 
            mStrings = new List<string>(theStringList.Length); // A list the size of the number of possible tags to add the replacement words into
            mIndicies = new List<int>(); // keep a list of indicies to keep track of each field

            // loop through and create fields for each tag
            // ToDo: see if there is a better way create these fields
            foreach (string madString in theStringList)
            {
                if (madString.Contains(closeTag))
                {
                    string[] theMadStrings = madString.Split(new string[] { closeTag }, StringSplitOptions.None);
                    if (theMadStrings.Length > 2)
                    {
                        // Possibly handle it differently and just ignore the additional greater than signs.
                        MessageBox.Show("Madlib incorrectly formatted.");
                        return;
                    }

                    // once the tag is found add a marker and the tag name
                    mStrings.Add("*" + theMadStrings[0]);
                    

                    // Create field label
                    Label aLabel = new Label();

                    aLabel.AutoSize = true;
                    aLabel.Left = ((mIndicies.Count % _numCols) * _rowSize.Width) + 12;
                    aLabel.Top = ((mIndicies.Count / _numCols) * _rowSize.Height) + 19;
                    aLabel.Name = "label" + mIndicies.Count;
                    aLabel.Size = new System.Drawing.Size(33, 13);
                    aLabel.TabIndex = 1;
                    char[] thePOS = theMadStrings[0].ToCharArray();
                    thePOS[0] = char.ToUpper(thePOS[0]);
                    aLabel.Text = new string(thePOS);
                    // end of label definition

                    // keep track of label for later identification
                    mLabels.Add(aLabel);

                    // create field textBox
                    TextBox aTextBox = new TextBox();
                    aTextBox.Left = ((mIndicies.Count % _numCols) * _rowSize.Width) + 100;
                    aTextBox.Top = ((mIndicies.Count / _numCols) * _rowSize.Height) + 16;
                    aTextBox.Name = "textBox" + mIndicies.Count;
                    aTextBox.Size = new System.Drawing.Size(127, 20);
                    aTextBox.TabIndex = 3;
                    mTexts.Add(aTextBox);
                    panelScroll.Controls.Add(aTextBox);
                    panelScroll.Controls.Add(aLabel);
                    // end of textBox definition

                    // keep track of index for later identification
                    mIndicies.Add(mStrings.Count - 1);
                    if (theMadStrings.Length == 2) 
                    {
                        mStrings.Add(theMadStrings[1]);
                    }
                    // ToDo: possibly make loop to re-add '>' symbols and the content after
                }
                else // !madString.Contains(">")
                {
                    // just add the string into the list as it is not really a tag.
                    mStrings.Add(madString);
                }
            }
        }

        /// <summary>
        /// Button event to submit the form. Collects the data 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            // read 
            foreach (TextBox theTextBox in mTexts)
            {
                if (String.IsNullOrEmpty(theTextBox.Text))
                {
                    MessageBox.Show("You did not fill out: " + theTextBox.Name);
                    this.DialogResult = DialogResult.None;
                    return;
                }
            }
            for (int i = 0; i < mTexts.Count; ++i)
            {
                mStrings[mIndicies[i]] = mTexts[i].Text;
                if (mIndicies[i] == 0)
                {
                    mStrings[mIndicies[i]] = char.ToUpper(mStrings[mIndicies[i]].First()) + mStrings[mIndicies[i]].Substring(1);
                }
                else
                {
                    string testString = mStrings[mIndicies[i] - 1].TrimEnd(' ');
                    try
                    {
                        if (!string.IsNullOrEmpty(testString) && testString.Last() == '.')
                        {
                            mStrings[mIndicies[i]] = char.ToUpper(mStrings[mIndicies[i]].First()).ToString() + mStrings[mIndicies[i]].Substring(1);
                        }
                    }
                    catch { /*Do Nothing*/ }
                }
            }
            foreach (string theString in mStrings)
            {
                retString += theString;
            }
        }

        /// <summary>
        /// re-arrange the fields on resize if it will fit more or less columns
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WordSelect_Resize(object sender, EventArgs e)
        {
            // Set button and panel sizes in preparations for re-arranging controls
            btnOK.Top = Height - btnOK.Height - 50;
            btnOK.Left = (Width / 2) - (btnOK.Width / 2);
            panelScroll.Width = Width;
            panelScroll.Height = (btnOK.Top - 20);

            // Now re-arrange the fields
            int whatis = Width / _rowSize.Width;
            if (_numCols != Width / _rowSize.Width)
            {
                // New number of rows
                _numCols = Width / _rowSize.Width;
                for (int i = 0; i < mLabels.Count; ++i)
                {
                    // This seems a little slow, I wonder if there is a faster way to do this or if I'm just constrained by the redraw time...
                    // If I move the bottom items I might get away with moving less at a time
                    mLabels[i].Left = ((i % _numCols) * _rowSize.Width) + 12;
                    mLabels[i].Top = ((i / _numCols) * _rowSize.Height) + 19;
                    mTexts[i].Left = ((i % _numCols) * _rowSize.Width) + 100;
                    mTexts[i].Top = ((i / _numCols) * _rowSize.Height) + 16;
                }
            }
        }
    }
}
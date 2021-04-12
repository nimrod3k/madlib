using System;
using System.Windows.Forms;
using System.IO;
namespace MadLib
{
    /// <summary>
    /// Mainform
    /// </summary>
    public partial class MainForm : Form
    {
        private MadLib madLib;

        /// <summary>Default Constructor</summary>
        public MainForm()
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
            try
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
            catch
            {
                getError(Error.UnknownException);
            }
        }

        /// <summary>
        /// Create Madlib from a text file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateText_Click(object sender, EventArgs e)
        {
            madLib = new MadLib(StoryBox.Text);
            getError(madLib.create());
            StoryBox.Text = madLib.MadLibText;
        }

        /// <summary>
        /// Create Madlib from a text file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateFile_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult theResult = DialogOpen.ShowDialog();
                if (theResult == DialogResult.OK)
                {
                    if (File.Exists(DialogOpen.FileName))
                    {
                        madLib = new MadLib(File.ReadAllText(DialogOpen.FileName));
                        getError(madLib.create());
                        StoryBox.Text = madLib.MadLibText;
                    }
                }
            }
            catch
            {
                getError(Error.UnknownException);
            }
        }

        /// <summary>
        /// Save the current text box to a file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog theDlg = new SaveFileDialog();
                theDlg.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                theDlg.ShowDialog();

                File.WriteAllText(theDlg.FileName, StoryBox.Text);
            }
            catch
            {
                getError(Error.UnknownException);
            }
        }

        /// <summary>
        /// Gets the error message 
        /// </summary>
        /// <param name="aErr">the Error code</param>
        /// <param name="aSuppressPopup">true to not pop up error message</param>
        /// <returns>the Error Message</returns>
        private string getError(Error aErr, bool aSuppressPopup = false)
        {
            string message = MadLib.getError(aErr);
            bool popup = false;
            switch (aErr)
            {
                case Error.UnknownException:
                    popup = true;
                    break;
                case Error.Create_IllegalTags:
                    popup = true;
                    break;
                case Error.Create_TooSmall:
                    popup = true;
                    break;
                default:
                    break;

            }
            if (!string.IsNullOrEmpty(message))
            {
                if (popup && !aSuppressPopup)
                    MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return message;
        }

    }
}

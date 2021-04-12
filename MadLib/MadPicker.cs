using System.Drawing;
using System.Windows.Forms;

namespace MadLib
{
    public partial class MadPicker : Form
    {
        public MadPicker()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor to build a Part of Speech selection box and populate it
        /// </summary>
        /// <param name="word"></param>
        /// <param name="context"></param>
        public MadPicker(string word, string context)
        {
            InitializeComponent();
            labelWord.Text = word;
            richTextContext.Text = context;
            
            // Bold occurences of the word in the context
            int index = context.IndexOf(word, 0);
            while (index != -1)
            {
                richTextContext.SelectionStart = index;

                richTextContext.SelectionLength = word.Length;
                richTextContext.SelectionFont = new Font(richTextContext.Font, FontStyle.Bold);
                index = context.IndexOf(word, index + 1);

            }
        }

        /// <summary>
        /// Get specified Part of Speech
        /// </summary>
        public string POS
        {
            get
            {
                return comboPOS.Text;
            }
        }

        /// <summary>
        /// Validate before form closes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MadPicker_FormClosing(object sender, FormClosingEventArgs e)
        {
            // if nothing is specified then don't close form
            if (DialogResult == DialogResult.OK)
                if (string.IsNullOrEmpty(comboPOS.Text))
                {
                    MessageBox.Show("You did not select a valid option");
                    e.Cancel = true;
                }
        }
    }
}

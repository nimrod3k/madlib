namespace MadLib
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOpen = new System.Windows.Forms.Button();
            this.DialogOpen = new System.Windows.Forms.OpenFileDialog();
            this.StoryBox = new System.Windows.Forms.RichTextBox();
            this.btnCreateText = new System.Windows.Forms.Button();
            this.btnCreateFile = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(12, 12);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(106, 23);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "Select Madlib";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // DialogOpen
            // 
            this.DialogOpen.FileName = "openFileDialog1";
            // 
            // StoryBox
            // 
            this.StoryBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StoryBox.Location = new System.Drawing.Point(12, 51);
            this.StoryBox.Name = "StoryBox";
            this.StoryBox.Size = new System.Drawing.Size(665, 416);
            this.StoryBox.TabIndex = 1;
            this.StoryBox.Text = "";
            // 
            // btnCreateText
            // 
            this.btnCreateText.Location = new System.Drawing.Point(124, 12);
            this.btnCreateText.Name = "btnCreateText";
            this.btnCreateText.Size = new System.Drawing.Size(155, 23);
            this.btnCreateText.TabIndex = 2;
            this.btnCreateText.Text = "Create Mad Lib From Text";
            this.btnCreateText.UseVisualStyleBackColor = true;
            this.btnCreateText.Click += new System.EventHandler(this.btnCreateText_Click);
            // 
            // btnCreateFile
            // 
            this.btnCreateFile.Location = new System.Drawing.Point(285, 12);
            this.btnCreateFile.Name = "btnCreateFile";
            this.btnCreateFile.Size = new System.Drawing.Size(155, 23);
            this.btnCreateFile.TabIndex = 3;
            this.btnCreateFile.Text = "Create Mad Lib From File";
            this.btnCreateFile.UseVisualStyleBackColor = true;
            this.btnCreateFile.Click += new System.EventHandler(this.btnCreateFile_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(446, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save to File";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // MadLib
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 479);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCreateFile);
            this.Controls.Add(this.btnCreateText);
            this.Controls.Add(this.StoryBox);
            this.Controls.Add(this.btnOpen);
            this.Name = "MadLib";
            this.Text = "Mad Lib";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.OpenFileDialog DialogOpen;
        private System.Windows.Forms.RichTextBox StoryBox;
        private System.Windows.Forms.Button btnCreateText;
        private System.Windows.Forms.Button btnCreateFile;
        private System.Windows.Forms.Button btnSave;
    }
}


namespace RITCHARD_Windows
{
    partial class Interface
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
            this.components = new System.ComponentModel.Container();
            this.txtSentence = new System.Windows.Forms.TextBox();
            this.lblFindWord = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tmrCheckWordStatus = new System.Windows.Forms.Timer(this.components);
            this.tpgWordData = new System.Windows.Forms.TabPage();
            this.grpVerbConjugations = new System.Windows.Forms.GroupBox();
            this.btnStoreConjugation = new System.Windows.Forms.Button();
            this.lstVerbs = new System.Windows.Forms.ListBox();
            this.pbrVerbs = new System.Windows.Forms.ProgressBar();
            this.lstMatches = new System.Windows.Forms.ListBox();
            this.lstTenses = new System.Windows.Forms.ListBox();
            this.grpSentence = new System.Windows.Forms.GroupBox();
            this.lstWords = new System.Windows.Forms.ListBox();
            this.pbrWords = new System.Windows.Forms.ProgressBar();
            this.lstResults = new System.Windows.Forms.ListBox();
            this.grpThesaurus = new System.Windows.Forms.GroupBox();
            this.btnStoreEntry = new System.Windows.Forms.Button();
            this.lstAntonyms = new System.Windows.Forms.ListBox();
            this.lblAntonyms = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lstSynonyms = new System.Windows.Forms.ListBox();
            this.txtPOS = new System.Windows.Forms.TextBox();
            this.lblPOS = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.tabWords = new System.Windows.Forms.TabControl();
            this.tpgSentenceStructure = new System.Windows.Forms.TabPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tmrCheckVerbStatus = new System.Windows.Forms.Timer(this.components);
            this.grpParser = new System.Windows.Forms.GroupBox();
            this.lstConsole = new System.Windows.Forms.ListBox();
            this.definitionsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.relationshipsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.definitionsBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.tpgWordData.SuspendLayout();
            this.grpVerbConjugations.SuspendLayout();
            this.grpSentence.SuspendLayout();
            this.grpThesaurus.SuspendLayout();
            this.tabWords.SuspendLayout();
            this.tpgSentenceStructure.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.grpParser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.definitionsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.relationshipsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.definitionsBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSentence
            // 
            this.txtSentence.Location = new System.Drawing.Point(74, 12);
            this.txtSentence.Name = "txtSentence";
            this.txtSentence.Size = new System.Drawing.Size(728, 20);
            this.txtSentence.TabIndex = 1;
            this.txtSentence.Text = "Have you heard about the new extension for Google Chrome? It\'s called greasemonke" +
    "y and it lets you customise the way pages are displayed.";
            this.txtSentence.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSentence_KeyPress);
            // 
            // lblFindWord
            // 
            this.lblFindWord.AutoSize = true;
            this.lblFindWord.Location = new System.Drawing.Point(12, 15);
            this.lblFindWord.Name = "lblFindWord";
            this.lblFindWord.Size = new System.Drawing.Size(56, 13);
            this.lblFindWord.TabIndex = 0;
            this.lblFindWord.Text = "Sentence:";
            // 
            // tmrCheckWordStatus
            // 
            this.tmrCheckWordStatus.Interval = 1000;
            // 
            // tpgWordData
            // 
            this.tpgWordData.Controls.Add(this.grpVerbConjugations);
            this.tpgWordData.Controls.Add(this.grpSentence);
            this.tpgWordData.Controls.Add(this.grpThesaurus);
            this.tpgWordData.Location = new System.Drawing.Point(4, 25);
            this.tpgWordData.Name = "tpgWordData";
            this.tpgWordData.Padding = new System.Windows.Forms.Padding(3);
            this.tpgWordData.Size = new System.Drawing.Size(786, 330);
            this.tpgWordData.TabIndex = 0;
            this.tpgWordData.Text = "Retrieved Word Data";
            this.tpgWordData.UseVisualStyleBackColor = true;
            // 
            // grpVerbConjugations
            // 
            this.grpVerbConjugations.Controls.Add(this.btnStoreConjugation);
            this.grpVerbConjugations.Controls.Add(this.lstVerbs);
            this.grpVerbConjugations.Controls.Add(this.pbrVerbs);
            this.grpVerbConjugations.Controls.Add(this.lstMatches);
            this.grpVerbConjugations.Controls.Add(this.lstTenses);
            this.grpVerbConjugations.Location = new System.Drawing.Point(522, 6);
            this.grpVerbConjugations.Name = "grpVerbConjugations";
            this.grpVerbConjugations.Size = new System.Drawing.Size(258, 320);
            this.grpVerbConjugations.TabIndex = 20;
            this.grpVerbConjugations.TabStop = false;
            this.grpVerbConjugations.Text = "Verb Conjugations";
            // 
            // btnStoreConjugation
            // 
            this.btnStoreConjugation.Location = new System.Drawing.Point(6, 289);
            this.btnStoreConjugation.Name = "btnStoreConjugation";
            this.btnStoreConjugation.Size = new System.Drawing.Size(246, 23);
            this.btnStoreConjugation.TabIndex = 14;
            this.btnStoreConjugation.Text = "Store Verb Conjugation";
            this.btnStoreConjugation.UseVisualStyleBackColor = true;
            this.btnStoreConjugation.Click += new System.EventHandler(this.btnStoreConjugation_Click);
            // 
            // lstVerbs
            // 
            this.lstVerbs.FormattingEnabled = true;
            this.lstVerbs.Location = new System.Drawing.Point(6, 37);
            this.lstVerbs.Name = "lstVerbs";
            this.lstVerbs.Size = new System.Drawing.Size(120, 121);
            this.lstVerbs.TabIndex = 19;
            this.lstVerbs.SelectedIndexChanged += new System.EventHandler(this.lstVerbs_SelectedIndexChanged);
            // 
            // pbrVerbs
            // 
            this.pbrVerbs.Location = new System.Drawing.Point(6, 19);
            this.pbrVerbs.Name = "pbrVerbs";
            this.pbrVerbs.Size = new System.Drawing.Size(246, 12);
            this.pbrVerbs.TabIndex = 23;
            // 
            // lstMatches
            // 
            this.lstMatches.FormattingEnabled = true;
            this.lstMatches.Location = new System.Drawing.Point(132, 37);
            this.lstMatches.Name = "lstMatches";
            this.lstMatches.Size = new System.Drawing.Size(120, 121);
            this.lstMatches.TabIndex = 20;
            // 
            // lstTenses
            // 
            this.lstTenses.FormattingEnabled = true;
            this.lstTenses.Location = new System.Drawing.Point(6, 164);
            this.lstTenses.Name = "lstTenses";
            this.lstTenses.Size = new System.Drawing.Size(246, 121);
            this.lstTenses.TabIndex = 1;
            // 
            // grpSentence
            // 
            this.grpSentence.Controls.Add(this.lstWords);
            this.grpSentence.Controls.Add(this.pbrWords);
            this.grpSentence.Controls.Add(this.lstResults);
            this.grpSentence.Location = new System.Drawing.Point(6, 6);
            this.grpSentence.Name = "grpSentence";
            this.grpSentence.Size = new System.Drawing.Size(258, 320);
            this.grpSentence.TabIndex = 18;
            this.grpSentence.TabStop = false;
            this.grpSentence.Text = "Sentence";
            // 
            // lstWords
            // 
            this.lstWords.FormattingEnabled = true;
            this.lstWords.Location = new System.Drawing.Point(6, 37);
            this.lstWords.Name = "lstWords";
            this.lstWords.Size = new System.Drawing.Size(120, 277);
            this.lstWords.TabIndex = 18;
            this.lstWords.SelectedIndexChanged += new System.EventHandler(this.lstWords_SelectedIndexChanged);
            // 
            // pbrWords
            // 
            this.pbrWords.Location = new System.Drawing.Point(6, 19);
            this.pbrWords.Name = "pbrWords";
            this.pbrWords.Size = new System.Drawing.Size(246, 12);
            this.pbrWords.TabIndex = 24;
            // 
            // lstResults
            // 
            this.lstResults.FormattingEnabled = true;
            this.lstResults.Location = new System.Drawing.Point(132, 37);
            this.lstResults.Name = "lstResults";
            this.lstResults.Size = new System.Drawing.Size(120, 277);
            this.lstResults.TabIndex = 19;
            this.lstResults.SelectedIndexChanged += new System.EventHandler(this.lstResults_SelectedIndexChanged);
            // 
            // grpThesaurus
            // 
            this.grpThesaurus.Controls.Add(this.btnStoreEntry);
            this.grpThesaurus.Controls.Add(this.lstAntonyms);
            this.grpThesaurus.Controls.Add(this.lblAntonyms);
            this.grpThesaurus.Controls.Add(this.label1);
            this.grpThesaurus.Controls.Add(this.lstSynonyms);
            this.grpThesaurus.Controls.Add(this.txtPOS);
            this.grpThesaurus.Controls.Add(this.lblPOS);
            this.grpThesaurus.Controls.Add(this.txtDescription);
            this.grpThesaurus.Controls.Add(this.lblDescription);
            this.grpThesaurus.Location = new System.Drawing.Point(270, 6);
            this.grpThesaurus.Name = "grpThesaurus";
            this.grpThesaurus.Size = new System.Drawing.Size(246, 320);
            this.grpThesaurus.TabIndex = 12;
            this.grpThesaurus.TabStop = false;
            this.grpThesaurus.Text = "Thesaurus";
            // 
            // btnStoreEntry
            // 
            this.btnStoreEntry.Location = new System.Drawing.Point(6, 289);
            this.btnStoreEntry.Name = "btnStoreEntry";
            this.btnStoreEntry.Size = new System.Drawing.Size(234, 23);
            this.btnStoreEntry.TabIndex = 13;
            this.btnStoreEntry.Text = "Store Thesaurus Entry";
            this.btnStoreEntry.UseVisualStyleBackColor = true;
            this.btnStoreEntry.Click += new System.EventHandler(this.btnStoreEntry_Click);
            // 
            // lstAntonyms
            // 
            this.lstAntonyms.FormattingEnabled = true;
            this.lstAntonyms.Location = new System.Drawing.Point(126, 110);
            this.lstAntonyms.Name = "lstAntonyms";
            this.lstAntonyms.Size = new System.Drawing.Size(114, 173);
            this.lstAntonyms.TabIndex = 12;
            // 
            // lblAntonyms
            // 
            this.lblAntonyms.AutoSize = true;
            this.lblAntonyms.Location = new System.Drawing.Point(123, 94);
            this.lblAntonyms.Name = "lblAntonyms";
            this.lblAntonyms.Size = new System.Drawing.Size(56, 13);
            this.lblAntonyms.TabIndex = 11;
            this.lblAntonyms.Text = "Antonyms:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Synonyms:";
            // 
            // lstSynonyms
            // 
            this.lstSynonyms.FormattingEnabled = true;
            this.lstSynonyms.Location = new System.Drawing.Point(6, 110);
            this.lstSynonyms.Name = "lstSynonyms";
            this.lstSynonyms.Size = new System.Drawing.Size(114, 173);
            this.lstSynonyms.TabIndex = 9;
            // 
            // txtPOS
            // 
            this.txtPOS.Location = new System.Drawing.Point(6, 71);
            this.txtPOS.Name = "txtPOS";
            this.txtPOS.Size = new System.Drawing.Size(234, 20);
            this.txtPOS.TabIndex = 8;
            // 
            // lblPOS
            // 
            this.lblPOS.AutoSize = true;
            this.lblPOS.Location = new System.Drawing.Point(3, 55);
            this.lblPOS.Name = "lblPOS";
            this.lblPOS.Size = new System.Drawing.Size(92, 13);
            this.lblPOS.TabIndex = 7;
            this.lblPOS.Text = "Part(s) of Speech:";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(6, 32);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(234, 20);
            this.txtDescription.TabIndex = 6;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(3, 16);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(63, 13);
            this.lblDescription.TabIndex = 5;
            this.lblDescription.Text = "Description:";
            // 
            // tabWords
            // 
            this.tabWords.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabWords.Controls.Add(this.tpgWordData);
            this.tabWords.Controls.Add(this.tpgSentenceStructure);
            this.tabWords.Location = new System.Drawing.Point(12, 38);
            this.tabWords.Name = "tabWords";
            this.tabWords.SelectedIndex = 0;
            this.tabWords.Size = new System.Drawing.Size(794, 359);
            this.tabWords.TabIndex = 2;
            // 
            // tpgSentenceStructure
            // 
            this.tpgSentenceStructure.Controls.Add(this.pictureBox1);
            this.tpgSentenceStructure.Location = new System.Drawing.Point(4, 25);
            this.tpgSentenceStructure.Name = "tpgSentenceStructure";
            this.tpgSentenceStructure.Padding = new System.Windows.Forms.Padding(3);
            this.tpgSentenceStructure.Size = new System.Drawing.Size(786, 330);
            this.tpgSentenceStructure.TabIndex = 1;
            this.tpgSentenceStructure.Text = "Sentence Structure";
            this.tpgSentenceStructure.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(6, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(774, 318);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // tmrCheckVerbStatus
            // 
            this.tmrCheckVerbStatus.Tick += new System.EventHandler(this.tmrCheckVerbStatus_Tick);
            // 
            // grpParser
            // 
            this.grpParser.Controls.Add(this.lstConsole);
            this.grpParser.Location = new System.Drawing.Point(16, 406);
            this.grpParser.Name = "grpParser";
            this.grpParser.Size = new System.Drawing.Size(786, 149);
            this.grpParser.TabIndex = 20;
            this.grpParser.TabStop = false;
            this.grpParser.Text = "Console";
            // 
            // lstConsole
            // 
            this.lstConsole.FormattingEnabled = true;
            this.lstConsole.Location = new System.Drawing.Point(6, 19);
            this.lstConsole.Name = "lstConsole";
            this.lstConsole.Size = new System.Drawing.Size(774, 121);
            this.lstConsole.TabIndex = 0;
            // 
            // Interface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(818, 567);
            this.Controls.Add(this.grpParser);
            this.Controls.Add(this.tabWords);
            this.Controls.Add(this.lblFindWord);
            this.Controls.Add(this.txtSentence);
            this.Name = "Interface";
            this.Text = "RITCHARD";
            this.tpgWordData.ResumeLayout(false);
            this.grpVerbConjugations.ResumeLayout(false);
            this.grpSentence.ResumeLayout(false);
            this.grpThesaurus.ResumeLayout(false);
            this.grpThesaurus.PerformLayout();
            this.tabWords.ResumeLayout(false);
            this.tpgSentenceStructure.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.grpParser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.definitionsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.relationshipsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.definitionsBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSentence;
        private System.Windows.Forms.Label lblFindWord;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Timer tmrCheckWordStatus;
        private System.Windows.Forms.TabPage tpgWordData;
        private System.Windows.Forms.TabControl tabWords;
        private System.Windows.Forms.GroupBox grpThesaurus;
        private System.Windows.Forms.TextBox txtPOS;
        private System.Windows.Forms.Label lblPOS;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.ListBox lstAntonyms;
        private System.Windows.Forms.Label lblAntonyms;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lstSynonyms;
        private System.Windows.Forms.Timer tmrCheckVerbStatus;
        private System.Windows.Forms.GroupBox grpSentence;
        private System.Windows.Forms.ListBox lstResults;
        private System.Windows.Forms.ListBox lstWords;
        private System.Windows.Forms.GroupBox grpVerbConjugations;
        private System.Windows.Forms.ListBox lstTenses;
        private System.Windows.Forms.ListBox lstVerbs;
        private System.Windows.Forms.ListBox lstMatches;
        private System.Windows.Forms.ProgressBar pbrVerbs;
        private System.Windows.Forms.ProgressBar pbrWords;
        private System.Windows.Forms.TabPage tpgSentenceStructure;
        private System.Windows.Forms.Button btnStoreConjugation;
        private System.Windows.Forms.Button btnStoreEntry;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox grpParser;
        private System.Windows.Forms.ListBox lstConsole;
        private System.Windows.Forms.BindingSource definitionsBindingSource;
        private System.Windows.Forms.BindingSource relationshipsBindingSource;
        private System.Windows.Forms.BindingSource definitionsBindingSource1;
    }
}


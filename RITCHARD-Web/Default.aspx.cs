using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using RITCHARD_Data;
using RITCHARD_Processing;

namespace RITCHARD_Web
{
    public partial class _Default : System.Web.UI.Page
    {
        private RitchardDataContext _db;

        private string[] originalWordList;
        private string[] lowercaseWordList;

        private DataSet ds;

        protected void Page_Load(object sender, EventArgs e)
        {
            _db = new RitchardDataContext();
        }

        public DataSet ProcessWords(List<string> words)
        {
            originalWordList = new string[words.Count];
            lowercaseWordList = new string[words.Count];
            
            words.CopyTo(originalWordList);
            words.CopyTo(lowercaseWordList);

            for (int i = 0; i < words.Count; i++)
            {
                lowercaseWordList[i] = lowercaseWordList[i].ToLower();
            }

            ds = new DataSet();
            DataTable dt = new DataTable("inputWords");
            dt.Columns.Add("class");
            dt.Columns.Add("wordNumber");
            dt.Columns.Add("word");
            dt.Columns.Add("pos");

            for (int i = 0; i < words.Count; i++)
            {
                string[] data = new string[4];

                Definition d = RitchardDataHelper.GetDefinitionCaseSensitive(originalWordList[i]);

                data[1] = string.Format("{0}", i + 1);
                data[2] = originalWordList[i];
                data[3] = "";

                if (d != null)
                {
                    data[0] = "";

                    foreach (var posDef in _db.PartsOfSpeeches.Where(ps => ps.DefinitionID == d.DefinitionID))
                    {
                        if (data[3].Length > 0)
                        {
                            data[3] += ", ";
                        }

                        data[3] += posDef.PartOfSpeechDefinition.String;
                    }
                }
                else
                {
                    data[0] = "warning";
                    data[3] = "";
                }

                

                dt.Rows.Add(data);
            }

            ds.Tables.Add(dt);

            return ds;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtInput.Text.Length > 0)
            {
                InputProcessor ip = new InputProcessor();

                ip.ProcessInput(txtInput.Text);
                txtInput.Text = ip.GetNewInput();

                repeater.DataSource = ProcessWords(ip.GetWordsFromInput(txtInput.Text));
                repeater.DataBind();

                working.Visible = true;
            }
        }
    }
}

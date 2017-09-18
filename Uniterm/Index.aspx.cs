using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Uniterm
{
    public partial class Index : System.Web.UI.Page
    {
        private Uniterm uniterm;
        private UnitermDesigner unitermDesigner;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                initFonts();
                initList();
            }

            if (ViewState["uniterm"] != null)
            {
                uniterm = (Uniterm)ViewState["uniterm"];
            }
            else
            {
                uniterm = new Uniterm();
                ViewState["uniterm"] = uniterm;
            }

            uniterm.fontSize = Int32.Parse(ddListFontSize.SelectedItem.Value);
            uniterm.fontFamily = ddListFont.SelectedItem.Value;

            unitermDesigner = new UnitermDesigner(uniterm);
            redrawUniterm(); 
        }


        private void initFonts()
        {
            InstalledFontCollection installedFonts = new InstalledFontCollection();

            foreach (FontFamily font in installedFonts.Families)
                ddListFont.Items.Add(font.Name);            

            for (int i = 8; i < 40; i++)
                ddListFontSize.Items.Add(i.ToString());

            ddListFontSize.SelectedIndex = 4;
        }

        private void initList()
        {
            List<string> unitermNames = UnitermDatabase.GetInstance().GetUnitermNames();
            UnitermListBox.SelectedIndexChanged -= UnitermListBox_SelectedIndexChanged;
            UnitermListBox.Items.Clear();

            foreach (string unitermName in unitermNames)
                UnitermListBox.Items.Add(unitermName);

            UnitermListBox.SelectedIndexChanged += UnitermListBox_SelectedIndexChanged;
        }

        private void redrawUniterm()
        {
            Bitmap bitmap = unitermDesigner.Draw();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, ImageFormat.Jpeg);

                Byte[] bytes = new Byte[memoryStream.Length];
                memoryStream.Position = 0;
                memoryStream.Read(bytes, 0, (int)memoryStream.Length);

                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);

                UnitermImage.ImageUrl = "data:image/png;base64," + base64String;
            }
        }

        private void showMessage(string text)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "message", "alert('" + text + "');", true);
        }

        protected void ButtonNew_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            if (tbUnitermName.Text.Trim().Length > 0)
            {
                uniterm.name = tbUnitermName.Text.Trim();
                uniterm.description = tbUnitermDescription.Text.Trim();

                bool saved = UnitermDatabase.GetInstance().AddUniterm(uniterm);

                if (saved)
                {
                    initList();
                    DescriptionLabel.Text = uniterm.description;
                    showMessage("Uniterm został zapisany");
                }                    
                else
                {
                    showMessage("Uniterm nie został zapisany");
                }                    
            }
            else
            {
                showMessage("Brak nazwy");
            }
        }

        protected void ButtonRemove_Click(object sender, EventArgs e)
        {
            bool removed = UnitermDatabase.GetInstance().RemoveUnitermById(uniterm.id);

            if (removed)
            {
                initList();
                showMessage("Uniterm został usunięty");

                ButtonClear_Click(sender, e);
            }               
            else
            {
                showMessage("Uniterm nie został usunięty");
            }
        }

        protected void ButtonZrownAdd_Click(object sender, EventArgs e)
        {
            string opA = tbZrownA.Text.Trim();
            string opB = tbZrownB.Text.Trim();
            string opS = rbZrownSemicolon.Checked ? ";" : ",";

            if (opA.Length != 0 && opB.Length != 0)
            {
                uniterm.eA = opA;
                uniterm.eB = opB;
                uniterm.eOp = opS;

                redrawUniterm();
            }
        }

        protected void ButtonSekwAdd_Click(object sender, EventArgs e)
        {
            string opA = tbSekwA.Text.Trim();
            string opB = tbSekwB.Text.Trim();
            string opS = rbSekwSemicolon.Checked ? ";" : ",";

            if (opA.Length != 0 && opB.Length != 0)
            {
                uniterm.sA = opA;
                uniterm.sB = opB;
                uniterm.sOp = opS;

                redrawUniterm();
            }
        }

        protected void ButtonSwap_Click(object sender, EventArgs e)
        {
            if (uniterm.sA != "" && uniterm.eA != "")
            {
                char swap = rbSwapA.Checked ? 'A' : 'B';

                uniterm.switched = swap;
                redrawUniterm();
            }
        }

        protected void ButtonRefresh_Click(object sender, EventArgs e)
        {
            redrawUniterm();
        }

        protected void ButtonClear_Click(object sender, EventArgs e)
        {
            uniterm = new Uniterm();
            ViewState["uniterm"] = uniterm;

            unitermDesigner = new UnitermDesigner(uniterm);
            redrawUniterm();

            DescriptionLabel.Text = "";
        }


        protected void UnitermListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedName = UnitermListBox.SelectedValue;

            uniterm = UnitermDatabase.GetInstance().GetUnitermByName(selectedName);

            ViewState["uniterm"] = uniterm;

            unitermDesigner = new UnitermDesigner(uniterm);
            redrawUniterm();

            tbUnitermName.Text = uniterm.name;
            tbUnitermDescription.Text = uniterm.description;
            tbSekwA.Text = uniterm.sA;
            tbSekwB.Text = uniterm.sB;
            tbZrownA.Text = uniterm.eA;
            tbZrownB.Text = uniterm.eB;
            ddListFontSize.SelectedValue = uniterm.fontSize + "";
            ddListFont.SelectedValue = uniterm.fontFamily;
            DescriptionLabel.Text = uniterm.description;

            if (uniterm.sOp == ";")
            {
                rbSekwSemicolon.Checked = true;
                rbSekwComma.Checked = false;
            }
            else
            {
                rbSekwSemicolon.Checked = false;
                rbSekwComma.Checked = true;
            }

            if (uniterm.eOp == ";")
            {
                rbZrownSemicolon.Checked = true;
                rbZrownComma.Checked = false;
            }
            else
            {
                rbZrownSemicolon.Checked = false;
                rbZrownComma.Checked = true;
            }
        }       
    }
}
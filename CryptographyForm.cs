using Main.Support_Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Main
{
    public partial class CryptographyForm : Form
    {
        public AppSetting appSetting;
        public CryptographyForm(AppSetting appSetting)
        {
            InitializeComponent();
            this.appSetting = appSetting;
        }

        private async void CryptographyForm_Load(object sender, EventArgs e)
        {
            Config.CryptographicOperationSucceeded = true;

            ApplyUserTheme();

            var selectedFiles = new List<string>(Config.SelectedFiles);
            selectedFiles.RemoveAll(f => !File.Exists(f));

            if (selectedFiles.Count == 0)
            {
                MessageBox.Show("No valid files selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            // Setup Progress<T> for UI updates
            var generalProgress = new Progress<(string file, bool success)>(update =>
            {
                var (file, success) = update;
                string text = success ? file : file + " (Error)";
                CryptList.Items.Add(text);
                if (!success) Config.CryptographicOperationSucceeded = false;
            });
            var fileProgress = new Progress<string>(file =>
            {
                Lb_FileProcessing.Text = "Processing file: " + file;
            });

            Lb_Status.Text = Config.CryptoMode == Config.CryptographyMode.Encrypt
                ? "Status: Encrypting files..."
                : "Status: Decrypting files...";

            await Task.Run(() =>
            {
                //Process file
                if (appSetting.SmartWriteSelector_AllOutMode)
                {
                    CryptographicOperation.ProcessFiles(selectedFiles, Config.Password, Config.CryptoMode, generalProgress, fileProgress);
                }
                else
                {
                    CryptographicOperation.ProcessFiles(selectedFiles, Config.Password, appSetting.SmartWriteSelector_Threshold, Config.CryptoMode, generalProgress, fileProgress);
                }
            });

            // Final UI update
            Lb_Status.ForeColor = Config.CryptographicOperationSucceeded ? Color.Green : Color.Red;
            Lb_Status.Text = Config.CryptographicOperationSucceeded
                ? "Operation completed successfully."
                : "Operation completed with errors.";
            Lb_FileProcessing.Text = "Processing file: None";
        }

        private void CryptList_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            string itemText = CryptList.Items[e.Index].ToString()!;

            // Determine text color based on error and theme
            Color textColor;
            if (itemText.Contains("(Error)"))
            {
                textColor = Color.Red; // Keep errors red
            }
            else
            {
                textColor = appSetting.App_Theme == AppSetting.InternalTheme.Light
                    ? Config.Color_LightTextColor
                    : Config.Color_TextColor;
            }

            // Determine background color based on theme
            Color backColor = appSetting.App_Theme == AppSetting.InternalTheme.Light
                ? Config.Color_LightControlBackground
                : Config.Color_ControlBackground;

            e.Graphics.FillRectangle(new SolidBrush(backColor), e.Bounds);
            e.Graphics.DrawString(itemText, e.Font, new SolidBrush(textColor), e.Bounds);

            e.DrawFocusRectangle();
        }

        private void ApplyUserTheme()
        {
            if (appSetting.App_Theme == AppSetting.InternalTheme.Light)
            {
                this.BackColor = Config.Color_LightBackground;
                CryptList.BackColor = Config.Color_LightControlBackground;
                CryptList.ForeColor = Config.Color_LightTextColor;
                Lb_Status.ForeColor = Config.Color_LightTextColor;
                Lb_FileProcessing.ForeColor = Config.Color_LightTextColor;
            }
            else
            {
                this.BackColor = Config.Color_Background;
                CryptList.BackColor = Config.Color_ControlBackground;
                CryptList.ForeColor = Config.Color_TextColor;
                Lb_Status.ForeColor = Config.Color_TextColor;
                Lb_FileProcessing.ForeColor = Config.Color_TextColor;
            }
        }

    }
}


namespace Main
{
    public partial class CryptographyForm : Form
    {
        public CryptographyForm()
        {
            InitializeComponent();
        }

        private async void CryptographyForm_Load(object sender, EventArgs e)
        {
            // Reset the flag before starting any operation
            Config.CryptographicOperationSucceeded = true;

            Focus();
            BringToFront();

            List<string> SelectedFiles = Config.SelectedFiles;

            for (int i = SelectedFiles.Count - 1; i >= 0; i--)
            {
                if (!File.Exists(SelectedFiles[i]))
                {
                    MessageBox.Show($"File not found: {SelectedFiles[i]}. This file will be skipped.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SelectedFiles.RemoveAt(i);
                    continue;
                }
                else continue;
            }
            if (SelectedFiles.Count == 0)
            {
                MessageBox.Show("No valid files selected for processing.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            await Task.Run(async () =>
            {
                if (Config.CryptoMode == Config.CryptographyMode.Encrypt)
                {
                    Invoke(new Action(() =>
                    {
                        Lb_Status.Text = "Status: Encrypting files... Please do not close this window until encryption has been completed.";
                    }));
                    foreach (string file in SelectedFiles)
                    {
                        Invoke(new Action(() =>
                        {
                            Lb_FileProcessing.Text = "Processing file: " + file;
                        }));
                        string pass = Config.Password;
                        byte[] data = File.ReadAllBytes(file);
                        byte[] encryptedData = Tool.EncryptData(data, pass);

                        File.WriteAllBytes(file, encryptedData);
                        Invoke(new Action(() =>
                        {
                            CryptList.Items.Add(file);
                        }));
                        await Task.Delay(1); // Simulate some delay for UI update
                    }
                }
                else if (Config.CryptoMode == Config.CryptographyMode.Decrypt)
                {
                    Invoke(new Action(() =>
                    {
                        Lb_Status.Text = "Status: Decrypting files... Please do not close this window until decryption has been completed.";
                    }));
                    foreach (string file in SelectedFiles)
                    {
                        if (Lb_Status.ForeColor == Color.Red)
                        {
                            Invoke(new Action(() =>
                            {
                                Lb_Status.ForeColor = Color.Black;
                            }));
                        }
                        else { }

                        Invoke(new Action(() =>
                        {
                            Lb_FileProcessing.Text = "Processing file: " + file;
                        }));

                        try
                        {
                            string pass = Config.Password;
                            byte[] data = File.ReadAllBytes(file);
                            byte[] decryptedData = Tool.DecryptData(data, pass);

                            File.WriteAllBytes(file, decryptedData);
                        }
                        catch
                        {
                            Invoke(new Action(() =>
                            {
                                Lb_Status.Text = $"Status: Error decrypting file: {file}.";
                                Lb_Status.ForeColor = Color.Red;

                                CryptList.Items.Add(file + " (Error: Decryption failed)");
                            }));
                            await Task.Delay(1); // Wait for 3s to show the error message
                            Config.CryptographicOperationSucceeded = false;
                            continue; // Skip to the next file
                        }

                        Invoke(new Action(() =>
                        {
                            CryptList.Items.Add(file);
                        }));
                        await Task.Delay(1); // Simulate some delay for UI update
                    }
                }
                Invoke(new Action(() =>
                {
                    if (Config.CryptographicOperationSucceeded)
                    {
                        Lb_Status.Text = "Status: Operation completed successfully. You may close this window now.";
                        Lb_Status.ForeColor = Color.Green;
                        Lb_FileProcessing.Text = "All files processed successfully.";
                        Lb_FileProcessing.ForeColor = Color.Green;
                    }
                    else
                    {
                        Lb_Status.Text = "Status: Operation completed with errors. Either key is incorrect or the files have been tampered with.";
                        Lb_Status.ForeColor = Color.Red;
                        Lb_FileProcessing.Text = "Some/all files could not be processed.";
                        Lb_FileProcessing.ForeColor = Color.Red;
                    }
                }));
            });
        }
        private void CryptList_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            string itemText = CryptList.Items[e.Index].ToString();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Color textColor = itemText.Contains("(Error: Decryption failed)") ? Color.Red : Color.Black;
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            e.DrawBackground();
            using (Brush brush = new SolidBrush(textColor))
            {
                e.Graphics.DrawString(itemText, e.Font, brush, e.Bounds);
            }
            e.DrawFocusRectangle();
        }
    }
}

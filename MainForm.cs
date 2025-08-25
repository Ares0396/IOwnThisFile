
namespace Main
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private async void Btn_AddFiles_Enc_Click(object sender, EventArgs e)
        {
            int InitialCount = Box_SelectedFiles_Enc.Items.Count;
            Dlg_OpenFiles.ShowDialog();
            string[] SelectedFiles = Dlg_OpenFiles.FileNames;
            await Task.Run(() =>
            {
                foreach (string file in SelectedFiles)
                {
                    Invoke(new Action(() =>
                    {
                        Box_SelectedFiles_Enc.Items.Add(file);
                    }));
                }
            });
            int Count = Box_SelectedFiles_Enc.Items.Count;
            await Task.Run(async () =>
            {
                for (int i = InitialCount; i <= Count; i++)
                {
                    Invoke(new Action(() =>
                    {
                        Lb_FileNum_Enc.Text = "Number of Files Ready: " + i.ToString();
                    }));
                    await Task.Delay(1); // Simulate some delay for UI update
                }
            });
        }

        private async void Btn_ClearFiles_Enc_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                int InitialCount = Box_SelectedFiles_Enc.Items.Count;
                Invoke(new Action(() =>
                {
                    Box_SelectedFiles_Enc.Items.Clear();
                }));
                for (int i = InitialCount; i >= 0; i--)
                {
                    Invoke(new Action(() =>
                    {
                        Lb_FileNum_Enc.Text = "Number of Files Ready: " + i.ToString();
                    }));
                    Thread.Sleep(1); // Simulate some delay for UI update
                }
            });
        }

        private void ChkBox_IOLock_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkBox_IOLock.Checked)
            {
                if (MessageBox.Show("Enabling IO-Lock may render your files unmovable and inaccessible. Are you sure you want to continue?", "Possible side-effects", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    ChkBox_IOLock.Checked = false;
                    return;
                }
            }
        }

        private async void Btn_Encrypt_Click(object sender, EventArgs e)
        {
            if (Box_SelectedFiles_Enc.Items.Count == 0 | TxtBox_Pass_Enc.Text == string.Empty)
            {
                MessageBox.Show("Encryption is unable to be initialized. Please check that you have at least one file ready and a password filled in and then try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            await Task.Run(() =>
            {
                //Get the selected files from the list box
                List<string> SelectedFiles = [];
                foreach (var item in Box_SelectedFiles_Enc.Items)
                {
                    SelectedFiles.Add(item.ToString());
                }

                //Encryption logic goes here
                Config.SelectedFiles = SelectedFiles;
                Config.Password = TxtBox_Pass_Enc.Text;
                Config.CryptoMode = Config.CryptographyMode.Encrypt; // Set the mode to Encrypt

                var cryptographyForm = new CryptographyForm();
                cryptographyForm.ShowDialog(); //Transfer control to the CryptographyForm for processing

                //LockStream here, if ChkBox_IOLock.Checked is true
                if (ChkBox_IOLock.Checked)
                {
                    foreach (string file in SelectedFiles)
                    {
                        FileStream lockStream = Tool.InitializeLockStream(file);
                        if (lockStream != null)
                        {
                            // Add the lock stream to the dictionary
                            Config.LockStream_Dictionary.Add(file, lockStream);
                        }
                        else
                        {
                            MessageBox.Show($"Failed to lock file: {file}. The file will be skipped.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        Invoke(new Action(() =>
                        {
                            EncFile_List.Items.Add(file + " (IO-Locked)");
                        }));

                        //Save info to Config.cs
                        Config.Property_FileLastEncrypted.Add(file, DateTime.Now);
                        Config.Property_FileLastLockstreamed.Add(file, DateTime.Now);
                    }
                }
                else
                {
                    foreach (string file in SelectedFiles)
                    {
                        Invoke(new Action(() =>
                        {
                            EncFile_List.Items.Add(file);
                        }));

                        //Save info to Config.cs
                        Config.Property_FileLastEncrypted.Add(file, DateTime.Now); //No need for IO-Lock, since the file is not locked
                    }
                }

                //Clean up
                Config.SelectedFiles.Clear();
                Config.Password = string.Empty;
                Config.CryptoMode = Config.CryptographyMode.Encrypt; // Reset the mode to Encrypt for future operations
            });
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Config.LockStream_Dictionary.Count > 0)
            {
                if (MessageBox.Show("There are still locked files. Do you want to close the application anyway?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
                // Dispose all lock streams
                try
                {
                    foreach (var lockStream in Config.LockStream_Dictionary.Values)
                    {
                        lockStream.Dispose();
                        lockStream.Close();
                    }
                    Config.LockStream_Dictionary.Clear();
                    Config.SelectedFiles.Clear();
                    Config.Password = string.Empty;
                    Config.Property_FileLastLockstreamed.Clear();
                    Config.Property_FileLastEncrypted.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error while closing lock streams: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else { }
        }

        private async void Btn_AddFiles_Dec_Click(object sender, EventArgs e)
        {
            int InitialCount = Box_SelectedFiles_Dec.Items.Count;
            Dlg_OpenFiles.ShowDialog();
            string[] SelectedFiles = Dlg_OpenFiles.FileNames;
            await Task.Run(() =>
            {
                foreach (string file in SelectedFiles)
                {
                    Invoke(new Action(() =>
                    {
                        Box_SelectedFiles_Dec.Items.Add(file);
                    }));
                }
            });
            int Count = Box_SelectedFiles_Dec.Items.Count;
            await Task.Run(async () =>
            {
                for (int i = InitialCount; i <= Count; i++)
                {
                    Invoke(new Action(() =>
                    {
                        Lb_FileNum_Dec.Text = "Number of Files Ready: " + i.ToString();
                    }));
                    await Task.Delay(1); // Simulate some delay for UI update
                }
            });
        }

        private async void Btn_Clear_Dec_Click(object sender, EventArgs e)
        {
            await Task.Run(async () =>
            {
                int InitialCount = Box_SelectedFiles_Dec.Items.Count;
                Invoke(new Action(() =>
                {
                    Box_SelectedFiles_Dec.Items.Clear();
                }));
                for (int i = InitialCount; i >= 0; i--)
                {
                    Invoke(new Action(() =>
                    {
                        Lb_FileNum_Dec.Text = "Number of Files Ready: " + i.ToString();
                    }));
                    await Task.Delay(1); // Simulate some delay for UI update
                }
            });
        }

        private async void Btn_Decrypt_Click(object sender, EventArgs e)
        {
            if (Box_SelectedFiles_Dec.Items.Count == 0 | TxtBox_Pass_Dec.Text == string.Empty)
            {
                MessageBox.Show("Decryption is unable to be initialized. Please check that you have at least one file ready and a password filled in and then try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            await Task.Run(() =>
            {

                //get the selected files from the list box
                List<string> SelectedFiles = [];
                foreach (var item in Box_SelectedFiles_Dec.Items)
                {
                    SelectedFiles.Add(item.ToString());
                }

                //Unlock relevant lock streams and delete related entries from the File Management tab
                foreach (string file in SelectedFiles)
                {
                    if (Config.LockStream_Dictionary.TryGetValue(file, out FileStream? value))
                    {
                        //Unleash the lock stream
                        value.Dispose();
                        value.Close();
                        Config.LockStream_Dictionary.Remove(file);

                        //Remove info from config.cs
                        Config.Property_FileLastLockstreamed.Remove(file);
                        Config.Property_FileLastEncrypted.Remove(file);
                        continue;
                    }
                    else
                    {
                        Config.Property_FileLastEncrypted.Remove(file); //If the file is not locked, just remove the last encrypted date
                    }

                    //Update the File Management tab
                    if (EncFile_List.Items.Contains(file) || EncFile_List.Items.Contains(file + " (IO-Locked)")) //Bug: File is locked, but not in the listbox
                    {
                        Invoke(new Action(() =>
                        {
                            EncFile_List.Items.Remove(file);
                        }));
                    }
                }

                //Decryption logic goes here
                Config.SelectedFiles = SelectedFiles;
                Config.Password = TxtBox_Pass_Dec.Text;
                Config.CryptoMode = Config.CryptographyMode.Decrypt; // Set the mode to Decrypt

                var cryptographyForm = new CryptographyForm();
                cryptographyForm.ShowDialog(); //Transfer control to the CryptographyForm for processing

                //Clean up
                Config.SelectedFiles.Clear();
                Config.Password = string.Empty;
                Config.CryptoMode = Config.CryptographyMode.Encrypt; // Reset the mode to Encrypt for future operations
            });
        }

        private void EncFile_List_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            string itemText = EncFile_List.Items[e.Index].ToString()!;
            Color textColor = itemText.Contains("(IO-Locked)") ? Color.Green : Color.Black;

            e.DrawBackground();
            using (Brush brush = new SolidBrush(textColor))
            {
                e.Graphics.DrawString(itemText, e.Font, brush, e.Bounds);
            }
            e.DrawFocusRectangle();
        }

        private void EncFile_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EncFile_List.SelectedItems.Count == 1 & EncFile_List.Items.Count > 0) //Ensures no buggy behavior when no items present while one is selected (possibly null)
            {
                Btn_ViewProperties.Enabled = true;
                ToolTip_Btn_ViewProperties.Active = false;
            }
            else
            {
                Btn_ViewProperties.Enabled = false;
                ToolTip_Btn_ViewProperties.Active = true;
            }

            Lb_NumSelected_FileManage.Text = "Number of files selected: " + EncFile_List.SelectedItems.Count.ToString();
        }

        private async void Btn_LockstreamCtrl_Click(object sender, EventArgs e)
        {
            //Check if the ListBox is empty
            if (EncFile_List.SelectedItems.Count == 0)
            {
                MessageBox.Show("There are no locked files to control.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //Define lists for locked and unlocked files
            List<string> LockedFiles = [];
            List<string> UnlockedFiles = [];
            List<string> errorMessages = [];

            //Lock all controls
            Btn_LockstreamCtrl.Enabled = false;
            Btn_Encrypt.Enabled = false;
            Btn_Decrypt.Enabled = false;
            Btn_AddFiles_Enc.Enabled = false;
            Btn_AddFiles_Dec.Enabled = false;
            Btn_ClearFiles_Enc.Enabled = false;
            Btn_Clear_Dec.Enabled = false;
            Btn_ViewProperties.Enabled = false;
            ToolTip_Btn_ViewProperties.Active = false;


            //Get the locked files from the ListBox and remove the "(IO-Locked)" suffix
            List<string> LockedFiles_Original = EncFile_List.SelectedItems.Cast<string>()
                .Where(item => item.Contains("(IO-Locked)"))
                .ToList();
            foreach (string file in LockedFiles_Original)
            {
                string fileName = file.Replace(" (IO-Locked)", "");
                LockedFiles.Add(fileName);
            }

            //The same for unlocked files
            List<string> UnlockedFiles_Original = EncFile_List.SelectedItems.Cast<string>()
                .Where(item => !item.Contains("(IO-Locked)"))
                .ToList();
            foreach (string file in UnlockedFiles_Original)
            {
                string fileName = file.Replace(" (IO-Locked)", "");
                UnlockedFiles.Add(fileName);
            }

            await Task.Run(() =>
            {
                //Lock the unlocked files first
                foreach (string file in UnlockedFiles)
                {
                    if (Config.LockStream_Dictionary.ContainsKey(file))
                    {
                        continue; //Make sure to skip files that are already locked (avoid edgy cases)
                    }
                    FileStream lockStream = Tool.InitializeLockStream(file);
                    if (lockStream != null)
                    {
                        Config.LockStream_Dictionary.Add(file, lockStream);
                        Config.Property_FileLastLockstreamed.Add(file, DateTime.Now); // Save the lock time
                        Invoke(new Action(() =>
                        {
                            EncFile_List.Items.Remove(file); // Remove the original entry
                            EncFile_List.Items.Add(file + " (IO-Locked)"); // Add the locked entry with "(IO-Locked)" suffix
                        }));
                    }
                    else
                    {
                        errorMessages.Add($"Failed to lock file: {file}. The file will be skipped.");
                    }
                }

                //Same for locked files-Unlock them
                foreach (string file in LockedFiles)
                {
                    if (!Config.LockStream_Dictionary.TryGetValue(file, out FileStream? lockStream))
                    {
                        continue; //Make sure to skip files that are not locked
                    }

                    lockStream.Dispose();
                    lockStream.Close();
                    Config.LockStream_Dictionary.Remove(file);

                    Invoke(new Action(() =>
                    {
                        EncFile_List.Items.Remove(file + " (IO-Locked)"); // Remove the locked entry
                        EncFile_List.Items.Add(file); // Add the unlocked entry without "(IO-Locked)" suffix
                    }));
                }
            });

            //Release all controls
            Btn_LockstreamCtrl.Enabled = true;
            Btn_Encrypt.Enabled = true;
            Btn_Decrypt.Enabled = true;
            Btn_AddFiles_Enc.Enabled = true;
            Btn_AddFiles_Dec.Enabled = true;
            Btn_ClearFiles_Enc.Enabled = true;
            Btn_Clear_Dec.Enabled = true;
            if (EncFile_List.SelectedItems.Count == 1)
            {
                Btn_ViewProperties.Enabled = true;
                ToolTip_Btn_ViewProperties.Active = false;
            }
            else
            {
                Btn_ViewProperties.Enabled = false;
                ToolTip_Btn_ViewProperties.Active = true;
            }

            //Show a message box indicating the end of the operation
            if (errorMessages.Count > 0)
            {
                string errorMessage = string.Join(Environment.NewLine, errorMessages);
                MessageBox.Show($"Some files could not be locked or unlocked:{Environment.NewLine}{errorMessage}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Lock/Unlock operation completed successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void Btn_Delete_Click(object sender, EventArgs e)
        {
            //Define some variables first
            List<string> LockedFiles = [];
            List<string> UnlockedFiles = [];
            List<string> errorMessages = [];
            byte[] randomData;

            await Task.Run(() =>
            {
                //Get and differentiate the locked and unlocked files from the ListBox
                List<string> LockedFiles_Original = EncFile_List.SelectedItems.Cast<string>()
                .Where(item => item.Contains("(IO-Locked)"))
                .ToList();
                foreach (string file in LockedFiles_Original)
                {
                    string fileName = file.Replace(" (IO-Locked)", "");
                    LockedFiles.Add(fileName);
                }

                //The same for unlocked files
                List<string> UnlockedFiles_Original = EncFile_List.SelectedItems.Cast<string>()
                    .Where(item => !item.Contains("(IO-Locked)"))
                    .ToList();
                foreach (string file in UnlockedFiles_Original)
                {
                    string fileName = file.Replace(" (IO-Locked)", "");
                    UnlockedFiles.Add(fileName);
                }

                //Check if there are any files to delete
                if (LockedFiles.Count == 0 && UnlockedFiles.Count == 0)
                {
                    errorMessages.Add("There are no files selected for deletion.");
                    return;
                }

                //Unlock all locked files first
                foreach (string file in LockedFiles)
                {
                    if (Config.LockStream_Dictionary.TryGetValue(file, out FileStream? lockStream))
                    {
                        //Unleash the lock stream
                        lockStream.Dispose();
                        lockStream.Close();
                        Config.LockStream_Dictionary.Remove(file);

                        //Remove info from config.cs
                        Config.Property_FileLastLockstreamed.Remove(file);
                        Config.Property_FileLastEncrypted.Remove(file);
                    }
                    else
                    {
                        Config.Property_FileLastEncrypted.Remove(file); //If the file is not locked, just remove the last encrypted date
                        continue;
                    }
                }

                //Now we have both lists ready, we can delete the files
                //Check if the user wants to securely shred the files
                if (ChkBox_SecureShred.Checked)
                {
                    foreach (string file in LockedFiles)
                    {
                        try
                        {
                            //Get the file size
                            long fileSize = new FileInfo(file).Length;

                            //Get random data
                            if (fileSize <= int.MaxValue)
                            {
                                randomData = Tool.GenerateRandomBytes((int)fileSize);
                            }
                            else
                            {
                                using (MemoryStream ms = new())
                                {
                                    Tool.GenerateRandomBytes(fileSize, ms);
                                    randomData = ms.ToArray();
                                }
                            }

                            //Overwrite the file with random data
                            File.WriteAllBytes(file, randomData);

                            //Delete the file
                            File.Delete(file);
                        }
                        catch (Exception ex)
                        {
                            errorMessages.Add($"Failed to delete file: {file}. Error: {ex.Message}");
                        }
                    }
                    foreach (string file in UnlockedFiles)
                    {
                        try
                        {
                            //Get the file size
                            long fileSize = new FileInfo(file).Length;

                            //Get random data
                            if (fileSize <= int.MaxValue)
                            {
                                randomData = Tool.GenerateRandomBytes((int)fileSize);
                            }
                            else
                            {
                                using (MemoryStream ms = new())
                                {
                                    Tool.GenerateRandomBytes(fileSize, ms);
                                    randomData = ms.ToArray();
                                }
                            }

                            //Overwrite the file with random data
                            File.WriteAllBytes(file, randomData);

                            //Delete the file
                            File.Delete(file);
                        }
                        catch (Exception ex)
                        {
                            errorMessages.Add($"Failed to delete file: {file}. Error: {ex.Message}");
                        }
                    }
                }
                else
                {
                    foreach (string file in LockedFiles)
                    {
                        try
                        {
                            File.Delete(file);
                        }
                        catch (Exception ex)
                        {
                            errorMessages.Add($"Failed to delete file: {file}. Error: {ex.Message}");
                        }
                    }
                    foreach (string file in UnlockedFiles)
                    {
                        try
                        {
                            File.Delete(file);
                        }
                        catch (Exception ex)
                        {
                            errorMessages.Add($"Failed to delete file: {file}. Error: {ex.Message}");
                        }
                    }
                }

                //Update the ListBox after deletion
                Invoke(new Action(() =>
                {
                    foreach (string file in LockedFiles_Original)
                    {
                        EncFile_List.Items.Remove(file);
                    }
                    foreach (string file in UnlockedFiles_Original)
                    {
                        EncFile_List.Items.Remove(file);
                    }
                }));
            });

            //Show a message box indicating the end of the operation
            if (errorMessages.Count > 0)
            {
                string errorMessage = string.Join(Environment.NewLine, errorMessages);
                MessageBox.Show($"Some files could not be deleted:{Environment.NewLine}{errorMessage}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Delete operation completed successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Btn_ViewProperties_Click(object sender, EventArgs e)
        {
            //Coding still in progress XD
            //We already have EncFile_List_SelectedIndexChanged() handle the case of selected item num not equal 1
            //So we don't need to check it here

            //Filter the "IO-Locked" suffix from the selected item
            string selectedItem = EncFile_List.SelectedItem!.ToString()!;
            if (selectedItem.Contains("(IO-Locked)"))
            {
                selectedItem = selectedItem.Replace(" (IO-Locked)", "");
            }

            //Set configs, and then open PropertyForm
            Config.Property_FilePath = selectedItem;
            Config.Property_FileIsLocked = EncFile_List.SelectedItem.ToString()!.Contains("(IO-Locked)");

            //Check if the file is locked. If locked, then unleash temporarily. After we have the information, we will lock it again.
            if (Config.LockStream_Dictionary.TryGetValue(Config.Property_FilePath, out FileStream? lockStream))
            {
                try
                {
                    //Close the lock stream to unlock the file temporarily
                    lockStream.Dispose();
                    lockStream.Close();
                    Config.LockStream_Dictionary.Remove(Config.Property_FilePath); // Remove the lock stream from the dictionary, temporarily
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error unlocking the file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            //Check if the file exists
            if (!File.Exists(Config.Property_FilePath))
            {
                MessageBox.Show("The specified file does not exist. Please check the file path and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Open the PropertyForm
            PropertyForm propertyForm = new();
            propertyForm.ShowDialog();
        }
    }
}

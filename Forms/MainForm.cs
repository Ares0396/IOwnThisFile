using Main.Support_Tools;
using System.Reflection;

namespace Main
{
    public partial class MainForm : Form
    {
        private readonly AppSetting appSetting;
        private bool formBeingInitialized = true; //Default
        public MainForm(AppSetting appSetting)
        {
            InitializeComponent();
            this.appSetting = appSetting;

            //Update title
            Version currentVer = Assembly.GetExecutingAssembly().GetName().Version!;
            Text = $"IOwnThisFile v{currentVer.Major}.{currentVer.Minor}.{currentVer.Build}";

            //Apply settings from appSetting to the UI elements
            ApplySettings(appSetting);
        }

        private async void Btn_AddFiles_Enc_Click(object sender, EventArgs e)
        {
            int InitialCount = Box_SelectedFiles_Enc.Items.Count;

            Dlg_OpenFiles.ShowDialog();
            string[] SelectedFiles = Dlg_OpenFiles.FileNames;

            Box_SelectedFiles_Enc.Items.AddRange(SelectedFiles);
            int Count = Box_SelectedFiles_Enc.Items.Count;

            for (int i = InitialCount; i <= Count; i++)
            {
                Invoke(new Action(() =>
                {
                    Lb_FileNum_Enc.Text = "Number of Files Ready: " + i.ToString();
                }));
                await Task.Delay(1); // Simulate some delay for UI update
            }
        }

        private async void Btn_ClearFiles_Enc_Click(object sender, EventArgs e)
        {
            int InitialCount = Box_SelectedFiles_Enc.Items.Count;
            Box_SelectedFiles_Enc.Items.Clear();
            for (int i = InitialCount; i >= 0; i--)
            {
                Lb_FileNum_Enc.Text = "Number of Files Ready: " + i.ToString();
                await Task.Delay(1);
            }
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
                List<string> SelectedFiles = [.. Box_SelectedFiles_Enc.Items.Cast<string>()];

                //Encryption logic goes here
                Config.SelectedFiles = SelectedFiles;
                Config.Password = TxtBox_Pass_Enc.Text;
                Config.CryptoMode = Config.CryptographyMode.Encrypt; // Set the mode to Encrypt

                var cryptographyForm = new CryptographyForm(appSetting);
                cryptographyForm.ShowDialog(); //Transfer control to the CryptographyForm for processing

                //LockStream here, if ChkBox_IOLock.Checked is true
                if (ChkBox_IOLock.Checked)
                {
                    foreach (string file in SelectedFiles)
                    {
                        FileStream lockStream = LockStream.Initialize(file);
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
                        Config.Property_FileLastEncrypted[file] = DateTime.Now;
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
            Box_SelectedFiles_Dec.Items.AddRange(SelectedFiles);
            int Count = Box_SelectedFiles_Dec.Items.Count;
            for (int i = InitialCount; i <= Count; i++)
            {
                Lb_FileNum_Dec.Text = "Number of Files Ready: " + i.ToString();
                await Task.Delay(1); // Simulate some delay for UI update
            }
        }

        private async void Btn_Clear_Dec_Click(object sender, EventArgs e)
        {
            int InitialCount = Box_SelectedFiles_Dec.Items.Count;
            Box_SelectedFiles_Dec.Items.Clear();
            for (int i = InitialCount; i >= 0; i--)
            {
                Lb_FileNum_Dec.Text = "Number of Files Ready: " + i.ToString();
                await Task.Delay(1); // Simulate some delay for UI update
            }
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
                List<string> SelectedFiles = [.. Box_SelectedFiles_Dec.Items.Cast<string>()];

                //Unlock relevant lock streams and delete related entries from the File Management tab
                foreach (string file in SelectedFiles)
                {
                    if (Config.LockStream_Dictionary.TryGetValue(file, out var value))
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

                var cryptographyForm = new CryptographyForm(appSetting);
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

            // Get the item text
            string itemText = EncFile_List.Items[e.Index].ToString()!;

            // Define your background theme flag
            bool IsLightBackground = appSetting.App_Theme == AppSetting.InternalTheme.Light; // or get this value from elsewhere

            // Base text color depending on whether the background is light or dark
            Color defaultTextColor = IsLightBackground ? Color.Black : Color.White;

            // Special case for "(IO-Locked)" items
            Color lockedTextColor = IsLightBackground ? Color.DarkGreen : Color.LightGreen;

            // Choose text color based on the item
            Color textColor = itemText.Contains("(IO-Locked)") ? lockedTextColor : defaultTextColor;

            // Draw the background
            e.DrawBackground();

            // Draw the text
            using (Brush brush = new SolidBrush(textColor))
            {
                e.Graphics.DrawString(itemText, e.Font, brush, e.Bounds);
            }

            // Draw focus rectangle if needed
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
                    FileStream lockStream = LockStream.Initialize(file);
                    if (lockStream != null)
                    {
                        Config.LockStream_Dictionary.Add(file, lockStream);

                        //Bug fixed: No more same key with 2 values
                        Config.Property_FileLastLockstreamed[file] = DateTime.Now;

                        Invoke(new Action(() =>
                        {
                            int index = EncFile_List.Items.IndexOf(file); //Get index

                            EncFile_List.Items.Remove(file); // Remove the original entry
                            EncFile_List.Items.Insert(index, file + " (IO-Locked)"); // Add the locked entry with "(IO-Locked)" suffix
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
                    if (!Config.LockStream_Dictionary.TryGetValue(file, out var lockStream))
                    {
                        continue; //Make sure to skip files that are not locked
                    }

                    lockStream.Dispose();
                    lockStream.Close();
                    Config.LockStream_Dictionary.Remove(file);

                    Invoke(new Action(() =>
                    {
                        int index = EncFile_List.Items.IndexOf(file + " (IO-Locked)"); //Get index

                        EncFile_List.Items.Remove(file + " (IO-Locked)"); // Remove the locked entry
                        EncFile_List.Items.Insert(index, file); // Add the unlocked entry without "(IO-Locked)" suffix
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
                if (Config.LockStream_Dictionary.TryGetValue(file, out var lockStream))
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

                        using (FileStream fs = new(file, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                        {
                            await CryptographicOperation.GenerateRandomBytesAsync(fileSize, fs); //Write random data
                        }

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

                        using (FileStream fs = new(file, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                        {
                            await CryptographicOperation.GenerateRandomBytesAsync(fileSize, fs); //Write random data
                        }

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
            if (Config.LockStream_Dictionary.TryGetValue(Config.Property_FilePath, out var lockStream))
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
        private void LkLb_ImportSettingFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Dlg_OpenFiles_Setting.ShowDialog(); //Get the setting file
            AppSetting importedSettings = AppSetting.LoadSettings(Dlg_OpenFiles_Setting.FileName); //Import settings

            try
            {
                ApplySettings(importedSettings); //Apply settings to the UI elements
            }
            catch
            {
                MessageBox.Show("Failed to apply the imported settings. The file may be corrupted or incompatible.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void ChkBox_SettingReg_CheckedChanged(object sender, EventArgs e)
        {
            //Since we can only have one settings storage method, we need to uncheck the other checkbox
            if (ChkBox_SettingReg.Checked)
            {
                ChkBox_SettingFile.Checked = false;
            }
            else { }
            Config.SettingsChanged = true; //Mark that settings have changed
            SetLb_SaveSettingStatus(false); //Mark that settings have changed and need to be saved
        }
        private void ChkBox_SettingFile_CheckedChanged(object sender, EventArgs e)
        {
            //Since we can only have one settings storage method, we need to uncheck the other checkbox
            if (ChkBox_SettingFile.Checked)
            {
                ChkBox_SettingReg.Checked = false;
            }
            else { }
            Config.SettingsChanged = true; //Mark that settings have changed
            SetLb_SaveSettingStatus(false); //Mark that settings have changed and need to be saved
        }
        private void Btn_SaveSetting_Click(object sender, EventArgs e)
        {
            if (!Config.SettingsChanged) //If settings are saved already, do nothing
            {
                return;
            }

            //Determine which saving method to use
            if (ChkBox_SettingFile.Checked)
            {
                if (appSetting.App_SettingSaveMode_FilePath == string.Empty || !File.Exists(appSetting.App_SettingSaveMode_FilePath)) Dlg_SaveFiles_Setting.ShowDialog();
                if (Dlg_SaveFiles_Setting.FileName == string.Empty)
                {
                    //User cancelled the save dialog
                    SetLb_SaveSettingStatus(false);
                }
                appSetting.App_SettingSaveMode_FilePath = Dlg_SaveFiles_Setting.FileName; //Set the file path
                appSetting.App_SettingSaveMode = AppSetting.InternalSettingSaveMode.File; //Set the saving method
                AppSetting.SaveSettingsFile(appSetting, Dlg_SaveFiles_Setting.FileName); //Save settings to file

                //Save file path to registry here
                AppSetting.SaveSettingsRegistry(new AppSetting() { App_SettingSaveMode_FilePath = Dlg_SaveFiles_Setting.FileName, App_SettingSaveMode = AppSetting.InternalSettingSaveMode.File });

                SetLb_SaveSettingStatus(true);
                Config.SettingsChanged = false; //Mark that settings have been saved
            }
            else if (ChkBox_SettingReg.Checked)
            {
                appSetting.App_SettingSaveMode = AppSetting.InternalSettingSaveMode.Registry; //Set the saving method
                AppSetting.SaveSettingsRegistry(appSetting); //Save settings to registry
                SetLb_SaveSettingStatus(true);
                Config.SettingsChanged = false; //Mark that settings have been saved
            }
            else
            {
                SetLb_SaveSettingStatus(false);
                MessageBox.Show("Please select a settings storage method before saving settings.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        private void ChkBox_AllOutWriteMode_CheckedChanged_1(object sender, EventArgs e)
        {
            if (ChkBox_AllOutWriteMode.Checked)
            {
                if (!formBeingInitialized)
                {
                    if (MessageBox.Show("All-Out Write Mode is a potentially dangerous mode that may slow your computer. Are you sure you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        appSetting.SmartWriteSelector_AllOutMode = true; //Enable All-Out Write Mode (TODO)
                        NumUpDown_NumParallelProcessing.Enabled = false; //Disable Parallel Processing Number Thread selection (TODO)
                        return;
                    }
                    ChkBox_AllOutWriteMode.Checked = false;
                }
                else
                {
                    appSetting.SmartWriteSelector_AllOutMode = true; //Enable All-Out Write Mode (TODO)
                    NumUpDown_NumParallelProcessing.Enabled = false; //Disable Parallel Processing Number Thread selection (TODO)
                    formBeingInitialized = false;
                    return;
                }
            }
            else
            {
                //Disable All-Out Write Mode (TODO)
                appSetting.SmartWriteSelector_AllOutMode = false;

                //Enable Parallel Processing Number Thread selection (TODO)
                NumUpDown_NumParallelProcessing.Enabled = true;
            }
            Config.SettingsChanged = true; //Mark that settings have changed
            SetLb_SaveSettingStatus(false); //Mark that settings have changed and need to be saved
        }
        private void ChkBox_CheckUpdate_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkBox_CheckUpdate.Checked)
            {
                //Enable Retry Count selection
                NumUpDown_UpdateAttemptCount.Enabled = true;
                appSetting.UpdateChecker_AutoCheck = true;
            }
            else
            {
                //Disable Retry Count selection
                NumUpDown_UpdateAttemptCount.Enabled = false;
                appSetting.UpdateChecker_AutoCheck = false;
            }
            Config.SettingsChanged = true; //Mark that settings have changed
            SetLb_SaveSettingStatus(false); //Mark that settings have changed and need to be saved
        }
        private void CbBox_Theme_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbBox_Theme.SelectedItem == null) return; //Ensures no buggy behavior when no item is selected (possibly null)
            if (CbBox_Theme.SelectedItem.ToString() == "Light")
            {
                appSetting.App_Theme = AppSetting.InternalTheme.Light;
                SetTheme(AppSetting.InternalTheme.Light);
            }
            else if (CbBox_Theme.SelectedItem.ToString() == "Dark")
            {
                appSetting.App_Theme = AppSetting.InternalTheme.Dark;
                SetTheme(AppSetting.InternalTheme.Dark);
            }
            else
            {
                //Should never reach here
                MessageBox.Show("An unexpected error occurred while changing the theme. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Config.SettingsChanged = true; //Mark that settings have changed
            SetLb_SaveSettingStatus(false); //Mark that settings have changed and need to be saved
        }

        private void ApplySettings(AppSetting appSetting)
        {
            //Saving mode
            if (appSetting.App_SettingSaveMode == AppSetting.InternalSettingSaveMode.File)
            {
                ChkBox_SettingFile.Checked = true;
            }
            else if (appSetting.App_SettingSaveMode == AppSetting.InternalSettingSaveMode.Registry)
            {
                ChkBox_SettingReg.Checked = true;
            }

            //Check update
            if (appSetting.UpdateChecker_AutoCheck)
            {
                ChkBox_CheckUpdate.Checked = true;
            }
            else
            {
                ChkBox_CheckUpdate.Checked = false;
            }

            //Update attempt count
            NumUpDown_UpdateAttemptCount.Value = appSetting.UpdateChecker_RetryMaxCount;

            //Theme
            if (appSetting.App_Theme == AppSetting.InternalTheme.Light)
            {
                CbBox_Theme.SelectedIndex = 0; //Light
                SetTheme(AppSetting.InternalTheme.Light);
            }
            else if (appSetting.App_Theme == AppSetting.InternalTheme.Dark)
            {
                CbBox_Theme.SelectedIndex = 1; //Dark
                SetTheme(AppSetting.InternalTheme.Dark);

            }

            //Smart Write Selector Threshold
            NumUpDown_NumParallelProcessing.Value = appSetting.SmartWriteSelector_Threshold;

            //All-Out Write Mode
            if (appSetting.SmartWriteSelector_AllOutMode)
            {
                ChkBox_AllOutWriteMode.Checked = true;
            }
            else
            {
                ChkBox_AllOutWriteMode.Checked = false;
            }

            //Cb_Optimize
            CbBox_Optimize.SelectedIndex = appSetting.OptimizeSpeedOrRes;

            SetLb_SaveSettingStatus(true);
            Config.SettingsChanged = false;
        }
        private void SetTheme(AppSetting.InternalTheme theme)
        {
            AppSetting.SetTheme(this, theme);
            if (appSetting.App_Theme == AppSetting.InternalTheme.Light)
            {
                this.BackColor = Config.Color_LightBackground;
                EncFile_List.BackColor = Config.Color_LightControlBackground;
                EncFile_List.ForeColor = Config.Color_LightTextColor;
            }
            else
            {
                this.BackColor = Config.Color_Background;
                EncFile_List.BackColor = Config.Color_ControlBackground;
                EncFile_List.ForeColor = Config.Color_TextColor;
            }
        }
        private void SetLb_SaveSettingStatus(bool settingSaved)
        {
            if (settingSaved)
            {
                Lb_SaveSettingStatus.Text = "Settings saved successfully.";
                Lb_SaveSettingStatus.Location = new Point(490, 51);
                Lb_SaveSettingStatus.ForeColor = Color.LimeGreen;
            }
            else
            {
                Lb_SaveSettingStatus.Text = "Be careful! You haven't saved your settings yet!";
                Lb_SaveSettingStatus.Location = new Point(384, 53);
                Lb_SaveSettingStatus.ForeColor = Color.Red;
            }
        }

        private void NumUpDown_NumParallelProcessing_ValueChanged(object sender, EventArgs e)
        {
            appSetting.SmartWriteSelector_Threshold = (int)NumUpDown_NumParallelProcessing.Value;
            Config.SettingsChanged = true;
            SetLb_SaveSettingStatus(false);
        }

        private void NumUpDown_UpdateAttemptCount_ValueChanged(object sender, EventArgs e)
        {
            appSetting.UpdateChecker_RetryMaxCount = (int)NumUpDown_UpdateAttemptCount.Value;
            SetLb_SaveSettingStatus(false);
            Config.SettingsChanged = true;
        }

        private void CbBox_Optimize_SelectedIndexChanged(object sender, EventArgs e)
        {
            appSetting.OptimizeSpeedOrRes = CbBox_Optimize.SelectedIndex;
            SetLb_SaveSettingStatus(false);
            Config.SettingsChanged = true;
        }
    }
}

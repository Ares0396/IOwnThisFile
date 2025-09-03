using Main.Support_Tools;
using Microsoft.Win32;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;

namespace Main
{
    public partial class UpdateForm : Form
    {
        private readonly CancellationTokenSource cts = new();

        byte[]? exeContent;
        // Change the declaration of appSetting to nullable to fix CS8618
        AppSetting? appSetting;
        public UpdateForm()
        {
            InitializeComponent();
        }
        private async void UpdateForm_Load(object sender, EventArgs e)
        {
            await Task.Delay(1000); //Simulate loading time

            //Do some background work here
            RegistryKey? key = Registry.CurrentUser.OpenSubKey(Config.Reg_AppSettingPath);
            if (key != null)
            {
                appSetting = AppSetting.LoadSettings(); //Load settings
                AppSetting.SetTheme(this, appSetting.App_Theme); //Apply theme
            }
            else
            {
                AppSetting.InitializeDefaultRegistrySettings(); //Ensure default settings are set for first-run
                appSetting = AppSetting.GetDefaultAppSettings(); //Load default settings
            }

            //Check if Check-for-update is enabled
            if (!appSetting.UpdateChecker_AutoCheck)
            {
                //Skip update check, set labels and start app
                Lb_UpdateStatus.Text = "Update disabled. Starting app...";
                Lb_UpdateStatus.Location = new Point(62, 54);
                await Task.Delay(1000);

                Form NewForm = new MainForm(appSetting);
                Hide();
                NewForm.ShowDialog();
                Close();
                return; //Exit this function
            }

            //Check update mode
            if (Config.updateMode == Config.UpdateMode.Phase1)
            {
                Form NewForm = new MainForm(appSetting);

                //Set the panel visibility
                Pn_UpdateCheck.Visible = true;
                Pn_UpdateFound.Visible = false;
                Pn_Update.Visible = false;

                //Set label
                Lb_UpdateStatus.Text = "Checking for updates...";
                Lb_UpdateStatus.Location = new Point(101, 54);
                await Task.Delay(1000);

                //Check update
                try
                {
                    using (HttpClient client = new())
                    {
                        //Get raw version info from URL
                        string url = Config.UpdateChecker_URL;
                        string getVersionInfo = await client.GetStringAsync(url);

                        //Parse the version info
                        Config.versionInfoLines = getVersionInfo.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);

                        //Convert string to version
                        Config.latestVersion_String = Config.versionInfoLines[0]; //The VersionInfo.txt format is: first line is version, second line is released date
                        Config.latestVersion = new(Config.latestVersion_String);

                        //Get current version
                        Config.currentVersion = Assembly.GetExecutingAssembly().GetName().Version!;
                    }
                    if (Config.latestVersion > Config.currentVersion)
                    {
                        //Set properties
                        Lb_LatestVer.Text += Config.latestVersion.ToString();
                        Lb_ReleasedDate.Text += Config.versionInfoLines[1];
                        Lb_CurrentVer.Text += Config.currentVersion.ToString();

                        //Hide the CheckUpdate panel and show the CheckFound panel
                        Pn_UpdateCheck.Visible = false;
                        Pn_UpdateFound.Visible = true;
                        Pn_Update.Visible = false;
                    }
                    else
                    {
                        //Latest update here
                        //Inform user
                        Lb_UpdateStatus.Text = "No new update available. Starting app...";
                        Lb_UpdateStatus.Location = new Point(28, 54);
                        await Task.Delay(1000);

                        //Start app
                        Hide();
                        NewForm.ShowDialog();
                        Close();
                    }
                }
                catch
                {
                    //It means failure to check for updates, we can ignore it.
                    //Inform user
                    Lb_UpdateStatus.Text = "Failed to check for update. Starting app...";
                    Lb_UpdateStatus.Location = new Point(20, 54);
                    await Task.Delay(1000);

                    //Start app
                    Hide();
                    NewForm.ShowDialog();
                    Close();
                }
            }
            else if (Config.updateMode == Config.UpdateMode.Phase2)
            {
                Pn_UpdateCheck.Visible = false;
                Pn_UpdateFound.Visible = false;
                Pn_Update.Visible = true;

                //Inform user
                Lb_UpdateProgress.Text = "Update Status: Progressing...";
                await Task.Delay(1000);
                Lb_UpdateProgress.Text = "Update Status: Copying new file...";
                await Task.Delay(1000);

                //Phase2
                File.Copy(Application.ExecutablePath, Config.ExePath, true);

                //Run phase 3
                Lb_UpdateProgress.Text = "Update Status: Restarting...";
                await Task.Delay(1000);
                Process.Start(Config.ExePath, Config.Command_UpdatePhase3);
                Application.Exit();
            }
            else if (Config.updateMode == Config.UpdateMode.Phase3)
            {
                Pn_UpdateCheck.Visible = false;
                Pn_UpdateFound.Visible = false;
                Pn_Update.Visible = true;

                //Inform user
                Lb_UpdateProgress.Text = "Update Status: Cleaning up...";
                await Task.Delay(1000);

                //Clean up the temp file
                File.Delete(Config.UpdateChecker_TempFilePath);

                //Inform user that update is complete
                Lb_UpdateProgress.Text = "Update Status: All done!";
                await Task.Delay(1000);

                //Open MainForm
                Hide();
                Form mainForm = new MainForm(appSetting);
                mainForm.ShowDialog();
                Close();
            }
        }

        private async void Btn_Update_Click(object sender, EventArgs e)
        {
            int attemptCount = 0;
            bool Success = false; //Default is false
            bool Cancelled = false; //For informing user if they cancelled the update
            CancellationToken token = cts.Token;
            AppSetting appSetting = this.appSetting!; //Non-null assertion since we ensure it's loaded in UpdateForm_Load

            Lb_UpdateProgress.Text = "Update Status: Downloading..."; //Inform user

            //Set the panel visibility
            Pn_UpdateCheck.Visible = false;
            Pn_UpdateFound.Visible = false;
            Pn_Update.Visible = true;

            //Allow user to cancel update
            Btn_CancelUpdate.Enabled = true;

            try
            {
                using (HttpClient client = new())
                {
                    //Download
                    string downloadURL = $@"https://github.com/Ares0396/IOwnThisFile/releases/download/{Config.latestVersion_String}/IOwnThisFile.exe";
                    exeContent = await client.GetByteArrayAsync(downloadURL, token);

                    //Inform user
                    Lb_UpdateProgress.Text = "Update Status: Finished downloading. Verifying...";
                    await Task.Delay(1000, token);

                    //Verify hash
                    using (SHA256 hasher = SHA256.Create())
                    {
                        //Hash content
                        string localHash = await Task.Run(() => Tool.HashContent(exeContent, hasher), token);

                        //Get hash from VersionInfo.txt
                        string buildHash = Config.versionInfoLines[2]; //Hash is line 3

                        //Compare hashes
                        if (localHash == buildHash)
                        {
                            Success = true;
                            return; //Skip to the Finally block
                        }
                        else
                        {
                            //Retry
                            while (attemptCount < appSetting.UpdateChecker_RetryMaxCount && !Success)
                            {
                                token.ThrowIfCancellationRequested(); // <-- stop immediately if cancelled

                                attemptCount++; //+1 failed attempt, until it turns 3

                                //Redownload
                                exeContent = await client.GetByteArrayAsync(downloadURL, token); //The same as previous code, but the difference is that we're restarting the process

                                //Re-hash
                                localHash = await Task.Run(() => Tool.HashContent(exeContent, hasher), token);

                                //Compare again
                                if (localHash == buildHash)
                                {
                                    Success = true; //Set flag to stop loop
                                }
                            }
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Success = false; //In case of any exception, we consider it a failure
                Cancelled = true; //Set cancelled flag
            }
            catch
            {
                Success = false; //In case of any exception, we consider it a failure
            }
            finally
            {
                if (!Success)
                {
                    if (Cancelled)
                    {
                        Lb_UpdateProgress.Text = "Update Status: Update cancelled by user. Starting app...";
                        await Task.Delay(1000);

                        Hide();
                        Form NewForm = new MainForm(appSetting);
                        NewForm.ShowDialog();
                        Close();
                    }
                    else
                    {
                        Lb_UpdateProgress.Text = "Update Status: Failed. Starting app...";
                        await Task.Delay(1000);

                        Hide();
                        Form NewForm = new MainForm(appSetting);
                        NewForm.ShowDialog();
                        Close();
                    }
                }
                else
                {
                    //Too late, users can't cancel now
                    Btn_CancelUpdate.Enabled = false;
                    Lb_UpdateProgress.Text = "Update Status: Verified. Installing...";
                    await Task.Delay(1000);

                    //Extract file to temp
                    File.WriteAllBytes(Config.UpdateChecker_TempFilePath, exeContent);

                    //Continue update Phase 2
                    Process.Start(Config.UpdateChecker_TempFilePath, Config.Command_UpdatePhase2);
                    Application.Exit();
                }
            }
        }

        private void Btn_CancelUpdate_Click(object sender, EventArgs e)
        {
            Btn_CancelUpdate.Enabled = false;
            cts.Cancel(); //Signal cancellation
        }
    }
}

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
        AppSetting? appSetting;

        public UpdateForm()
        {
            InitializeComponent();
        }
        private async void UpdateForm_Load(object sender, EventArgs e)
        {
            //Do some background work here
            Pn_UpdateCheck.Visible = true;
            Pn_UpdateFound.Visible = false;
            Pn_Update.Visible = false;

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


            //Check update mode
            if (Config.updateMode == Config.UpdateMode.Phase1)
            {
                Form NewForm = new MainForm(appSetting);

                //Check if Check-for-update is enabled
                if (!appSetting.UpdateChecker_AutoCheck)
                {
                    //Skip update check, set labels and start app
                    Lb_UpdateStatus.Text = "Update disabled. Starting app...";
                    Lb_UpdateStatus.Location = new Point(62, 54);
                    await Task.Delay(1000);

                    Hide();
                    NewForm.ShowDialog();
                    Close();
                    return; //Exit this function
                }
                

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
                        Lb_CurrentVer.Text += $"{Config.currentVersion.Major}.{Config.currentVersion.Minor}.{Config.currentVersion.Build}";

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
                catch (Exception ex)
                {
                    Support_Tools.Debugger.Handle(ex, async() =>
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
                    });
                }
            }
            else if (Config.updateMode == Config.UpdateMode.Phase2)
            {
                Pn_UpdateCheck.Visible = false;
                Pn_UpdateFound.Visible = false;
                Pn_Update.Visible = true;

                //Inform user
                Lb_UpdateProgress.Text = "Update Status: Getting file ready...";
                await Task.Delay(1000);
                Lb_UpdateProgress.Text = "Update Status: Copying new file...";
                await Task.Delay(1000);

                Progress<double> copyProgress = new(p =>
                {
                    Lb_UpdateProgress.Text = $"Update Status: Copying new file... {p:P2}";
                }); //Prepare a progress instance for reporting

                using (FileStream fsSrc = new(Application.ExecutablePath, FileMode.Open, FileAccess.Read))
                using (FileStream fsDes = new(Config.ExePath, FileMode.Create, FileAccess.Write))
                {
                    await Streamer.WriteLocalStreamAsync(fsSrc, fsDes, copyProgress);
                }

                //Run phase 3
                Lb_UpdateProgress.Text = "Update Status: Done. Restarting...";
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
                await Task.Run(() => File.Delete(Config.UpdateChecker_TempFilePath));

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
            bool Success = false; //Default is false
            bool Cancelled = false; //For informing user if they cancelled the update
            CancellationToken token = cts.Token;
            AppSetting appSetting = this.appSetting!; //Non-null assertion since we ensure it's loaded in UpdateForm_Load

            Progress<double> downloadProgress = new(p =>
            {
                Lb_UpdateProgress.Text = $"Update Status: Downloading... ({p:P2})";
            });
            Progress<double> overwriteProgress = new(p =>
            {
                Lb_UpdateProgress.Text = $"Update Status: Copying new file... ({p:P2})";
            });

            //Download process starts here
            Lb_UpdateProgress.Text = "Update Status: Downloading..."; //Inform user

            //Set the panel visibility
            Pn_UpdateCheck.Visible = false;
            Pn_UpdateFound.Visible = false;
            Pn_Update.Visible = true;

            //Allow user to cancel update
            Btn_CancelUpdate.Enabled = true;

            try
            {
                string downloadURL = $@"https://github.com/Ares0396/IOwnThisFile/releases/download/{Config.latestVersion_String}/IOwnThisFile.exe";
                string buildHash = Config.versionInfoLines[2]; //Hash is line 3
                string localHash;

                //Download to file and start processing
                
                using (FileStream fs = new(Config.UpdateChecker_TempFilePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    await Streamer.WriteDownloadedDataAsync(downloadURL, fs, downloadProgress, 65536, null, appSetting.UpdateChecker_RetryMaxCount, token); //Retry is also handled here

                    Lb_UpdateProgress.Text = "Update Status: Finished downloading. Verifying...";
                    await Task.Delay(1000, token);

                    //Compute and verify hash
                    using (IncrementalHash SHA256 = IncrementalHash.CreateHash(Config.RFC_Algorithm))
                    {
                        localHash = await CryptographicOperation.ComputeHashAsync(fs, SHA256, token);
                    }

                    if (localHash == buildHash) Success = true;
                }
            }
            catch (OperationCanceledException)
            {
                Success = false; //In case of any exception, we consider it a failure
                Cancelled = true; //Set cancelled flag
                //We don't use a debugger here as it's user's intention
            }
            catch (Exception ex)
            {
                Support_Tools.Debugger.Handle(ex, () => Success = false);
            }
            finally
            {
                if (!Success)
                {
                    if (Cancelled)
                    {
                        Lb_UpdateProgress.Text = "Update Status: Update cancelled by user. Cleaning up...";
                        await Task.Delay(1000);

                        await Task.Run(() => File.Delete(Config.UpdateChecker_TempFilePath));

                        Lb_UpdateProgress.Text = "Update Status: Done. Starting app...";
                    }
                    else
                    {
                        Lb_UpdateProgress.Text = "Update Status: Failed. Starting app...";
                    }

                    await Task.Delay(1000);

                    Hide();
                    Form NewForm = new MainForm(appSetting);
                    NewForm.ShowDialog();
                    Close();
                }
                else
                {
                    //Too late, users can't cancel now
                    Btn_CancelUpdate.Enabled = false;
                    Lb_UpdateProgress.Text = "Update Status: Verified. Restarting...";
                    await Task.Delay(1000);

                    //Continue update Phase 2
                    Process.Start(Config.UpdateChecker_TempFilePath, $"{Config.Command_UpdatePhase2} \"{Config.ExePath}\"");
                    Application.Exit();
                }
            }
        }

        private void Btn_CancelUpdate_Click(object sender, EventArgs e)
        {
            Btn_CancelUpdate.Enabled = false;
            cts.Cancel(); //Signal cancellation
        } //Cancel updating

        private async void Btn_UpdateCancel_Click(object sender, EventArgs e)
        {
            Pn_UpdateFound.Visible = false;
            Pn_UpdateCheck.Visible = true;

            Lb_UpdateProgress.Text = "Update Status: Update cancelled by user. Starting app...";
            await Task.Delay(1000);

            Hide();
            Form NewForm = new MainForm(appSetting);
            NewForm.ShowDialog();
            Close();
        } //Cancel and go straight to MainForm

        private void UpdateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Btn_CancelUpdate.Enabled = false;
            cts.Cancel(); //Signal cancellation
        }
    }
}

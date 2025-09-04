using Main.Support_Tools;

namespace Main
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
            components = new System.ComponentModel.Container();
            Dlg_OpenFiles = new OpenFileDialog();
            Tab_Decryption = new TabPage();
            label2 = new Label();
            TxtBox_Pass_Dec = new TextBox();
            Btn_Decrypt = new Button();
            Lb_FileNum_Dec = new Label();
            Btn_Clear_Dec = new Button();
            Btn_AddFiles_Dec = new Button();
            Box_SelectedFiles_Dec = new ListBox();
            Tab_Encryption = new TabPage();
            ChkBox_IOLock = new CheckBox();
            label1 = new Label();
            TxtBox_Pass_Enc = new TextBox();
            Btn_Encrypt = new Button();
            Lb_FileNum_Enc = new Label();
            Btn_ClearFiles_Enc = new Button();
            Btn_AddFiles_Enc = new Button();
            Box_SelectedFiles_Enc = new ListBox();
            Main_TabControl = new TabControl();
            Tab_File = new TabPage();
            Lb_NumSelected_FileManage = new Label();
            ChkBox_SecureShred = new CheckBox();
            Btn_ViewProperties = new Button();
            Btn_Delete = new Button();
            Btn_LockstreamCtrl = new Button();
            EncFile_List = new ListBox();
            Tab_Setting = new TabPage();
            Pn_SettingPreference = new Panel();
            ChkBox_AllOutWriteMode = new CheckBox();
            NumUpDown_NumParallelProcessing = new NumericUpDown();
            label8 = new Label();
            Lb_SaveSettingStatus = new Label();
            CbBox_Theme = new ComboBox();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            NumUpDown_UpdateAttemptCount = new NumericUpDown();
            ChkBox_CheckUpdate = new CheckBox();
            label4 = new Label();
            Btn_SaveSetting = new Button();
            LkLb_ImportSettingFile = new LinkLabel();
            ChkBox_SettingFile = new CheckBox();
            ChkBox_SettingReg = new CheckBox();
            label3 = new Label();
            ToolTip_Btn_ViewProperties = new ToolTip(components);
            Dlg_OpenFiles_Setting = new OpenFileDialog();
            Dlg_SaveFiles_Setting = new SaveFileDialog();
            Tab_Decryption.SuspendLayout();
            Tab_Encryption.SuspendLayout();
            Main_TabControl.SuspendLayout();
            Tab_File.SuspendLayout();
            Tab_Setting.SuspendLayout();
            Pn_SettingPreference.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)NumUpDown_NumParallelProcessing).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NumUpDown_UpdateAttemptCount).BeginInit();
            SuspendLayout();
            // 
            // Dlg_OpenFiles
            // 
            Dlg_OpenFiles.Filter = "All Files|*.*";
            Dlg_OpenFiles.Multiselect = true;
            Dlg_OpenFiles.Title = "Select file(s)";
            // 
            // Tab_Decryption
            // 
            Tab_Decryption.Controls.Add(label2);
            Tab_Decryption.Controls.Add(TxtBox_Pass_Dec);
            Tab_Decryption.Controls.Add(Btn_Decrypt);
            Tab_Decryption.Controls.Add(Lb_FileNum_Dec);
            Tab_Decryption.Controls.Add(Btn_Clear_Dec);
            Tab_Decryption.Controls.Add(Btn_AddFiles_Dec);
            Tab_Decryption.Controls.Add(Box_SelectedFiles_Dec);
            Tab_Decryption.Location = new Point(4, 24);
            Tab_Decryption.Margin = new Padding(2);
            Tab_Decryption.Name = "Tab_Decryption";
            Tab_Decryption.Padding = new Padding(2);
            Tab_Decryption.Size = new Size(656, 399);
            Tab_Decryption.TabIndex = 2;
            Tab_Decryption.Text = "Decryption";
            Tab_Decryption.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(5, 315);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(109, 20);
            label2.TabIndex = 6;
            label2.Text = "Security Pass:";
            // 
            // TxtBox_Pass_Dec
            // 
            TxtBox_Pass_Dec.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            TxtBox_Pass_Dec.Location = new Point(131, 310);
            TxtBox_Pass_Dec.Margin = new Padding(2);
            TxtBox_Pass_Dec.Name = "TxtBox_Pass_Dec";
            TxtBox_Pass_Dec.Size = new Size(508, 26);
            TxtBox_Pass_Dec.TabIndex = 5;
            // 
            // Btn_Decrypt
            // 
            Btn_Decrypt.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Btn_Decrypt.Location = new Point(525, 344);
            Btn_Decrypt.Margin = new Padding(2);
            Btn_Decrypt.Name = "Btn_Decrypt";
            Btn_Decrypt.Size = new Size(99, 37);
            Btn_Decrypt.TabIndex = 4;
            Btn_Decrypt.Text = "Decrypt";
            Btn_Decrypt.UseVisualStyleBackColor = true;
            Btn_Decrypt.Click += Btn_Decrypt_Click;
            // 
            // Lb_FileNum_Dec
            // 
            Lb_FileNum_Dec.AutoSize = true;
            Lb_FileNum_Dec.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Lb_FileNum_Dec.Location = new Point(345, 260);
            Lb_FileNum_Dec.Margin = new Padding(2, 0, 2, 0);
            Lb_FileNum_Dec.Name = "Lb_FileNum_Dec";
            Lb_FileNum_Dec.Size = new Size(196, 20);
            Lb_FileNum_Dec.TabIndex = 3;
            Lb_FileNum_Dec.Text = "Number of Files Ready: 00";
            // 
            // Btn_Clear_Dec
            // 
            Btn_Clear_Dec.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Btn_Clear_Dec.Location = new Point(111, 253);
            Btn_Clear_Dec.Margin = new Padding(2);
            Btn_Clear_Dec.Name = "Btn_Clear_Dec";
            Btn_Clear_Dec.Size = new Size(99, 37);
            Btn_Clear_Dec.TabIndex = 2;
            Btn_Clear_Dec.Text = "Clear";
            Btn_Clear_Dec.UseVisualStyleBackColor = true;
            Btn_Clear_Dec.Click += Btn_Clear_Dec_Click;
            // 
            // Btn_AddFiles_Dec
            // 
            Btn_AddFiles_Dec.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Btn_AddFiles_Dec.Location = new Point(7, 253);
            Btn_AddFiles_Dec.Margin = new Padding(2);
            Btn_AddFiles_Dec.Name = "Btn_AddFiles_Dec";
            Btn_AddFiles_Dec.Size = new Size(99, 37);
            Btn_AddFiles_Dec.TabIndex = 1;
            Btn_AddFiles_Dec.Text = "Add Files";
            Btn_AddFiles_Dec.UseVisualStyleBackColor = true;
            Btn_AddFiles_Dec.Click += Btn_AddFiles_Dec_Click;
            // 
            // Box_SelectedFiles_Dec
            // 
            Box_SelectedFiles_Dec.Dock = DockStyle.Top;
            Box_SelectedFiles_Dec.FormattingEnabled = true;
            Box_SelectedFiles_Dec.ItemHeight = 15;
            Box_SelectedFiles_Dec.Location = new Point(2, 2);
            Box_SelectedFiles_Dec.Margin = new Padding(2);
            Box_SelectedFiles_Dec.Name = "Box_SelectedFiles_Dec";
            Box_SelectedFiles_Dec.SelectionMode = SelectionMode.None;
            Box_SelectedFiles_Dec.Size = new Size(652, 244);
            Box_SelectedFiles_Dec.TabIndex = 0;
            // 
            // Tab_Encryption
            // 
            Tab_Encryption.Controls.Add(ChkBox_IOLock);
            Tab_Encryption.Controls.Add(label1);
            Tab_Encryption.Controls.Add(TxtBox_Pass_Enc);
            Tab_Encryption.Controls.Add(Btn_Encrypt);
            Tab_Encryption.Controls.Add(Lb_FileNum_Enc);
            Tab_Encryption.Controls.Add(Btn_ClearFiles_Enc);
            Tab_Encryption.Controls.Add(Btn_AddFiles_Enc);
            Tab_Encryption.Controls.Add(Box_SelectedFiles_Enc);
            Tab_Encryption.Location = new Point(4, 24);
            Tab_Encryption.Margin = new Padding(2);
            Tab_Encryption.Name = "Tab_Encryption";
            Tab_Encryption.Padding = new Padding(2);
            Tab_Encryption.Size = new Size(656, 399);
            Tab_Encryption.TabIndex = 0;
            Tab_Encryption.Text = "Encryption";
            Tab_Encryption.UseVisualStyleBackColor = true;
            // 
            // ChkBox_IOLock
            // 
            ChkBox_IOLock.AutoSize = true;
            ChkBox_IOLock.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ChkBox_IOLock.Location = new Point(9, 352);
            ChkBox_IOLock.Margin = new Padding(2);
            ChkBox_IOLock.Name = "ChkBox_IOLock";
            ChkBox_IOLock.Size = new Size(319, 21);
            ChkBox_IOLock.TabIndex = 7;
            ChkBox_IOLock.Text = "Enable IO-Lock after encryption (max security)";
            ChkBox_IOLock.UseVisualStyleBackColor = true;
            ChkBox_IOLock.CheckedChanged += ChkBox_IOLock_CheckedChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(5, 315);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(109, 20);
            label1.TabIndex = 6;
            label1.Text = "Security Pass:";
            // 
            // TxtBox_Pass_Enc
            // 
            TxtBox_Pass_Enc.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            TxtBox_Pass_Enc.Location = new Point(131, 310);
            TxtBox_Pass_Enc.Margin = new Padding(2);
            TxtBox_Pass_Enc.Name = "TxtBox_Pass_Enc";
            TxtBox_Pass_Enc.Size = new Size(508, 26);
            TxtBox_Pass_Enc.TabIndex = 5;
            // 
            // Btn_Encrypt
            // 
            Btn_Encrypt.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Btn_Encrypt.Location = new Point(525, 344);
            Btn_Encrypt.Margin = new Padding(2);
            Btn_Encrypt.Name = "Btn_Encrypt";
            Btn_Encrypt.Size = new Size(99, 37);
            Btn_Encrypt.TabIndex = 4;
            Btn_Encrypt.Text = "Encrypt";
            Btn_Encrypt.UseVisualStyleBackColor = true;
            Btn_Encrypt.Click += Btn_Encrypt_Click;
            // 
            // Lb_FileNum_Enc
            // 
            Lb_FileNum_Enc.AutoSize = true;
            Lb_FileNum_Enc.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Lb_FileNum_Enc.Location = new Point(8, 247);
            Lb_FileNum_Enc.Margin = new Padding(2, 0, 2, 0);
            Lb_FileNum_Enc.Name = "Lb_FileNum_Enc";
            Lb_FileNum_Enc.Size = new Size(187, 20);
            Lb_FileNum_Enc.TabIndex = 3;
            Lb_FileNum_Enc.Text = "Number of Files Ready: 0";
            // 
            // Btn_ClearFiles_Enc
            // 
            Btn_ClearFiles_Enc.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Btn_ClearFiles_Enc.Location = new Point(537, 240);
            Btn_ClearFiles_Enc.Margin = new Padding(2);
            Btn_ClearFiles_Enc.Name = "Btn_ClearFiles_Enc";
            Btn_ClearFiles_Enc.Size = new Size(99, 37);
            Btn_ClearFiles_Enc.TabIndex = 2;
            Btn_ClearFiles_Enc.Text = "Clear";
            Btn_ClearFiles_Enc.UseVisualStyleBackColor = true;
            Btn_ClearFiles_Enc.Click += Btn_ClearFiles_Enc_Click;
            // 
            // Btn_AddFiles_Enc
            // 
            Btn_AddFiles_Enc.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Btn_AddFiles_Enc.Location = new Point(433, 240);
            Btn_AddFiles_Enc.Margin = new Padding(2);
            Btn_AddFiles_Enc.Name = "Btn_AddFiles_Enc";
            Btn_AddFiles_Enc.Size = new Size(99, 37);
            Btn_AddFiles_Enc.TabIndex = 1;
            Btn_AddFiles_Enc.Text = "Add Files";
            Btn_AddFiles_Enc.UseVisualStyleBackColor = true;
            Btn_AddFiles_Enc.Click += Btn_AddFiles_Enc_Click;
            // 
            // Box_SelectedFiles_Enc
            // 
            Box_SelectedFiles_Enc.Dock = DockStyle.Top;
            Box_SelectedFiles_Enc.Font = new Font("Segoe Fluent Icons", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Box_SelectedFiles_Enc.FormattingEnabled = true;
            Box_SelectedFiles_Enc.Location = new Point(2, 2);
            Box_SelectedFiles_Enc.Margin = new Padding(2);
            Box_SelectedFiles_Enc.Name = "Box_SelectedFiles_Enc";
            Box_SelectedFiles_Enc.SelectionMode = SelectionMode.None;
            Box_SelectedFiles_Enc.Size = new Size(652, 212);
            Box_SelectedFiles_Enc.TabIndex = 0;
            // 
            // Main_TabControl
            // 
            Main_TabControl.Controls.Add(Tab_File);
            Main_TabControl.Controls.Add(Tab_Encryption);
            Main_TabControl.Controls.Add(Tab_Decryption);
            Main_TabControl.Controls.Add(Tab_Setting);
            Main_TabControl.Dock = DockStyle.Fill;
            Main_TabControl.Location = new Point(0, 0);
            Main_TabControl.Margin = new Padding(2);
            Main_TabControl.Name = "Main_TabControl";
            Main_TabControl.SelectedIndex = 0;
            Main_TabControl.Size = new Size(664, 427);
            Main_TabControl.TabIndex = 0;
            // 
            // Tab_File
            // 
            Tab_File.Controls.Add(Lb_NumSelected_FileManage);
            Tab_File.Controls.Add(ChkBox_SecureShred);
            Tab_File.Controls.Add(Btn_ViewProperties);
            Tab_File.Controls.Add(Btn_Delete);
            Tab_File.Controls.Add(Btn_LockstreamCtrl);
            Tab_File.Controls.Add(EncFile_List);
            Tab_File.Location = new Point(4, 24);
            Tab_File.Margin = new Padding(2);
            Tab_File.Name = "Tab_File";
            Tab_File.Size = new Size(656, 399);
            Tab_File.TabIndex = 3;
            Tab_File.Text = "File Management";
            Tab_File.UseVisualStyleBackColor = true;
            // 
            // Lb_NumSelected_FileManage
            // 
            Lb_NumSelected_FileManage.AutoSize = true;
            Lb_NumSelected_FileManage.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Lb_NumSelected_FileManage.Location = new Point(392, 276);
            Lb_NumSelected_FileManage.Name = "Lb_NumSelected_FileManage";
            Lb_NumSelected_FileManage.Size = new Size(194, 21);
            Lb_NumSelected_FileManage.TabIndex = 6;
            Lb_NumSelected_FileManage.Text = "Number of files selected: 0";
            // 
            // ChkBox_SecureShred
            // 
            ChkBox_SecureShred.AutoSize = true;
            ChkBox_SecureShred.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ChkBox_SecureShred.Location = new Point(110, 342);
            ChkBox_SecureShred.Name = "ChkBox_SecureShred";
            ChkBox_SecureShred.Size = new Size(151, 24);
            ChkBox_SecureShred.TabIndex = 5;
            ChkBox_SecureShred.Text = "Secure Shredding?";
            ChkBox_SecureShred.UseVisualStyleBackColor = true;
            // 
            // Btn_ViewProperties
            // 
            Btn_ViewProperties.Enabled = false;
            Btn_ViewProperties.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Btn_ViewProperties.Location = new Point(432, 325);
            Btn_ViewProperties.Name = "Btn_ViewProperties";
            Btn_ViewProperties.Size = new Size(132, 41);
            Btn_ViewProperties.TabIndex = 4;
            Btn_ViewProperties.Text = "View properties";
            ToolTip_Btn_ViewProperties.SetToolTip(Btn_ViewProperties, "This option is available only when you select one item.");
            Btn_ViewProperties.UseVisualStyleBackColor = true;
            Btn_ViewProperties.Click += Btn_ViewProperties_Click;
            // 
            // Btn_Delete
            // 
            Btn_Delete.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Btn_Delete.Location = new Point(8, 332);
            Btn_Delete.Name = "Btn_Delete";
            Btn_Delete.Size = new Size(96, 41);
            Btn_Delete.TabIndex = 3;
            Btn_Delete.Text = "Delete";
            Btn_Delete.UseVisualStyleBackColor = true;
            Btn_Delete.Click += Btn_Delete_Click;
            // 
            // Btn_LockstreamCtrl
            // 
            Btn_LockstreamCtrl.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Btn_LockstreamCtrl.Location = new Point(8, 276);
            Btn_LockstreamCtrl.Name = "Btn_LockstreamCtrl";
            Btn_LockstreamCtrl.Size = new Size(237, 41);
            Btn_LockstreamCtrl.TabIndex = 2;
            Btn_LockstreamCtrl.Text = "Enable/Disable IO-Lock";
            Btn_LockstreamCtrl.UseVisualStyleBackColor = true;
            Btn_LockstreamCtrl.Click += Btn_LockstreamCtrl_Click;
            // 
            // EncFile_List
            // 
            EncFile_List.Dock = DockStyle.Top;
            EncFile_List.DrawMode = DrawMode.OwnerDrawFixed;
            EncFile_List.FormattingEnabled = true;
            EncFile_List.ItemHeight = 15;
            EncFile_List.Location = new Point(0, 0);
            EncFile_List.Margin = new Padding(4, 3, 4, 3);
            EncFile_List.Name = "EncFile_List";
            EncFile_List.SelectionMode = SelectionMode.MultiExtended;
            EncFile_List.Size = new Size(656, 259);
            EncFile_List.TabIndex = 1;
            EncFile_List.DrawItem += EncFile_List_DrawItem;
            EncFile_List.SelectedIndexChanged += EncFile_List_SelectedIndexChanged;
            // 
            // Tab_Setting
            // 
            Tab_Setting.Controls.Add(Pn_SettingPreference);
            Tab_Setting.Location = new Point(4, 24);
            Tab_Setting.Name = "Tab_Setting";
            Tab_Setting.Size = new Size(656, 399);
            Tab_Setting.TabIndex = 4;
            Tab_Setting.Text = "Setting & Preference";
            Tab_Setting.UseVisualStyleBackColor = true;
            // 
            // Pn_SettingPreference
            // 
            Pn_SettingPreference.AutoScroll = true;
            Pn_SettingPreference.Controls.Add(ChkBox_AllOutWriteMode);
            Pn_SettingPreference.Controls.Add(NumUpDown_NumParallelProcessing);
            Pn_SettingPreference.Controls.Add(label8);
            Pn_SettingPreference.Controls.Add(Lb_SaveSettingStatus);
            Pn_SettingPreference.Controls.Add(CbBox_Theme);
            Pn_SettingPreference.Controls.Add(label7);
            Pn_SettingPreference.Controls.Add(label6);
            Pn_SettingPreference.Controls.Add(label5);
            Pn_SettingPreference.Controls.Add(NumUpDown_UpdateAttemptCount);
            Pn_SettingPreference.Controls.Add(ChkBox_CheckUpdate);
            Pn_SettingPreference.Controls.Add(label4);
            Pn_SettingPreference.Controls.Add(Btn_SaveSetting);
            Pn_SettingPreference.Controls.Add(LkLb_ImportSettingFile);
            Pn_SettingPreference.Controls.Add(ChkBox_SettingFile);
            Pn_SettingPreference.Controls.Add(ChkBox_SettingReg);
            Pn_SettingPreference.Controls.Add(label3);
            Pn_SettingPreference.Dock = DockStyle.Fill;
            Pn_SettingPreference.Location = new Point(0, 0);
            Pn_SettingPreference.Name = "Pn_SettingPreference";
            Pn_SettingPreference.Size = new Size(656, 399);
            Pn_SettingPreference.TabIndex = 0;
            // 
            // ChkBox_AllOutWriteMode
            // 
            ChkBox_AllOutWriteMode.AutoSize = true;
            ChkBox_AllOutWriteMode.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ChkBox_AllOutWriteMode.Location = new Point(398, 322);
            ChkBox_AllOutWriteMode.Name = "ChkBox_AllOutWriteMode";
            ChkBox_AllOutWriteMode.Size = new Size(201, 25);
            ChkBox_AllOutWriteMode.TabIndex = 33;
            ChkBox_AllOutWriteMode.Text = "Process all files parallelly";
            ChkBox_AllOutWriteMode.UseVisualStyleBackColor = true;
            ChkBox_AllOutWriteMode.CheckedChanged += ChkBox_AllOutWriteMode_CheckedChanged_1;
            // 
            // NumUpDown_NumParallelProcessing
            // 
            NumUpDown_NumParallelProcessing.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            NumUpDown_NumParallelProcessing.Location = new Point(252, 318);
            NumUpDown_NumParallelProcessing.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            NumUpDown_NumParallelProcessing.Name = "NumUpDown_NumParallelProcessing";
            NumUpDown_NumParallelProcessing.Size = new Size(120, 29);
            NumUpDown_NumParallelProcessing.TabIndex = 32;
            NumUpDown_NumParallelProcessing.Value = new decimal(new int[] { 10, 0, 0, 0 });
            NumUpDown_NumParallelProcessing.ValueChanged += NumUpDown_NumParallelProcessing_ValueChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label8.Location = new Point(36, 320);
            label8.Name = "label8";
            label8.Size = new Size(195, 21);
            label8.TabIndex = 31;
            label8.Text = "Parallel processing thread: ";
            // 
            // Lb_SaveSettingStatus
            // 
            Lb_SaveSettingStatus.AutoSize = true;
            Lb_SaveSettingStatus.ForeColor = Color.Red;
            Lb_SaveSettingStatus.Location = new Point(384, 53);
            Lb_SaveSettingStatus.Name = "Lb_SaveSettingStatus";
            Lb_SaveSettingStatus.Size = new Size(253, 15);
            Lb_SaveSettingStatus.TabIndex = 29;
            Lb_SaveSettingStatus.Text = "Be careful! You haven't saved your settings yet!";
            // 
            // CbBox_Theme
            // 
            CbBox_Theme.DropDownStyle = ComboBoxStyle.DropDownList;
            CbBox_Theme.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            CbBox_Theme.FormattingEnabled = true;
            CbBox_Theme.Items.AddRange(new object[] { "Light", "Dark" });
            CbBox_Theme.Location = new Point(114, 279);
            CbBox_Theme.Name = "CbBox_Theme";
            CbBox_Theme.Size = new Size(144, 29);
            CbBox_Theme.TabIndex = 28;
            CbBox_Theme.SelectedIndexChanged += CbBox_Theme_SelectedIndexChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.Location = new Point(36, 282);
            label7.Name = "label7";
            label7.Size = new Size(64, 21);
            label7.TabIndex = 27;
            label7.Text = "Theme: ";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(8, 249);
            label6.Name = "label6";
            label6.Size = new Size(95, 21);
            label6.TabIndex = 26;
            label6.Text = "User Setting";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(36, 201);
            label5.Name = "label5";
            label5.Size = new Size(123, 21);
            label5.TabIndex = 25;
            label5.Text = "Number of tries:";
            // 
            // NumUpDown_UpdateAttemptCount
            // 
            NumUpDown_UpdateAttemptCount.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            NumUpDown_UpdateAttemptCount.Location = new Point(180, 201);
            NumUpDown_UpdateAttemptCount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            NumUpDown_UpdateAttemptCount.Name = "NumUpDown_UpdateAttemptCount";
            NumUpDown_UpdateAttemptCount.Size = new Size(120, 29);
            NumUpDown_UpdateAttemptCount.TabIndex = 24;
            NumUpDown_UpdateAttemptCount.Value = new decimal(new int[] { 1, 0, 0, 0 });
            NumUpDown_UpdateAttemptCount.ValueChanged += NumUpDown_UpdateAttemptCount_ValueChanged;
            // 
            // ChkBox_CheckUpdate
            // 
            ChkBox_CheckUpdate.AutoSize = true;
            ChkBox_CheckUpdate.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ChkBox_CheckUpdate.Location = new Point(36, 167);
            ChkBox_CheckUpdate.Name = "ChkBox_CheckUpdate";
            ChkBox_CheckUpdate.Size = new Size(222, 25);
            ChkBox_CheckUpdate.TabIndex = 23;
            ChkBox_CheckUpdate.Text = "Check for update on startup";
            ChkBox_CheckUpdate.UseVisualStyleBackColor = true;
            ChkBox_CheckUpdate.CheckedChanged += ChkBox_CheckUpdate_CheckedChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(8, 134);
            label4.Name = "label4";
            label4.Size = new Size(113, 21);
            label4.TabIndex = 22;
            label4.Text = "Startup Setting";
            // 
            // Btn_SaveSetting
            // 
            Btn_SaveSetting.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Btn_SaveSetting.Location = new Point(518, 12);
            Btn_SaveSetting.Name = "Btn_SaveSetting";
            Btn_SaveSetting.Size = new Size(81, 36);
            Btn_SaveSetting.TabIndex = 21;
            Btn_SaveSetting.Text = "Save";
            Btn_SaveSetting.UseVisualStyleBackColor = true;
            Btn_SaveSetting.Click += Btn_SaveSetting_Click;
            // 
            // LkLb_ImportSettingFile
            // 
            LkLb_ImportSettingFile.AutoSize = true;
            LkLb_ImportSettingFile.Location = new Point(59, 106);
            LkLb_ImportSettingFile.Name = "LkLb_ImportSettingFile";
            LkLb_ImportSettingFile.Size = new Size(135, 15);
            LkLb_ImportSettingFile.TabIndex = 20;
            LkLb_ImportSettingFile.TabStop = true;
            LkLb_ImportSettingFile.Text = "Import settings from file";
            LkLb_ImportSettingFile.VisitedLinkColor = Color.Red;
            LkLb_ImportSettingFile.LinkClicked += LkLb_ImportSettingFile_LinkClicked;
            // 
            // ChkBox_SettingFile
            // 
            ChkBox_SettingFile.AutoSize = true;
            ChkBox_SettingFile.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ChkBox_SettingFile.Location = new Point(36, 78);
            ChkBox_SettingFile.Name = "ChkBox_SettingFile";
            ChkBox_SettingFile.Size = new Size(297, 25);
            ChkBox_SettingFile.TabIndex = 19;
            ChkBox_SettingFile.Text = "Save settings to File (advanced, buggy)";
            ChkBox_SettingFile.UseVisualStyleBackColor = true;
            ChkBox_SettingFile.CheckedChanged += ChkBox_SettingFile_CheckedChanged;
            // 
            // ChkBox_SettingReg
            // 
            ChkBox_SettingReg.AutoSize = true;
            ChkBox_SettingReg.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ChkBox_SettingReg.Location = new Point(36, 47);
            ChkBox_SettingReg.Name = "ChkBox_SettingReg";
            ChkBox_SettingReg.Size = new Size(296, 25);
            ChkBox_SettingReg.TabIndex = 18;
            ChkBox_SettingReg.Text = "Save settings to Registry (best, easiest)";
            ChkBox_SettingReg.UseVisualStyleBackColor = true;
            ChkBox_SettingReg.CheckedChanged += ChkBox_SettingReg_CheckedChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(8, 12);
            label3.Name = "label3";
            label3.Size = new Size(464, 21);
            label3.TabIndex = 17;
            label3.Text = "Warning: You must save this setting first before modifying others!";
            // 
            // ToolTip_Btn_ViewProperties
            // 
            ToolTip_Btn_ViewProperties.ToolTipIcon = ToolTipIcon.Warning;
            ToolTip_Btn_ViewProperties.ToolTipTitle = "Warning!";
            // 
            // Dlg_OpenFiles_Setting
            // 
            Dlg_OpenFiles_Setting.Filter = "JSON files|*.json";
            Dlg_OpenFiles_Setting.Title = "Select a setting file";
            // 
            // Dlg_SaveFiles_Setting
            // 
            Dlg_SaveFiles_Setting.Filter = "JSON file|*.json";
            Dlg_SaveFiles_Setting.Title = "Save settings to a file";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(664, 427);
            Controls.Add(Main_TabControl);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(2);
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "IOwnThisFile v1.0.2";
            FormClosing += MainForm_FormClosing;
            Tab_Decryption.ResumeLayout(false);
            Tab_Decryption.PerformLayout();
            Tab_Encryption.ResumeLayout(false);
            Tab_Encryption.PerformLayout();
            Main_TabControl.ResumeLayout(false);
            Tab_File.ResumeLayout(false);
            Tab_File.PerformLayout();
            Tab_Setting.ResumeLayout(false);
            Pn_SettingPreference.ResumeLayout(false);
            Pn_SettingPreference.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)NumUpDown_NumParallelProcessing).EndInit();
            ((System.ComponentModel.ISupportInitialize)NumUpDown_UpdateAttemptCount).EndInit();
            ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog Dlg_OpenFiles;
        private System.Windows.Forms.TabPage Tab_Decryption;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtBox_Pass_Dec;
        private System.Windows.Forms.Button Btn_Decrypt;
        private System.Windows.Forms.Label Lb_FileNum_Dec;
        private System.Windows.Forms.Button Btn_Clear_Dec;
        private System.Windows.Forms.Button Btn_AddFiles_Dec;
        private System.Windows.Forms.ListBox Box_SelectedFiles_Dec;
        private System.Windows.Forms.TabPage Tab_Encryption;
        private System.Windows.Forms.CheckBox ChkBox_IOLock;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtBox_Pass_Enc;
        private System.Windows.Forms.Button Btn_Encrypt;
        private System.Windows.Forms.Label Lb_FileNum_Enc;
        private System.Windows.Forms.Button Btn_ClearFiles_Enc;
        private System.Windows.Forms.Button Btn_AddFiles_Enc;
        private System.Windows.Forms.ListBox Box_SelectedFiles_Enc;
        private System.Windows.Forms.TabControl Main_TabControl;
        private System.Windows.Forms.TabPage Tab_File;
        private System.Windows.Forms.ListBox EncFile_List;
        private Button Btn_ViewProperties;
        private Button Btn_Delete;
        private Button Btn_LockstreamCtrl;
        private CheckBox ChkBox_SecureShred;
        private Label Lb_NumSelected_FileManage;
        private ToolTip ToolTip_Btn_ViewProperties;
        private TabPage Tab_Setting;
        private OpenFileDialog Dlg_OpenFiles_Setting;
        private Panel Pn_SettingPreference;
        private CheckBox ChkBox_AllOutWriteMode;
        private NumericUpDown NumUpDown_NumParallelProcessing;
        private Label label8;
        private Label Lb_SaveSettingStatus;
        private ComboBox CbBox_Theme;
        private Label label7;
        private Label label6;
        private Label label5;
        private NumericUpDown NumUpDown_UpdateAttemptCount;
        private CheckBox ChkBox_CheckUpdate;
        private Label label4;
        private Button Btn_SaveSetting;
        private LinkLabel LkLb_ImportSettingFile;
        private CheckBox ChkBox_SettingFile;
        private CheckBox ChkBox_SettingReg;
        private Label label3;
        private SaveFileDialog Dlg_SaveFiles_Setting;
    }
}


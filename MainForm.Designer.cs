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
            ToolTip_Btn_ViewProperties = new ToolTip(components);
            Tab_Decryption.SuspendLayout();
            Tab_Encryption.SuspendLayout();
            Main_TabControl.SuspendLayout();
            Tab_File.SuspendLayout();
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
            Tab_Decryption.Size = new Size(645, 399);
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
            Box_SelectedFiles_Dec.Size = new Size(641, 244);
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
            Tab_Encryption.Size = new Size(645, 399);
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
            Box_SelectedFiles_Enc.Size = new Size(641, 212);
            Box_SelectedFiles_Enc.TabIndex = 0;
            // 
            // Main_TabControl
            // 
            Main_TabControl.Controls.Add(Tab_File);
            Main_TabControl.Controls.Add(Tab_Encryption);
            Main_TabControl.Controls.Add(Tab_Decryption);
            Main_TabControl.Dock = DockStyle.Fill;
            Main_TabControl.Location = new Point(0, 0);
            Main_TabControl.Margin = new Padding(2);
            Main_TabControl.Name = "Main_TabControl";
            Main_TabControl.SelectedIndex = 0;
            Main_TabControl.Size = new Size(653, 427);
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
            Tab_File.Size = new Size(645, 399);
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
            EncFile_List.Size = new Size(645, 259);
            EncFile_List.TabIndex = 1;
            EncFile_List.DrawItem += EncFile_List_DrawItem;
            EncFile_List.SelectedIndexChanged += EncFile_List_SelectedIndexChanged;
            // 
            // ToolTip_Btn_ViewProperties
            // 
            ToolTip_Btn_ViewProperties.ToolTipIcon = ToolTipIcon.Warning;
            ToolTip_Btn_ViewProperties.ToolTipTitle = "Warning!";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(653, 427);
            Controls.Add(Main_TabControl);
            Margin = new Padding(2);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "IOwnThisFile 1.0";
            FormClosing += MainForm_FormClosing;
            Tab_Decryption.ResumeLayout(false);
            Tab_Decryption.PerformLayout();
            Tab_Encryption.ResumeLayout(false);
            Tab_Encryption.PerformLayout();
            Main_TabControl.ResumeLayout(false);
            Tab_File.ResumeLayout(false);
            Tab_File.PerformLayout();
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
    }
}


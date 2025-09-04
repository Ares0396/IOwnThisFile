namespace Main
{
    partial class PropertyForm
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
            Lb_FileName = new Label();
            Lb_FileSize = new Label();
            Lb_LockStream = new Label();
            Lb_LastEncrypted = new Label();
            Lb_LastLockstreamed = new Label();
            SuspendLayout();
            // 
            // Lb_FileName
            // 
            Lb_FileName.AutoSize = true;
            Lb_FileName.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Lb_FileName.Location = new Point(13, 20);
            Lb_FileName.Name = "Lb_FileName";
            Lb_FileName.Size = new Size(128, 21);
            Lb_FileName.TabIndex = 0;
            Lb_FileName.Text = "Name of the file: ";
            // 
            // Lb_FileSize
            // 
            Lb_FileSize.AutoSize = true;
            Lb_FileSize.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Lb_FileSize.Location = new Point(13, 66);
            Lb_FileSize.Name = "Lb_FileSize";
            Lb_FileSize.Size = new Size(73, 21);
            Lb_FileSize.TabIndex = 1;
            Lb_FileSize.Text = "File Size: ";
            // 
            // Lb_LockStream
            // 
            Lb_LockStream.AutoSize = true;
            Lb_LockStream.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Lb_LockStream.Location = new Point(13, 114);
            Lb_LockStream.Name = "Lb_LockStream";
            Lb_LockStream.Size = new Size(130, 21);
            Lb_LockStream.TabIndex = 2;
            Lb_LockStream.Text = "IO-Lock enabled: ";
            // 
            // Lb_LastEncrypted
            // 
            Lb_LastEncrypted.AutoSize = true;
            Lb_LastEncrypted.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Lb_LastEncrypted.Location = new Point(15, 168);
            Lb_LastEncrypted.Name = "Lb_LastEncrypted";
            Lb_LastEncrypted.Size = new Size(153, 21);
            Lb_LastEncrypted.TabIndex = 3;
            Lb_LastEncrypted.Text = "Last time encrypted: ";
            // 
            // Lb_LastLockstreamed
            // 
            Lb_LastLockstreamed.AutoSize = true;
            Lb_LastLockstreamed.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Lb_LastLockstreamed.Location = new Point(15, 222);
            Lb_LastLockstreamed.Name = "Lb_LastLockstreamed";
            Lb_LastLockstreamed.Size = new Size(155, 21);
            Lb_LastLockstreamed.TabIndex = 4;
            Lb_LastLockstreamed.Text = "Last time IO-Locked: ";
            // 
            // PropertyForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(434, 289);
            Controls.Add(Lb_LastLockstreamed);
            Controls.Add(Lb_LastEncrypted);
            Controls.Add(Lb_LockStream);
            Controls.Add(Lb_FileSize);
            Controls.Add(Lb_FileName);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "PropertyForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "View Property";
            Load += PropertyForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label Lb_FileName;
        private Label Lb_FileSize;
        private Label Lb_LockStream;
        private Label Lb_LastEncrypted;
        private Label Lb_LastLockstreamed;
    }
}
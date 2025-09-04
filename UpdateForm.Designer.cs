namespace Main
{
    partial class UpdateForm
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
            Pn_UpdateFound = new Panel();
            Lb_LatestVer = new Label();
            Lb_ReleasedDate = new Label();
            Lb_CurrentVer = new Label();
            label1 = new Label();
            Btn_Update = new Button();
            Btn_UpdateCancel = new Button();
            Pn_UpdateCheck = new Panel();
            progressBar1 = new ProgressBar();
            Lb_UpdateStatus = new Label();
            Pn_Update = new Panel();
            progressBar2 = new ProgressBar();
            Btn_CancelUpdate = new Button();
            Lb_UpdateProgress = new Label();
            label2 = new Label();
            Pn_UpdateFound.SuspendLayout();
            Pn_UpdateCheck.SuspendLayout();
            Pn_Update.SuspendLayout();
            SuspendLayout();
            // 
            // Pn_UpdateFound
            // 
            Pn_UpdateFound.Controls.Add(Lb_LatestVer);
            Pn_UpdateFound.Controls.Add(Lb_ReleasedDate);
            Pn_UpdateFound.Controls.Add(Lb_CurrentVer);
            Pn_UpdateFound.Controls.Add(label1);
            Pn_UpdateFound.Controls.Add(Btn_Update);
            Pn_UpdateFound.Controls.Add(Btn_UpdateCancel);
            Pn_UpdateFound.Location = new Point(0, 0);
            Pn_UpdateFound.Name = "Pn_UpdateFound";
            Pn_UpdateFound.Size = new Size(435, 195);
            Pn_UpdateFound.TabIndex = 0;
            // 
            // Lb_LatestVer
            // 
            Lb_LatestVer.AutoSize = true;
            Lb_LatestVer.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Lb_LatestVer.Location = new Point(13, 52);
            Lb_LatestVer.Name = "Lb_LatestVer";
            Lb_LatestVer.Size = new Size(114, 21);
            Lb_LatestVer.TabIndex = 3;
            Lb_LatestVer.Text = "Latest Version: ";
            // 
            // Lb_ReleasedDate
            // 
            Lb_ReleasedDate.AutoSize = true;
            Lb_ReleasedDate.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Lb_ReleasedDate.Location = new Point(12, 85);
            Lb_ReleasedDate.Name = "Lb_ReleasedDate";
            Lb_ReleasedDate.Size = new Size(115, 21);
            Lb_ReleasedDate.TabIndex = 4;
            Lb_ReleasedDate.Text = "Released Date: ";
            // 
            // Lb_CurrentVer
            // 
            Lb_CurrentVer.AutoSize = true;
            Lb_CurrentVer.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Lb_CurrentVer.Location = new Point(13, 120);
            Lb_CurrentVer.Name = "Lb_CurrentVer";
            Lb_CurrentVer.Size = new Size(126, 21);
            Lb_CurrentVer.TabIndex = 5;
            Lb_CurrentVer.Text = "Current Version: ";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(138, 9);
            label1.Name = "label1";
            label1.Size = new Size(164, 21);
            label1.TabIndex = 1;
            label1.Text = "New update available!";
            // 
            // Btn_Update
            // 
            Btn_Update.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Btn_Update.Location = new Point(119, 152);
            Btn_Update.Name = "Btn_Update";
            Btn_Update.Size = new Size(87, 31);
            Btn_Update.TabIndex = 6;
            Btn_Update.Text = "Update";
            Btn_Update.UseVisualStyleBackColor = true;
            Btn_Update.Click += Btn_Update_Click;
            // 
            // Btn_UpdateCancel
            // 
            Btn_UpdateCancel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Btn_UpdateCancel.Location = new Point(227, 152);
            Btn_UpdateCancel.Name = "Btn_UpdateCancel";
            Btn_UpdateCancel.Size = new Size(87, 31);
            Btn_UpdateCancel.TabIndex = 7;
            Btn_UpdateCancel.Text = "Cancel";
            Btn_UpdateCancel.UseVisualStyleBackColor = true;
            Btn_UpdateCancel.Click += Btn_UpdateCancel_Click;
            // 
            // Pn_UpdateCheck
            // 
            Pn_UpdateCheck.Controls.Add(progressBar1);
            Pn_UpdateCheck.Controls.Add(Lb_UpdateStatus);
            Pn_UpdateCheck.Location = new Point(0, 0);
            Pn_UpdateCheck.Name = "Pn_UpdateCheck";
            Pn_UpdateCheck.Size = new Size(435, 195);
            Pn_UpdateCheck.TabIndex = 2;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(26, 112);
            progressBar1.MarqueeAnimationSpeed = 1;
            progressBar1.Maximum = 10000;
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(380, 17);
            progressBar1.Style = ProgressBarStyle.Marquee;
            progressBar1.TabIndex = 2;
            // 
            // Lb_UpdateStatus
            // 
            Lb_UpdateStatus.AutoSize = true;
            Lb_UpdateStatus.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Lb_UpdateStatus.Location = new Point(151, 54);
            Lb_UpdateStatus.Name = "Lb_UpdateStatus";
            Lb_UpdateStatus.Size = new Size(123, 30);
            Lb_UpdateStatus.TabIndex = 1;
            Lb_UpdateStatus.Text = "Initializing...";
            Lb_UpdateStatus.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Pn_Update
            // 
            Pn_Update.Controls.Add(progressBar2);
            Pn_Update.Controls.Add(Btn_CancelUpdate);
            Pn_Update.Controls.Add(Lb_UpdateProgress);
            Pn_Update.Controls.Add(label2);
            Pn_Update.Location = new Point(0, 0);
            Pn_Update.Name = "Pn_Update";
            Pn_Update.Size = new Size(435, 195);
            Pn_Update.TabIndex = 1;
            // 
            // progressBar2
            // 
            progressBar2.Location = new Point(13, 109);
            progressBar2.MarqueeAnimationSpeed = 1;
            progressBar2.Maximum = 1000;
            progressBar2.Name = "progressBar2";
            progressBar2.Size = new Size(410, 23);
            progressBar2.Style = ProgressBarStyle.Marquee;
            progressBar2.TabIndex = 5;
            // 
            // Btn_CancelUpdate
            // 
            Btn_CancelUpdate.Enabled = false;
            Btn_CancelUpdate.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Btn_CancelUpdate.Location = new Point(138, 152);
            Btn_CancelUpdate.Name = "Btn_CancelUpdate";
            Btn_CancelUpdate.Size = new Size(145, 35);
            Btn_CancelUpdate.TabIndex = 4;
            Btn_CancelUpdate.Text = "Cancel update";
            Btn_CancelUpdate.UseVisualStyleBackColor = true;
            Btn_CancelUpdate.Click += Btn_CancelUpdate_Click;
            // 
            // Lb_UpdateProgress
            // 
            Lb_UpdateProgress.AutoSize = true;
            Lb_UpdateProgress.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Lb_UpdateProgress.Location = new Point(13, 73);
            Lb_UpdateProgress.Name = "Lb_UpdateProgress";
            Lb_UpdateProgress.Size = new Size(113, 21);
            Lb_UpdateProgress.TabIndex = 3;
            Lb_UpdateProgress.Text = "Update Status: ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(171, 9);
            label2.Name = "label2";
            label2.Size = new Size(83, 21);
            label2.TabIndex = 2;
            label2.Text = "Updating...";
            // 
            // UpdateForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(435, 195);
            Controls.Add(Pn_UpdateCheck);
            Controls.Add(Pn_Update);
            Controls.Add(Pn_UpdateFound);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "UpdateForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "IOTF Updater";
            Load += UpdateForm_Load;
            Pn_UpdateFound.ResumeLayout(false);
            Pn_UpdateFound.PerformLayout();
            Pn_UpdateCheck.ResumeLayout(false);
            Pn_UpdateCheck.PerformLayout();
            Pn_Update.ResumeLayout(false);
            Pn_Update.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel Pn_UpdateFound;
        private Panel Pn_UpdateCheck;
        private ProgressBar progressBar1;
        private Label Lb_UpdateStatus;
        private Label label1;
        private Label Lb_CurrentVer;
        private Label Lb_ReleasedDate;
        private Label Lb_LatestVer;
        private Button Btn_UpdateCancel;
        private Button Btn_Update;
        private Panel Pn_Update;
        private Label Lb_UpdateProgress;
        private Label label2;
        private Button Btn_CancelUpdate;
        private ProgressBar progressBar2;
    }
}
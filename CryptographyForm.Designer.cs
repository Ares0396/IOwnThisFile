namespace Main
{
    partial class CryptographyForm
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
            CryptList = new ListBox();
            Lb_Status = new Label();
            Lb_FileProcessing = new Label();
            SuspendLayout();
            // 
            // CryptList
            // 
            CryptList.Dock = DockStyle.Top;
            CryptList.DrawMode = DrawMode.OwnerDrawVariable;
            CryptList.FormattingEnabled = true;
            CryptList.ItemHeight = 15;
            CryptList.Location = new Point(0, 0);
            CryptList.Name = "CryptList";
            CryptList.SelectionMode = SelectionMode.None;
            CryptList.Size = new Size(855, 259);
            CryptList.TabIndex = 0;
            CryptList.DrawItem += CryptList_DrawItem;
            // 
            // Lb_Status
            // 
            Lb_Status.AutoSize = true;
            Lb_Status.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Lb_Status.Location = new Point(12, 282);
            Lb_Status.Name = "Lb_Status";
            Lb_Status.Size = new Size(59, 21);
            Lb_Status.TabIndex = 1;
            Lb_Status.Text = "Status: ";
            // 
            // Lb_FileProcessing
            // 
            Lb_FileProcessing.AutoSize = true;
            Lb_FileProcessing.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Lb_FileProcessing.Location = new Point(12, 327);
            Lb_FileProcessing.Name = "Lb_FileProcessing";
            Lb_FileProcessing.Size = new Size(117, 21);
            Lb_FileProcessing.TabIndex = 2;
            Lb_FileProcessing.Text = "Processing file: ";
            // 
            // CryptographyForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(855, 388);
            Controls.Add(Lb_FileProcessing);
            Controls.Add(Lb_Status);
            Controls.Add(CryptList);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "CryptographyForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Cryptographic Operator";
            Load += CryptographyForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox CryptList;
        private Label Lb_Status;
        private Label Lb_FileProcessing;
    }
}
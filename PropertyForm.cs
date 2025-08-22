
namespace Main
{
    public partial class PropertyForm : Form
    {
        public PropertyForm()
        {
            InitializeComponent();
        }

        private void PropertyForm_Load(object sender, EventArgs e)
        {
            //We have the check installed in MainForm.cs, so we can assume that FilePath is set.
            string FilePath = Config.Property_FilePath;

            //Get file information
            FileInfo fileInfo = new FileInfo(FilePath);
            string fileName = fileInfo.Name;
            long fileSize = fileInfo.Length;
            bool isLocked = Config.Property_FileIsLocked;
            DateTime lastEncrypted = Config.Property_FileLastEncrypted.TryGetValue(FilePath, out DateTime value)
                ? value : DateTime.MinValue;
            string lastLockedText;
            if (isLocked)
            {
                lastLockedText = Config.Property_FileLastLockstreamed.TryGetValue(FilePath, out DateTime lockTime)
                    ? "Last time IO-Locked: " + lockTime.ToString()
                    : "Last time IO-Locked: " + "File is locked but no lock time recorded.";

                //We just unlocked the lockstream temporarily, but since it's considered locked, we need to lock it again, without updating the time
                FileStream lockStream = Tool.InitializeLockStream(FilePath);
                Config.LockStream_Dictionary.Add(FilePath, lockStream);
            }
            else
            {
                lastLockedText = "Last time IO-Locked: " + "File is not locked.";
            }

            //Set the properties in the form
            Lb_FileName.Text = "Name of the file: " + fileName;
            Lb_FileSize.Text = "File size: " + fileSize.ToString() + " bytes"; //In newer versions, consider converting to KB, MB, etc.
            Lb_LockStream.Text = "File is locked: " + (isLocked ? "True" : "False");
            Lb_LastEncrypted.Text = "Last time encrypted: " + lastEncrypted.ToString();
            Lb_LastLockstreamed.Text = lastLockedText;
        }
    }
}

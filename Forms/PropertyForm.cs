using Main.Support_Tools;

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

            //Get file information and define placeholders
            FileInfo fileInfo = new(FilePath);
            string fileName = fileInfo.Name; //File name
            long fileSize = fileInfo.Length; //File size
            bool isLocked = Config.Property_FileIsLocked; //IO-Locked flag
            DateTime lastEncrypted = Config.Property_FileLastEncrypted.TryGetValue(FilePath, out DateTime value)
                ? value : DateTime.MinValue;
            string lastLockedText;
            string lastEncryptedText;
            string fileSizeText = ByteUnit.ConvertRawBytes(fileSize);

            //Customize and refine the display text
            if (isLocked)
            {
                lastLockedText = Config.Property_FileLastLockstreamed.TryGetValue(FilePath, out DateTime lockTime)
                    ? "Last time IO-Locked: " + lockTime.ToString()
                    : "Last time IO-Locked: " + "File is locked but no lock time recorded.";

                //We just unlocked the lockstream temporarily, but since it's considered locked, we need to lock it again, without updating the time
                FileStream lockStream = LockStream.Initialize(FilePath);
                Config.LockStream_Dictionary.Add(FilePath, lockStream);
            }
            else lastLockedText = "Last time IO-Locked: " + "File is not locked.";
            if (lastEncrypted ==  DateTime.MinValue)
            {
                //Unknown encryption time,
                lastEncryptedText = "File is encrypted but no encryption time recorded.";
            }
            else lastEncryptedText = lastEncrypted.ToString();

            //Set the properties in the form
            Lb_FileName.Text = "Name of the file: " + fileName;
            Lb_FileSize.Text = "File size: " + fileSizeText; //In newer versions, consider converting to KB, MB, etc.
            Lb_LockStream.Text = "File is locked: " + (isLocked ? "True" : "False");
            Lb_LastEncrypted.Text = "Last time encrypted: " + lastEncryptedText;
            Lb_LastLockstreamed.Text = lastLockedText;
        }
    }
}

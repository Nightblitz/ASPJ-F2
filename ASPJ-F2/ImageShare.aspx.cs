using nClam;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ASPJ_F2
{
    public partial class ImageShare : System.Web.UI.Page
    {
        private static byte[] _salt = Encoding.ASCII.GetBytes("jasdh7834y8hfeur73rsharks214");

        //Watemarking Image Var [Main]
        private string secretTextMain;

        private string secretTextKeyMain;
        private string encrytedSecretTextMain;

        //Watermarking Image var[Sec]
        private string secretTextSec;

        private string secretTextKeySec;
        private string encrytedSecretTextSec;

        //Important Stuff - Gallery var Database
        private int galleryID = 0;

        private string title;
        private int categoryID;
        private string categoryName;
        private int amount;

        //Main upload var
        private string extensionMain;

        private string fromRootToPhotosMain = @"C:\Users\User\Source\Repos\NewRepo\ASPJ-F2\Images\UploadedPhotos\Main\";
        private string photoFolderMain;
        private string randomPicIDMain;
        private string uniqueFileNameMain;

        //Secondary upload var
        private string extensionSec;

        private string fromRootToPhotosSec = @"C:\Users\User\Source\Repos\NewRepo\ASPJ-F2\Images\UploadedPhotos\Secondary\";
        private string photoFolderSec;
        private string randomPicIDSec;
        private string uniqueFileNameSec;

        //FileuploadMain database var
        private string filesizeMain;

        private string medianameMain;
        private int fileUploadSecretID;
        private int fileUploadID;

        //FileuploadSec database var
        private string filesizeSec;

        private int fileUploadSecondarySecretID;
        private int fileUploadSecondaryID;

        //Temp
        private string userid = "123";

        protected void Page_Load(object sender, EventArgs e)
        {
            string idFromPrevious = Cryptography.DecryptUrl(HttpUtility.UrlDecode(Request.QueryString["23rewwr343wd9jfsk23dmjd2q33c3g"]));
            galleryID = Int32.Parse(idFromPrevious);

            //SecretText Generate
            //if (ViewState["StoredText"] == null)
            //{
            //    secretEncryptionKey = Cryptography.GetRandomString();
            //}
            //else
            //{
            //    secretEncryptionKey = (string)ViewState["StoredText"];
            //}
            //Encrypt of secret text in image of Main
            if (ViewState["StoredIdMain"] == null)
            {
                randomPicIDMain = Cryptography.GetRandomString();
            }
            else
            {
                randomPicIDMain = (string)ViewState["StoredIdMain"];
            }
            //Encrypt of secret text in image of Sec
            if (ViewState["StoredIdSec"] == null)
            {
                randomPicIDSec = Cryptography.GetRandomString();
            }
            else
            {
                randomPicIDSec = (string)ViewState["StoredIdSec"];
            }

            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["F2DatabaseConnectionString"].ConnectionString))
            {
                SqlDataReader reader;
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT [DesignName],[Cost],[CategoryID] FROM [dbo].[Gallery] WHERE [GalleryID]= @GalleryID AND [UserID] = @UserID;";
                cmd.Parameters.AddWithValue("GalleryID", galleryID);
                cmd.Parameters.AddWithValue("UserID", userid);
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    title = reader.GetString(0);
                    amount = reader.GetInt32(1);
                    categoryID = reader.GetInt32(2);
                }
                connection.Close();

                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandText = "SELECT [CategoryName] FROM [dbo].[Category] WHERE [CategoryID]= @CategoryID;";
                cmd2.Parameters.AddWithValue("CategoryID", categoryID);
                cmd2.Connection = connection;
                connection.Open();
                cmd2.ExecuteNonQuery();

                reader = cmd2.ExecuteReader();
                while (reader.Read())
                {
                    categoryName = reader.GetString(0);
                }
                connection.Close();
            }
            this.DesignTitleLabel.Text = title;
            this.CategoryLabel.Text = categoryName;
            this.CostLabel.Text = "$" + amount.ToString();
            this.SellerLabel.Text = userid;
        }

        protected void btnPreviewMain_Click(object sender, EventArgs e)
        {
            //Storage of database essentials
            ViewState["filesizeMain"] = FileUploadMain.PostedFile.ContentLength.ToString();
            ViewState["medianameMain"] = FileUploadMain.PostedFile.FileName;
            //filesizeMain = FileUploadMain.PostedFile.ContentLength.ToString();
            //medianameMain = FileUploadMain.PostedFile.FileName;

            extensionMain = Path.GetExtension(FileUploadMain.FileName);
            extensionSec = Path.GetExtension(FileUploadMain.FileName);
            ViewState["extensionMain"] = extensionMain;
            ViewState["extensionSec"] = extensionSec;
            if (FileUploadMain.HasFile)
            {
                try
                {
                    if (FileUploadMain.PostedFile.ContentType == "image/png" || FileUploadMain.PostedFile.ContentType == "text/plain")
                    {
                        if (FileUploadMain.PostedFile.ContentLength < 1000000)
                        {
                            photoFolderMain = Path.Combine(fromRootToPhotosMain, randomPicIDMain);
                            ViewState["StoredIdMain"] = randomPicIDMain;

                            photoFolderSec = Path.Combine(fromRootToPhotosSec, randomPicIDSec);
                            ViewState["StoredIdSec"] = randomPicIDSec;
                            //Main Create Dir
                            if (!Directory.Exists(photoFolderMain))
                            {
                                Directory.CreateDirectory(photoFolderMain);
                            }
                            //Sec Create Dir
                            if (!Directory.Exists(photoFolderSec))
                            {
                                Directory.CreateDirectory(photoFolderSec);
                            }
                            ViewState["PhotoFolderMain"] = photoFolderMain;
                            ViewState["PhotoFolderSec"] = photoFolderSec;
                            uniqueFileNameMain = Path.ChangeExtension(FileUploadMain.FileName, DateTime.Now.Ticks.ToString());
                            ViewState["uniqueFileNameMain"] = uniqueFileNameMain;

                            //Editing of main image
                            //Stream strm = FileUploadMain.PostedFile.InputStream;
                            //string targetPath;
                            //using (var img = System.Drawing.Image.FromStream(strm))
                            //{
                            //    int newWidth = 240;
                            //    int newHeight = 240;
                            //    var thumbImg = new Bitmap(newWidth, newHeight);
                            //    var thumbGraph = Graphics.FromImage(thumbImg);
                            //    thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                            //    thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            //    thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                            //    var imgRectangle = new Rectangle(0, 0, newWidth, newHeight);
                            //    thumbGraph.DrawImage(img, imgRectangle);

                            //    targetPath = photoFolderMain + randomPicIDMain + "\\" + uniqueFileNameMain;
                            //    using (Bitmap bmp = new Bitmap(thumbImg))
                            //    {
                            //        bmp.Save(targetPath ,bmp.RawFormat);
                            //    }
                            //}

                            //Embedding secretText into ImageMain
                            Stream strm = FileUploadMain.PostedFile.InputStream;
                            Bitmap WatermarkedImageMain = (Bitmap)Image.FromStream(strm);
                            secretTextMain = Cryptography.GetRandomString();
                            secretTextKeyMain = Cryptography.GetRandomString();
                            encrytedSecretTextMain = EncryptStringAesIntoImage(secretTextMain, secretTextKeyMain);
                            WatermarkedImageMain = Cryptography.embedText(encrytedSecretTextMain, WatermarkedImageMain);
                            WatermarkedImageMain.Save(Path.Combine(photoFolderMain, uniqueFileNameMain + extensionMain));

                            ViewState["secretTextKeyMain"] = secretTextKeyMain;
                            ViewState["encrytedSecretTextMain"] = encrytedSecretTextMain;

                            var clam = new ClamClient("localhost", 3310);
                            var scanResult = clam.ScanFileOnServer(Path.Combine(photoFolderMain, uniqueFileNameMain + extensionMain));

                            switch (scanResult.Result)
                            {
                                case ClamScanResults.Clean:
                                    StatusLabelMain.CssClass = "label label-success";
                                    StatusLabelMain.Text = "Upload status: File uploaded!";
                                    DisplayMainUploadedPhotos(imageToByteArray(WatermarkedImageMain));
                                    DisplaySecondaryUploadedPhotos();

                                    break;

                                case ClamScanResults.VirusDetected:
                                    StatusLabelMain.Text = "Upload status: Virus Found!!!!!";
                                    File.Delete(Path.Combine(photoFolderMain, uniqueFileNameMain + extensionMain));
                                    StatusLabelMain.CssClass = "label label-danger";
                                    break;

                                case ClamScanResults.Error:
                                    StatusLabelMain.Text = scanResult.RawResult;
                                    File.Delete(Path.Combine(photoFolderMain, uniqueFileNameMain + extensionMain));
                                    StatusLabelMain.CssClass = "label label-danger";
                                    break;
                            }
                        }
                        else
                        {
                            StatusLabelMain.Text = "Upload status: The file has to be less than 1 MB!";
                            StatusLabelMain.CssClass = "label label-danger";
                        }
                    }
                    else
                    {
                        StatusLabelMain.Text = "Upload status: Only PNG Or BMP files are accepted!";
                        StatusLabelMain.CssClass = "label label-danger";
                    }
                }
                catch (Exception ex)
                {
                    StatusLabelMain.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
                }
            }
            else
            {
                StatusLabelMain.Text = "Upload status: You have not chosen a picture to preview!!";
                StatusLabelMain.CssClass = "label label-danger";
            }
        }

        public void DisplayMainUploadedPhotos(byte[] img)
        {
            //Stream fs = FileUploadMain.PostedFile.InputStream;
            //BinaryReader br = new BinaryReader(fs);
            Byte[] bytes = img;
            string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
            MainUploadedImage.ImageUrl = "data:image/png;base64," + base64String;
            MainUploadedImage.Visible = true;
            MainUploadedImage.Width = 150;
            MainUploadedImage.Height = 150;
        }

        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }

        public void DisplaySecondaryUploadedPhotos()
        {
            string watermarkText = "© F2-ASPJ";

            //Get the file name.
            string fileName = Path.GetFileNameWithoutExtension(FileUploadMain.PostedFile.FileName) + ".png";

            //Read the File into a Bitmap.
            using (Bitmap bmp = new Bitmap(FileUploadMain.PostedFile.InputStream, false))
            {
                using (Graphics grp = Graphics.FromImage(bmp))
                {
                    //Set the Color of the Watermark text.
                    Brush brush = new SolidBrush(Color.Red);

                    //Set the Font and its size.
                    Font font = new System.Drawing.Font("Arial", 30, FontStyle.Bold, GraphicsUnit.Pixel);

                    //Determine the size of the Watermark text.
                    SizeF textSize = new SizeF();
                    textSize = grp.MeasureString(watermarkText, font);

                    //Position the text and draw it on the image.
                    Point position = new Point((bmp.Width - ((int)textSize.Width + 10)), (bmp.Height - ((int)textSize.Height + 10)));
                    grp.DrawString(watermarkText, font, brush, position);

                    //Embedding secretText into ImageMain

                    Bitmap WatermarkedImageSec = bmp;
                    secretTextSec = Cryptography.GetRandomString();
                    secretTextKeySec = Cryptography.GetRandomString();
                    encrytedSecretTextSec = EncryptStringAesIntoImage(secretTextSec, secretTextKeySec);
                    WatermarkedImageSec = Cryptography.embedText(encrytedSecretTextSec, WatermarkedImageSec);

                    ViewState["secretTextKeySec"] = secretTextKeySec;
                    ViewState["encrytedSecretTextSec"] = encrytedSecretTextSec;

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        //Save the Watermarked image to the MemoryStream.
                        WatermarkedImageSec.Save(memoryStream, ImageFormat.Png);
                        //Save image
                        uniqueFileNameSec = Path.ChangeExtension(FileUploadMain.FileName, DateTime.Now.Ticks.ToString());
                        ViewState["uniqueFileNameSec"] = uniqueFileNameSec;
                        string SecTargetPath = Path.Combine(photoFolderSec, uniqueFileNameSec + extensionSec);
                        WatermarkedImageSec.Save(SecTargetPath, WatermarkedImageSec.RawFormat);

                        //Displaying of image
                        Byte[] bytes = memoryStream.ToArray();
                        ViewState["filesizeSec"] = bytes.Length.ToString();
                        //filesizeSec = bytes.Length.ToString();
                        string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                        SecondaryUploadedImage.ImageUrl = "data:image/png;base64," + base64String;
                        SecondaryUploadedImage.Visible = true;
                        SecondaryUploadedImage.Width = 150;
                        SecondaryUploadedImage.Height = 150;
                        StatusLabelMain.Visible = false;
                    }
                }
            }
        }

        protected void btnDeleteMain_Click(object sender, EventArgs e)
        {
            photoFolderMain = (string)ViewState["PhotoFolderMain"];
            photoFolderSec = (string)ViewState["PhotoFolderSec"];
            uniqueFileNameMain = (string)ViewState["uniqueFileNameMain"];
            uniqueFileNameSec = (string)ViewState["uniqueFileNameSec"];
            extensionMain = (string)ViewState["extensionMain"];
            extensionSec = (string)ViewState["extensionSec"];
            string fileToDeleteMain = Path.Combine(photoFolderMain, uniqueFileNameMain + extensionMain);
            string fileToDeleteSec = Path.Combine(photoFolderSec, uniqueFileNameSec + extensionSec);
            File.Delete(fileToDeleteMain);
            File.Delete(fileToDeleteSec);
            MainUploadedImage.ImageUrl = null;
            MainUploadedImage.Visible = false;
            SecondaryUploadedImage.ImageUrl = null;
            SecondaryUploadedImage.Visible = false;
            StatusLabelMain.Text = "Upload status: PLease choose another file!!";
            StatusLabelMain.CssClass = "label label-warning";
        }

        protected void ShareBtn_Click(object sender, EventArgs e)
        {
            photoFolderMain = (string)ViewState["PhotoFolderMain"];
            photoFolderSec = (string)ViewState["PhotoFolderSec"];

            uniqueFileNameMain = (string)ViewState["uniqueFileNameMain"];
            uniqueFileNameSec = (string)ViewState["uniqueFileNameSec"];

            extensionMain = (string)ViewState["extensionMain"];
            extensionSec = (string)ViewState["extensionSec"];

            filesizeMain = (string)ViewState["filesizeMain"];
            filesizeSec = (string)ViewState["filesizeSec"];

            medianameMain = (string)ViewState["medianameMain"];

            secretTextKeyMain=(string) ViewState["secretTextKeyMain"];
            secretTextKeySec = (string)ViewState["secretTextKeySec"];

            encrytedSecretTextMain=(string) ViewState["encrytedSecretTextMain"] ;
            encrytedSecretTextSec = (string)ViewState["encrytedSecretTextSec"];

            string pathToCheckMain = Path.Combine(photoFolderMain, uniqueFileNameMain + extensionMain);
            string pathToCheckSec = Path.Combine(photoFolderSec, uniqueFileNameMain + extensionSec);

            if (File.Exists(pathToCheckMain))
            {
                using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["F2DatabaseConnectionString"].ConnectionString))
                {
                    SqlDataReader reader;
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "INSERT INTO [dbo].[FileUpload] ([FileType],[FilePath],[FileSize],[MediaName],[UserID]) VALUES (@FileType,@FilePath,@FileSize,@MediaName,@UserID);";
                    cmd.Parameters.Add("@FileType", SqlDbType.VarChar).Value = extensionMain;
                    cmd.Parameters.Add("@FilePath", SqlDbType.VarChar).Value = Path.Combine(photoFolderMain, uniqueFileNameMain + extensionMain);
                    cmd.Parameters.Add("@FileSize", SqlDbType.VarChar).Value = filesizeMain;
                    cmd.Parameters.Add("@MediaName", SqlDbType.VarChar).Value = medianameMain;
                    cmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = userid;
                    cmd.Connection = connection;
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();

                    SqlCommand cmd2 = new SqlCommand();
                    cmd2.CommandText = "INSERT INTO [dbo].[FileUploadSecret] ([EmbeddedSecretText],[EmbeddedSecretTextKey]) VALUES (@EmbeddedSecretText,@EmbeddedSecretTextKey);";
                    cmd2.Parameters.Add("@EmbeddedSecretText", SqlDbType.VarChar).Value = encrytedSecretTextMain;
                    cmd2.Parameters.Add("@EmbeddedSecretTextKey", SqlDbType.VarChar).Value = secretTextKeyMain;
                    cmd2.Connection = connection;
                    connection.Open();
                    cmd2.ExecuteNonQuery();
                    connection.Close();

                    SqlCommand cmd3 = new SqlCommand();
                    cmd3.CommandText = "INSERT INTO [dbo].[FileUploadSecondary] ([FileType],[FilePath],[FileSize],[UserID]) VALUES (@FileType,@FilePath,@FileSize,@UserID);";
                    cmd3.Parameters.Add("@FileType", SqlDbType.VarChar).Value = extensionSec;
                    cmd3.Parameters.Add("@FilePath", SqlDbType.VarChar).Value = Path.Combine(photoFolderSec, uniqueFileNameSec + extensionSec);
                    cmd3.Parameters.Add("@FileSize", SqlDbType.VarChar).Value = filesizeSec;
                    cmd3.Parameters.Add("@UserID", SqlDbType.VarChar).Value = userid;
                    cmd3.Connection = connection;
                    connection.Open();
                    cmd3.ExecuteNonQuery();
                    connection.Close();

                    SqlCommand cmd4 = new SqlCommand();
                    cmd4.CommandText = "INSERT INTO [dbo].[FileUploadSecondarySecret] ([EmbeddedSecretText],[EmbeddedSecretTextKey]) VALUES (@EmbeddedSecretText,@EmbeddedSecretTextKey);";
                    cmd4.Parameters.Add("@EmbeddedSecretText", SqlDbType.VarChar).Value = encrytedSecretTextSec;
                    cmd4.Parameters.Add("@EmbeddedSecretTextKey", SqlDbType.VarChar).Value = secretTextKeySec;
                    cmd4.Connection = connection;
                    connection.Open();
                    cmd4.ExecuteNonQuery();
                    connection.Close();

                    SqlCommand cmd5 = new SqlCommand();
                    cmd5.CommandText = "SELECT [FileUploadSecretID] FROM [dbo].[FileUploadSecret] WHERE [EmbeddedSecretText] = @EmbeddedSecretText ;";
                    cmd5.Parameters.Add("@EmbeddedSecretText", SqlDbType.VarChar).Value = encrytedSecretTextMain;
                    cmd5.Connection = connection;
                    connection.Open();
                    cmd5.ExecuteNonQuery();

                    reader = cmd5.ExecuteReader();
                    while (reader.Read())
                    {
                        fileUploadSecretID = reader.GetInt32(0);
                    }
                    connection.Close();

                    SqlCommand cmd6 = new SqlCommand();
                    cmd6.CommandText = "SELECT [FileUploadSecondarySecretID] FROM [dbo].[FileUploadSecondarySecret] WHERE [EmbeddedSecretText] = @EmbeddedSecretText ;";
                    cmd6.Parameters.Add("@EmbeddedSecretText", SqlDbType.VarChar).Value = encrytedSecretTextSec;
                    cmd6.Connection = connection;
                    connection.Open();
                    cmd6.ExecuteNonQuery();

                    reader = cmd6.ExecuteReader();
                    while (reader.Read())
                    {
                        fileUploadSecondarySecretID = reader.GetInt32(0);
                    }
                    connection.Close();

                    SqlCommand cmd7 = new SqlCommand();
                    cmd7.CommandText = "SELECT [FileUploadID] FROM [dbo].[FileUpload] WHERE [FilePath] = @FilePath ;";
                    cmd7.Parameters.Add("@FilePath", SqlDbType.VarChar).Value = Path.Combine(photoFolderMain, uniqueFileNameMain + extensionMain);
                    cmd7.Connection = connection;
                    connection.Open();
                    cmd7.ExecuteNonQuery();

                    reader = cmd7.ExecuteReader();
                    while (reader.Read())
                    {
                        fileUploadID = reader.GetInt32(0);
                    }
                    connection.Close();

                    SqlCommand cmd8 = new SqlCommand();
                    cmd8.CommandText = "SELECT [FileUploadSecondaryID] FROM [dbo].[FileUploadSecondary] WHERE [FilePath] = @FilePath ;";
                    cmd8.Parameters.Add("@FilePath", SqlDbType.VarChar).Value = Path.Combine(photoFolderSec, uniqueFileNameSec + extensionSec);
                    cmd8.Connection = connection;
                    connection.Open();
                    cmd8.ExecuteNonQuery();

                    reader = cmd8.ExecuteReader();
                    while (reader.Read())
                    {
                        fileUploadSecondaryID = reader.GetInt32(0);
                    }
                    connection.Close();

                    SqlCommand cmd9 = new SqlCommand();
                    cmd9.CommandText = "UPDATE [dbo].[FileUpload] SET [FileUploadSecretID]= @FileUploadSecretID WHERE [FileUploadID] = @FileUploadID";
                    cmd9.Parameters.Add("@FileUploadSecretID", SqlDbType.Int).Value = fileUploadSecretID;
                    cmd9.Parameters.AddWithValue("FileUploadID", fileUploadID);
                    cmd9.Connection = connection;
                    connection.Open();
                    cmd9.ExecuteNonQuery();
                    connection.Close();

                    SqlCommand cmd10 = new SqlCommand();
                    cmd10.CommandText = "UPDATE [dbo].[FileUploadSecondary] SET [FileUploadSecondarySecretID]= @FileUploadSecondarySecretID WHERE [FileUploadSecondaryID] = @FileUploadSecondaryID";
                    cmd10.Parameters.Add("@FileUploadSecondarySecretID", SqlDbType.Int).Value = fileUploadSecondarySecretID;
                    cmd10.Parameters.AddWithValue("FileUploadSecondaryID", fileUploadSecondaryID);
                    cmd10.Connection = connection;
                    connection.Open();
                    cmd10.ExecuteNonQuery();
                    connection.Close();

                    SqlCommand cmd11 = new SqlCommand();
                    cmd11.CommandText = "UPDATE [dbo].[Gallery] SET [FileUploadID] = @FileUploadID ,[FileUploadSecondaryID] = @FileUploadSecondaryID WHERE [GalleryID] = @GalleryID;";
                    cmd11.Parameters.Add("@FileUploadID", SqlDbType.Int).Value = fileUploadID;
                    cmd11.Parameters.Add("@FileUploadSecondaryID", SqlDbType.Int).Value = fileUploadSecondaryID;
                    cmd11.Parameters.AddWithValue("GalleryID", galleryID);
                    cmd11.Connection = connection;
                    connection.Open();
                    cmd11.ExecuteNonQuery();
                    connection.Close();
                }
            }
            else
            {
                StatusLabelMain.Text = "Unable to Share!!! You did not upload any photos ";
                StatusLabelMain.CssClass = "label label-danger";
            }
        }

        //Encryption for the secret text for image
        public static string EncryptStringAesIntoImage(string plainText, string sharedSecret)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException("plainText");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException("sharedSecret");

            string outStr = null;                       // Encrypted string to return
            RijndaelManaged aesAlg = null;              // RijndaelManaged object used to encrypt the data.

            try
            {
                // generate the key from the shared secret and the salt
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);

                // Create a RijndaelManaged object
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

                // Create a decryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    // prepend the IV
                    msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                    }
                    outStr = Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            // Return the encrypted bytes from the memory stream.
            return outStr;
        }
    }
}
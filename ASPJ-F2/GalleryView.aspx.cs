using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASPJ_F2
{
    public partial class GalleryView : System.Web.UI.Page
    {
        private static byte[] _salt = Encoding.ASCII.GetBytes("jasdh7834y8hfeur73rsharks214");
        //temp
        private string userid="123";
        private string gid="72";

        //Gallery Database var
        private string title;
        private int amount;
        private string desc;

        //UploadedImage [Main]
        private byte[] imageMain;

        //UploadedImage Sec
        private byte[] imageSec;

        //FileUploadMain Database var
        private int fileuploadID;
        private string filetypeMain;
        private string filepathMain;
        private string filesizeMain;
        private string filenameMain;
        private int fileuploadsecretID;
        private string embeddedsecrettextMain;
        private string embeddedsecrettextkeyMain;

        //FileUploadSecondary Database  var
        private int fileuploadsecondaryID;
        private string filetypeSec;
        private string filepathSec;
        private int fileuploadsecondarysecretID;
        private string embeddedsecrettextSec;
        private string embeddedsecrettextkeySec;



        protected void Page_Load(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["F2DatabaseConnectionString"].ConnectionString))
            {
                SqlDataReader reader;
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT [DesignName],[FileUploadID],[FileUploadSecondaryID],[Cost],[Description] FROM [dbo].[Gallery] WHERE [GalleryID]= @GalleryID AND [UserID] = @UserID;";
                cmd.Parameters.Add("@GalleryID", SqlDbType.Int).Value = gid;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userid;
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    title = reader.GetString(0);
                    fileuploadID = reader.GetInt32(1);
                    fileuploadsecondaryID = reader.GetInt32(2);
                    amount = reader.GetInt32(3);
                    desc = reader.GetString(4);
                }
                connection.Close();

                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandText = "SELECT [FileType],[FilePath],[FileSize],[MediaName],[FileUploadSecretID] FROM [dbo].[FileUpload] WHERE [FileUploadID]= @FileUploadID AND [UserID] = @UserID;";
                cmd2.Parameters.Add("@FileUploadID", SqlDbType.Int).Value = fileuploadID;
                cmd2.Parameters.Add("@UserID", SqlDbType.Int).Value = userid;
                cmd2.Connection = connection;
                connection.Open();
                cmd2.ExecuteNonQuery();

                reader = cmd2.ExecuteReader();
                while (reader.Read())
                {
                    filetypeMain = reader.GetString(0);
                    filepathMain = reader.GetString(1);
                    filesizeMain = reader.GetString(2);
                    filenameMain = reader.GetString(3);
                    fileuploadsecretID = reader.GetInt32(4);
                }
                connection.Close();

                SqlCommand cmd3 = new SqlCommand();
                cmd3.CommandText = "SELECT [FileType],[FilePath],[FileUploadSecondarySecretID] FROM [dbo].[FileUploadSecondary] WHERE [FileUploadSecondaryID]= @FileUploadSecondaryID AND [UserID] = @UserID;";
                cmd3.Parameters.Add("@FileUploadSecondaryID", SqlDbType.Int).Value = fileuploadsecondaryID;
                cmd3.Parameters.Add("@UserID", SqlDbType.Int).Value = userid;
                cmd3.Connection = connection;
                connection.Open();
                cmd3.ExecuteNonQuery();

                reader = cmd3.ExecuteReader();
                while (reader.Read())
                {
                    filetypeSec = reader.GetString(0);
                    filepathSec = reader.GetString(1);
                    fileuploadsecondarysecretID = reader.GetInt32(2);
                }
                connection.Close();

                SqlCommand cmd4 = new SqlCommand();
                cmd4.CommandText = "SELECT [EmbeddedSecretText],[EmbeddedSecretTextKey] FROM [dbo].[FileUploadSecret] WHERE [FileUploadSecretID]= @FileUploadSecretID;";
                cmd4.Parameters.Add("@FileUploadSecretID", SqlDbType.Int).Value = fileuploadsecretID;
                cmd4.Connection = connection;
                connection.Open();
                cmd4.ExecuteNonQuery();

                reader = cmd4.ExecuteReader();
                while (reader.Read())
                {
                    embeddedsecrettextMain = reader.GetString(0);
                    embeddedsecrettextkeyMain = reader.GetString(1);
                }
                connection.Close();

                SqlCommand cmd5 = new SqlCommand();
                cmd5.CommandText = "SELECT [EmbeddedSecretText],[EmbeddedSecretTextKey] FROM [dbo].[FileUploadSecondarySecret] WHERE [FileUploadSecondarySecretID]= @FileUploadSecondarySecretID;";
                cmd5.Parameters.Add("@FileUploadSecondarySecretID", SqlDbType.Int).Value = fileuploadsecondarysecretID;
                cmd5.Connection = connection;
                connection.Open();
                cmd5.ExecuteNonQuery();

                reader = cmd5.ExecuteReader();
                while (reader.Read())
                {
                    embeddedsecrettextSec = reader.GetString(0);
                    embeddedsecrettextkeySec = reader.GetString(1);
                }
                connection.Close();
            }

            DesignTitleLabel.Text = title;
            NameLabel.Text = "Blah Blah need to change";

            //Image 
            if (File.Exists(filepathMain) && File.Exists(filepathSec))
            {
                imageMain = File.ReadAllBytes(filepathMain);
                imageSec = File.ReadAllBytes(filepathSec);
                System.Drawing.Image picMain = byteArrayToImage(imageMain);
                System.Drawing.Image picSec = byteArrayToImage(imageSec);
                Bitmap bmpMain = new Bitmap(picMain);
                Bitmap bmpSec = new Bitmap(picSec);

                //Extraction of secret text
                string ExtractedTextMain = Cryptography.extractText(bmpMain);
                string ExtractedTextSec = Cryptography.extractText(bmpSec);

                //Decrytion of secret text
                string plainExtractedTextMain = DecryptImageAesIntoString(ExtractedTextMain, embeddedsecrettextkeyMain);
                string plainExtractedTextSec = DecryptImageAesIntoString(ExtractedTextSec, embeddedsecrettextkeySec);
                string originalPlainTextMain = DecryptImageAesIntoString(embeddedsecrettextMain, embeddedsecrettextkeyMain);
                string originalPlainTextSec = DecryptImageAesIntoString(embeddedsecrettextSec, embeddedsecrettextkeySec);

                if (originalPlainTextMain == plainExtractedTextMain && originalPlainTextSec == plainExtractedTextSec)
                {
                    //Displaying of sec Image
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bmpSec.Save(ms, ImageFormat.Png);
                        byte[] byteImageSec = ms.ToArray();
                        string base64StringImageSec = Convert.ToBase64String(byteImageSec);
                        SecImage.ImageUrl = "data:image/png;base64," + base64StringImageSec;
                    }
                }
            }
            
        }
        //Decryption for the secet text in image
        public static string DecryptImageAesIntoString(string cipherText, string sharedSecret)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException("cipherText");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException("sharedSecret");

            // Declare the RijndaelManaged object
            // used to decrypt the data.
            RijndaelManaged aesAlg = null;

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            try
            {
                // generate the key from the shared secret and the salt
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);

                // Create the streams used for decryption.
                byte[] bytes = Convert.FromBase64String(cipherText);
                using (MemoryStream msDecrypt = new MemoryStream(bytes))
                {
                    // Create a RijndaelManaged object
                    // with the specified key and IV.
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                    // Get the initialization vector from the encrypted stream
                    aesAlg.IV = ReadByteArray(msDecrypt);
                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            return plaintext;
        }
        private static byte[] ReadByteArray(Stream s)
        {
            byte[] rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
                throw new SystemException("Stream did not contain properly formatted byte array");
            }

            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new SystemException("Did not read byte array properly");
            }

            return buffer;
        }
        public System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }
    }
}
using nClam;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace ASPJ_F2
{
    public partial class ImageShare : System.Web.UI.Page
    {
        private static byte[] _salt = Encoding.ASCII.GetBytes("jasdh7834y8hfeur73rsharks214");
        private string encrytedBytetext;
        private byte[] encrytedImages;
        private string secretEncryptionKey;
        private int counter = 0;

        //Important Stuff - Gallery
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
        private string targetPath;

        //Secondary upload var
        private string randomPicIDSec;

        private List<string> PathList = new List<string>();
        private string photoFolderSec;
        private string extensionSec;
        private string fromRootToPhotosSecondary = @"C:\Users\User\Documents\Visual Studio 2015\Projects\ASPJ-F2\ASPJ-F2\Images\UploadedPhotos\Secondary\";

        //Temp
        private string userid = "123";

        protected void Page_Load(object sender, EventArgs e)
        {
            string idFromPrevious = Cryptography.DecryptUrl(HttpUtility.UrlDecode(Request.QueryString["23rewwr343wd9jfsk23dmjd2q33c3g"]));
            galleryID = Int32.Parse(idFromPrevious);

            //SecretText Generate
            if (ViewState["StoredText"] == null)
            {
                secretEncryptionKey = Cryptography.GetRandomString();
            }
            else
            {
                secretEncryptionKey = (string)ViewState["StoredText"];
            }
            //Encrypt of secret text in image of Main
            if (ViewState["StoredIdMain"] == null)
            {
                randomPicIDMain = Cryptography.GetRandomString();
            }
            else
            {
                randomPicIDMain = (string)ViewState["StoredIdMain"];
            }
            //Encrypt of secret text in image of Secondary
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
            extensionMain = Path.GetExtension(FileUploadMain.FileName);
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
                            if (!Directory.Exists(photoFolderMain))
                            {
                                Directory.CreateDirectory(photoFolderMain);
                            }
                            ViewState["PhotoFolderMain"] = photoFolderMain;
                            string extensionMain = Path.GetExtension(FileUploadMain.FileName);
                            string uniqueFileNameMain = Path.ChangeExtension(FileUploadMain.FileName, DateTime.Now.Ticks.ToString());

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
                            FileUploadMain.SaveAs(Path.Combine(photoFolderMain, uniqueFileNameMain + extensionMain));
                            var clam = new ClamClient("localhost", 3310);
                            var scanResult = clam.ScanFileOnServer(Path.Combine(photoFolderMain, uniqueFileNameMain + extensionMain));

                            switch (scanResult.Result)
                            {
                                case ClamScanResults.Clean:
                                    StatusLabelMain.CssClass = "label label-success";
                                    StatusLabelMain.Text = "Upload status: File uploaded!";
                                    DisplayMainUploadedPhotos();
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

        public void DisplayMainUploadedPhotos()
        {
            Stream fs = FileUploadMain.PostedFile.InputStream;
            BinaryReader br = new BinaryReader(fs);
            Byte[] bytes = br.ReadBytes((Int32)fs.Length);
            string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
            MainUploadedImage.ImageUrl = "data:image/png;base64," + base64String;
            MainUploadedImage.Visible = true;
            MainUploadedImage.Width = 150;
            MainUploadedImage.Height = 150;
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

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        //Save the Watermarked image to the MemoryStream.
                        bmp.Save(memoryStream, ImageFormat.Png);
                        Byte[] bytes = memoryStream.ToArray();
                        string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                        SecondaryUploadedImage.ImageUrl = "data:image/png;base64," + base64String;
                        SecondaryUploadedImage.Visible = true;
                        SecondaryUploadedImage.Width = 150;
                        SecondaryUploadedImage.Height = 150;
                    }
                }
            }
        }
       

        protected void btnDeleteMain_Click(object sender, EventArgs e)
        {
        }

        //protected void btnPreviewSecondary_Click(object sender, EventArgs e)
        //{
        //    extensionSec = Path.GetExtension(FileUploadSec.FileName);
        //    if (FileUploadSec.HasFile)
        //    {
        //        try
        //        {
        //            if (FileUploadSec.PostedFile.ContentType == "image/png" || FileUploadSec.PostedFile.ContentType == "text/plain")
        //            {
        //                if (FileUploadSec.PostedFile.ContentLength < 1000000)
        //                {
        //                    photoFolderSec = Path.Combine(fromRootToPhotosSecondary, randomPicIDSec);
        //                    ViewState["StoredIdSec"] = randomPicIDSec;
        //                    if (!Directory.Exists(photoFolderSec))
        //                    {
        //                        Directory.CreateDirectory(photoFolderSec);
        //                    }
        //                    ViewState["PhotoFolderSec"] = photoFolderSec;
        //                    string extensionSec = Path.GetExtension(FileUploadSec.FileName);
        //                    string uniqueFileNameSec = Path.ChangeExtension(FileUploadSec.FileName, DateTime.Now.Ticks.ToString());
        //                    FileUploadSec.SaveAs(Path.Combine(photoFolderSec, uniqueFileNameSec + extensionSec));
        //                    var clam = new ClamClient("localhost", 3310);
        //                    var scanResult = clam.ScanFileOnServer(Path.Combine(photoFolderSec, uniqueFileNameSec + extensionSec));

        //                    switch (scanResult.Result)
        //                    {
        //                        case ClamScanResults.Clean:
        //                            StatusLabelSec.CssClass = "label label-success";
        //                            StatusLabelSec.Text = "Upload status: File uploaded!";

        //                            DisplayUploadedPhotos(photoFolderSec);
        //                            break;

        //                        case ClamScanResults.VirusDetected:
        //                            StatusLabelSec.Text = "Upload status: Virus Found!!!!!";
        //                            File.Delete(Path.Combine(photoFolderSec, uniqueFileNameSec + extensionSec));
        //                            StatusLabelSec.CssClass = "label label-danger";
        //                            break;

        //                        case ClamScanResults.Error:
        //                            StatusLabelSec.Text = scanResult.RawResult;
        //                            File.Delete(Path.Combine(photoFolderSec, uniqueFileNameSec + extensionSec));
        //                            StatusLabelSec.CssClass = "label label-danger";
        //                            break;
        //                    }
        //                }
        //                else
        //                {
        //                    StatusLabelSec.Text = "Upload status: The file has to be less than 1 MB!";
        //                    StatusLabelSec.CssClass = "label label-danger";
        //                }
        //            }
        //            else
        //            {
        //                StatusLabelSec.Text = "Upload status: Only PNG Or BMP files are accepted!";
        //                StatusLabelSec.CssClass = "label label-danger";
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            StatusLabelSec.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
        //        }
        //    }
        //    else
        //    {
        //        StatusLabelSec.Text = "Upload status: You have not chosen a picture to preview!!";
        //        StatusLabelSec.CssClass = "label label-danger";
        //    }
        //}

        //public void DisplayUploadedPhotos(string folder)
        //{
        //    string[] allPhotoFiles = Directory.GetFiles(folder);
        //    IList<string> allPhotoPaths = new List<string>();

        //    string fileName;

        //    foreach (string f in allPhotoFiles)
        //    {
        //        fileName = Path.GetFileName(f);
        //        allPhotoPaths.Add("images/UploadedPhotos/Secondary/" + randomPicIDSec + "/" + fileName);
        //        PathList.Add("images/UploadedPhotos/Secondary/" + randomPicIDSec + "/" + fileName);
        //        ViewState["PathList"] = PathList;
        //    }
        //    rptrUserPhotos.DataSource = allPhotoPaths;
        //    rptrUserPhotos.DataBind();
        //}

        protected void imgUserPhoto_Command(object sender, CommandEventArgs e)
        {
            StringBuilder script = new StringBuilder();
            script.Append("<script type='text/javascript'>");
            script.Append("var viewer = new PhotoViewer(); ");
            script.Append("viewer.setBorderWidth(0);");
            script.Append("viewer.disableToolbar();");
            script.Append("viewer.add('" + e.CommandArgument + "');");
            script.Append("viewer.show(0);");
            script.Append("</script>");

            ClientScript.RegisterStartupScript(GetType(), "viewer", script.ToString());
        }

        //protected void btnDeleteSecondary_Click(object sender, EventArgs e)
        //{
        //    {
        //        foreach (RepeaterItem ri in rptrUserPhotos.Items)
        //        {
        //            CheckBox cb = ri.FindControl("cbDelete") as CheckBox;
        //            if (cb.Checked)
        //            {
        //                string fromPhotosToExtension = cb.Attributes["special"];
        //                string fromRootToHome = @"C:\Users\User\Documents\Visual Studio 2015\Projects\ASPJ-F2\ASPJ-F2\Images\UploadedPhotos\Secondary\S";
        //                string fileToDelete = Path.Combine(Directory.GetParent(fromRootToHome).ToString(), fromPhotosToExtension);
        //                File.Delete(fileToDelete);
        //            }
        //        }
        //        DisplayUploadedPhotos(Path.Combine(fromRootToPhotosSecondary, randomPicIDSec));
        //    }
        //}

        protected void ShareBtn_Click(object sender, EventArgs e)
        {
        }
    }
}
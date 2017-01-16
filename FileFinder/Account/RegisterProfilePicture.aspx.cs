using FileFinder.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;

namespace FileFinder.Account
{
    public partial class RegisterProfilePicture : System.Web.UI.Page
    {
        protected void ButtonUpload_Click(object sender, EventArgs e)
        {
            if (ProfilePictureUpload.HasFile)
            {
                string fileName = ProfilePictureUpload.FileName;
                Image profilePicture = new Image();
                string guid = Guid.NewGuid().ToString();
                FileInfo fileInfo = new FileInfo(fileName);
                profilePicture.ImageUrl = "~/Image_ProfilePicture/" + guid + fileInfo.Name;
                profilePicture.Width = Unit.Pixel(256);
                profilePicture.Height = Unit.Pixel(256);
                profilePicture.Style.Add("padding", "5px");
                ProfilePictureUpload.PostedFile.SaveAs(Server.MapPath("~/Image_ProfilePicture/" + guid + fileName));
                ProfilePicturePanel.Controls.Add(profilePicture);
                ProfilePictureFileName.Text = guid + fileName;
            }
        }

        protected void ProfilePictureNext_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationUser user = manager.FindById(User.Identity.GetUserId());
            user.ProfilePicture = ProfilePictureFileName.Text;
            IdentityResult result = manager.Update(user);
            if (result.Succeeded)
            {
                Response.Redirect("ImageAuthentication.aspx");
            }
        }
    }
}
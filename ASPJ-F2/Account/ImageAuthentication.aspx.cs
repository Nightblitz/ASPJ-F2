using System;

namespace ASPJ_F2.Account
{
    public partial class ImageAuthentication : System.Web.UI.Page
    {
        protected void ImageAuthenticationNext_Click(object sender, EventArgs e)
        {
            //var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            //var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            //var user = new ApplicationUser();

            //IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
            Response.Redirect("EmailConfirmation.aspx");
        }
    }
}
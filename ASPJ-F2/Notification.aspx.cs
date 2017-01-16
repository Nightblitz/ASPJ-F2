using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASPJ_F2
{
    public partial class Notification : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Label2.Text = "Page loaded at: " + DateTime.Now.ToLongTimeString();
            String lol = session.SName;
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            Label1.Text = "Page refreshed at: " + DateTime.Now.ToLongTimeString();
        }

        protected void LikeB_Click(object sender, EventArgs e)
        {
            String la = Label3.Text;

            using (SqlConnection connection123 = new
SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[
"NotificationConnectionString"].ConnectionString))
            {
                connection123.Open();
                SqlCommand command = new SqlCommand();
                //cmd.CommandText = "INSERT INTO [nspj].[dbo].[Company] (CompanyName,CompanyAddress,CompanySize,CompanyLocation,CompanyNo)  VALUES ('" + Cname.Text + "','" + address.Text + "','" + RadioButtonList1.SelectedValue + "','" + RadioButtonList2.SelectedValue + "','" + PhoneNo.Text + "');";
                command.CommandText = "INSERT INTO [nspj].[dbo].[Notification] (Type, Sender,Receiver,Message,Status) VALUES (@0,@1);";
                command.Parameters.Add(new SqlParameter("@0",1 ));
                command.Parameters.Add(new SqlParameter("@1", session.SName));
                command.Parameters.Add(new SqlParameter("@2", la));
                command.Parameters.Add(new SqlParameter("@3", ""));
                command.Parameters.Add(new SqlParameter("@4", "No"));

                command.Connection = connection123;

                command.ExecuteNonQuery();
                connection123.Close();
            }
        }
    }
}
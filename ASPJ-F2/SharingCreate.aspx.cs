﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASPJ_F2
{
    public partial class SharingCreate : System.Web.UI.Page
    {
        private string title;
        private string desc;
        private string category;
        private int cost;
        //temp
        private int userid = 123;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void NextBtn_Click(object sender, EventArgs e)
        {
            int catID = 0;
            int galleryID = 0;

            title = TitleTextBox.Text;
            desc = DescriptionTextBox.Text;
            category = CategoryDropDownList.SelectedItem.Text;
            cost = Convert.ToInt32(CostTextBox.Text);

            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["F2DatabaseConnectionString"].ConnectionString))
            {
                SqlDataReader reader;
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT [CategoryID] FROM [dbo].[Category] WHERE [CategoryName] = @CategoryName;";
                cmd.Parameters.Add("@CategoryName", SqlDbType.VarChar).Value = category;
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                     catID= reader.GetInt32(0);
                }
                connection.Close();

                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandText = "INSERT INTO [dbo].[Gallery] ([DesignName],[Description],[Cost],[CategoryID],[UserID]) VALUES (@DesignName,@Description,@Cost,@CategoryID,@UserID);";
                cmd2.Parameters.Add("@DesignName", SqlDbType.VarChar).Value = title;
                cmd2.Parameters.Add("@Description", SqlDbType.VarChar).Value = desc;
                cmd2.Parameters.Add("@Cost", SqlDbType.Int).Value = cost;
                cmd2.Parameters.Add("@CategoryID", SqlDbType.Int).Value = catID;
                cmd2.Parameters.Add("@UserID", SqlDbType.Int).Value = userid;
                cmd2.Connection = connection;
                connection.Open();
                cmd2.ExecuteNonQuery();
                connection.Close();

                SqlCommand cmd3 = new SqlCommand();
                cmd3.CommandText = "SELECT [GalleryID] FROM [dbo].[Gallery] WHERE [DesignName] = @DesignName AND [UserID] = @UserID;";
                cmd3.Parameters.Add("@DesignName", SqlDbType.VarChar).Value = title;
                cmd3.Parameters.Add("@UserID", SqlDbType.Int).Value = userid;
                cmd3.Connection = connection;
                connection.Open();
                cmd3.ExecuteNonQuery();

                reader = cmd3.ExecuteReader();
                while (reader.Read())
                {
                    galleryID = reader.GetInt32(0);
                }
                connection.Close();
            }
            string GID = HttpUtility.UrlEncode(Cryptography.EncryptUrl(galleryID.ToString().Trim()));
            Response.Redirect(string.Format("ImageShare.aspx?23rewwr343wd9jfsk23dmjd2q33c3g={0}", GID));
        }
    }
}
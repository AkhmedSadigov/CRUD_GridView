using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace WebApplication17
{
    public partial class WebForm1 : System.Web.UI.Page
    {
            SqlConnection con = new SqlConnection("Data Source=LAPTOP-D0D6I3G9\\SQLEXPRESS;Initial Catalog=Northwind;Integrated Security=True");
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                refreshdata();
            }
        }

        public void refreshdata()
        {
            con.Open();
            SqlCommand cmd=new SqlCommand("select ProductID, ProductName, QuantityPerUnit, UnitPrice, UnitsInStock from Products", con);
            SqlDataAdapter sda=new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            GridView1.DataSource= dt;
            GridView1.DataBind();
            con.Close();
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];

            int ProductID = int.Parse((row.Cells[0].Controls[0] as TextBox).Text);
            string ProductName = (row.Cells[1].Controls[0] as TextBox).Text;
            string QuantityPerUnit = (row.Cells[2].Controls[0] as TextBox).Text;
            double UnitPrice = double.Parse((row.Cells[3].Controls[0] as TextBox).Text);
            int UnitsInStock = int.Parse((row.Cells[4].Controls[0] as TextBox).Text);

            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE Products SET ProductName=@ProductName, QuantityPerUnit=@QuantityPerUnit, UnitPrice=@UnitPrice, UnitsInStock=@UnitsInStock WHERE ProductID=@ProductID", con);
            cmd.Parameters.AddWithValue("@ProductName", ProductName);
            cmd.Parameters.AddWithValue("@QuantityPerUnit", QuantityPerUnit);
            cmd.Parameters.AddWithValue("@UnitPrice", UnitPrice);
            cmd.Parameters.AddWithValue("@UnitsInStock", UnitsInStock);
            cmd.Parameters.AddWithValue("@ProductID", ProductID);
            cmd.ExecuteNonQuery();
            con.Close();
            GridView1.EditIndex = -1;
            refreshdata();

        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            refreshdata();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("delete from Products where ProductID =@id", con);
            int id = Convert.ToInt16(GridView1.Rows[e.RowIndex].Cells[0].Text.ToString());
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            con.Close();
            refreshdata();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            refreshdata();
        }
    }
}
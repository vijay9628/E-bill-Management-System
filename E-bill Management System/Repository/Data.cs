using E_bill_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using E_bill_Management_System.Repository;

namespace E_bill_Management_System.Repository
{
    public class Data : IData
    {
        public string ConnectionString { get; set; }

        public Data()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
        }
        public void saveBillDetails(BillDetails bd)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            try
            {
                bd.TotalAmount = bd.Items.Sum(x => x.Price * x.Quantity);

                con.Open();
                SqlCommand cmd = new SqlCommand("sp_saveBillDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerName",bd.CustomerName);
                cmd.Parameters.AddWithValue("@MobileNumber",bd.MobileNumber);
                cmd.Parameters.AddWithValue("@Address",bd.Address);
                cmd.Parameters.AddWithValue("@TotalAmount",bd.TotalAmount);
                SqlParameter outputpara = new SqlParameter();
                outputpara.DbType = DbType.Int32;
                outputpara.Direction = ParameterDirection.Output;
                outputpara.ParameterName = "@Id";
                cmd.Parameters.Add(outputpara);
                cmd.ExecuteNonQuery();
                int id = int.Parse(outputpara.Value.ToString());
                if(bd.Items.Count > 0)
                {
                    saveBillItems(bd.Items,con,id);
                }
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public void saveBillItems(List<Items> items, SqlConnection con, int id)
        {
            try
            {
                string qry = "insert into tbl_BillItems(ProductName,Price,Quantity ,BillId) values";
                foreach (var item in items)
                {
                    qry += string.Format("('{0}',{1},{2},{3}),", item.ProductName, item.Price, item.Quantity, id);
                }
                qry = qry.Remove(qry.Length - 1);
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<BillDetails> GetAllDetails()
        {
            List<BillDetails> list = new List<BillDetails>();
                SqlConnection con = new SqlConnection(ConnectionString);
            BillDetails details;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_getAllBillDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    details = new BillDetails();
                    details.Id = int.Parse(reader["Id"].ToString());
                    details.CustomerName= reader["CustomerName"].ToString();
                    details.MobileNumber= reader["MobileNumber"].ToString();
                    details.Address= reader["Address"].ToString();
                    details.TotalAmount = int.Parse(reader["TotalAmount"].ToString());
                    list.Add(details);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
            return list;
        }

        public BillDetails GetDetails(int Id)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            BillDetails detail = new BillDetails();
            Items items;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_getBillDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", Id);
                  SqlDataReader reader = cmd.ExecuteReader();
              

                while(reader.Read())
                {
                    detail.Id = int.Parse(reader["BillId"].ToString());
                    detail.CustomerName = reader["CustomerName"].ToString();
                    detail.MobileNumber = reader["MobileNumber"].ToString();
                    detail.Address = reader["Address"].ToString();
                    detail.TotalAmount = int.Parse(reader["TotalAmount"].ToString());
                    items = new Items();
                    items.Id = int.Parse(reader["ItemId"].ToString());
                    items.ProductName = reader["ProductName"].ToString();
                    items.Price= int.Parse(reader["Price"].ToString());
                    items.Quantity= int.Parse(reader["Quantity"].ToString());
                    detail.Items.Add(items);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }
            return detail;
        }

        //public BillDetails changecol(BillDetails details)
        //{
        //    SqlConnection con = new SqlConnection(ConnectionString);

        //    try
        //    {
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand("", con);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    finally
        //    {

        //    }
        //}
    }
}
using E_bill_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_bill_Management_System.Repository
{
    interface IData
    {
        void saveBillDetails(BillDetails bd);
        void saveBillItems(List<Items> items,SqlConnection con,int id);
        List<BillDetails> GetAllDetails();
        BillDetails GetDetails(int Id);

        //BillDetails changecol(BillDetails details);
    }
}

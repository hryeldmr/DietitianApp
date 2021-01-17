using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;

namespace Diyetisyen
{
    class baglanti
    {
        private OleDbConnection con;
        private OleDbCommand komut;
        private OleDbDataAdapter da;


        public OleDbConnection acik()
        {
            con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Huriş\Desktop\a\Diyetisyen\Diyetisyen\App_Data");
            con.Open();
            return con;
        }

        public void kapali()
        {
            con.Close();
        }

        public int idu(string cumle)
        {
            this.acik();
            this.komut = new OleDbCommand(cumle, con);
            int sonuc = 0;
            try
            {
                sonuc = komut.ExecuteNonQuery();
            }
            catch (Exception msj)
            {
                throw new Exception(msj.Message);
            }
            return sonuc;
            this.kapali();
        }



        public DataTable tablogetir(string sorgu)
        {
            this.acik();
            this.komut = new OleDbCommand(sorgu, con);
            DataTable tb = new DataTable();
            da = new OleDbDataAdapter(this.komut);
            try
            {
                da.Fill(tb);
            }
            catch (Exception msj)
            {
                throw new Exception(msj.Message);
            }
            return tb;
            this.kapali();
        }




    }
}

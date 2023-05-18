using MySql.Data.MySqlClient;
using System.Collections;

namespace WpfApp1
{
    class models
    {
    }

    //OBJECT CLIENT

    #region

    class Client : IEnumerable
    {
        public string documentI { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public int type_client { get; set; }

        public IEnumerator GetEnumerator()
        {
            yield return documentI;
            yield return name;
            yield return address;
            yield return email;
            yield return phone;
            yield return type_client;
        }
    }


    #endregion

    //QUERY CLIENT

    #region

    class Query_Client
    {
        private conector c = new conector();
        public MySqlDataAdapter GetListClient()
        {
            string query = "select * from client";
            MySqlCommand cmd = new MySqlCommand(query, c.Conect());
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            return adapter;
        }
    }

    #endregion


}

using System.Data;
using System.Data.SqlClient;

namespace CreatTransferTransactiom.DLL
{
    public class DAL
    {
        public async void InsertLog(string RequestName, DateTime MWRequestTime, String MWRequest, string status, string ChannelName, string RequestID)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var DBConnection = MyConfig.GetValue<string>("AppSettings:DBConnection");
            SqlConnection Database_Conection = new SqlConnection(DBConnection);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "IMALLogInsert";
            cmd.Parameters.AddWithValue("RequestName", RequestName);
            cmd.Parameters.AddWithValue("MWRequestTime", MWRequestTime);
            cmd.Parameters.AddWithValue("MWRequest", MWRequest);
            cmd.Parameters.AddWithValue("status", status);
            cmd.Parameters.AddWithValue("ChannelName", ChannelName);
            cmd.Parameters.AddWithValue("RequestID", RequestID);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = Database_Conection;
            Database_Conection.Open();
            cmd.ExecuteNonQuery();
            Database_Conection.Close();

        }

        public async void UpdateLog(string MWResponseTime, string MWResponse, string Status, string ChannelName, string RequestID)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var DBConnection = MyConfig.GetValue<string>("AppSettings:DBConnection");
            SqlConnection Database_Conection = new SqlConnection(DBConnection);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "UpdateLog";
            cmd.Parameters.AddWithValue("MWResponseTime", MWResponseTime);
            cmd.Parameters.AddWithValue("MWResponse", MWResponse);
            cmd.Parameters.AddWithValue("Status", Status);
            cmd.Parameters.AddWithValue("ChannelName", ChannelName);
            cmd.Parameters.AddWithValue("RequestID", RequestID);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = Database_Conection;
            Database_Conection.Open();
            cmd.ExecuteNonQuery();
            Database_Conection.Close();
        }

        public DataTable IMALChannelstatus(string ChannelName, string username, string ChannelIPAddress, string ServiceName)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var DBConnection = MyConfig.GetValue<string>("AppSettings:DBConnection");
            SqlConnection Database_Conection = new SqlConnection(DBConnection);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "IMALChannelstatus";
            cmd.Parameters.AddWithValue("ChannelName", ChannelName);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("IPAddress", ChannelIPAddress);
            cmd.Parameters.AddWithValue("ServiceName", ServiceName);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = Database_Conection;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
    }
}


using MySql.Data.MySqlClient;
using RestSharp;
using System;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Collections.Generic;
using System.Linq;



namespace ProjectTemplate
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]

    public class ProjectServices : System.Web.Services.WebService
    {
        ////////////////////////////////////////////////////////////////////////
        ///replace the values of these variables with your database credentials
        ////////////////////////////////////////////////////////////////////////
        private string dbID = "team248";
        private string dbPass = "!!Team248";
        private string dbName = "team248";
        ////////////////////////////////////////////////////////////////////////

        ////////////////////////////////////////////////////////////////////////
        ///call this method anywhere that you need the connection string!
        ////////////////////////////////////////////////////////////////////////
        private string getConString()
        {
            return "SERVER=107.180.1.16; PORT=3306; DATABASE=" + dbName + "; UID=" + dbID + "; PASSWORD=" + dbPass;
        }
        ////////////////////////////////////////////////////////////////////////



        /////////////////////////////////////////////////////////////////////////
        //don't forget to include this decoration above each method that you want
        //to be exposed as a web service!
        //[WebMethod(EnableSession = true)]
        /////////////////////////////////////////////////////////////////////////
        //public string TestConnection()
        //{
        //    try
        //    {
        //        string testQuery = "select * from users";

        //        ////////////////////////////////////////////////////////////////////////
        //        ///here's an example of using the getConString method!
        //        ////////////////////////////////////////////////////////////////////////
        //        MySqlConnection con = new MySqlConnection(getConString());
        //        ////////////////////////////////////////////////////////////////////////

        //        MySqlCommand cmd = new MySqlCommand(testQuery, con);
        //        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
        //        DataTable table = new DataTable();
        //        adapter.Fill(table);
        //        return "Success!";
        //    }
        //    catch (Exception e)
        //    {
        //        return "Something went wrong, please check your credentials and db name and try again.  Error: " + e.Message;
        //    }
        //}

        // Log On WebMethod
        [WebMethod(EnableSession = true)]
        public bool LogOnMentee(string uid, string pass)
        {
            //LOGIC: pass the parameters into the database to see if an account
            //with these credentials exist.  If it does, then return true.  If
            //it doesn't, then return false

            //we return this flag to tell them if they logged in or not
            bool success = false;

            //our connection string comes from our web.config file like we talked about earlier
            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["myDB"].ConnectionString;
            //here's our query.  A basic select with nothing fancy.  Note the parameters that begin with @
            string sqlSelect = "SELECT id FROM 2_Login_Mentee WHERE email=@userName and password=@passValue";

            //set up our connection object to be ready to use our connection string
            MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);
            //set up our command object to use our connection, and our query
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);

            //tell our command to replace the @parameters with real values
            //we decode them because they came to us via the web so they were encoded
            //for transmission (funky characters escaped, mostly)
            sqlCommand.Parameters.AddWithValue("@userName", HttpUtility.UrlDecode(uid));
            sqlCommand.Parameters.AddWithValue("@passValue", HttpUtility.UrlDecode(pass));

            //a data adapter acts like a bridge between our command object and 
            //the data we are trying to get back and put in a table object
            MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);
            //here's the table we want to fill with the results from our query
            DataTable sqlDt = new DataTable();
            //here we go filling it!
            sqlDa.Fill(sqlDt);
            //check to see if any rows were returned.  If they were, it means it's 
            //a legit account
            if (sqlDt.Rows.Count > 0)
            {
                //flip our flag to true so we return a value that lets them know they're logged in
                success = true;
                Session["id"] = sqlDt.Rows[0]["id"];
                Session["user_type"] = "mentee";
            }
            //return the result!
            return success;


            // ------- 
        }

        // Log On Mentor
        [WebMethod(EnableSession = true)]
        public bool LogOnMentor(string uid, string pass)
        {
            bool success = false;

            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["myDB"].ConnectionString;
            string sqlSelect = "SELECT id FROM 2_Login_Mentor WHERE email=@userName and password=@passValue";

            MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);

            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);

            sqlCommand.Parameters.AddWithValue("@userName", HttpUtility.UrlDecode(uid));
            sqlCommand.Parameters.AddWithValue("@passValue", HttpUtility.UrlDecode(pass));



            MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);

            DataTable sqlDt = new DataTable();

            sqlDa.Fill(sqlDt);


            if (sqlDt.Rows.Count > 0)
            {
                success = true;
                Session["id"] = sqlDt.Rows[0]["id"];
                Session["user_type"] = "mentor";
            }

            return success;

        }

        // Log On Admin
        [WebMethod(EnableSession = true)]
        public bool LogOnAdmin(string uid, string pass)
        {
            bool success = false;

            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["myDB"].ConnectionString;

            string sqlSelect = "SELECT id FROM 2_Login_Admin WHERE email=@userName and password=@passValue";

            MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);

            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);

            sqlCommand.Parameters.AddWithValue("@userName", HttpUtility.UrlDecode(uid));
            sqlCommand.Parameters.AddWithValue("@passValue", HttpUtility.UrlDecode(pass));



            MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);

            DataTable sqlDt = new DataTable();

            sqlDa.Fill(sqlDt);


            if (sqlDt.Rows.Count > 0)
            {
                success = true;
                Session["id"] = sqlDt.Rows[0]["id"];
                Session["user_type"] = "admin";
            }

            return success;

        }

        [WebMethod(EnableSession = true)]
        public bool LogOff()
        {
            //if they log off, then we remove the session.  That way, if they access
            //again later they have to log back on in order for their ID to be back
            //in the session!
            Session.Abandon();
            return true;
        }

        //[WebMethod]
        //public string SearchRequest(string zip)
        //{
        //    // SQL Insert
        //    string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["myDB"].ConnectionString;

        //    string sqlSelect = "INSERT INTO searches (zip) VALUES (@zip)";

        //    MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);

        //    MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);

        //    sqlCommand.Parameters.AddWithValue("@zip", HttpUtility.UrlDecode(zip));


        //    MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);
        //    DataTable sqlDt = new DataTable();
        //    sqlDa.Fill(sqlDt);

        //    // Census Grab
        //    var client = new RestClient("https://api.census.gov/data/2018/acs/acs5/profile?get=DP05_0002PE,DP05_0003PE,DP05_0018E&for=zip%20code%20tabulation%20area:" + zip);
        //    var response = client.Execute(new RestRequest());
        //    return response.Content;
        //}

        [WebMethod(EnableSession = true)]
        public void NewMentee(string email, string password, string fname, string lname, string zip, string interest_area, string availability)
        {
            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["myDB"].ConnectionString;

            string sqlSelect = "INSERT INTO 2_Login_Mentee (email, password, fname, lname, zip, interest_area, availability) VALUES (@email, @password, @fname, @lname, @zip, @interest_area, @availability)";

            MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);

            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);

            sqlCommand.Parameters.AddWithValue("@email", HttpUtility.UrlDecode(email));
            sqlCommand.Parameters.AddWithValue("@password", HttpUtility.UrlDecode(password));
            sqlCommand.Parameters.AddWithValue("@fname", HttpUtility.UrlDecode(fname));
            sqlCommand.Parameters.AddWithValue("@lname", HttpUtility.UrlDecode(lname));
            sqlCommand.Parameters.AddWithValue("@zip", HttpUtility.UrlDecode(zip));
            sqlCommand.Parameters.AddWithValue("@interest_area", HttpUtility.UrlDecode(interest_area));
            sqlCommand.Parameters.AddWithValue("@availability", HttpUtility.UrlDecode(availability));


            MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);
            DataTable sqlDt = new DataTable();
            sqlDa.Fill(sqlDt);


        }

        [WebMethod(EnableSession = true)]
        public void NewMentor(string email, string password, string fname, string lname, string zip, string interest_area, string availability, string mentee_count)
        {
            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["myDB"].ConnectionString;

            string sqlSelect = "INSERT INTO 2_Login_Mentor (email, password, fname, lname, zip, interest_area, availability, mentee_count) VALUES (@email, @password, @fname, @lname, @zip, @interest_area, @availability, @mentee_count)";

            MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);

            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);

            sqlCommand.Parameters.AddWithValue("@email", HttpUtility.UrlDecode(email));
            sqlCommand.Parameters.AddWithValue("@password", HttpUtility.UrlDecode(password));
            sqlCommand.Parameters.AddWithValue("@fname", HttpUtility.UrlDecode(fname));
            sqlCommand.Parameters.AddWithValue("@lname", HttpUtility.UrlDecode(lname));
            sqlCommand.Parameters.AddWithValue("@zip", HttpUtility.UrlDecode(zip));
            sqlCommand.Parameters.AddWithValue("@interest_area", HttpUtility.UrlDecode(interest_area));
            sqlCommand.Parameters.AddWithValue("@availability", HttpUtility.UrlDecode(availability));
            sqlCommand.Parameters.AddWithValue("@mentee_count", HttpUtility.UrlDecode(mentee_count));


            MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);
            DataTable sqlDt = new DataTable();
            sqlDa.Fill(sqlDt);


        }

        [WebMethod(EnableSession = true)]
        public Mentor[] GetMentors()
        {
            //check out the return type.  It's an array of Mentor objects.  You can look at our custom Mentor class in this solution to see that it's 
            //just a container for public class-level variables.  It's a simple container that asp.net will have no trouble converting into json.  When we return
            //sets of information, it's a good idea to create a custom container class to represent instances (or rows) of that information, and then return an array of those objects.  
            //Keeps everything simple.

            //LOGIC: get all the active accounts and return them!

            DataTable sqlDt = new DataTable("mentors");

            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["myDB"].ConnectionString;
            string sqlSelect = "SELECT * FROM 2_Login_Mentor WHERE id NOT IN (SELECT DISTINCT mentor_id FROM 2_Mentor_Mentee WHERE mentee_id = " + Session["id"] + "); ";

            MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);

            //gonna use this to fill a data table
            MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);
            //filling the data table
            sqlDa.Fill(sqlDt);

            //loop through each row in the dataset, creating instances
            //of our container class Account.  Fill each mentor with
            //data from the rows, then dump them in a list.
            List<Mentor> mentors = new List<Mentor>();
            for (int i = 0; i < sqlDt.Rows.Count; i++)
            {
                mentors.Add(new Mentor
                {
                    id             = Convert.ToInt32(sqlDt.Rows[i]["id"]),
                    email          = sqlDt.Rows[i]["email"].ToString(),
                    fname          = sqlDt.Rows[i]["fname"].ToString(),
                    lname          = sqlDt.Rows[i]["lname"].ToString(),
                    zip            = sqlDt.Rows[i]["zip"].ToString(),
                    interest_area  = sqlDt.Rows[i]["interest_area"].ToString(),
                    availability   = sqlDt.Rows[i]["availability"].ToString()
                });
            }
            //convert the list of accounts to an array and return!
            return mentors.ToArray();
        }

        [WebMethod(EnableSession = true)]
        public Mentor[] GetMyMentors()
        {
            //check out the return type.  It's an array of Mentor objects.  You can look at our custom Mentor class in this solution to see that it's 
            //just a container for public class-level variables.  It's a simple container that asp.net will have no trouble converting into json.  When we return
            //sets of information, it's a good idea to create a custom container class to represent instances (or rows) of that information, and then return an array of those objects.  
            //Keeps everything simple.

            //LOGIC: get all the active accounts and return them!

            DataTable sqlDt = new DataTable("mentors");

            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["myDB"].ConnectionString;
            string sqlSelect = "SELECT * FROM 2_Login_Mentor WHERE id IN (SELECT DISTINCT mentor_id FROM 2_Mentor_Mentee WHERE mentee_id = " + Session["id"] + "); ";

            MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);

            //gonna use this to fill a data table
            MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);
            //filling the data table
            sqlDa.Fill(sqlDt);

            //loop through each row in the dataset, creating instances
            //of our container class Account.  Fill each mentor with
            //data from the rows, then dump them in a list.
            List<Mentor> mentors = new List<Mentor>();
            for (int i = 0; i < sqlDt.Rows.Count; i++)
            {
                mentors.Add(new Mentor
                {
                    id = Convert.ToInt32(sqlDt.Rows[i]["id"]),
                    email = sqlDt.Rows[i]["email"].ToString(),
                    fname = sqlDt.Rows[i]["fname"].ToString(),
                    lname = sqlDt.Rows[i]["lname"].ToString(),
                    zip = sqlDt.Rows[i]["zip"].ToString(),
                    interest_area = sqlDt.Rows[i]["interest_area"].ToString(),
                    availability = sqlDt.Rows[i]["availability"].ToString()
                });
            }
            //convert the list of accounts to an array and return!
            return mentors.ToArray();
        }


        [WebMethod(EnableSession = true)]
        public Mentee[] GetMyMentees()
        {
            //check out the return type.  It's an array of Mentor objects.  You can look at our custom Mentor class in this solution to see that it's 
            //just a container for public class-level variables.  It's a simple container that asp.net will have no trouble converting into json.  When we return
            //sets of information, it's a good idea to create a custom container class to represent instances (or rows) of that information, and then return an array of those objects.  
            //Keeps everything simple.

            //LOGIC: get all the active accounts and return them!

            DataTable sqlDt = new DataTable("mentees");

            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["myDB"].ConnectionString;
            string sqlSelect = "SELECT * FROM 2_Login_Mentee WHERE id IN (SELECT DISTINCT mentee_id FROM 2_Mentor_Mentee WHERE mentor_id = " + Session["id"] + "); ";

            MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);

            //gonna use this to fill a data table
            MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);
            //filling the data table
            sqlDa.Fill(sqlDt);

            //loop through each row in the dataset, creating instances
            //of our container class Account.  Fill each mentor with
            //data from the rows, then dump them in a list.
            List<Mentee> mentees = new List<Mentee>();
            for (int i = 0; i < sqlDt.Rows.Count; i++)
            {
                mentees.Add(new Mentee
                {
                    id = Convert.ToInt32(sqlDt.Rows[i]["id"]),
                    email = sqlDt.Rows[i]["email"].ToString(),
                    fname = sqlDt.Rows[i]["fname"].ToString(),
                    lname = sqlDt.Rows[i]["lname"].ToString(),
                    zip = sqlDt.Rows[i]["zip"].ToString(),
                    interest_area = sqlDt.Rows[i]["interest_area"].ToString(),
                    availability = sqlDt.Rows[i]["availability"].ToString()
                });
            }
            //convert the list of accounts to an array and return!
            return mentees.ToArray();
        }



        [WebMethod(EnableSession = true)]
        public void AddPair(string mentor_id)
        {
            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["myDB"].ConnectionString;

            string sqlSelect = "INSERT INTO 2_Mentor_Mentee (mentor_id, mentee_id) VALUES (@mentor_id, " + Session["id"] + ")";

            MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);

            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);

            sqlCommand.Parameters.AddWithValue("@mentor_id", HttpUtility.UrlDecode(mentor_id));


            MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);
            DataTable sqlDt = new DataTable();
            sqlDa.Fill(sqlDt);


        }




    }
}

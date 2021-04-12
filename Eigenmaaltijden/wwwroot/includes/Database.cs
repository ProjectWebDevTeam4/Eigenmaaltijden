using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Http;
using Eigenmaaltijden.wwwroot.classes;


namespace Eigenmaaltijden.wwwroot.includes
{
    public class Database
    {
        private UserData user;
        private static Database instance;

        public static Database get()
        {
            if (instance == null)
            {
                instance = new Database();
            }
            return instance;
        }

        /// <summary>
        /// Connects to the database using a MySQL connection string.
        /// </summary>
        /// <returns>returns the connection to the database.</returns>
        public IDbConnection Connect()
        {
            return new MySqlConnection(@"Server=localhost;Port=3306;Database=eigenmaaltijden;Uid=root;Pwd=NEWPASSWORD;");
        }

        /// <summary>
        /// Checks if the user is logged in.
        /// </summary>
        /// <param name="SessionID">Current SessionID</param>
        /// <param name="UserID">The users ID</param>
        /// <returns>Returns true if user is logged in, false if otherwise</returns>
        public bool loginCheck(string SessionID, string UserID)
        {   
            if (SessionID != null)
            {
                using var connection = Connect();
                user = connection.QuerySingle<UserData>("SELECT tb2.UserID, tb2.`Name`, tb1.`Email`, tb3.`Street`, tb3.`Number`, tb3.`Addon`, tb3.`City`, tb3.`Country`, tb3.`PostCode` FROM `verkoper` AS `tb1` INNER JOIN `verkoper_profiel` AS `tb2` ON tb1.`UserID` = tb2.`UserID` INNER JOIN `verkoper_adres` AS tb3 ON tb1.`UserID` = tb3.`UserID` WHERE tb1.`SessionID`=@SessionID AND tb1.`UserID`=@UserID LIMIT 1", new { SessionID, UserID });
                if (user != null)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Function for accessing the Dats of the User that is currently logged in.
        /// </summary>
        /// <returns>The user data </returns>
        public UserData GetLoggedInUser()
        {
            return user;
        }

        /// <summary>
        /// clear user from variable;
        /// </summary>
        public void LogoutUser()
        {
            user = null;
        }
    }
}

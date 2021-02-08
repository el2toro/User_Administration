using PropertyChanged;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;


namespace User_Administration.Model 
{
    [AddINotifyPropertyChangedInterface]
    public class User : Repository, INotifyDataErrorInfo
    {
        private int id;
        private string userName;
        private string password;
        private bool isAdmin;
        private Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public int Id
        {
            get { return id; }
            set
            {
                if (id == value)
                {
                    return;
                }
                id = value;
            }
        }

        public string UserName
        {
            get { return userName; }
            set
            {
                if (userName == value)
                {
                    return;
                }
                userName = value;

                List<string> errors = new List<string>();
                bool valid = true;

                if (value == null || value == "")
                {
                    errors.Add("User name can't be empty.");
                    SetErrors("UserName", errors);
                    valid = false;
                }


                if (!Regex.Match(value, @"^[a-zA-Z]+$").Success)
                {
                    errors.Add("User Name can only contain letters.");
                    SetErrors("UserName", errors);
                    valid = false;
                }

                if (valid)
                {
                    ClearErrors("UserName");
                }
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                if (password == value)
                {
                    return;
                }
                password = value;

                List<string> errors = new List<string>();
                bool valid = true;

                if (value == null || value == "")
                {
                    errors.Add("Password can't be empty.");
                    SetErrors("Password", errors);
                    valid = false;
                }

                if (valid)
                {
                    ClearErrors("Password");
                }
            }
        }

      

        public bool IsAdmin
        {
            get { return isAdmin; }
            set
            {
                if (isAdmin == value)
                {
                    return;
                }
                isAdmin = value;
            }
        }

        public bool HasErrors
        {
            get
            {
                return errors.Count > 0;
            }
        }

        public User(int id, string userName, string password, bool isAdmin)
        {
            Id = id;
            UserName = userName;
            Password = password;
            IsAdmin = isAdmin;
        }

        public User()
        {

        }

        public static List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            User user = null;

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ToString();
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT ID, UsernAME, uSERpASS, IsAdmin FROM Users", connection);

                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        user = User.GetUserFromResultSet(dataReader);

                         users.Add(user);
                    }
                }
            }
            return users;
        }

        public static User GetUserFromResultSet(SqlDataReader reader)
        {
           
            User user = new User((int)reader["ID"], (string)reader["UsernAME"],
                                        (string)reader["uSERpASS"], (bool)reader["IsAdmin"]);

            return user;
        }

        public  void UpdateUser()
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ToString();
                connection.Open();

                SqlCommand updateCommand = new SqlCommand("UPDATE Users SET  UsernAME=@UserName, uSERpASS=@Password, IsAdmin=@IsAdmin WHERE ID=@Id", connection);

                SqlParameter idParam = new SqlParameter("@Id", SqlDbType.Int);
                idParam.Value = this.Id;

                SqlParameter userNameParam = new SqlParameter("@UserName", SqlDbType.NVarChar);
                userNameParam.Value = this.UserName;

                SqlParameter passwordParam = new SqlParameter("@Password", SqlDbType.NVarChar);
                passwordParam.Value = this.Password;

                SqlParameter isAdmin = new SqlParameter("@IsAdmin", SqlDbType.Bit);
                isAdmin.Value = this.IsAdmin;

                updateCommand.Parameters.Add(idParam);
                updateCommand.Parameters.Add(userNameParam);
                updateCommand.Parameters.Add(passwordParam);
                updateCommand.Parameters.Add(isAdmin);

                int rows = updateCommand.ExecuteNonQuery();
            }
        }

        public void InsertUser()
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ToString();
                connection.Open();

                SqlCommand insertCommand = new SqlCommand("INSERT INTO Users(UsernAME, uSERpASS, IsAdmin) " +
                                                          "VALUES(@UserName, @Password, @IsAdmin);  SELECT IDENT_CURRENT('Users');", connection);

               
                SqlParameter userNameParam = new SqlParameter("@UserName", SqlDbType.NVarChar);
                userNameParam.Value = this.UserName;

                SqlParameter passwordParam = new SqlParameter("@Password", SqlDbType.NVarChar);
                passwordParam.Value = this.Password;

                SqlParameter isAdmin = new SqlParameter("@IsAdmin", SqlDbType.Bit);
                isAdmin.Value = this.IsAdmin;

                insertCommand.Parameters.Add(userNameParam);
                insertCommand.Parameters.Add(passwordParam);
                insertCommand.Parameters.Add(isAdmin);

                var id = insertCommand.ExecuteScalar();

                if (id != null)
                {

                    this.Id = Convert.ToInt32(id);
                }
            }
        }


        public void Save()
        {
            if (Id == 0)
            {
                InsertUser();
            }
            else
            {
                UpdateUser();
            }
        }

        private void SetErrors(string propertyName, List<string> propertyErrors)
        {
            // Clear any errors that already exist for this property.
            errors.Remove(propertyName);
            // Add the list collection for the specified property.
            errors.Add(propertyName, propertyErrors);
            // Raise the error-notification event.
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }
        private void ClearErrors(string propertyName)
        {
            // Remove the error list for this property.
            errors.Remove(propertyName);
            // Raise the error-notification event.
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                // Provide all the error collections.
                return (errors.Values);
            }
            else
            {
                // Provice the error collection for the requested property
                // (if it has errors).
                if (errors.ContainsKey(propertyName))
                {
                    return (errors[propertyName]);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}

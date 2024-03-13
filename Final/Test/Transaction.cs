using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_Airline
{
    public class Transaction : ICrudOperation
    {

        //why it there ??

        //Properties
        public int? TransactionID { get; set; }
        public string? Description { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? Date { get; set; }
        public int? AirlineID { get; set; }
        public bool? IsDeleted { get; set; }

        public Transaction()
        {

        }
        public Transaction(int? id, decimal amount, string desc)
        {
            string query = $"select AirlineID from AirCraft where AircraftID={id}";
            using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        id = (int?)reader["AirlineID"];
                    }
                }
            }
            query = $"INSERT INTO Transactions (Description,Amount,Date,AirlineID,IsDeleted) VALUES ('{desc}', {amount},GETDATE() ,{id}, 1)";
            using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
            {
                command.ExecuteNonQuery();
            }
        }
        //Constructor





        //Main Functions : 
        //Function To create a new Row IN Transaction class
        public bool Create()
        {
            try
            {

                while (true)
                {
                    Console.Write("Enter Description: ");
                    Description = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(Description))
                        break;

                    canNotBeEmpty("Description");
                }


                //Validation for Ensure That the Distance Is Int
                Console.Write("Enter Amount: ");

                int _amount;
                while (!int.TryParse(Console.ReadLine(), out _amount) || _amount <= 0)
                {
                    NotValidInt();
                }


                //Validation for Ensure That the Date Is In Correct Format 
                Console.Write("Enter Date (yyyy-MM-dd): ");

                DateTime _date;
                while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _date))
                {
                    NotValidDate();
                }


                //Validation for Ensure That the _AirlineID Is Int
                Console.Write("Enter AirlineID: ");

                int _AirlineID;
                while (true)
                {
                    if (!int.TryParse(Console.ReadLine(), out _AirlineID) || _AirlineID <= 0)
                        NotValidIDPositive();
                    else if (!IsIDExist(_AirlineID))
                        NotValidIDExistance();
                    else
                        break;

                }
                IsDeleted = true;
                string query = "INSERT INTO Transactions (Description,Amount,Date,AirlineID,IsDeleted) VALUES (@Description, @Amount,@Date ,@AirlineID, @IsDeleted)";

                using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
                {
                    command.Parameters.AddWithValue("@Description", Description);
                    command.Parameters.AddWithValue("@Amount", _amount);
                    command.Parameters.AddWithValue("@Date", _date);
                    command.Parameters.AddWithValue("@AirlineID", _AirlineID);
                    command.Parameters.AddWithValue("@IsDeleted", IsDeleted);

                    command.ExecuteNonQuery();
                }


                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The Row of Transactions is Created Successfully.");
                Console.ForegroundColor = ConsoleColor.White;





            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating Transaction: {ex.Message}");
                return false;
            }
            return true;
        }


        //To Edit Specific Row With It's ID
        public bool Edit()
        {

            try

            {
                //To Ensure That ID is Int
                Console.Write("Enter ID of Row You Want To Edit: ");

                int InputID;
                while (true)
                {
                    if (!int.TryParse(Console.ReadLine(), out InputID) || InputID <= 0)
                        NotValidIDPositive();
                    else if (!IsIDTransExist(InputID))
                        NotValidIDTransExistance();
                    else
                        break;

                }

                while (true)
                {
                    Console.Write("Enter Description: ");
                    Description = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(Description))
                        break;

                    canNotBeEmpty("Description");
                }


                //Validation for Ensure That the Distance Is Int
                Console.Write("Enter Amount: ");

                int _amount;
                while (!int.TryParse(Console.ReadLine(), out _amount) || _amount <= 0)
                {
                    NotValidInt();
                }


                //Validation for Ensure That the Date Is In Correct Format 
                Console.Write("Enter Date (yyyy-MM-dd): ");

                DateTime _date;
                while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _date))
                {
                    NotValidDate();
                }


                //Validation for Ensure That the _AirlineID Is Int
                Console.Write("Enter AirlineID: ");

                int _AirlineID;
                while (true)
                {
                    if (!int.TryParse(Console.ReadLine(), out _AirlineID) || _AirlineID <= 0)
                        NotValidIDPositive();
                    else if (!IsIDExist(_AirlineID))
                        NotValidIDExistance();
                    else
                        break;

                }

                IsDeleted = true;

                string query = $"UPDATE Transactions SET Description = @Description, Amount = @Amount,Date = @Date, AirlineID = @AirlineID, IsDeleted = @IsDeleted WHERE TransactionID=@InputID";
                SqlCommand command = new SqlCommand(query, SystemRepostory.connection);
                command.Parameters.AddWithValue("@InputID", InputID);

                command.Parameters.AddWithValue("@Description", Description);
                command.Parameters.AddWithValue("@Amount", _amount);
                command.Parameters.AddWithValue("@Date", _date);
                command.Parameters.AddWithValue("@AirlineID", _AirlineID);
                command.Parameters.AddWithValue("@IsDeleted", IsDeleted);

                command.ExecuteNonQuery();


                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The Row of Transactions Is Updated Successfully.");
                Console.ForegroundColor = ConsoleColor.White;



            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating Transaction: {ex.Message}");
                return false;
            }
            return true;


        }


        //To Delete Specific Row With It's ID
        public bool Delete()
        {

            try
            {
                //To Ensure That ID is Int
                Console.Write("Enter Transaction ID of Row You Want To Delete: ");


                int InputID;
                while (true)
                {
                    if (!int.TryParse(Console.ReadLine(), out InputID) || InputID <= 0)
                        NotValidIDPositive();
                    else if (!IsIDTransExist(InputID))
                        NotValidIDTransExistance();
                    else
                        break;

                }
                string query = "DELETE FROM Transactions WHERE TransactionID = @InputID";
                using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
                {
                    command.Parameters.AddWithValue("@InputID", InputID);

                    command.ExecuteNonQuery();
                }



                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The Row of Transactions Deleted Successfully.");
                Console.ForegroundColor = ConsoleColor.White;


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting Transaction: {ex.Message}");
                return false;
            }
            return true;

        }


        //To Show all Rows 
        public void GetAll()
        {
            List<Transaction> transaction = new List<Transaction>();

            try
            {


                string query = "SELECT * FROM Transactions";
                using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            transaction.Add(new Transaction()
                            {
                                TransactionID = reader.IsDBNull(reader.GetOrdinal("TransactionID")) ? null : (int)reader["TransactionID"],
                                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : (string)reader["Description"],
                                Amount = reader.IsDBNull(reader.GetOrdinal("Amount")) ? null : (int)reader["Amount"],
                                Date = reader.IsDBNull(reader.GetOrdinal("Date")) ? null : (DateTime)reader["Date"],
                                AirlineID = reader.IsDBNull(reader.GetOrdinal("AirlineID")) ? null : (int)reader["AirlineID"],
                                IsDeleted = reader.IsDBNull(reader.GetOrdinal("IsDeleted")) ? false : (bool)reader["IsDeleted"]
                            });
                        }
                    }
                }
                Console.WriteLine("All Transaction:");
                TableMaker.PrintLine();
                TableMaker.PrintRow("TransactionID", "Description", "Amount", "Date", "AirlineID", "IsDeleted");
                TableMaker.PrintLine();

                foreach (var a in transaction)
                {

                    TableMaker.PrintRow(a.TransactionID.ToString(), a.Description, a.Amount.ToString(), a.Date.ToString(), a.AirlineID.ToString(), a.IsDeleted.ToString());
                    TableMaker.PrintLine();
                }

                //Successfully Statement

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The Rows of Transaction are Getted Successfully.");
                Console.ForegroundColor = ConsoleColor.White;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Transaction: {ex.Message}");
            }

        }

        //To Get Specific Row With It's ID
        public void GetByID()
        {

            try
            {

                Console.Write("Enter ID of Row You Want To GET: ");

                //To Ensure That ID is Int
                int InputID;
                while (true)
                {
                    if (!int.TryParse(Console.ReadLine(), out InputID) || InputID <= 0)
                        NotValidIDPositive();
                    else if (!IsIDTransExist(InputID))
                        NotValidIDTransExistance();
                    else
                        break;

                }
                string query = "SELECT * FROM Transactions WHERE TransactionID = @InputID";

                using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
                {
                    command.Parameters.AddWithValue("@InputID", InputID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            TransactionID = reader.IsDBNull(reader.GetOrdinal("TransactionID")) ? null : (int)reader["TransactionID"];
                            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : (string)reader["Description"];
                            Amount = reader.IsDBNull(reader.GetOrdinal("Amount")) ? null : (int)reader["Amount"];
                            Date = reader.IsDBNull(reader.GetOrdinal("Date")) ? null : (DateTime)reader["Date"];
                            AirlineID = reader.IsDBNull(reader.GetOrdinal("AirlineID")) ? null : (int)reader["AirlineID"];
                            IsDeleted = reader.IsDBNull(reader.GetOrdinal("IsDeleted")) ? false : (bool)reader["IsDeleted"];


                        }
                    }
                }
                TableMaker.PrintLine();
                TableMaker.PrintRow("TransactionID", "Description", "Amount", "Date", "AirlineID", "IsDeleted");
                TableMaker.PrintLine();
                TableMaker.PrintRow(TransactionID.ToString(), Description, Amount.ToString(), Date.ToString(), AirlineID.ToString(), IsDeleted.ToString());
                TableMaker.PrintLine();


                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The Row of Transaction Is Getted Successfully.");
                Console.ForegroundColor = ConsoleColor.White;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Transaction: {ex.Message}");
            }

        }



        //
        //


        //Helper Functions :
        //The NotValidInt Message
        private void NotValidInt()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Invalid input for Amount. Please enter a positive integer: ");
            Console.ForegroundColor = ConsoleColor.White;
        }


        //The NotValidDate Message
        private void NotValidDate()
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine("Invalid date format. Please enter the date in the format yyyy-MM-dd.");
            Console.Write("Enter Date again: ");

            Console.ForegroundColor = ConsoleColor.White;
        }


        //The NotValid of AirlineID Existance Message
        private void NotValidIDExistance()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid AirlineID. The specified AirlineID does not Exist.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Please enter a Exist ID:");

        }


        //The NotValid of TransactionID Existance Message
        private void NotValidIDTransExistance()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid TransactionID. The specified TransactionID does not Exist.");
            Console.ForegroundColor = ConsoleColor.White;

        }

        //The NotValidIDPositive Message
        private void NotValidIDPositive()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Invalid input for TransactionID. Please enter a positive integer: ");
            Console.ForegroundColor = ConsoleColor.White;
        }


        //Check if user enter empty
        private void canNotBeEmpty(String str)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{str} cannot be empty. Please enter a valid {str}.");
            Console.ForegroundColor = ConsoleColor.White;

        }
        //
        //


        //Function that check if AirlineID is on DB
        private bool IsIDExist(int AirlineID)
        {
            bool exists = false;

            try
            {
                string query = "IF EXISTS (SELECT 1 FROM Airline WHERE AirlineID = @AirlineID)SELECT 1 ELSE SELECT 0";

                using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
                {
                    command.Parameters.AddWithValue("@AirlineID", AirlineID);
                    exists = ((int)command.ExecuteScalar() == 1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking AirlineID existence: {ex.Message}");
            }

            return exists;
        }


        //Function that check if TransactionID is on DB
        private bool IsIDTransExist(int TransactionID)
        {
            bool exists = false;

            try
            {
                string query = "IF EXISTS (SELECT 1 FROM Transactions WHERE TransactionID = @TransactionID)SELECT 1 ELSE SELECT 0";

                using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
                {
                    command.Parameters.AddWithValue("@TransactionID", TransactionID);
                    exists = ((int)command.ExecuteScalar() == 1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking TransactionID existence: {ex.Message}");
            }

            return exists;
        }






    }
}


using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Reflection;

namespace c_Airline
{
    public class Airline : ICrudOperation
    {


        //Properties
        public int AirlineID { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Contact_person { get; set; }
        public bool? IsDeleted { get; set; }





        //Main Functions : 
        //Function To create a new Row IN Airline class
        public bool Create()
        {
            try
            {
                while (true)
                {
                    Console.Write("Enter Name: ");
                    Name = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(Name))
                        break;

                    canNotBeEmpty("Name");
                }


                while (true)
                {
                    Console.Write("Enter Address: ");
                    Address = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(Address))
                        break;
                    canNotBeEmpty("Address");
                }

                while (true)
                {
                    Console.Write("Enter Contact_person: ");
                    Contact_person = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(Contact_person))
                        break;
                    canNotBeEmpty("Contact_person");
                }
                IsDeleted = true;

                string query = "INSERT INTO Airline (Name, Address, Contact_person, IsDeleted) VALUES (@Name, @Address, @Contact_person, @IsDeleted)";

                using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
                {
                    command.Parameters.AddWithValue("@Name", Name);
                    command.Parameters.AddWithValue("@Address", Address);
                    command.Parameters.AddWithValue("@Contact_person", Contact_person);
                    command.Parameters.AddWithValue("@IsDeleted", IsDeleted);

                    command.ExecuteNonQuery();
                }

                //Successfully 

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The Row of Airline is Created Successfully.");
                Console.ForegroundColor = ConsoleColor.White;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating Airline: {ex.Message}");
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

                Console.Write("Enter AirlineID of Row You Want to Edit: ");



                int inputID;
                while (true)
                {
                    if (!int.TryParse(Console.ReadLine(), out inputID) || inputID <= 0)
                        NotValidIDPositive();
                    else if (!IsIDExist(inputID))
                        NotValidIDExistance();
                    else
                        break;

                }

                while (true)
                {
                    Console.Write("Enter Name: ");
                    Name = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(Name))
                        break;

                    canNotBeEmpty("Name");
                }


                while (true)
                {
                    Console.Write("Enter Address: ");
                    Address = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(Address))
                        break;
                    canNotBeEmpty("Address");
                }

                while (true)
                {
                    Console.Write("Enter Contact_person: ");
                    Contact_person = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(Contact_person))
                        break;
                    canNotBeEmpty("Contact_person");
                }

                IsDeleted = true;

                string query = $"UPDATE Airline SET Name = @Name, Address = @Address, Contact_person = @Contact_person, IsDeleted = @IsDeleted WHERE AirlineID = @InputID";
                SqlCommand command = new SqlCommand(query, SystemRepostory.connection);
                command.Parameters.AddWithValue("@InputID", inputID);

                command.Parameters.AddWithValue("@Name", Name);
                command.Parameters.AddWithValue("@Address", Address);
                command.Parameters.AddWithValue("@Contact_person", Contact_person);
                command.Parameters.AddWithValue("@IsDeleted", IsDeleted);

                command.ExecuteNonQuery();



                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The Row of Airline Is Updated Successfully.");
                Console.ForegroundColor = ConsoleColor.White;


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating Airline: {ex.Message}");
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
                Console.Write("Enter Airline ID of Row You Want To Delete: ");

                int inputID;
                while (true)
                {
                    if (!int.TryParse(Console.ReadLine(), out inputID) || inputID <= 0)
                        NotValidIDPositive();
                    else if (!IsIDExist(inputID))
                        NotValidIDExistance();
                    else
                        break;

                }

                string query = "DELETE FROM Airline WHERE AirlineID = @InputID";
                using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
                {
                    command.Parameters.AddWithValue("@InputID", inputID);

                    command.ExecuteNonQuery();
                }


                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The Row of Airline Deleted Successfully.");
                Console.ForegroundColor = ConsoleColor.White;
                //To Ensure That ID is in our DB


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting Airline: {ex.Message}");
                return false;
            }
            return true;
        }


        //To Show  all Rows 
        public void GetAll()
        {
            List<Airline> airlines = new List<Airline>();
            try
            {
                string query = "SELECT * FROM Airline";
                using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            airlines.Add(new Airline()
                            {
                                AirlineID = (int)reader["AirlineID"],
                                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? "" : (string)reader["Name"],
                                Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? "" : (string)reader["Address"],
                                Contact_person = reader.IsDBNull(reader.GetOrdinal("Contact_person")) ? "" : (string)reader["Contact_person"],
                                IsDeleted = reader.IsDBNull(reader.GetOrdinal("IsDeleted")) ? false : (bool)reader["IsDeleted"]
                            });
                        }
                    }
                }
                Console.WriteLine("All Airlines:");
                TableMaker.PrintLine();
                TableMaker.PrintRow("AirlineID", "Name", "Address", "Contact_person", "IsDeleted");
                TableMaker.PrintLine();
                foreach (var a in airlines)
                {
                    TableMaker.PrintRow(a.AirlineID.ToString(), a.Name, a.Address, a.Contact_person, a.IsDeleted?.ToString());
                    TableMaker.PrintLine();
                    //Console.WriteLine($"AirlineID: {a.AirlineID}, Name: {a.Name}," +
                    //    $" Address: {a.Address}, Contact_person: {a.Contact_person}, IsDeleted: {a.IsDeleted}");
                }


                //Successfully Statement

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The Rows of AirLine are Getted Successfully.");
                Console.ForegroundColor = ConsoleColor.White;


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Airlines: {ex.Message}");
            }
        }


        //To Get Specific Row With It's ID
        public void GetByID()
        {
            try
            {


                Console.Write("Enter Airline ID of Row You Want To GET: ");
                //To Ensure That ID is Int
                int inputID;
                while (true)
                {
                    if (!int.TryParse(Console.ReadLine(), out inputID) || inputID <= 0)
                        NotValidIDPositive();
                    else if (!IsIDExist(inputID))
                        NotValidIDExistance();
                    else
                        break;

                }

                string query = "SELECT * FROM Airline WHERE AirlineID = @InputID";
                using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
                {
                    command.Parameters.AddWithValue("@InputID", inputID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        TableMaker.PrintLine();
                        TableMaker.PrintRow("AirlineID", "Name", "Address", "Contact_person", "IsDeleted");
                        TableMaker.PrintLine();
                        while (reader.Read())
                        {
                            //Console.WriteLine($"AirlineID: {reader["AirlineID"]}, Name: {reader["Name"]}," +
                            //    $" Address: {reader["Address"]}, Contact_person: {reader["Contact_person"]}, IsDeleted: {reader["IsDeleted"]}");
                            TableMaker.PrintRow(reader["AirlineID"].ToString(), reader["Name"].ToString(), reader["Address"].ToString(), reader["Contact_person"].ToString(), reader["IsDeleted"].ToString());
                            TableMaker.PrintLine();
                        }
                    }
                }

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The Row of Airline Is Getted Successfully.");
                Console.ForegroundColor = ConsoleColor.White;




            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Airline: {ex.Message}");
            }
        }




        //Helper Functions :

        //The NotValidIDPositive Message
        private void NotValidIDPositive()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Invalid input . Please enter a positive integer: ");
            Console.ForegroundColor = ConsoleColor.White;
        }

        //The NotValid of AirlineID Existance Message
        private void NotValidIDExistance()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid AirlineID. The specified AirlineID does not Exist.");
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






    }
}

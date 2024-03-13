
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Xml.Linq;

namespace c_Airline
{
    public class Employee : ICrudOperation
    {

        //Properties
        public int Employee_ID { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public string? Position { get; set; }
        public DateTime? Birthday { get; set; }
        public int? AirlineID { get; set; }
        public bool IsDeleted { get; set; }



        //Main Functions : 
        //Function To create a new Row IN Employee class
        public bool Create()
        {
            try
            {

                //To Ensure the user insert Not Empty
                while (true)
                {
                    Console.Write("Enter Employee Name: ");
                    Name = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(Name))
                        break;

                    canNotBeEmpty("Employee Name");
                }

                //To Ensure the user insert Not Empty
                while (true)
                {
                    Console.Write("Enter Employee Address: ");
                    Address = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(Address))
                        break;

                    canNotBeEmpty("Employee Address");
                }


                //To Ensure the user insert correct Gender
                Console.Write("Enter Employee Gender(M/F): ");

                char _Gender;
                while (!char.TryParse(Console.ReadLine(), out _Gender) || char.ToUpper(_Gender) != 'M' && char.ToUpper(_Gender) != 'F')
                {
                    NotValidGender();
                }

                //To Ensure the user insert Not Empty
                while (true)
                {
                    Console.Write("Enter Employee Position: ");
                    Position = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(Position))
                        break;

                    canNotBeEmpty("Employee Position");
                }

                //To Ensure the user insert correct Birthday
                Console.Write("Enter Employee Birthday (MM/DD/YYYY): ");

                DateTime _date;
                while (!DateTime.TryParseExact(Console.ReadLine(), "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _date))
                {
                    NotValidDate();
                }

                //To Ensure That ID _AirlineID is int 
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

                string query = "INSERT INTO Employees (Name, Address, Gender, Position, Birthday, AirlineID, IsDeleted) " +
                               "VALUES (@Name, @Address, @Gender, @Position, @Birthday, @AirlineID, @IsDeleted)";

                using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
                {
                    command.Parameters.AddWithValue("@Name", Name);
                    command.Parameters.AddWithValue("@Address", Address);
                    command.Parameters.AddWithValue("@Gender", char.ToUpper(_Gender));
                    command.Parameters.AddWithValue("@Position", Position);
                    command.Parameters.AddWithValue("@Birthday", _date);
                    command.Parameters.AddWithValue("@AirlineID", _AirlineID);
                    command.Parameters.AddWithValue("@IsDeleted", IsDeleted);

                    command.ExecuteNonQuery();

                }



                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The Row of Employees is Created Successfully.");
                Console.ForegroundColor = ConsoleColor.White;




            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating Employees: {ex.Message}");
                return false;
            }
            return true;
        }


        //To Edit Specific Row With It's ID
        public bool Edit()
        {
            try
            {

                //To Ensure That ID is Int and ON DB
                Console.Write("Enter Employee ID of Row You Want to Edit: ");

                int InputID;

                while (true)
                {
                    if (!int.TryParse(Console.ReadLine(), out InputID) || InputID <= 0)
                        NotValidIDPositive();
                    else if (!IsIDEmpsExist(InputID))
                        NotValidIDEmpExistance();
                    else
                        break;

                }


                //To Ensure the user insert Not Empty
                while (true)
                {
                    Console.Write("Enter Employee Name: ");
                    Name = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(Name))
                        break;

                    canNotBeEmpty("Employee Name");
                }


                //To Ensure the user insert Not Empty
                while (true)
                {
                    Console.Write("Enter Employee Address: ");
                    Address = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(Address))
                        break;

                    canNotBeEmpty("Employee Address");
                }


                //To Ensure Gender is Correct
                Console.Write("Enter Employee Gender(M/F): ");
                char _Gender;
                while (!char.TryParse(Console.ReadLine(), out _Gender) || char.ToUpper(_Gender) != 'M' && char.ToUpper(_Gender) != 'F')
                {
                    NotValidGender();
                }


                //To Ensure the user insert Not Empty
                while (true)
                {
                    Console.Write("Enter Employee Position: ");
                    Position = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(Position))
                        break;

                    canNotBeEmpty("Employee Position");
                }

                //To Ensure The Date is Correct
                Console.Write("Enter Employee Birthday (MM/DD/YYYY): ");

                DateTime _date;
                while (!DateTime.TryParseExact(Console.ReadLine(), "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _date))
                {
                    NotValidDate();
                }

                //To Ensure That ID is Int and ON DB
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

                string query = $"UPDATE Employees SET Name = @Name, Address = @Address, Gender = @Gender, " +
                               "Position = @Position, Birthday = @Birthday, AirlineID = @AirlineID, IsDeleted = @IsDeleted " +
                               $"WHERE Employee_ID = @InputID";

                using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
                {
                    command.Parameters.AddWithValue("@InputID", InputID);
                    command.Parameters.AddWithValue("@Name", Name);
                    command.Parameters.AddWithValue("@Address", Address);
                    command.Parameters.AddWithValue("@Gender", char.ToUpper(_Gender));
                    command.Parameters.AddWithValue("@Position", Position);
                    command.Parameters.AddWithValue("@Birthday", _date);
                    command.Parameters.AddWithValue("@AirlineID", _AirlineID);
                    command.Parameters.AddWithValue("@IsDeleted", IsDeleted);

                    command.ExecuteNonQuery();

                }



                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The Row of Employee Is Updated Successfully.");
                Console.ForegroundColor = ConsoleColor.White;


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating Employees: {ex.Message}");
                return false;
            }
            return true;
        }



        //To Delete Specific Row With It's ID
        public bool Delete()
        {
            try
            {

                //To Ensure That ID is Int and That ID is in our DB
                Console.Write("Enter Employee ID of Row You Want To Delete: ");

                int inputID;

                while (true)
                {
                    if (!int.TryParse(Console.ReadLine(), out inputID) || inputID <= 0)
                        NotValidIDPositive();
                    else if (!IsIDEmpsExist(inputID))
                        NotValidIDEmpExistance();
                    else
                        break;

                }
                string query = "DELETE FROM Employees WHERE Employee_ID = @InputID";
                using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
                {
                    command.Parameters.AddWithValue("@InputID", inputID);

                    command.ExecuteNonQuery();
                }



                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The Row of Employees Deleted Successfully.");
                Console.ForegroundColor = ConsoleColor.White;


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting Employees: {ex.Message}");
                return false;
            }
            return true;
        }



        //To Show  all Rows 
        public void GetAll()
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                string query = "SELECT * FROM Employees";
                using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employees.Add(new Employee()
                            {
                                Employee_ID = (int)reader["Employee_ID"],
                                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? "" : (string)reader["Name"],
                                Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? "" : (string)reader["Address"],
                                Gender = reader.IsDBNull(reader.GetOrdinal("Gender")) ? "" : (string)reader["Gender"],
                                Position = reader.IsDBNull(reader.GetOrdinal("Position")) ? "" : (string)reader["Position"],
                                Birthday = reader.IsDBNull(reader.GetOrdinal("Birthday")) ? null : (DateTime)reader["Birthday"],
                                AirlineID = reader.IsDBNull(reader.GetOrdinal("AirlineID")) ? null : (int)reader["AirlineID"],
                                IsDeleted = reader.IsDBNull(reader.GetOrdinal("IsDeleted")) ? false : (bool)reader["IsDeleted"]
                            });
                        }
                    }
                }
                Console.WriteLine("All Employees:");
                TableMaker.PrintLine();
                TableMaker.PrintRow("Employee_ID", "Name", "Address", "Gender", "Position", "Birthday", "AirlineID", "IsDeleted");
                TableMaker.PrintLine();
                foreach (var a in employees)
                {
                    //Console.WriteLine($"Employee_ID: {a.Employee_ID}, Name: {a.Name}," +
                    //                  $" Address: {a.Address},Gender: {a.Gender},Position: {a.Position}," +
                    //                  $"Birthday: {a.Birthday}, AirlineID: {a.AirlineID}, IsDeleted: {a.IsDeleted}");
                    TableMaker.PrintRow(a.Employee_ID.ToString(), a.Name, a.Address, a.Gender, a.Position, a.Birthday.ToString(), a.AirlineID.ToString(), a.IsDeleted.ToString());
                    TableMaker.PrintLine();
                }

                //Successfully Statement

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The Rows of Employee are Getted Successfully.");
                Console.ForegroundColor = ConsoleColor.White;


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Employees: {ex.Message}");
            }
        }



        //To Get Specific Row With It's ID
        public void GetByID()
        {
            try
            {

                Console.Write("Enter Employee ID of Row You Want To GET: ");
                //To Ensure That ID is Int
                int InputID;

                while (true)
                {
                    if (!int.TryParse(Console.ReadLine(), out InputID) || InputID <= 0)
                        NotValidIDPositive();
                    else if (!IsIDEmpsExist(InputID))
                        NotValidIDEmpExistance();
                    else
                        break;

                }

                string query = "SELECT * FROM Employees WHERE Employee_ID = @InputID";

                using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
                {
                    command.Parameters.AddWithValue("@InputID", InputID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Employee_ID = (int)reader["Employee_ID"];
                            Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? "" : (string)reader["Name"];
                            Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? "" : (string)reader["Address"];
                            Gender = reader.IsDBNull(reader.GetOrdinal("Gender")) ? "" : (string)reader["Gender"];
                            Position = reader.IsDBNull(reader.GetOrdinal("Position")) ? "" : (string)reader["Position"];
                            Birthday = reader.IsDBNull(reader.GetOrdinal("Birthday")) ? null : (DateTime)reader["Birthday"];
                            AirlineID = reader.IsDBNull(reader.GetOrdinal("AirlineID")) ? null : (int)reader["AirlineID"];
                            IsDeleted = reader.IsDBNull(reader.GetOrdinal("IsDeleted")) ? false : (bool)reader["IsDeleted"];
                        }
                    }
                }
                TableMaker.PrintLine();
                TableMaker.PrintRow("Employee_ID", "Name", "Address", "Gender", "Position", "Birthday", "AirlineID", "IsDeleted");
                TableMaker.PrintLine();
                TableMaker.PrintRow(Employee_ID.ToString(), Name, Address, Gender, Position, Birthday.ToString(), AirlineID.ToString(), IsDeleted.ToString());
                TableMaker.PrintLine();
                //  Console.WriteLine($"Employee_ID: {Employee_ID}, Name: {Name}," +
                //                    $" Address: {Address},Gender: {Gender},Position: {Position}," +
                //                    $"Birthday: {Birthday}, AirlineID: {AirlineID}, IsDeleted: {IsDeleted}");



                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The Row of Employee Is Getted Successfully.");
                Console.ForegroundColor = ConsoleColor.White;


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Employees: {ex.Message}");
            }
        }





        //Helper Functions :

        //The NotValidIDPositive Message
        private void NotValidIDPositive()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Invalid input. ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Please enter a positive integer:");
        }

        //The NotValidGender Message
        private void NotValidGender()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Invalid input . Please enter Your Gender Like (M / F):");
            Console.ForegroundColor = ConsoleColor.White;
        }


        //The NotValidDate Message
        private void NotValidDate()
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine("Invalid date format. Please enter the date in the format (MM/DD/YYYY).");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Enter Date again: ");
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
        private void NotValidIDEmpExistance()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid EmployeeID. The specified EmployeeID does not Exist.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Please enter a Exist ID:");


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
        private bool IsIDEmpsExist(int Employee_ID)
        {
            bool exists = false;

            try
            {
                string query = "IF EXISTS (SELECT 1 FROM Employees WHERE Employee_ID = @Employee_ID)SELECT 1 ELSE SELECT 0";


                using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
                {
                    command.Parameters.AddWithValue("@Employee_ID", Employee_ID);
                    exists = ((int)command.ExecuteScalar() == 1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking Employee_ID existence: {ex.Message}");
            }

            return exists;
        }


        //Check if user enter empty
        private void canNotBeEmpty(String str)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{str} cannot be empty. Please enter a valid {str}.");
            Console.ForegroundColor = ConsoleColor.White;

        }







    }






}


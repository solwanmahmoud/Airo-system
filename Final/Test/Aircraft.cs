using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;

namespace c_Airline
{
    public class Aircraft : ICrudOperation
    {
        int aircraftId;
        int capcity;
        List<Seat> seats;


        //Constructor
        public Aircraft()
        {

        }

        public Aircraft(int _aircraftId, int _capacity)
        {

            aircraftId = _aircraftId;
            capcity = _capacity;
            seats = new List<Seat>();
            string query = "SELECT * FROM Seat  where Aircraft_id=@id;";

            using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
            {
                command.Parameters.AddWithValue("@id", _aircraftId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        seats.Add(new Seat((int)reader["Seat_num"], (bool)reader["IsAvailable"]));



                    }
                }
            }
        }

        //Properties

        public int Capcity { get { return capcity; } set { capcity = value; } }
        internal List<Seat> Seats { get { return seats; } }

        public int AircraftID { get; set; }
        public string? Model { get; set; }
        public int AirlineID { get; set; }
        public string? Piolt { get; set; }
        public string? Host1 { get; set; }
        public string? Host2 { get; set; }
        public bool IsDeleted { get; set; }



        //Main Functions:
        // Function To create a new Row in Aircraft class
        public bool Create()
        {
            try
            {

                //To Ensure Capcity int :
                Console.Write("Enter Capacity: ");

                int capacity;
                while (!int.TryParse(Console.ReadLine(), out capacity) || capacity <= 0)
                {
                    NotValidCapacity();
                }

                //To Ensure the user insert Not Empty
                while (true)
                {
                    Console.Write("Enter Model: ");
                    Model = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(Model))
                        break;

                    canNotBeEmpty("Model");
                }

                //To Ensure the user insert Not Empty
                while (true)
                {
                    Console.Write("Enter Pilot Name: ");

                    Piolt = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(Piolt))
                        break;

                    canNotBeEmpty("Piolt Name");
                }
                //To Ensure the user insert Not Empty
                while (true)
                {
                    Console.Write("Enter Host1 Name: ");

                    Host1 = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(Host1))
                        break;

                    canNotBeEmpty("Host1 Name");
                }

                //To Ensure the user insert Not Empty
                while (true)
                {
                    Console.Write("Enter Host2 Name: ");

                    Host2 = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(Host2))
                        break;

                    canNotBeEmpty("Host2 Name");
                }

                //To Ensure AirlineID int and on our DB :
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

                // Assuming IsDeleted is a member variable in your Aircraft class
                IsDeleted = true;


                //Deleting Process
                string query = "INSERT INTO AirCraft (Capacity, Model, Piolt, Host1, Host2, AirlineID, IsDeleted) VALUES (@Capacity, @Model, @Piolt, @Host1, @Host2, @AirlineID, @IsDeleted)";

                using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
                {
                    command.Parameters.AddWithValue("@Capacity", capacity);
                    command.Parameters.AddWithValue("@Model", Model);
                    command.Parameters.AddWithValue("@Piolt", Piolt);
                    command.Parameters.AddWithValue("@Host1", Host1);
                    command.Parameters.AddWithValue("@Host2", Host2);
                    command.Parameters.AddWithValue("@AirlineID", _AirlineID);
                    command.Parameters.AddWithValue("@IsDeleted", IsDeleted);

                    command.ExecuteNonQuery();

                }


                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The Row of Aircrafts is Created Successfully.");
                Console.ForegroundColor = ConsoleColor.White;


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating Aircraft: {ex.Message}");
                return false;
            }
            return true;
        }



        //To Edit Specific Row With Its ID
        public bool Edit()
        {
            try
            {
                //To Ensure That ID Int and On Db
                Console.Write("Enter ID of Row You Want To Edit: ");

                int inputID;
                while (true)
                {
                    if (!int.TryParse(Console.ReadLine(), out inputID) || inputID <= 0)
                        NotValidIDPositive();
                    else if (!IsAircraftIDExist(inputID))
                        NotValidIDAirCraft();
                    else
                        break;

                }

                Console.Write("Enter Capacity: ");

                int capacity;
                while (!int.TryParse(Console.ReadLine(), out capacity) || capacity <= 0)
                {
                    NotValidCapacity();

                }


                //To Ensure That ID Int and On Db
                Console.Write("Enter AirlineID: ");
                int airlineID;
                while (true)
                {
                    if (!int.TryParse(Console.ReadLine(), out airlineID) || airlineID <= 0)
                        NotValidIDPositive();
                    else if (!IsIDExist(airlineID))
                        NotValidIDExistance();
                    else
                        break;

                }

                //To Ensure the user insert Not Empty
                while (true)
                {
                    Console.Write("Enter Model: ");
                    Model = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(Model))
                        break;

                    canNotBeEmpty("Model");
                }

                while (true)
                {
                    Console.Write("Enter Pilot Name: ");

                    Piolt = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(Piolt))
                        break;

                    canNotBeEmpty("Piolt Name");
                }

                while (true)
                {
                    Console.Write("Enter Host1 Name: ");

                    Host1 = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(Host1))
                        break;

                    canNotBeEmpty("Host1 Name");
                }

                while (true)
                {
                    Console.Write("Enter Host2 Name: ");

                    Host2 = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(Host2))
                        break;

                    canNotBeEmpty("Host2 Name");
                }


                IsDeleted = true;
                string query = $"UPDATE AirCraft SET Capacity = @Capacity, Model = @Model, Piolt = @Piolt, Host1 = @Host1, Host2 = @Host2, AirlineID = @AirlineID, IsDeleted = @IsDeleted WHERE AircraftID = @InputID";
                SqlCommand command = new SqlCommand(query, SystemRepostory.connection);

                command.Parameters.AddWithValue("@InputID", inputID);
                command.Parameters.AddWithValue("@Capacity", capacity);
                command.Parameters.AddWithValue("@Model", Model);
                command.Parameters.AddWithValue("@Piolt", Piolt);
                command.Parameters.AddWithValue("@Host1", Host1);
                command.Parameters.AddWithValue("@Host2", Host2);
                command.Parameters.AddWithValue("@AirlineID", airlineID);
                command.Parameters.AddWithValue("@IsDeleted", IsDeleted);

                command.ExecuteNonQuery();


                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The Row of Aircraft Is Updated Successfully.");
                Console.ForegroundColor = ConsoleColor.White;


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating AirCraft: {ex.Message}");
                return false;
            }
            return true;
        }



        //To Delete Specific Row With Its ID
        public bool Delete()
        {
            try
            {
                //To Ensure That ID is Int
                Console.Write("Enter AircraftID of Row You Want To Delete: ");

                int inputID;
                while (true)
                {
                    if (!int.TryParse(Console.ReadLine(), out inputID) || inputID <= 0)
                        NotValidIDPositive();
                    else if (!IsAircraftIDExist(inputID))
                        NotValidIDAirCraft();
                    else
                        break;

                }

                string query = "DELETE FROM AirCraft WHERE AircraftID = @InputID";
                using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
                {
                    command.Parameters.AddWithValue("@InputID", inputID);
                    command.ExecuteNonQuery();
                }


                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The Row of Aircraft Deleted Successfully.");
                Console.ForegroundColor = ConsoleColor.White;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting Aircraft: {ex.Message}");
                return false;
            }
            return true;
        }



        //To Show all Rows 
        public void GetAll()
        {
            try
            {
                string query = "SELECT * FROM AirCraft";
                using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        TableMaker.PrintLine();
                        TableMaker.PrintRow("AircraftID", "Capacity", "Model", "Pilot", "Host1", "Host2", "AirlineID", "IsDeleted");
                        TableMaker.PrintLine();
                        while (reader.Read())
                        {
                            TableMaker.PrintRow(reader["AircraftID"]?.ToString(), reader["Capacity"]?.ToString(), reader["Model"]?.ToString(), reader["Piolt"]?.ToString(), reader["Host1"]?.ToString()
                              , reader["Host2"]?.ToString(), reader["AirlineID"]?.ToString(), reader["IsDeleted"]?.ToString());
                            TableMaker.PrintLine();
                        }
                    }

                }

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The Rows of Aircraft are Retrieved Successfully.");
                Console.ForegroundColor = ConsoleColor.White;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Aircrafts: {ex.Message}");
            }
        }



        //To Get Specific Row With It's ID
        public void GetByID()
        {
            try
            {

                Console.Write("Enter AircraftID of Row You Want To GET: ");
                //To Ensure That ID is Int

                int inputID;
                while (true)
                {
                    if (!int.TryParse(Console.ReadLine(), out inputID) || inputID <= 0)
                        NotValidIDPositive();
                    else if (!IsAircraftIDExist(inputID))
                        NotValidIDAirCraft();
                    else
                        break;

                }

                //The Process
                string query = "SELECT * FROM AirCraft WHERE AircraftID = @InputID";
                using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
                {
                    command.Parameters.AddWithValue("@InputID", inputID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        TableMaker.PrintLine();
                        TableMaker.PrintRow("AircraftID", "Capacity", "Model", "Pilot", "Host1", "Host2", "AirlineID", "IsDeleted");
                        TableMaker.PrintLine();
                        while (reader.Read())
                        {
                            //Console.WriteLine($"AircraftID: {reader["AircraftID"]}, Capacity: {reader["Capacity"]}," +
                            //    $" Model: {reader["Model"]}, Pilot: {reader["Piolt"]}, Host1: {reader["Host1"]}, Host2: {reader["Host2"]}," +
                            //    $" AirlineID: {reader["AirlineID"]}, IsDeleted: {reader["IsDeleted"]}");
                            TableMaker.PrintRow(reader["AircraftID"]?.ToString(), reader["Capacity"]?.ToString(), reader["Model"]?.ToString(), reader["Piolt"]?.ToString(), reader["Host1"]?.ToString()
                              , reader["Host2"]?.ToString(), reader["AirlineID"]?.ToString(), reader["IsDeleted"]?.ToString());
                            TableMaker.PrintLine();
                        }
                    }

                }

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The Row of Aircraft Is Getted Successfully.");
                Console.ForegroundColor = ConsoleColor.White;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Aircraft: {ex.Message}");
            }
        }



        //==========================================

        //Not Valid Int Capacity
        private void NotValidCapacity()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Invalid input. Please Enter Your INT Capacity Like (1000/2000): ");
            Console.ForegroundColor = ConsoleColor.White;
        }

        //Not Valid IDPositive
        private void NotValidIDPositive()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Invalid input . Please enter a positive integer: ");
            Console.ForegroundColor = ConsoleColor.White;
        }

        //Not ValidID Existance AirlineID
        private void NotValidIDExistance()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid AirlineID. The specified AirlineID does not Exist.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Please enter a Exist ID:");
        }

        //Not Valid ID AirCraft
        private void NotValidIDAirCraft()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid AirCarftID. The specified AirCarftID does not Exist.");
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


        //Function that check if AircraftID is on DB
        private bool IsAircraftIDExist(int AircraftID)
        {
            bool exists = false;

            try
            {
                string query = "IF EXISTS (select 1 FROM AirCraft WHERE AircraftID = @AircraftID)SELECT 1 ELSE SELECT 0";

                using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
                {
                    command.Parameters.AddWithValue("@AircraftID", AircraftID);
                    exists = ((int)command.ExecuteScalar() == 1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking AircraftID existence: {ex.Message}");
            }

            return exists;
        }


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




        /////////////////////////////////////////////////////////////////////////
        public bool bookSeats(int s)
        {
            if (!seats[s - 1].Avaliablity)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Sorry this Seat is Unavailable ");
                Console.ForegroundColor = ConsoleColor.White;

                return false;
            }
            if (s <= capcity && s >= 0)
            {
                seats[s - 1].Avaliablity = false;
                string query = "update Seat set [IsAvailable]=0 where Aircraft_id=@aircraftId and Seat_num=@s;";

                using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
                {
                    command.Parameters.AddWithValue("@aircraftId", aircraftId);
                    command.Parameters.AddWithValue("@s", s);
                    command.ExecuteNonQuery();
                }
                return true;
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invaild Option ");
            Console.ForegroundColor = ConsoleColor.White;

            return false;
        }
    }
}

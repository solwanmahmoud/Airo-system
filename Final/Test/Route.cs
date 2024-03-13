
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Transactions;
using System.Windows.Input;

namespace c_Airline
{
    class Route : ICrudOperation
    {
        //Fields


        //Constructor

        //Properties 
        public int? RouteID { get; set; }
        public string? Origin { get; set; }
        public string? Destination { get; set; }
        public int? Distance { get; set; }
        public string? Classification { get; set; }
        public bool? IsDeleted { get; set; }


        //Main Functions : 
        //Function To create a new Row IN Route class
        public bool Create()
        {
            try
            {
                //Entering Data of The Row:

                //Entering Data of The Row:
                while (true)
                {
                    Console.Write("Enter Origin: ");
                    Origin = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(Origin))
                        break;

                    canNotBeEmpty("Origin");
                }


                while (true)
                {
                    Console.Write("Enter Destination: ");
                    Destination = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(Destination))
                        break;

                    canNotBeEmpty("Destination");
                }


                //Validation for Ensure That the Distance Is Int
                Console.Write("Enter Distance: ");
                int _distance;

                while (!int.TryParse(Console.ReadLine(), out _distance) || _distance <= 0)
                {
                    NotValidInt();
                }


                while (true)
                {
                    Console.Write("Enter Classification: ");
                    Classification = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(Classification))
                        break;

                    canNotBeEmpty("Classification");
                }

                //Fixed Value , Future Work For soft Delete
                IsDeleted = true;

                //Insert Query
                string query = "INSERT INTO Route (Distance,Destination,Origin,Classification,IsDeleted) VALUES (@distance,@destination,@origin,@classification,@IsDeleted)";

                //Assigning values to the variables of the Query
                using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
                {
                    command.Parameters.AddWithValue("@origin", Origin);
                    command.Parameters.AddWithValue("@destination", Destination);
                    command.Parameters.AddWithValue("@distance", _distance);
                    command.Parameters.AddWithValue("@classification", Classification);
                    command.Parameters.AddWithValue("@IsDeleted", IsDeleted);

                    command.ExecuteNonQuery();
                }

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The Row of Route is Created Successfully.");
                Console.ForegroundColor = ConsoleColor.White;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating Route: {ex.Message}");
                return false;
            }
            return true;
        }


        //To Delete Specific Row With It's ID
        public bool Delete()
        {
            try
            {

                Console.Write("Enter RouteID of The Row You Want To Delete: ");

                //To Ensure That ID is Int
                int InputID;
                while (true)
                {
                    if (!int.TryParse(Console.ReadLine(), out InputID) || InputID <= 0)
                        NotValidIDPositive();
                    else if (!IsIDExist(InputID))
                        NotValidIDExistance();
                    else
                        break;

                }

                string query = "DELETE FROM Route WHERE Route_ID = @InputID";

                using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
                {
                    command.Parameters.AddWithValue("@InputID", InputID);
                    command.ExecuteNonQuery();
                }


                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The Row of Route Deleted Successfully.");
                Console.ForegroundColor = ConsoleColor.White;


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting Route: {ex.Message}");
                return false;
            }
            return true;
        }


        //To Edit Specific Row With It's ID
        public bool Edit()
        {
            try
            {
                Console.Write("Enter RouteID of Row you Want To Edit: ");

                //To Ensure That ID is Int
                int InputID;
                while (true)
                {
                    if (!int.TryParse(Console.ReadLine(), out InputID) || InputID <= 0)
                        NotValidIDPositive();
                    else if (!IsIDExist(InputID))
                        NotValidIDExistance();
                    else
                        break;

                }


                while (true)
                {
                    Console.Write("Enter Origin: ");
                    Origin = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(Origin))
                        break;

                    canNotBeEmpty("Origin");
                }


                while (true)
                {
                    Console.Write("Enter Destination: ");
                    Destination = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(Destination))
                        break;

                    canNotBeEmpty("Destination");
                }


                //Validation for Ensure That the Distance Is Int
                Console.Write("Enter Distance: ");
                int _distance = 0;

                while (!int.TryParse(Console.ReadLine(), out _distance) || _distance <= 0)
                {
                    NotValidInt();
                }


                while (true)
                {
                    Console.Write("Enter Classification: ");
                    Classification = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(Classification))
                        break;

                    canNotBeEmpty("Classification");
                }

                IsDeleted = true;

                //Update Process
                string query = "UPDATE Route SET Distance=@Distance,Origin=@Origin,Destination=@Destination,Classification=@Classification,IsDeleted=@IsDeleted WHERE Route_ID=@InputID";
                using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
                {
                    command.Parameters.AddWithValue("@Distance", _distance);
                    command.Parameters.AddWithValue("@Origin", Origin);
                    command.Parameters.AddWithValue("@Destination", Destination);
                    command.Parameters.AddWithValue("@Classification", Classification);
                    command.Parameters.AddWithValue("@IsDeleted", IsDeleted);
                    command.Parameters.AddWithValue("@InputID", InputID);
                    command.ExecuteNonQuery();
                }


                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The Row of Route Is Updated Successfully.");
                Console.ForegroundColor = ConsoleColor.White;


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating Route: {ex.Message}");
                return false;
            }
            return true;
        }


        //To Show Specific all Rows 
        public void GetAll()
        {
            List<Route> routes = new List<Route>();

            try
            {
                //Query Process
                string query = "SELECT * FROM Route";
                using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            routes.Add(new Route()
                            {
                                RouteID = reader.IsDBNull(reader.GetOrdinal("Route_ID")) ? null : (int)reader["Route_ID"],
                                Distance = reader.IsDBNull(reader.GetOrdinal("Distance")) ? null : (int)reader["Distance"],
                                Destination = reader.IsDBNull(reader.GetOrdinal("Destination")) ? null : (string)reader["Destination"],
                                Origin = reader.IsDBNull(reader.GetOrdinal("Origin")) ? null : (string)reader["Origin"],
                                Classification = reader.IsDBNull(reader.GetOrdinal("Classification")) ? null : (string)reader["Classification"],
                                IsDeleted = reader.IsDBNull(reader.GetOrdinal("IsDeleted")) ? null : (bool)reader["IsDeleted"]
                            });
                        }
                    }
                }
                // Print The Data As a Table
                Console.WriteLine("All Routes:");
                TableMaker.PrintLine();
                TableMaker.PrintRow("Route_ID", "Distance", "Destination", "Origin", "Classification", "IsDeleted");
                TableMaker.PrintLine();

                foreach (var a in routes)
                {
                    TableMaker.PrintRow(a.RouteID.ToString(), a.Distance.ToString(), a.Destination, a.Origin, a.Classification, a.IsDeleted.ToString());
                    TableMaker.PrintLine();
                }

                //Successfully Statement

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The Rows of Route are Getted Successfully.");
                Console.ForegroundColor = ConsoleColor.White;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Route: {ex.Message}");
            }
        }

        //To Get Specific Row With It's ID
        public void GetByID()
        {
            try
            {

                Console.Write("Enter RouteID of Row you Want To GET BY ID: ");

                //To Ensure That ID is Int
                //To Ensure That ID is Int
                int InputID;
                while (true)
                {
                    if (!int.TryParse(Console.ReadLine(), out InputID) || InputID <= 0)
                        NotValidIDPositive();
                    else if (!IsIDExist(InputID))
                        NotValidIDExistance();
                    else
                        break;

                }

                string query = "SELECT * FROM Route WHERE Route_id = @InputID";

                using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
                {
                    command.Parameters.AddWithValue("@InputID", InputID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RouteID = reader.IsDBNull(reader.GetOrdinal("Route_ID")) ? null : (int)reader["Route_ID"];
                            Distance = reader.IsDBNull(reader.GetOrdinal("Distance")) ? null : (int)reader["Distance"];
                            Destination = reader.IsDBNull(reader.GetOrdinal("Destination")) ? null : (string)reader["Destination"];
                            Origin = reader.IsDBNull(reader.GetOrdinal("Origin")) ? null : (string)reader["Origin"];
                            Classification = reader.IsDBNull(reader.GetOrdinal("Classification")) ? null : (string)reader["Classification"];
                            IsDeleted = reader.IsDBNull(reader.GetOrdinal("IsDeleted")) ? null : (bool)reader["IsDeleted"];
                        }
                    }
                }
                TableMaker.PrintLine();
                TableMaker.PrintRow("Route_ID", "Distance", "Destination", "Origin", "Classification", "IsDeleted");
                TableMaker.PrintLine();

                TableMaker.PrintRow(RouteID.ToString(), Distance.ToString(), Destination, Origin, Classification, IsDeleted.ToString());
                TableMaker.PrintLine();


                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The Row of Route Is Getted Successfully.");
                Console.ForegroundColor = ConsoleColor.White;




            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Route: {ex.Message}");
            }
        }




        //Helper Functions : 
        //Function that check if id is on DB
        private bool IsIDExist(int Route_ID)
        {
            bool exists = false;

            try
            {
                string query = "IF EXISTS (SELECT 1 FROM Route WHERE Route_ID = @Route_ID) SELECT 1 ELSE SELECT 0";

                using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
                {
                    command.Parameters.AddWithValue("@Route_ID", Route_ID);
                    exists = ((int)command.ExecuteScalar() == 1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking Route_ID existence: {ex.Message}");
            }

            return exists;
        }




        //The NotValidIDExistance Message
        private void NotValidIDExistance()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid RouteID. The specified RouteID does not Exist.");
            Console.ForegroundColor = ConsoleColor.White;

        }


        //The NotValidInt Message
        private void NotValidInt()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Invalid input for Distance. Please enter a positive integer:");
            Console.ForegroundColor = ConsoleColor.White;
        }


        //The NotValidIDPositive Message
        private void NotValidIDPositive()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Invalid input for RouteID. Please enter a positive integer: ");
            Console.ForegroundColor = ConsoleColor.White;
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

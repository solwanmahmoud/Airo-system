using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_Airline
{
    static class Book
    {
        public static List<int> UpcomingFlight()
        {
            List<int> arr = new List<int>();
            Console.Clear();

            List<Flight> flights = new List<Flight>();

            try
            {
                string orgins, dest;
                SelectFunc(out orgins, out dest);
                string query = "select * from dbo.showFlights( @orgins,@dest)";
                using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
                {
                    command.Parameters.AddWithValue("@orgins", orgins);
                    command.Parameters.AddWithValue("@dest", dest);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            arr.Add((int)reader["FlightID"]);
                            flights.Add(new Flight()
                            {
                                FlightID = reader.IsDBNull(reader.GetOrdinal("FlightID")) ? null : (int?)reader["FlightID"],
                                AircraftId = reader.IsDBNull(reader.GetOrdinal("AircraftID")) ? null : (int?)reader["AircraftID"],
                                DepartedTime = reader.IsDBNull(reader.GetOrdinal("Dept_Date")) ? null : (DateTime?)reader["Dept_Date"],
                                RemainingSeats = reader.IsDBNull(reader.GetOrdinal("Destination")) ? null : (int?)reader["remainigSeats"],
                                PricePerPassenger = reader.IsDBNull(reader.GetOrdinal("Destination")) ? null : (decimal?)reader["Price"],
                                Origins = reader.IsDBNull(reader.GetOrdinal("Destination")) ? "" : reader["Origin"].ToString(),
                                Dest = reader.IsDBNull(reader.GetOrdinal("Destination")) ? "" : reader["Destination"].ToString(),

                            });
                        }
                    }
                }
                Console.WriteLine("All Flights:");
                TableMaker.PrintLine();
                TableMaker.PrintRow("FlightID", "AircraftID", "remainigSeats", "Price", "Dept_Date", "Dept_time", "Origin", "Destination");
                TableMaker.PrintLine();
                string? date="", time="";
                string[] Separate=new string[2];
                foreach (var a in flights)
                {
                     Separate = a.DepartedTime.ToString().Split(' ');

                    date = Separate[0];
                    time = Separate[1];
                    //Console.WriteLine($"FlightID: {a.FlightID}, AircraftID: {a.AircraftId}, Route_ID: {a.RouteId}," +
                    //    $" Num Of Passengers: {a.NumOfPassenger},Price Per Passenger: {a.PricePerPassenger}, Departed Time: {a.DepartedTime}, ArrivalTime: {a.ArrivalTime}," +
                    //    $"TimeSpent: {a.TimeSpent} , IsDeleted: {a.IsDeleted}");
                    TableMaker.PrintRow(a.FlightID.ToString(), a.AircraftId.ToString(), a.RemainingSeats.ToString(), a.PricePerPassenger.ToString(),
                      date, time, a.Origins, a.Dest);
                    TableMaker.PrintLine();
                }


                //Successfully Statement

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The Rows of Flight are Getted Successfully.");
                Console.ForegroundColor = ConsoleColor.White;
                return arr;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Aircraft: {ex.Message}");
                return new List<int>();
            }
        }

        public static void SelectFunc(out string origns, out string Dest)
        {
            List<string> list = new List<string>() {
            "Cairo", "Hurghada", "Luxor",
            "Alex", "Sharm", "Aswan",
        };
            list.Remove("a");

            origns = AnsiConsole.Prompt(new SelectionPrompt<string>()
             .Title("What's your [green]yout Origins[/]?")
             .AddChoices(new[] {
            "Cairo", "Hurghada", "Luxor",
            "Alex", "Sharm", "Aswan",
             }));
            list.Remove(origns);
            string[] cities = new string[6];
            for (int i = 0; i < list.Count; i++)
                cities[i] = list[i].ToString();

            Dest = AnsiConsole.Prompt(new SelectionPrompt<string>()
             .Title("What's your [green]yout destination[/]?")
             .AddChoices(cities));


        }

        public static void BookSeat(int num)
        {
            int capctiy = 0, id = 0;
            string query = "select * from GetAircraftDetails(@num)";
            using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
            {
                command.Parameters.AddWithValue("@num", num);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        id = (int)reader["ID"];
                        capctiy = (int)reader["Capacity"];
                    }
                }
            }
            Aircraft a1 = new Aircraft(id, capctiy);
            Seats(a1.Capcity, a1.Seats);
            int s, c = 2;
            bool isDone = false;
            do
            {
                Console.WriteLine("select a Seat ");
        while ( !int.TryParse(Console.ReadLine(), out s))
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine("Invalid");
          Console.ForegroundColor = ConsoleColor.White;
        }
        isDone = a1.bookSeats(s);
                if (isDone)
                    break;
            }
            while (c-- > 0);
            if (isDone)
            {
                query = "Update Flights set Num_OF_Passanger=Num_OF_Passanger+1 where FlightID=@num;";
                using (SqlCommand command = new SqlCommand(query, SystemRepostory.connection))
                {
                    command.Parameters.AddWithValue("@num", num);
                    command.ExecuteNonQuery();
                }
            }

        }
        static void Seats(int n, List<Seat> l1)
        {
            Console.Clear();
            var rows = new List<Text>(){
    new Text("For Avaliable Seates", new Style(Color.LightSlateBlue, Color.Black)),
    new Text(""),
    new Text("For Unavaliable Seates", new Style(Color.Red3, Color.Black))
};
            AnsiConsole.Write(new Rows(rows));
            Console.WriteLine();

            var table = new Table();
            var table2 = new Table();
            var table3 = new Table();

            string avaliableColor = "", num;
            for (int i = 0; i <= (n / 15); i++)
            {
                table = new Table();
                table2 = new Table();
                table3 = new Table();
                for (int j = (i * 15) + 1; j <= (i * 15) + 15; j++)
                {
                    if (j < n)
                        avaliableColor = (l1[j - 1].Avaliablity) ? "lightslateblue" : "red3";
                    num = j < 10 ? $"0{j.ToString()}" : j.ToString();
                    if (j <= +(i * 15) + 5)
                    {

                        if (j <= n)
                            table.AddColumn(new TableColumn(new Markup($"[{avaliableColor}]{num}[/]").Centered()));
                        else
                            table.AddColumn(new TableColumn(new Markup($"[black]00[/]")));

                    }

                    else if (j <= (i * 15) + 10)
                    {
                        if (j <= n)
                            table2.AddColumn(new TableColumn(new Markup($"[{avaliableColor}]{num}[/]").Centered()));
                        else
                            table2.AddColumn(new TableColumn(new Markup($"[black]00[/]")));

                    }

                    else
                    {
                        if (j <= n)
                            table3.AddColumn(new TableColumn(new Markup($"[{avaliableColor}]{num}[/]").Centered()));
                        else
                            table3.AddColumn(new TableColumn(new Markup($"[black]00[/]")));

                    }


                }
                AnsiConsole.Write(new Columns(
                    table,
                    table2, table3));

            }
        }
    }
}

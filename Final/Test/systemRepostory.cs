
using c_Airline;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Color = Spectre.Console.Color;

namespace c_Airline
{
  class SystemRepostory
  {
    public static SqlConnection connection;
    static readonly string connectionString = "Data Source=DESKTOP-KKAM6IO;Initial Catalog=AirLine Company;Integrated Security=True;";
    static SystemRepostory()
    {
      connection = new SqlConnection(connectionString);
      connection.Open();
    }

    public static void MainMenuEntity()
    {
      Console.Clear();

      //Present In Table: 
      var table = AnsiConsole.Prompt(new SelectionPrompt<string>()
        .Title("Choose a [green]Table Name [/]:")
        .AddChoices(new[] {
            "Airline", "Employee", "Aircraft",
            "Route", "Flights", "Transaction","Other","Back",
        }));

      //Convert table to UserOptionsEntity Enum :
      UserOptionsEntity UserOptionsEntity = ConvertToOption(table);

      UserOptions usrOption = MainMenuMethod(UserOptionsEntity);

      AssignEntityToClass(UserOptionsEntity, usrOption);
      if (UserOptionsEntity == UserOptionsEntity.Back)
      {
        Console.Clear();
        Login();
      }
    }

    public static UserOptions MainMenuMethod(UserOptionsEntity str)
    {
      Console.Clear();

      if (str == UserOptionsEntity.Back) return 0;

      if (str == UserOptionsEntity.Other)
        OtherOptions();
      //Present In Table: 

      var menu = AnsiConsole.Prompt(new SelectionPrompt<string>()
       .Title("Choose an [green]option[/]:")
       .AddChoices(new[] {
            $"1-For Add {str}", $"2-get {str} by id", $"3-Show All {str}",
            $"4-delete {str}", $"5-edit {str}", "6-Back",
       }));

      //return an enum that have choosed 
      return (UserOptions)int.Parse(menu[0].ToString());

    }

    public static void AssignEntityToClass(UserOptionsEntity op, UserOptions usrOption)
    {
      switch (op)
      {
        case UserOptionsEntity.Airline:
          ICrudOperation airline = new Airline();
          GetUserOption(airline, op, usrOption);
          break;
        case UserOptionsEntity.Employee:
          ICrudOperation emp = new Employee();
          GetUserOption(emp, op, usrOption);
          break;
        case UserOptionsEntity.Aircraft:
          ICrudOperation aircraft = new Aircraft();
          GetUserOption(aircraft, op, usrOption);
          break;
        case UserOptionsEntity.Flight:
          ICrudOperation ac = new Flight();
          GetUserOption(ac, op, usrOption);
          break;
        case UserOptionsEntity.Route:
          ICrudOperation route = new Route();
          GetUserOption(route, op, usrOption);
          break;
        case UserOptionsEntity.Transaction:
          ICrudOperation transaction = new Transaction();
          GetUserOption(transaction, op, usrOption);
          break;
        default:
          {
            Console.WriteLine("Invalid Input");
          }
          break;
      }
    }


    public static void GetUserOption(ICrudOperation e, UserOptionsEntity str, UserOptions userInput)
    {

      switch (userInput)
      {
        case UserOptions.Add:
          {
            e.Create();
            Console.ReadKey();
            userInput = MainMenuMethod(str);
            GetUserOption(e, str, userInput);
          }
          break;
        case UserOptions.GetByID:
          {
            e.GetByID();
            Console.ReadKey();
            userInput = MainMenuMethod(str);
            GetUserOption(e, str, userInput);
          }
          break;
        case UserOptions.GetAll:
          {
            e.GetAll();
            Console.ReadKey();
            userInput = MainMenuMethod(str);
            GetUserOption(e, str, userInput);
          }
          break;
        case UserOptions.Edit:
          {
            e.Edit();
            Console.ReadKey();
            userInput = MainMenuMethod(str);
            GetUserOption(e, str, userInput);
          }
          break;
        case UserOptions.Delete:
          {
            e.Delete();
            Console.ReadKey();
            userInput = MainMenuMethod(str);
            GetUserOption(e, str, userInput);
          }
          break;
        case UserOptions.Back:
          {
            MainMenuEntity();
          }
          break;
        default:
          {
            Console.WriteLine("Invalid Input");
          }
          break;
      }

    }


    //To convert str to enum UserOptionsEntity
    public static UserOptionsEntity ConvertToOption(string str)
    {
      return str switch
      {
        "Airline" => UserOptionsEntity.Airline,
        "Employee" => UserOptionsEntity.Employee,
        "Aircraft" => UserOptionsEntity.Aircraft,
        "Route" => UserOptionsEntity.Route,
        "Flights" => UserOptionsEntity.Flight,
        "Transaction" => UserOptionsEntity.Transaction,
        "Other"=> UserOptionsEntity.Other,
        "Back" => UserOptionsEntity.Back,
        _ => UserOptionsEntity.Exit,
      };
    }

    //main function
    public static void Login()
    {
      //Welcome Function : 
      Panel();

      //Menu Of Work :
      var table = AnsiConsole.Prompt(new SelectionPrompt<string>()
     .Title("Choose an [green]Option[/]:")
     .AddChoices(new[] {
            "1-Employee", "2-Passenger", "3-Exit",

     }));
      int option = int.Parse(table[0].ToString());

      //Employee Or Admin Domain:
      if (option == 1)
      {
        //Log In Step To Admin's Profile :
        int maxAttempts = 3;

        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
          Console.Write("Admin Name: ");
          string? userName = Console.ReadLine();

          Console.Write("Password: ");
          string pass = string.Empty;
          ConsoleKey key;

          //To Hide The password while writing it
          do
          {
            // the pressed key won't be displayed on the console.
            var keyInfo = Console.ReadKey(intercept: true);
            //relieve the value representing the key that was pressed.
            key = keyInfo.Key;


            //Erase the last character from both the console display and
            //the stored password string when enter Backspace
            if (key == ConsoleKey.Backspace && pass.Length > 0)
            {
              Console.Write("\b \b");
              pass = pass[0..^1];
            }

            //To ensure not to print any char and make it as ( * ):
            else if (!char.IsControl(keyInfo.KeyChar))
            {
              Console.Write("*");
              pass += keyInfo.KeyChar;
            }
            //When Press Enter it means the word is end 
          } while (key != ConsoleKey.Enter);


          //Make sure the User name and Password Correct:
          if (userName?.ToLower() == "admin" && (pass?.ToLower() == "admin"))
          {
            Console.Clear();

            //Loading Bar
            Console.Title = "C# Console Progress bar";
            Console.CursorVisible = false;
            Console.SetCursorPosition(1, 1);

            for (int i = 0; i <= 50; i++)
            {
              for (int j = 0; j < i; j++)
              {
                string pb = "\u2551";
                Console.Write(pb);
              }
              Console.WriteLine(i * 2 + " / 100");
              Console.SetCursorPosition(1, 1);
              Console.ForegroundColor = ConsoleColor.DarkBlue;
              System.Threading.Thread.Sleep(50);
            }

            Console.ForegroundColor = ConsoleColor.White;

            MainMenuEntity();
            break;
          }
          //If There is a problem and they don't match :
          else
          {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Invalid credentials. Attempts left: {maxAttempts - attempt}");

            if (attempt < maxAttempts)
            {
              Console.WriteLine("Please try again.");
              Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
              Console.WriteLine("Maximum attempts reached. Exiting...");
              Console.ForegroundColor = ConsoleColor.White;

            }
          }
        }
      }

      //Passenger Or User Domain:
      else if (option == 2)
      {
        Console.Clear();

        //Welcoming 
        Panel();

        //Options of Passenger:
        var table2 = AnsiConsole.Prompt(new SelectionPrompt<string>()
        .Title("Choose an [green]Option[/]?")
        .AddChoices(new[] {
            "1-Show Upcoming Flights", "2-BooK Flight", "3-Back", }));
        int opt = int.Parse(table2[0].ToString());

        if (opt == 1)
        {
          Book.UpcomingFlight();
          Console.ReadKey();
          Console.Clear();
          Login();
        }
        else if (opt == 2)
        {
          List<int> fl = Book.UpcomingFlight();

          Console.WriteLine("select flight");

          int input;
          while (!(int.TryParse(Console.ReadLine(), out input)) || !fl.Contains(input))
          {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invaild Option ");
            Console.ForegroundColor = ConsoleColor.White;
          }

          Book.BookSeat(input);
          Console.Clear();
          Console.Clear();
          Console.Clear();
          Login();
        }
        else
        {

          Console.Clear();
          Login();
        }

      }

      //Exit From The System
      else if (option == 3)
      {
        //close the connection
        connection.Close();

        //clear the console 
        Console.Clear();

        //end the program
        return;
      }

    }

    //Welcoming Function
    public static void Panel()
    {
      AnsiConsole.Write(
      new FigletText("Welcome To ")
       .LeftJustified()
       .Color(Color.DeepSkyBlue4));
      AnsiConsole.Write(
      new FigletText("Airo")
       .Centered()
       .Color(Color.DarkCyan));
      AnsiConsole.Write(
      new FigletText("System")
       .RightJustified()
       .Color(Color.Green1));
      Console.WriteLine();
      Console.WriteLine();
    }


    public static void OtherOptions()
    {
      var menu = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Choose an [green]option[/]:")
            .AddChoices(new[] {
            $"1-Set Flights Arival ","2-Add Flight Transaction ","3-Back",
            }));
      int op = int.Parse(menu[0].ToString());
      

      if (op == 1)
      {
        Flight flight;
          Console.Clear();
        do
        {
          Console.WriteLine("Enter Flight ID");
          int InputID;
          while (!int.TryParse(Console.ReadLine(), out InputID) || InputID <= 0)
          {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Invalid input. ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Please enter a positive integer:");
          }
          flight = new Flight(InputID);
        }while (flight.AircraftId==null);
        flight.FlightArrival();
        Console.ForegroundColor = ConsoleColor.Green;

        Console.WriteLine($"the flight {flight.FlightID} Arrival At {flight.ArrivalTime} in {flight.DestinationCity()}, Time Spent is {flight.TimeSpent} hour");
        Console.ForegroundColor = ConsoleColor.White;

        Console.ReadKey();
      }
      else if(op == 2)
      {
        Flight flight;
        Console.Clear();
        do
        {
          Console.WriteLine("Enter Flight ID");
          int InputID;
          while (!int.TryParse(Console.ReadLine(), out InputID) || InputID <= 0)
          {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Invalid input. ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Please enter a positive integer:");
          }
          flight = new Flight(InputID);
        } while (flight.AircraftId == null);
        decimal totalPrice=flight.TotalPrice();
        Transaction transaction = new Transaction(flight.AircraftId,totalPrice,$"the total amount for Flight id{flight.FlightID}");
        Console.ForegroundColor = ConsoleColor.Green;

        Console.WriteLine($"the Transaction Added successfully");
        Console.ForegroundColor = ConsoleColor.White;
        
        Console.ReadKey();

      }
      Console.Clear();
      MainMenuEntity();

    }


  }
}






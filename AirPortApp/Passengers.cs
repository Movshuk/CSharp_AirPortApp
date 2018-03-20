using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Globalization;


namespace AirPortApp
{
   class Passengers
   {
      internal string FirstName { get; set; }
      internal string LastName { get; set; }
      internal string IdNumber { get; set; }
      internal string Age { get; set; }
      internal string Gender { get; set; }

      // вывод всех пассажиров
      internal void ShowAllPassengers()
      {
         string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\LocalDBAirPortApp.mdf;Integrated Security=True";


         Console.WriteLine();
         Console.WriteLine("> Список всех [Пассажиров]:");

         // изменить строку адреса!!!!!!!!!!!!!
         //string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\LocalDBAirPortApp.mdf;Integrated Security=True";

         string sqlExp = "SELECT * FROM TablePassengers";

         using (SqlConnection connection = new SqlConnection(connectionString))
         {
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExp, connection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows) // если есть данные
            {
               // выводим названия столбцов
               Console.WriteLine("{0, 10}{1, 10}{2, 10}{3, 10}{4, 10}", reader.GetName(1), reader.GetName(2), reader.GetName(3), reader.GetName(4), reader.GetName(5));
               Console.WriteLine("-------------------------------------------");

               while (reader.Read()) // построчно считываем данные
               {
                  string coll1 = reader.GetString(1);
                  string coll2 = reader.GetString(2);
                  string coll3 = reader.GetString(3);
                  string coll4 = reader.GetString(4);
                  string coll5 = reader.GetString(5);

                  Console.WriteLine("{0, 10}{1, 10}{2, 10}{3, 10}{4, 10}", coll1, coll2, coll3, coll4, coll5);
               }
            }
            else
            {
               Console.WriteLine("Данных нет. Таблица пустая.");
            }
            reader.Close();

         }
         Console.WriteLine();

      }

      // проверяет пассажир новый или уже ранее покупал билеты
      internal bool SelectPass(string fn, string ln, string idn)
      {
         bool flag = false;

         string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\LocalDBAirPortApp.mdf;Integrated Security=True";

         string sqlExp = @"SELECT Name, COUNT(LName) AS COUNTTICKET 
            FROM TableTickets 
            WHERE Name = @fn AND LName = @ln AND IdNumber = @idn
            GROUP BY Name";

         using (SqlConnection connection = new SqlConnection(connectionString))
         {
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExp, connection);
            command.Parameters.AddWithValue("@fn", fn);
            command.Parameters.AddWithValue("@ln", ln);
            command.Parameters.AddWithValue("@idn", idn);

            SqlDataReader reader = command.ExecuteReader();

            // если есть данные
            if (reader.HasRows) 
            {
               while (reader.Read())
               {
                  int coll1 = reader.GetInt32(1);
                  if (coll1 > 0)
                  {
                     //reader.Close();
                     flag = true;
                     break;
                  }
                  else
                  {
                     //reader.Close();
                     break;
                  }
               }
               return flag;
            }
            else
            {
               return flag;
            }
            
         }
      }

      // регистрация пассажира
      internal void InsertToPassengers()
      {
         string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\LocalDBAirPortApp.mdf;Integrated Security=True";

         Console.WriteLine();

         // выбор направления
         Console.WriteLine("> СПИСОК ДЕЙСТВУЮЩИХ НАПРАВЛЕНИЙ");
         AirDirection ad = new AirDirection();
         ad.ShowAllDiretions();

         Console.WriteLine("> Введите номер рейса:");
         int idDir = Convert.ToInt32(Console.ReadLine()); // здесь отловить исключение
         Console.Clear();

         Console.WriteLine(">> Регистрируем нового пассажира на рейс:");

         Console.WriteLine("> Введите дату рейса [ГГГГ.ММ.ДД]");

         DateTime dateTrip = DateTime.ParseExact(Console.ReadLine(), "yyyy.MM.dd", CultureInfo.InvariantCulture);
         Console.WriteLine("====== {0}", dateTrip);

         Console.WriteLine("> Введите [ИМЯ]:");
         FirstName = Console.ReadLine().ToUpper();

         Console.WriteLine("> Введите [ФАМИЛИЮ]:");
         LastName = Console.ReadLine().ToUpper();

         Console.WriteLine("> Введите [НОМЕР ПАСПОРТА]:");
         IdNumber = Console.ReadLine().ToUpper();

         Console.WriteLine("> Введите [ВОЗРАСТ]:");
         Age = Console.ReadLine();

         Console.WriteLine("> Введите [ПОЛ]:");
         Gender = Console.ReadLine().ToUpper();

         // запрос, существует ли такой пассажир в базе регистрировавшихся пассажиров
         bool flagInsert = false;
         string sqlExp = @"SELECT * FROM TablePassengers 
            WHERE 
            FirstName=@FirstName 
            and LastName = @LastName
            and IdNumber = @IdNumber
            and Age = @Age
            and Gender = @Gender";

         using (SqlConnection connection = new SqlConnection(connectionString))
         {
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExp, connection);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@IdNumber", IdNumber);
            command.Parameters.AddWithValue("@Age", Age);
            command.Parameters.AddWithValue("@Gender", Gender);

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows) // если есть данные
            {
               Console.WriteLine("> Такой ПАССАЖИР уже добавлен в БД регистрации!");
            }
            else
            {
               flagInsert = true; // если пассажир первый раз регистрируется
            }
            reader.Close();
         }

         // если пассажир регистрируется первый раз, то добавляю его данные в таблицу Пассажиры
         if (flagInsert)
         {
            string sqlExpression = "INSERT INTO TablePassengers VALUES (@FirstName, @LastName, @IdNumber, @Age, @Gender)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
               connection.Open();
               SqlCommand command = new SqlCommand(sqlExpression, connection);
               command.Parameters.AddWithValue("@FirstName", FirstName);
               command.Parameters.AddWithValue("@LastName", LastName);
               command.Parameters.AddWithValue("@IdNumber", IdNumber);
               command.Parameters.AddWithValue("@Age", Age);
               command.Parameters.AddWithValue("@Gender", Gender);

               command.ExecuteNonQuery();

            }
            Console.WriteLine(">> Данные пассажира внесены в БД!");
            Console.WriteLine();
         }

         // получаем массив данных на рейс (цена, класс, направление)
         ArrayList dpc = new ArrayList();

         dpc.AddRange(ad.SelectDPC(idDir));
         //Console.WriteLine("====> {0} {1} {2}", dpc[1], dpc[2], dpc[3]);


         // проверка новый или постоянный пассажир для расчета скидки
         bool flagPas = SelectPass(FirstName, LastName, IdNumber);
         double discount;
         double priceDisc;

         // true = постоянный, false = первый раз
         if (flagPas)
         {
            discount = 0.1; // скидка 10%
            priceDisc = Math.Round((Convert.ToDouble(dpc[2]) - (Convert.ToDouble(dpc[2]) * discount)),2);
         }
         else
         {
            priceDisc = Convert.ToDouble(dpc[2]) - 2; // фиксированная скидка -2у.е.
            if (priceDisc <= 0)
            {
               Console.WriteLine("> Внимание! Цена с учетом скидки не может быть меньше 0! Цена билета принята без скидки!");
               priceDisc += 2;
               discount = 0;
            }
            else
            {
               discount = Math.Round(2 / Convert.ToDouble(dpc[2]), 2);
            }
         }

            //Console.WriteLine("====//////////////////////////====");
            // оформление билета


            string sqlExpression2 = @"INSERT INTO TableTickets 
               VALUES (@Direction, @Name, @LName, @IdNumber, @PriceDirection,
               @Discount, @PriceWithDiscount, @RegDate, @Class)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
               connection.Open();
               SqlCommand command = new SqlCommand(sqlExpression2, connection);
               command.Parameters.AddWithValue("@Direction", dpc[1]);
               command.Parameters.AddWithValue("@Name", FirstName);
               command.Parameters.AddWithValue("@LName", LastName);
               command.Parameters.AddWithValue("@IdNumber", IdNumber);
               command.Parameters.AddWithValue("@PriceDirection", dpc[2]);
               command.Parameters.AddWithValue("@Discount", discount);
               command.Parameters.AddWithValue("@PriceWithDiscount", priceDisc);
               command.Parameters.AddWithValue("@RegDate", dateTrip);
               command.Parameters.AddWithValue("@Class", dpc[3]);

               command.ExecuteNonQuery();

            }
            Console.WriteLine(">> Данные добавлены!");
            Console.WriteLine();

         // выбираю данные по билетам:
         string sqlExp2 = "SELECT * FROM TableTickets";

         using (SqlConnection connection = new SqlConnection(connectionString))
         {
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExp2, connection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows) // если есть данные
            {
               // выводим названия столбцов
               Console.WriteLine("{0, 5}{1, 20}{2, 10}{3, 10}{4, 10}{5, 10}{6, 10}{7, 10}{8, 13}{9, 13}", reader.GetName(0), reader.GetName(1), reader.GetName(2), reader.GetName(3), reader.GetName(4), "Price", reader.GetName(6), "Price-%", reader.GetName(8), reader.GetName(9));
               Console.WriteLine("---------------------------------------------------------------------");

               while (reader.Read()) // построчно считываем данные
               {
                  //object id = reader.GetValue(1);
                  //object name = reader.GetValue(2);
                  //object age = reader.GetValue(3);
                  //Console.WriteLine("****************{0}", reader.GetValue(9).ToString().Length);
                  //Console.WriteLine("****************{0}", reader.GetDateTime(8).ToShortDateString());

                  Console.WriteLine("{0, 5}{1, 20}{2, 10}{3, 10}{4, 10}{5, 10}{6, 10}{7, 10}{8, 13}{9, 13}", reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), reader.GetValue(3), reader.GetValue(4), reader.GetValue(5), reader.GetValue(6), reader.GetValue(7), reader.GetDateTime(8).ToShortDateString(), reader.GetValue(9));
               }
            }
            reader.Close();
         }















      }

      // стоимость всех проданных билетов
      internal void SumAllTickets()
      {
         string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\LocalDBAirPortApp.mdf;Integrated Security=True";

         string sqlExp = @"SELECT SUM(PriceWithDiscount) FROM TableTickets";

         using (SqlConnection connection = new SqlConnection(connectionString))
         {
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExp, connection);
           
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows) // если есть данные
            {
               Console.WriteLine("{0}\t", reader.GetName(0));
               Console.WriteLine("-------------------------------------------");

               while (reader.Read()) // построчно считываем данные
               {
                  // проверяю на null
                  if (reader.IsDBNull(0))
                  {
                     Console.WriteLine("Всего продано билетов с учетом скидки: <0> $");
                  }
                  else
                  {
                     string coll1 = reader.GetDouble(0).ToString();

                     Console.WriteLine("Всего продано билетов с учетом скидки: {0} $", coll1);
                  }
               }

            }
            else
            {
               Console.WriteLine("Нет зарегистрированных билетов!");
            }

         }
      }

      // стоимость всех проданных билетов для выбранного пассажира
      internal void SumAllTicketsThisPass()
      {
         string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\LocalDBAirPortApp.mdf;Integrated Security=True";

         Console.WriteLine(">> Стоимость все купленных пассажиром билетов:");

         Console.WriteLine("> Введите [ИМЯ]:");
         FirstName = Console.ReadLine().ToUpper();

         Console.WriteLine("> Введите [ФАМИЛИЮ]:");
         LastName = Console.ReadLine().ToUpper();

         Console.WriteLine("> Введите [НОМЕР ПАСПОРТА]:");
         IdNumber = Console.ReadLine().ToUpper();

         string sqlExp = @"SELECT Name, LName, IdNumber, SUM(PriceWithDiscount) AS НаСумму$
            FROM TableTickets
            WHERE Name = @FirstName AND LName = @LastName AND IdNumber = @IdNumber
            GROUP BY Name, LName, IdNumber";

         using (SqlConnection connection = new SqlConnection(connectionString))
         {
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExp, connection);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@IdNumber", IdNumber);

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows) // если есть данные
            {

               while (reader.Read()) // построчно считываем данные
               {
                  // проверяю на null
                  if (reader.IsDBNull(0))
                  {
                     Console.WriteLine("Всего продано билетов пассажиру с учетом скидки: <0> $");
                  }
                  else
                  {
                     Console.WriteLine("{0, 15}{1, 15}{2, 15}{3, 15}", reader.GetName(0), reader.GetName(1), reader.GetName(2), reader.GetName(3));
                     Console.WriteLine("-------------------------------------------------------");

                     Console.WriteLine("{0, 15}{1, 15}{2, 15}{3, 15}", reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), reader.GetValue(3));
                  }
               }

            }
            else
            {
               Console.WriteLine("Нет зарегистрированных билетов для выбранного пассажира!");
            }

         }

      }

   }
}

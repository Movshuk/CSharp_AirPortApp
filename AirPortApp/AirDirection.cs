using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


// Класс направление
namespace AirPortApp
{
   class AirDirection
   {
      internal string Direction { get; set; }
      internal float Price { get; set; }
      internal string PlaceClass { get; set; }

      // вывести на печать все данные о всех направлениях
      internal void ShowAllDiretions()
      {
         Console.WriteLine();
         Console.WriteLine("======== Список всех направлений: ========");

         string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\LocalDBAirPortApp.mdf;Integrated Security=True";

         string sqlExp = "SELECT * FROM TableDirection";

         using (SqlConnection connection = new SqlConnection(connectionString))
         {
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExp, connection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows) // если есть данные
            {
               // выводим названия столбцов
               Console.WriteLine("{0, 5} | {1, 20} | {2, 10} | {3, 15}", reader.GetName(0), reader.GetName(1), reader.GetName(2), reader.GetName(3));
               Console.WriteLine("---------------------------------------------------------------------");

               while (reader.Read()) // построчно считываем данные
               {
                  object id = reader.GetValue(1);
                  object name = reader.GetValue(2);
                  object age = reader.GetValue(3);

                  Console.WriteLine("{0, 5} | {1, 20} | {2, 10} | {3, 15}", reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), reader.GetValue(3));
               }
            }
            reader.Close();
           
         }
         Console.WriteLine();

      }

      // внести данные в таблицу
      internal void InsertToDirection()
      {
         Console.WriteLine();

         Console.WriteLine("======== Ввод нового направления в базу данных: ========");

         // контроль длины полей направление не менее 3 символов
         bool exLeng = false;
         string DirectionА = null; // пустая строка
         string DirectionB = null;

         for (; exLeng == false;)
         {
            Console.WriteLine("> Введите [направление] Пункт А:");
            DirectionА = Console.ReadLine().ToUpper();
            if (DirectionА.Length >= 3)
               exLeng = true;
            else
               Console.WriteLine("> Наименование [направление] должно содержать не меньше 3 символов. Повторите ввод!:");
         }
         exLeng = false;
         for (; exLeng == false;)
         {
            Console.WriteLine("> Введите [направление] Пункт B:");
            DirectionB = Console.ReadLine().ToUpper();
            if (DirectionB.Length >= 3)
               exLeng = true;
            else
               Console.WriteLine("> Наименование [направление] должно содержать не меньше 3 символов. Повторите ввод!:");
         }


         Direction = DirectionА + " - " + DirectionB;

         Console.WriteLine("> Введите [цену] направления (А->B):");
         Price = float.Parse(Console.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);

         Console.WriteLine("> Введите [класс] (А->B):");
         PlaceClass = Console.ReadLine().ToUpper();


         string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\LocalDBAirPortApp.mdf;Integrated Security=True";

         bool flagInsert = false;
         string sqlExp1 = "SELECT AirDirection, PlaceClass FROM TableDirection WHERE AirDirection=@Direction and PlaceClass = @PlaceClass";

         using (SqlConnection connection = new SqlConnection(connectionString))
         {
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExp1, connection);
            command.Parameters.AddWithValue("@Direction", Direction);
            command.Parameters.AddWithValue("@PlaceClass", PlaceClass);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows) // если есть данные
            {
               Console.WriteLine("> Нужно вводить УНИКАЛЬНЫЕ значения. Такое направление и класс уже есть!");
            }
            else
            {
               flagInsert = true;
            }
            reader.Close();
         }


         if (flagInsert)
         {
            string connectionString2 = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\LocalDBAirPortApp.mdf;Integrated Security=True";
            string sqlExpression = "INSERT INTO TableDirection VALUES (@Direction, @Price, @PlaceClass)";

            using (SqlConnection connection = new SqlConnection(connectionString2))
            {
               connection.Open();
               SqlCommand command = new SqlCommand(sqlExpression, connection);
               command.Parameters.AddWithValue("@Direction", Direction);
               command.Parameters.AddWithValue("@Price", Price);
               command.Parameters.AddWithValue("@PlaceClass", PlaceClass);

               command.ExecuteNonQuery();

            }
            Console.WriteLine(">> Данные добавлены!");
            Console.WriteLine();

         }
      }

      internal ArrayList SelectDPC(int id)
      {
         ArrayList al = new ArrayList(); // здесь будет строка данных по номеру рейса
         string idDir = id.ToString();
         string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\LocalDBAirPortApp.mdf;Integrated Security=True";

         string sqlExp = "SELECT * FROM TableDirection WHERE id = @idDir";

         using (SqlConnection connection = new SqlConnection(connectionString))
         {
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExp, connection);
            command.Parameters.AddWithValue("@idDir", idDir);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows) // если есть данные
            {

               while (reader.Read()) // построчно считываем данные
               {
                  al.Add(reader.GetValue(0));
                  al.Add(reader.GetValue(1));
                  al.Add(reader.GetValue(2));
                  al.Add(reader.GetValue(3));
               }
               reader.Close();
               return al;
            }
            else
            {
               al.Add(false);
               return al;
            }

         }
      }

   }






}

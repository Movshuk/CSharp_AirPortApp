using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;

namespace AirPortApp
{
   class Program
   {
      static void FirstMenu()
      {
         Console.Clear();
         Console.WriteLine("============ Программа касса Аэропорта ============");
         Console.WriteLine();

         Console.WriteLine("[Меню]");
         Console.WriteLine("[0] Обзор и изменение направления авиаперелетов.");
         Console.WriteLine("[1] Регистрация пассажира и билета");
         Console.WriteLine("[2] Отчеты (продажи билетов)");
         Console.WriteLine("[3] ВЫХОД из системы");

         // выбор пункта меню
         string chois = Console.ReadLine();

         // ветвление
         switch (chois)
         {
            case "0":
               Console.Clear();
               SecondMenu();
               break;
            case "1":
               Console.Clear();
               ThirdMenu();
               break;
            case "2":
               FourthMenu();
               break;
            // exit
            case "3":
               Console.WriteLine(">> Работа с Системой завершена. Досвидания!");
               break;
            default:
               Console.WriteLine("Не верный выбор пункта меню.");
               break;
         }
      }

      static void SecondMenu()
      {
         Console.WriteLine("============ Программа касса Аэропорта ============");
         Console.WriteLine("== Обзор и изменение направления авиаперелетов. ==");

         Console.WriteLine();

         Console.WriteLine("[Меню]");
         Console.WriteLine("[0] Вывести на экран все направления и тарифы");
         Console.WriteLine("[1] Ввод нового тарифа");
         Console.WriteLine("[2] Изменение цена тарифа");
         Console.WriteLine("[3] Возврат в Основное меню");
         Console.WriteLine("[4] ВЫХОД из системы");

         // выбор пункта меню
         string chois = Console.ReadLine();

         // ветвление
         switch (chois)
         {
            case "0":
               Console.Clear();
               AirDirection ad1 = new AirDirection();
               ad1.ShowAllDiretions();
               Console.WriteLine(">> Нажмите ENTER TO CONTINUE");
               Console.ReadLine();
               Console.Clear();
               SecondMenu();
               break;
            case "1":
               Console.Clear();
               AirDirection ad2 = new AirDirection();
               ad2.InsertToDirection();
               Console.WriteLine(">> Нажмите ENTER TO CONTINUE");
               Console.ReadLine();
               Console.Clear();
               ThirdMenu();
               break;
            case "2":
               // изменение цены перелета
               break;
            case "3":
               FirstMenu();
               break;
            case "4":
               Console.WriteLine(">> Работа с Системой завершена. Досвидания!");
               break;
            default:
               Console.WriteLine("Не верный выбор пункта меню.");
               break;
         }

      }

      static void ThirdMenu()
      {
         Console.WriteLine("============ Программа касса Аэропорта ============");
         Console.WriteLine("======== Регистрация пассажира и билета ==========");

         Console.WriteLine();

         Console.WriteLine("[Меню]");
         Console.WriteLine("[0] Регистрация пассажира на рейс");
         Console.WriteLine("[1] Возврат в Основное меню");
         Console.WriteLine("[2] ВЫХОД из системы");

         // выбор пункта меню
         string chois = Console.ReadLine();

         // ветвление
         switch (chois)
         {
            case "0":
               Passengers p1 = new Passengers();
               p1.InsertToPassengers();
               Console.WriteLine(">> Нажмите ENTER TO CONTINUE");
               Console.ReadLine();
               Console.Clear();
               ThirdMenu();
               break;
            case "1":
               FirstMenu();
               break;
            case "2":
               Console.WriteLine(">> Работа с Системой завершена. Досвидания!");
               break;
            default:
               Console.WriteLine("Не верный выбор пункта меню.");
               break;
         }


      }

      static void FourthMenu()
      {
         Console.Clear();
         Console.WriteLine("============ Программа касса Аэропорта ============");
         Console.WriteLine("======== Отчеты продажи билетов ==========");

         Console.WriteLine();

         Console.WriteLine("[Меню]");
         Console.WriteLine("[0] Сколько купил билетов пассажир?");
         Console.WriteLine("[1] Сколько всего продано билетов с учетом скидки.");
         Console.WriteLine("[2] Возврат в Основное меню");
         Console.WriteLine("[3] ВЫХОД из системы");

         // выбор пункта меню
         string chois = Console.ReadLine();

         // ветвление
         switch (chois)
         {
            case "0":
               Passengers p0 = new Passengers();
               p0.SumAllTicketsThisPass();
               Console.WriteLine(">> Нажмите ENTER TO CONTINUE");
               Console.ReadLine();
               Console.Clear();
               FourthMenu();
               break;
            case "1":
               Passengers p1 = new Passengers();
               p1.SumAllTickets();
               Console.WriteLine(">> Нажмите ENTER TO CONTINUE");
               Console.ReadLine();
               Console.Clear();
               FourthMenu();
               break;
            case "2":
               FirstMenu();
               break;
            case "3":
               Console.WriteLine(">> Работа с Системой завершена. Досвидания!");
               break;
            default:
               Console.WriteLine("Не верный выбор пункта меню.");
               break;
         }

      }

      static void Main(string[] args)
      {
         try
         {
            // первое меню
            FirstMenu();

         }
         catch (Exception ex)
         {
            Console.WriteLine("Ошибка! " + ex.Message);
         }
         
         
      }
   }
}

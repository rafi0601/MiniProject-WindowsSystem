﻿//Bs"d

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using BE;
using static System.Console;
//using BL;

namespace ConsoleApp1
{
    class PL_ConsoleImp
    {
        public static BL.IBL bl = BL.FactorySingleton.Instance;



        static void Main(string[] args)
        {
            try
            {
                MapReq(new Address(), new Address());
                Debug.WriteLine("jvnrfvnjrkl");


                Console.WriteLine("FrEf");
                //WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                ReadKey();
                ForegroundColor = ConsoleColor.Green;

                bl.AddTester(new Tester("323947747", new Person.PersonName { FirstName = "Shmuel", LastName = "Garber" }, new DateTime(1950, 7, 13), Gender.male, "0547424870", new Address { Street = "Hganenet", HouseNumber = 5, City = "Jerusalem" }, "1234", 10, 30, Vehicle.tractor, new Schedule(new bool[,] { { true, false, false, false, false, false, true, false }, { true, false, false, true, false, false, false, false }, { true, false, false, false, false, false, false, false }, { true, false, false, false, false, false, false, false }, { true, false, false, false, false, false, false, false } }), 20));
                bl.AddTester(new Tester("322680083", new Person.PersonName { FirstName = "Refael", LastName = "Goldis" }, new DateTime(1949, 5, 12), Gender.male, "0556824870", new Address { Street = "Gordon", HouseNumber = 22, City = "Tel-Aviv" }, "1234", 6, 16, Vehicle.heavyTruck, new Schedule(new bool[,] { { true, true, false, true, false, false, false, true }, { true, false, false, true, false, true, false, false }, { true, false, false, false, false, false, false, true }, { true, false, false, true, false, false, false, false }, { true, false, false, false, false, false, false, false } }), 16));

                bl.AddTrainee(new Trainee("212384507", new Person.PersonName { FirstName = "Yael", LastName = "katri" }, new DateTime(1995, 10, 6), Gender.female, "0541234567", new Address { Street = "Franco", HouseNumber = 16, City = "Hadera" }, "1234", Vehicle.tractor, Gearbox.manual, "TheBest", new Person.PersonName { FirstName = "Shmuel", LastName = "Garber" }, 34, default));
                bl.AddTrainee(new Trainee("323947739", new Person.PersonName { FirstName = "Asaf", LastName = "Levi" }, new DateTime(1948, 10, 6), Gender.female, "0541234567", new Address { Street = "Hatmarim", HouseNumber = 16, City = "Eilat" }, "1234", Vehicle.heavyTruck, Gearbox.manual, "TheBest", new Person.PersonName { FirstName = "Shmuel", LastName = "Garber" }, 34, default));

                bl.AddTest(bl.GetTrainee("212384507"), new DateTime(2019, 1, 2, 8, 0, 0), /*new DateTime(2019, 1, 2),*/ new Address { Street = "Hganenet", HouseNumber = 7, City = "Jerusalem" }, Vehicle.tractor);
                bl.AddTest(bl.GetTrainee("323947739"), new DateTime(2019, 1, 2, 8, 0, 0), /*new DateTime(2019, 1, 2),*/ new Address { Street = "Hatmarim", HouseNumber = 18, City = "Eilat" }, Vehicle.heavyTruck);

                bl.UpdateTester(new Tester("323947747", new Person.PersonName { FirstName = "Samuel", LastName = "Garber" }, new DateTime(1950, 7, 13), Gender.male, "0566824871", new Address { Street = "Jorg", HouseNumber = 9, City = "Jerusalem" }, "1234", 10, 30, Vehicle.tractor, new Schedule(new bool[,] { { true, false, false, false, false, false, true, false }, { true, false, false, true, false, false, false, false }, { true, false, false, false, false, false, false, false }, { true, false, false, false, false, false, false, false }, { true, false, false, false, false, false, false, false } }), 20));
                bl.UpdateTrainee(new Trainee("212384507", new Person.PersonName { FirstName = "Yosepa", LastName = "katri" }, new DateTime(1995, 10, 6), Gender.female, "054124545", new Address { Street = "Franco", HouseNumber = 16, City = "KiriatShmona" }, "1234", Vehicle.tractor, Gearbox.manual, "TheBest", new Person.PersonName { FirstName = "Shmuel", LastName = "Garber" }, 34, default));

                foreach (var test in bl.GetSortedFutureTests())
                {
                    Console.WriteLine(test.ToString());
                }
                Console.WriteLine("--------------------------------------------------");

                //bl.UpdateTest("212384507", Vehicle.tractor, true, true, false, true, true, false, true, "Good!");
                //bl.UpdateTest("323947739", Vehicle.heavyTruck, false, false, false, true, true, false, false, "Not good!");

                Console.WriteLine(bl.NumberOfDoneTests(bl.GetTrainee("212384507")));
                Console.WriteLine("--------------------------------------------------");

                foreach (var test in bl.GetTests(t => t.IsPass ?? false))
                {
                    Console.WriteLine(test.ToString());
                }
                Console.WriteLine("--------------------------------------------------");

                foreach (var t in bl.GetTesters())
                {
                    Console.WriteLine(t.ToString());
                }
                Console.WriteLine("--------------------------------------------------");

                foreach (var t in bl.GetTrainees())
                {
                    Console.WriteLine(t.ToString());
                }
                Console.WriteLine("--------------------------------------------------");

                foreach (var t in bl.GetTests())
                {
                    Console.WriteLine(t.ToString());
                }
                Console.WriteLine("--------------------------------------------------");

                foreach (var item in bl.TestersByExpertise(true))
                {
                    Console.WriteLine(item.Key);
                    foreach (var item2 in item)
                    {
                        Console.WriteLine(item2);
                    }
                }
                Console.WriteLine("--------------------------------------------------");

                foreach (var item in bl.TraineesByDrivingSchool(true))
                {
                    Console.WriteLine(item.Key);
                    foreach (var item2 in item)
                    {
                        Console.WriteLine(item2);
                    }
                }
                Console.WriteLine("--------------------------------------------------");

                foreach (var item in bl.TraineesByNumberOfTests(true))
                {
                    Console.WriteLine(item.Key);
                    foreach (var item2 in item)
                    {
                        Console.WriteLine(item2);
                    }
                }
                Console.WriteLine("--------------------------------------------------");

                foreach (var item in bl.TraineesByTeacher(true))
                {
                    Console.WriteLine(item.Key);
                    foreach (var item2 in item)
                    {
                        Console.WriteLine(item2);
                    }
                }
                Console.WriteLine("--------------------------------------------------");

                bl.RemoveTester(bl.GetTester("323947747"));
                bl.RemoveTester(bl.GetTester("322680083"));

                bl.RemoveTrainee(bl.GetTrainee("212384507"));
                bl.RemoveTrainee(bl.GetTrainee("323947739"));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.Read();
            //DateTimeOffset
        }

        private static void MapReq(Address address1, Address address2)
        {

        }

        //private static void Worker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {

        //        while (Dist(addresses.Item1, addresses.Item2))
        //            Thread.Sleep(2000);
        //        e.Result = true;
        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //    void Dist(Address address1, Address address2)
        //    {


        //    }
        //}

        //private static void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{

        //}


    }
}
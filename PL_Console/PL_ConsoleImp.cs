//Bs"d

using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public static BL.IBL bl = Singleton<BL.IBL, BL.Bl_imp>.Instance;



        static void Main(string[] args)
        {
            try
            {
                MapReq();




                Console.WriteLine("FrEf");
                //WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                ReadKey();
                ForegroundColor = ConsoleColor.Green;

                bl.AddTester(new Tester("323947747", new Name { FirstName = "Shmuel", LastName = "Garber" }, new DateTime(1950, 7, 13), Gender.male, "0547424870", new Address { Street = "Hganenet", HouseNumber = 5, City = "Jerusalem" }, 10, 30, Vehicle.tractor, new bool[,] { { true, false, false, false, false, false, true, false }, { true, false, false, true, false, false, false, false }, { true, false, false, false, false, false, false, false }, { true, false, false, false, false, false, false, false }, { true, false, false, false, false, false, false, false } }, 20));
                bl.AddTester(new Tester("322680083", new Name { FirstName = "Refael", LastName = "Goldis" }, new DateTime(1949, 5, 12), Gender.male, "0556824870", new Address { Street = "Gordon", HouseNumber = 22, City = "Tel-Aviv" }, 6, 16, Vehicle.heavyTruck, new bool[,] { { true, true, false, true, false, false, false, true }, { true, false, false, true, false, true, false, false }, { true, false, false, false, false, false, false, true }, { true, false, false, true, false, false, false, false }, { true, false, false, false, false, false, false, false } }, 16));

                bl.AddTrainee(new Trainee("212384507", new Name { FirstName = "Yael", LastName = "katri" }, new DateTime(1995, 10, 6), Gender.female, "0541234567", new Address { Street = "Franco", HouseNumber = 16, City = "Hadera" }, Vehicle.tractor, Gearbox.manual, "TheBest", new Name { FirstName = "Shmuel", LastName = "Garber" }, 34));
                bl.AddTrainee(new Trainee("323947739", new Name { FirstName = "Asaf", LastName = "Levi" }, new DateTime(1948, 10, 6), Gender.female, "0541234567", new Address { Street = "Hatmarim", HouseNumber = 16, City = "Eilat" }, Vehicle.heavyTruck, Gearbox.manual, "TheBest", new Name { FirstName = "Shmuel", LastName = "Garber" }, 34));

                bl.AddTest(bl.GetTrainee("212384507"), new DateTime(2019, 1, 2, 8, 0, 0), /*new DateTime(2019, 1, 2),*/ new Address { Street = "Hganenet", HouseNumber = 7, City = "Jerusalem" }, Vehicle.tractor);
                bl.AddTest(bl.GetTrainee("323947739"), new DateTime(2019, 1, 2, 8, 0, 0), /*new DateTime(2019, 1, 2),*/ new Address { Street = "Hatmarim", HouseNumber = 18, City = "Eilat" }, Vehicle.heavyTruck);

                bl.UpdateTester(new Tester("323947747", new Name { FirstName = "Samuel", LastName = "Garber" }, new DateTime(1950, 7, 13), Gender.male, "0566824871", new Address { Street = "Jorg", HouseNumber = 9, City = "Jerusalem" }, 10, 30, Vehicle.tractor, new bool[,] { { true, false, false, false, false, false, true, false }, { true, false, false, true, false, false, false, false }, { true, false, false, false, false, false, false, false }, { true, false, false, false, false, false, false, false }, { true, false, false, false, false, false, false, false } }, 20));
                bl.UpdateTrainee(new Trainee("212384507", new Name { FirstName = "Yosepa", LastName = "katri" }, new DateTime(1995, 10, 6), Gender.female, "054124545", new Address { Street = "Franco", HouseNumber = 16, City = "KiriatShmona" }, Vehicle.tractor, Gearbox.manual, "TheBest", new Name { FirstName = "Shmuel", LastName = "Garber" }, 34));

                foreach (var test in bl.SortedFutureTests())
                {
                    Console.WriteLine(test.ToString());
                }
                Console.WriteLine("--------------------------------------------------");

                //bl.UpdateTest("212384507", Vehicle.tractor, true, true, false, true, true, false, true, "Good!");
                //bl.UpdateTest("323947739", Vehicle.heavyTruck, false, false, false, true, true, false, false, "Not good!");

                Console.WriteLine(bl.NumberOfDoneTests(bl.GetTrainee("212384507")));
                Console.WriteLine("--------------------------------------------------");

                foreach (var test in bl.FindAllInTests(t => t.IsPass ?? false))
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

        private static void MapReq()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;

            for (int i = 0; i < 100; i++)
            {
                Thread thread = new Thread(Dist);
                thread.Start();
                Thread.Sleep(2000);
            }
        }

        private protected static void Dist()
        {
            //string origin = "pisga 45 st. jerusalem"; //or " אחד העם  פתח תקווה 100 " etc.
            //string destination = "gilgal 78 st. ramat-gan";//or " ז'בוטינסקי  רמת גן 10 " etc.
            string origin = "אהרוני 10 ירושלים"; //or " אחד העם  פתח תקווה 100 " etc.
            string destination = "אלעזר המודעי 5 ירושלים";//or " ז'בוטינסקי  רמת גן 10 " etc.
            string KEY = @"PffGghfFGtzNFx1MqL9NLykkFHmpHkmc";

            string url = @"https://www.mapquestapi.com/directions/v2/route" +
                @"?key=" + KEY +
                @"&from=" + origin +
                @"&to=" + destination +
                @"&outFormat=xml" +
                @"&ambiguities=ignore&routeType=fastest&doReverseGeocode=false" +
                @"&enhancedNarrative=false&avoidTimedConditions=false";

            //request from MapQuest service the distance between the 2 addresses
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader sreader = new StreamReader(dataStream);
            string responsereader = sreader.ReadToEnd();
            response.Close();
            //the response is given in an XML format 
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(responsereader);

            if (xmldoc.GetElementsByTagName("statusCode")[0].ChildNodes[0].InnerText == "0") //we have the expected answer
            {     //display the returned distance
                XmlNodeList distance = xmldoc.GetElementsByTagName("distance");
                double distInMiles = Convert.ToDouble(distance[0].ChildNodes[0].InnerText);
                Console.WriteLine("Distance In KM: " + distInMiles * 1.609344);

                //display the returned driving time   
                XmlNodeList formattedTime = xmldoc.GetElementsByTagName("formattedTime");
                string fTime = formattedTime[0].ChildNodes[0].InnerText;
                Console.WriteLine("Driving Time: " + fTime);
            }
            else if (xmldoc.GetElementsByTagName("statusCode")[0].ChildNodes[0].InnerText == "402")
            //we have an answer that an error occurred, one of the addresses is not found 
            {
                Console.WriteLine("an error occurred, one of the addresses is not found. try again.");
            }
            else //busy network or other error... 
            {
                Console.WriteLine("We have'nt got an answer, maybe the net is busy...");
            }

        }

        private static void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
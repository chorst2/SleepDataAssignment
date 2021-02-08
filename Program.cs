using System;
using System.IO;
using NLog.Web;

namespace SleepData
{
    class Program
    {
        static void Main(string[] args)
        {
            // create instance of Logger
            string path = Directory.GetCurrentDirectory() + "\\nlog.config";
            var logger = NLog.Web.NLogBuilder.ConfigureNLog(path).GetCurrentClassLogger();

            logger.Info("Program started");

            var file = "data.txt";
            // ask for input
            Console.WriteLine("Enter 1 to create data file.");
            Console.WriteLine("Enter 2 to parse data.");
            Console.WriteLine("Enter anything else to quit.");
            // input response
            string resp = Console.ReadLine();

            if (resp == "1")
            {
                // create data file

                // ask a question
                Console.WriteLine("How many weeks of data is needed?");
                // input the response (convert to int)
                string ans = Console.ReadLine();
                int weeks;
                if (!int.TryParse(ans, out weeks))
                {
                    logger.Error("Invalid input (integer): {Answer}", ans);
                }
                else
                {
                    // determine start and end date
                    DateTime today = DateTime.Now;
                    // we want full weeks sunday - saturday
                    DateTime dataEndDate = today.AddDays(-(int)today.DayOfWeek);
                    // subtract # of weeks from endDate to get startDate
                    DateTime dataDate = dataEndDate.AddDays(-(weeks * 7));
                    
                    // random number generator
                    Random rnd = new Random();

                    // create file
                    StreamWriter sw = new StreamWriter(file);
                    // loop for the desired # of weeks
                    while (dataDate < dataEndDate)
                    {
                        // 7 days in a week
                        int[] hours = new int[7];
                        for (int i = 0; i < hours.Length; i++)
                        {
                            // generate random number of hours slept between 4-12 (inclusive)
                            hours[i] = rnd.Next(4, 13);
                        }
                        // M/d/yyyy,#|#|#|#|#|#|#
                        //Console.WriteLine($"{dataDate:M/d/yy},{string.Join("|", hours)}");
                        sw.WriteLine($"{dataDate:M/d/yyyy},{string.Join("|", hours)}");
                        // add 1 week to date
                        dataDate = dataDate.AddDays(7);
                    }
                    sw.Close();
                }
            }
            else if (resp == "2")
            {
                // TODO: parse data file
                // read data from file
                    if (File.Exists(file))
                    {
                        // read data from file
                        StreamReader sr = new StreamReader(file);
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            // convert string to array
                            string[] arr = line.Split(',', '|');            
                            var parsedDate = DateTime.Parse(arr[0]);          

                            //add up arrays [ 1 - 7]
                            int parsedSleepOne = Int32.Parse(arr[1]);
                            int parsedSleepTwo = Int32.Parse(arr[2]);
                            int parsedSleepThree = Int32.Parse(arr[3]);
                            int parsedSleepFour = Int32.Parse(arr[4]);
                            int parsedSleepFive = Int32.Parse(arr[5]);
                            int parsedSleepSix = Int32.Parse(arr[6]);
                            int parsedSleepSeven = Int32.Parse(arr[7]);
                            int totalWeekSleep = parsedSleepOne + parsedSleepTwo + parsedSleepThree + parsedSleepFour + parsedSleepFive + parsedSleepSix + parsedSleepSeven;
                            //average of total
                            double avgWeekSleep = totalWeekSleep / 7.0;    
                    
                            //output the table
                            Console.WriteLine($"Week ending on {parsedDate:MMM}, {parsedDate:dd}, {parsedDate:yyyy}");
                            Console.WriteLine($"{"Mo",5}{"Tu",5}{"We",5}{"Th",5}{"Fr",5}{"Sa",5}{"Su",5}{"Tot",5}{"Avg",5}");
                            Console.WriteLine($"{"--",5}{"--",5}{"--",5}{"--",5}{"--",5}{"--",5}{"--",5}{"---",5}{"---",5}");
                            Console.WriteLine($"{arr[1],5}{arr[2],5}{arr[3],5}{arr[4],5}{arr[5],5}{arr[6],5}{arr[7],5}{totalWeekSleep,5}{avgWeekSleep,5:n1}");
                        }
                        sr.Close();
                    }
                    else
                    {
                        logger.Warn("File does not exists. {file}", file);
                    }

            }
            logger.Info("Program ended");
        }
    }
}

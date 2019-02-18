using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Multithreading.ConsoleUI.Producer_Consumer
{

    /// <summary>
    /// There are a few producer tasks which generate data(e. g.  Fill a collection)  that is "consumed"  by multiple consumer threads
    /// which stop when the producer tasks completed & their job is done. 
    /// 
    /// Usecase: This is a good pattern to use for long-running workloads where data generation speed is very different from data consumption speed
    /// The general pattern allows for one or more producers connected to one or more consumers by way of a shared data structure
    /// The data structure throttles faster component and keeps the system better balanced.
    /// Here the Blocking collection is used, where it is blocking producers if collection is full and blocking consumers if collection is empty.
    /// </summary>
    public class ProducerConsumerPattern
    {

        public void Start()
        {
            int maxCapacity = 2;
            var workQueue = new BlockingCollection<string>(maxCapacity);

            var taskFactory = new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.None);

            Task producer = taskFactory.StartNew(() =>
            {
                //blocks the task if queue is full
                foreach (string title in GetTitles(2))
                {
                    workQueue.Add(title);

                }

                //Signal the work is done
                workQueue.CompleteAdding();
            });

            Task consumer = taskFactory.StartNew(() =>
            {
                while (!workQueue.IsCompleted)
                {
                    try
                    {
                        Console.WriteLine("Next item");
                        string work = workQueue.Take();
                        //process work item
                        //add in a database
                        Console.WriteLine(work);
                    }
                    catch (ObjectDisposedException)
                    { //ignore
                    }
                    catch (InvalidOperationException)
                    {
                        //ignore
                    }
                }
            });
        }

        private IEnumerable<string> GetTitles(int pageMaxCount)
        {
            string url = "https://www.ebay.com/sch/i.html?_nkw=laptop&_pgn={0}";
            int pageCount = 1;
            string titleXPath = "//h3[@class='s-item__title']";
            using (var driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)))
            {
                while (pageCount <= pageMaxCount)
                {
                    driver.Navigate().GoToUrl(string.Format(url, pageCount++));
                    var elements = driver.FindElementsByXPath(titleXPath);
                    //elements.ToList().ForEach(n => Console.WriteLine(n.Text));
                    foreach (var title in elements.Select(n => n.Text))
                    {
                        yield return title;
                    }
                }
            }
        }
    }
}

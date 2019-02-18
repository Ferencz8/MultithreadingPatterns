using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Multithreading.ConsoleUI.Pipeline
{
    /// <summary>
    /// The Pipeline pattern is a linear flow from one task to the other
    /// Used for image processing, UI uploading, workflows
    /// 
    /// Usecase: linear flow of data from one task to another
    /// </summary>
    public class PipelinePattern
    {

        public void Start()
        {
            int pageMaxCount = 20;
            string url = "https://www.ebay.com/sch/i.html?_nkw=laptop&_pgn={0}";
            string titleXPath = "//h3[@class='s-item__title']";
            int pageCount = 1;
            var coresNumber = System.Environment.ProcessorCount;

            Task<IEnumerable<string>> task = Task.Factory.StartNew<IEnumerable<string>>(() =>
            {
                List<string> titles = new List<string>();
                using (var driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)))
                {
                    driver.Navigate().GoToUrl(string.Format(url, pageCount++));
                    var elements = driver.FindElementsByXPath(titleXPath);
                    titles.AddRange(elements.Select(n => n.Text));
                }
                return titles;
            });

            task.ContinueWith((antecedent) =>
            {
                antecedent.Result.ToList().ForEach(n => Console.WriteLine(n));
            });
        }
    }
}

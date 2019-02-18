using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Multithreading.ConsoleUI.APM
{
    /// <summary>
    /// Asynchronous programming model
    /// The pattern provides 2 methods. A "Begin" to start the operation and an "End" to complete the operation and harvest the result. 
    /// The good news is that various. NET classes already support the APM pattern including FileStream and HTTPWebRequest. The advantage 
    /// to use in this pattern and in particular these. NET classes is that the begin method starts the operation on a thread but then
    /// returns that thread to the worker pool until the operation completes. This allows that thread to be used for other work during 
    /// the I/O operation. To take advantage of classes that implement this pattern, the task parallel library provides first class support
    /// by way of facade tasks. In particular, you use the TPL's FromAsync method to create a task that wraps the calls to Begin and End, 
    /// and then simply use the standard task mechanisms to wait, continue with, or harvest the result.
    /// 
    /// Usecase: APM is commonly used for asynchronous operations such as file and network I/O.
    /// </summary>
    /// https://social.msdn.microsoft.com/Forums/vstudio/en-US/95355648-1fa6-4b2d-a260-954c3421c453/how-to-create-async-method-which-encapsulates-webrequest-begingetrequeststream-begingetresponse?forum=parallelextensions
    public class AsynchronousPattern
    {

        public void Start()
        {

            string url = "http://www.ebay.com/sch/i.html?_nkw=laptop&_pgn={0}";
            int index = 1;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(url, index++));

            var task = Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, TaskCreationOptions.None);

            task.ContinueWith((antecedent) =>
            {
                using( var webResponse = (HttpWebResponse)antecedent.Result)
                using (var responseStream = webResponse.GetResponseStream())
                {
                    using (var reader = new StreamReader(responseStream, UTF8Encoding.ASCII))
                    {
                        string responseText = reader.ReadToEnd();
                        Console.WriteLine(responseText);

                        return responseText;
                    }
                }
            });
            task.Wait();
            var result = task.Result;
        }
    }
}

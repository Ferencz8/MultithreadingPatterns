using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Multithreading.ConsoleUI.Pipeline;
using Multithreading.ConsoleUI.Producer_Consumer;
using Multithreading.ConsoleUI.APM;
using Multithreading.ConsoleUI.Speculative_Execution;

namespace Multithreading.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            //new PipelinePattern().Start();

            //new ProducerConsumerPattern().Start();

            //new AsynchronousPattern().Start();

            new SpeculativeExecutionPattern().Start();

            Console.ReadLine();
        }
    }
}

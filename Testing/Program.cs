using System;
using WebDriverManager;

namespace Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            ChromeDriverManager m = new ChromeDriverManager();
            string RunningPath = AppDomain.CurrentDomain.BaseDirectory;
            string driver_vers = m.download_and_install(RunningPath);

            Console.WriteLine(driver_vers);

            

            System.Threading.Thread.Sleep(5000);
        }
    }
}

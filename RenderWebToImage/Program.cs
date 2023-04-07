using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Drawing.Imaging;
using System.IO;

namespace RenderWebToImage
{
    class Program
    {

        static string [] usage = { "usage:", "RenderWebToImage webpageurl imagefilename.png" };

        static void Main(string[] args)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.White;
                if (args.Length < 2) throw new Exception("No arguments passed.");
                var url = args[0];
                var filename = args[1];
                CovertWebpageToFile(url, filename);

                if (File.Exists(filename))
                    Console.WriteLine($"{filename} created using {url}.\r\n");
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{filename} was not created using {url}.\r\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            } 
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error, {ex.Message}\r\n{ex.StackTrace}");
                Console.ForegroundColor = ConsoleColor.White;

                foreach(var s in usage) // print usage
                    Console.WriteLine($"{s}");

            }
        }

        private static void CovertWebpageToFile(string Url, string fileName)
        {
            try
            {
                Console.WriteLine($"Processing {Url}.\r\n");
                // Set up the ChromeDriver
                var chromeOptions = new ChromeOptions();
                chromeOptions.AddArgument("headless"); // Run Chrome in headless mode
                chromeOptions.AddArgument("disable-gpu"); // Disable the GPU to improve performance
                var driver = new ChromeDriver(chromeOptions);

                // Navigate to the web page
                driver.Navigate().GoToUrl(Url);

                // Wait for the page to load
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(_driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));

                // Take a screenshot of the entire page
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();

                // Save the screenshot to a file
                screenshot.SaveAsFile(fileName, ScreenshotImageFormat.Png);
                Console.WriteLine($"Saved {fileName}.\r\n");

                // Quit the driver
                driver.Quit();
            } 
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error, {ex.Message}\r\n");
            }
        }
    }
}




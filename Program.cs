using System;
using System.Diagnostics;
using System.Threading;


namespace rpi_monitor
{
    class Program
    {

        internal static float GetTemperature()
        {
            var result = "";

            // bash command / opt / vc / bin / vcgencmd measure_temp
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"/usr/bin/vcgencmd measure_temp\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();


            var temperatureResult = result.Substring(result.IndexOf('=') + 1, result.IndexOf("'") - (result.IndexOf('=') + 1)).Replace('.', ',');
            var temperature = 0.0f;
            if (float.TryParse(temperatureResult, out temperature))
                return temperature;
            else
                return 0.0f;
        }
        static void Main(string[] args)
        {
            while(1)
            {
                Console.WriteLine("Temprature:" + GetTemperature());
                Thread.Sleep(5000);
            }
        }
    }
}

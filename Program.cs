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

        internal static string GetThrottledState()
        {
            var result = "";
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"/opt/vc/bin/vcgencmd get_throttled\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            result = result.Trim();
            var resultCode = result[result.Length - 1];

            return resultCode.ToString();
        }

        internal static string[] GetProcessAverage()
        {

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/usr/bin/uptime",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            var processResult = process.StandardOutput.ReadToEnd().Trim();
            process.WaitForExit();

            // 13:43:40 up 21 min,  2 users,  load average: 0.12, 0.07, 0.01
            var loadAverages = processResult.Substring(processResult.IndexOf("average: ") + 9).Split(',');

            return loadAverages;
        }
        static void Main(string[] args)
        {
            while(true)
            {
                Console.WriteLine("Temprature:" + GetTemperature() + " ThrottleState:" + GetThrottledState());
                Console.WriteLine("[{0}]", string.Join(", ", GetProcessAverage()));
                Thread.Sleep(5000);
            }
        }
    }
}

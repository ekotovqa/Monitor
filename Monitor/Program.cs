using System.Diagnostics;
using System.Text;

namespace Monitor
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Press Q to exit");

            if (args.Length == 3)
            {
                try
                {
                    Console.WriteLine($"{args[0]} {args[1]} {args[2]}");
                    KillerAsync(args[0], int.Parse(args[1]), int.Parse(args[2]));
                }
                catch (FormatException)
                {
                    Console.WriteLine("Формат параметров задан не верно");
                }
            }
            else
            {
                Console.WriteLine("Указано неверное количество параметров");
            }

            while (Console.ReadKey(true).Key != ConsoleKey.Q) { };
        }

        static async void KillerAsync(string processName, int lifeTime, int checkInterval)
        {
            await Task.Run(() =>
            {
                using (var sw = new StreamWriter("log.txt", true, Encoding.UTF8))
                {
                    sw.WriteLine($"\n{DateTime.Now} Запуск {processName} {lifeTime} {checkInterval}");
                }
                while (true)
                {
                    foreach (var process in Process.GetProcesses())
                    {
                        if (process.ProcessName == processName && DateTime.Now > process.StartTime.AddMinutes(lifeTime))
                        {
                            using (var sw = new StreamWriter("log.txt", true, Encoding.UTF8))
                            {
                                sw.WriteLine($"{DateTime.Now} Завершен процесс {process.Id}: {process.ProcessName} - время жизни {DateTime.Now - process.StartTime}");
                            }
                            process.Kill();
                        }
                    }
                    Thread.Sleep(checkInterval * 60000);
                }
            });
        }
    }
}
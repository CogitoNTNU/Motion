


using Avalonia;

namespace Motion
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //BuildAvaloniaApp().SetupWithoutStarting();
            GeneticAlgorithm ga = new GeneticAlgorithm();
            ga.Main();
        }
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                         .UsePlatformDetect()
                         .LogToTrace();
    }
}
namespace RecipeBook
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            // Catch unhandled exceptions
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                var exception = (Exception)e.ExceptionObject;
                Console.WriteLine("Unhandled exception: " + exception.Message);
                // Log exception or handle it as needed
            };

            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                Console.WriteLine("Unobserved task exception: " + e.Exception.Message);
                e.SetObserved();
            };
        }
    }
}

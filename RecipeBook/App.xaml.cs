using RecipeBook.Themes;
using RecipeBook.Resources.Styles;

using static Microsoft.Maui.Controls.Device;

namespace RecipeBook
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

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

            string savedTheme = Preferences.Get("theme", "Dark");
            SetTheme(savedTheme);
        }

        public static void SetServiceProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider; // Store the service provider for global access
        }


        public void SetTheme(string theme)
        {
            // Clear the existing theme-specific resource dictionaries
            Resources.MergedDictionaries.Clear();

            // Apply the base Colors and Styles
            Resources.MergedDictionaries.Add(new RecipeBook.Resources.Styles.Colors());
            Resources.MergedDictionaries.Add(new RecipeBook.Resources.Styles.Styles());

            // Apply theme-specific resources
            if (theme == "Dark")
            {
                Resources.MergedDictionaries.Add(new DarkTheme());
            }
            else if (theme == "Sky Blue")
            {
                Resources.MergedDictionaries.Add(new SkyBlueTheme());
            }
            else if (theme == "Purple")
            {
                Resources.MergedDictionaries.Add(new PurpleTheme());
            }
            else if (theme == "Red + Black")
            {
                Resources.MergedDictionaries.Add(new RedBlackTheme());
            }
            else if (theme == "Halloween")
            {
                Resources.MergedDictionaries.Add(new HalloweenTheme());
            }
            else
            {
                Resources.MergedDictionaries.Add(new LightTheme());
            }

            // Save the selected theme preference
            Preferences.Set("theme", theme);

            Application.Current.MainPage?.ForceLayout();
        }
    }
}

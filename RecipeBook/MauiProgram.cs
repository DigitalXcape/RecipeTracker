using Microsoft.Extensions.Logging;
using RecipeBook.Recipes;
using RecipeBook.Services;

namespace RecipeBook
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
        builder.Logging.AddDebug();
#endif
            builder.Services.AddHttpClient<RecipeService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7148");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            });

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<EditRecipePage>();
            builder.Services.AddSingleton<FavoritesPage>();
            builder.Services.AddSingleton<RecipePage>();
            builder.Services.AddTransient<ViewRecipePage>();
            builder.Services.AddSingleton<RecipeList>();

            // Build the app and set the service provider
            var mauiApp = builder.Build();
            App.SetServiceProvider(builder.Services.BuildServiceProvider()); // Store the service provider

            return mauiApp;
        }
    }
}

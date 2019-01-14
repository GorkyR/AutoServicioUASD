using System.Windows;
using static Client.WPF.StateService;

namespace Client.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public const string DataBaseName = "ClientState.bin";

        public App()
        {
            using (var db = new LiteDB.LiteDatabase(DataBaseName))
            {
                var globals = db.GetCollection<Models.Global>();
                globals.EnsureIndex("key");

                void setupGlobal(string key, object value)
                {
                    if (!globals.Exists(g => g.Key == key))
                        globals.Insert(new Models.Global(key, value));
                }

                setupGlobal(nameof(IsLoggedIn), false);
                setupGlobal(nameof(LastIDUsed), "");
                setupGlobal(nameof(CurrentSession), null);
            }
        }
    }
}

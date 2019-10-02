using LiteDB;
using Client.WPF.Models;

namespace Client.WPF
{
    public static class StatePersistanceService
    {
        private static object GetGlobal(string key) {
            using (var db = new LiteDatabase(App.DataBaseName))
            {
                var globals = db.GetCollection<Global>();
                return globals.FindOne(g => g.Key == key).Value;
            }
        }
        private static void UpdateGlobal(string key, object value)
        {
            using (var db = new LiteDatabase(App.DataBaseName))
            {
                var globals = db.GetCollection<Global>();
                var global = globals.FindOne(g => g.Key == key);
                global.Value = value;
                globals.Update(global);
            }
        }

        public static void ResetSession()
        {
            using (var db = new LiteDatabase(App.DataBaseName)) {
                var globals = db.GetCollection<Global>();
                globals.Delete(g => g.Key == nameof(CurrentSession));
                globals.Insert(new Global(nameof(CurrentSession), null));
            };
        }

        public static bool IsLoggedIn {
            get => (bool)GetGlobal(nameof(IsLoggedIn));
            set => UpdateGlobal(nameof(IsLoggedIn), value);
        }
        public static string LastIDUsed {
            get => (string)GetGlobal(nameof(LastIDUsed));
            set => UpdateGlobal(nameof(LastIDUsed), value);
        }
        public static SessionInformation CurrentSession {
            get => (SessionInformation)GetGlobal(nameof(CurrentSession));
            set => UpdateGlobal(nameof(CurrentSession), value);
        }
    }
}

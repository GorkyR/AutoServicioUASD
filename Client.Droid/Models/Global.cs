namespace Client.Droid.Models
{
    public class Global
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public object Value { get; set; }

        public Global() { }
        public Global(string key, object value)
        {
            Key = key;
            Value = value;
        }
    }
}

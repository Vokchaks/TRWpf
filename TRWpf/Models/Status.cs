namespace TRWpf.Models
{
    public static class Status
    {
        private static string _text;

        public static string Text
        {
            get { return _text; }
            set
            {
                _text = value;               
            }
        }
    }
}

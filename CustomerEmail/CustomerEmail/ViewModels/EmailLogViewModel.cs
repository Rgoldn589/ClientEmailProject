using CustomerEmail.Helper;
using CustomerEmail.Models;
using CustomerEmail.Views;
using System.Configuration;
using System.IO;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace CustomerEmail.ViewModels
{
    public class EmailLogViewModel
    {

        public EmailLogView Window { get; set; }

        public List<EmailLog> Log { get; set; } = new List<EmailLog>();

        private RelayCommand _close;
        public RelayCommand Close
        {
            get
            {
                if (_close == null)
                {
                    _close = new RelayCommand(CloseForm, true);
                }

                return _close;
            }
        }

        public EmailLogViewModel()
        {
            GetData();
        }

        public void GetData()
        {
            var path = Path.Combine(ConfigurationManager.AppSettings["LoggingPath"],
                    ConfigurationManager.AppSettings["EmailLogFile"]);


            if (!File.Exists(path))
            {
                return;
            }

            var log = File.ReadAllLines(path);

            foreach (var logItem in log)
            {
                string[] line = logItem.Split(",");

                Log.Add(new EmailLog()
                {
                    Email = line[0],
                    Subject = line[1],
                    Message = line[2],
                    LogDate = DateTime.Parse(line[3]),
                    Result = line[4]          
                });
            }
        }

        private void CloseForm()
        {
            Window.Close();
        }
    }
}

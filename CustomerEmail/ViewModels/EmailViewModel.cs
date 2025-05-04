using CustomerEmail.Helper;
using EmailSend.Models;
using EmailSend;
using System.Configuration;
using CustomerEmail.Views;
using System.Windows;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace CustomerEmail.ViewModels
{
    public class EmailViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged = null;
        public EmailView Window { get; set; }
        public EmailValues EmailValues { get; set; } = new EmailValues();

        public string errorMessage = string.Empty;
        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }

            set
            {
                SetField(ref errorMessage, value, "errorMessage");
            }
        }

        private Visibility isError = Visibility.Hidden;
        public Visibility IsError
        {
            get
            {
                return isError;
            }

            set
            {
                SetField(ref isError, value, "isError");
            }
        }

        private RelayCommand _send;
        public RelayCommand Send
        {
            get
            {
                if (_send == null)
                {
                    _send = new RelayCommand(SendEmail, true);
                }

                return _send;
            }
        }

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

        private void SendEmail()
        {

            if (!Validate())
            {
                return;
            }

            var newEmailTrans = new SendEmailTransimission(
                    new EmailConfiguration()
                    {
                        EmailHost = ConfigurationManager.AppSettings["EmailHost"],
                        EmailPort = Int32.Parse(ConfigurationManager.AppSettings["EmailPort"]),
                        ClientName = ConfigurationManager.AppSettings["ClientName"],
                        ClientEmailAddress = ConfigurationManager.AppSettings["ClientEmailAddress"],
                        EmailPassword = ConfigurationManager.AppSettings["EmailPassword"],
                        LoggingPath = ConfigurationManager.AppSettings["LoggingPath"],
                        EmailLogFile = ConfigurationManager.AppSettings["EmailLogFile"]
                    });

            Task.Run(() => newEmailTrans.SendEmailAsync(EmailValues));
            MessageBox.Show("Email Sent.", "Success");
        }

        private void CloseForm()
        {
            Window.Close();
        }

        private bool Validate()
        {
            IsError = Visibility.Hidden;
            ErrorMessage = string.Empty;
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            if (EmailValues.EmailAddress == null)
            {
                ErrorMessage = "Email Address needs to be entered.\r";
            }
            else
            {
                Match match = regex.Match(EmailValues.EmailAddress);

                if (!match.Success)
                {
                    ErrorMessage = "Email Address not properly formatted.\r";
                }
            }

            if (EmailValues.Subject == null)
            {
                ErrorMessage += "Subject needs to be entered.\r";
            }

            if (EmailValues.Message == null)
            {
                ErrorMessage += "Message needs to be entered.";
            }

            if (ErrorMessage != "")
            {
                IsError = Visibility.Visible;
                return false;
            }

            return true;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }


    }
}

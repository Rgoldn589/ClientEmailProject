using CustomerEmail.Helper;
using CustomerEmail.Views;

namespace CustomerEmail.ViewModels
{

    public class MainWindowViewModel
    {

        public MainWindow Window {  get; set; }

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

        private RelayCommand _viewLog;
        public RelayCommand ViewLog
        {
            get
            {
                if (_viewLog == null)
                {
                    _viewLog = new RelayCommand(OpenViewLog, true);
                }

                return _viewLog;
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
            EmailView view = new EmailView();
            view.ShowDialog();
        }

        private void OpenViewLog()
        {
            EmailLogView view = new EmailLogView();
            view.ShowDialog();
        }

        private void CloseForm()
        {
            Window.Close();
        }
    }
}

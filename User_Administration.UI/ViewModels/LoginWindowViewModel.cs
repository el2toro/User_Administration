using Prism.Commands;
using System.ComponentModel;
using System.Windows;
using User_Administration.Model;
using System.Windows.Input;
using PropertyChanged;

namespace User_Administration.UI.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class LoginWindowViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }

        private ICommand checkUserCommand;

        public LoginWindowViewModel()
        {
            CheckUserCommand = new DelegateCommand(Execute, CanExecute);
        }
 

        public ICommand CheckUserCommand
        {
            get { return checkUserCommand; }
            set
            {
                if (checkUserCommand == value)
                {
                    return;
                }

                checkUserCommand = value;
            }
        }

        UserCollection userCollection = new UserCollection();

        public bool CheckUser(string username, string password)
        {
            int count = userCollection.CountUsers(username, password);

            return count > 0;
        }


        void Execute()
        {
            bool exist = CheckUser(Username, Password);
            if (exist == true)
            {
                MainWindowViewModel.ShowMainWindowDialog();
            }
            else
            {
                MessageBox.Show("Login failed. Please try again.");
            }

        }

        bool CanExecute()
        {
            return true;
        }

        
    }
}

using System;
using System.Windows.Input;
using PropertyChanged;
using User_Administration.Model;



namespace User_Administration.UI.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class NewEditViewModel
    {
        private User currentUser;
        private string windowTitle;

        public User CurrentUser
        {
            get { return currentUser; }
            set
            {
                if (currentUser == value)
                {
                    return;
                }
                currentUser = value;
            }
        }

        public string WindowTitle
        {
            get { return windowTitle; }
            set
            {
                if (windowTitle == value)
                {
                    return;
                }
                windowTitle = value;
            }
        }

        public NewEditViewModel(User user)
        {


            SaveCommand = new RelayCommand(SaveExecute, CanSave);

            CurrentUser = user;
            WindowTitle = "Edit User";
        }

        public NewEditViewModel()
        {


            SaveCommand = new RelayCommand(SaveExecute, CanSave);

            CurrentUser = new User();
            WindowTitle = "New User";
        }

        private ICommand saveCommand;

        public ICommand SaveCommand
        {
            get { return saveCommand; }
            set
            {
                if (saveCommand == value)
                {
                    return;
                }
                saveCommand = value;

            }
        }

        void SaveExecute(object obj)
        {
            if (CurrentUser != null && !CurrentUser.HasErrors)
            {
                CurrentUser.Save();
                OnDone(new DoneEventArgs("User was saved succesfully."));
            }
            else
            {
                OnDone(new DoneEventArgs("Check user input."));
            }
        }

        bool CanSave(object obj)
        {
            return true;
        }

        public delegate void DoneEventHandler(object sender, DoneEventArgs e);

        public class DoneEventArgs : EventArgs
        {
            private string message;

            public string Message
            {

                get { return message; }
                set
                {
                    if (message == value)
                    {
                        return;
                    }
                    message = value;
                }

            }

            public DoneEventArgs(string message)
            {
                this.message = message;
            }

        }

        public event DoneEventHandler Done;

        public void OnDone(DoneEventArgs e)
        {
            if (Done != null)
            {
                Done(this, e);
            }
        }
    }
}

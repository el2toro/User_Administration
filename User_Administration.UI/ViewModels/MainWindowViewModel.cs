using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using User_Administration.Model;

namespace User_Administration.UI.ViewModels
{
    public class MainWindowViewModel 
    {
        private User currentUser;
        private List<User> userList;
        private ListCollectionView listCollectionView;
        private string filteringText;

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

        public List<User> UserList
        {
            get { return userList; }
            set
            {
                if (userList == value)
                {
                    return;
                }

                userList = value;
            }
        }

        public ListCollectionView UserListView
        {
            get { return listCollectionView; }
            set
            {
                if (listCollectionView == value)
                {
                    return;
                }

                listCollectionView = value;
            }
        }

        public String FilteringText
        {
            get { return filteringText; }
            set
            {
                if (filteringText == value)
                {
                    return;
                }

                filteringText = value;
            }
        }

        public MainWindowViewModel()
        {

            UserList = User.GetAllUsers();

            UserListView = new ListCollectionView(UserList);
            UserListView.Filter = UserFilter;

            CurrentUser = new User();

        }


        private void MainWindowViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("FilteringText"))
            {
                UserListView.Refresh();
            }
        }

        private bool UserFilter(object obj)
        {
            if (FilteringText == null) return true;
            if (FilteringText.Equals("")) return true;

            User user = obj as User;

            return (user.UserName.ToLower().StartsWith(FilteringText.ToLower()));

        }

        public static void ShowMainWindowDialog()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.ShowDialog();
        }

        public static void NewUserDialog()
        {
            NewEditWindow newWindow = new NewEditWindow();
            newWindow.DataContext = new NewEditViewModel();
            newWindow.ShowDialog();
        }


    }
}

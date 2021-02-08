using System.Windows;
using User_Administration.UI.ViewModels;



namespace User_Administration.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
          
        }


        private void newBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel.NewUserDialog();
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel viewModel = (MainWindowViewModel)DataContext;
            NewEditWindow editWindow = new NewEditWindow();
            editWindow.DataContext = new NewEditViewModel(viewModel.CurrentUser);
            editWindow.ShowDialog();
        }
     

    }
}

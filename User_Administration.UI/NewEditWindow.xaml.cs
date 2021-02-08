using System.Windows;
using User_Administration.UI.ViewModels;


namespace User_Administration.UI
{
    /// <summary>
    /// Interaction logic for NewEditWindow.xaml
    /// </summary>
    public partial class NewEditWindow : Window
    {
        public NewEditWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NewEditViewModel newEditViewModel = (NewEditViewModel)DataContext;
            newEditViewModel.Done += NewEditViewModel_Done;
        }

        private void NewEditViewModel_Done(object sender, NewEditViewModel.DoneEventArgs e)
        {
            MessageBox.Show(e.Message);
        }
    }
}

using System.Windows.Controls;

namespace TRWpf.Views
{
    /// <summary>
    /// Логика взаимодействия для UsersView.xaml
    /// </summary>
    public partial class UsersView : UserControl
    {
        // UsersViewModel uvm;

        public UsersView()
        {
            InitializeComponent();
        }

        void Users_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox.SelectedItem != null)
                listBox.ScrollIntoView(listBox.SelectedItem);
        }
    }
}

using System.Windows.Controls;

namespace TRWpf.Views
{
    public partial class CardsView : UserControl
    {

        public CardsView()
        {
            InitializeComponent();
        }

        private void Cards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            if (dataGrid.SelectedItem != null)
                dataGrid.ScrollIntoView(dataGrid.SelectedItem);
        }
    }
}
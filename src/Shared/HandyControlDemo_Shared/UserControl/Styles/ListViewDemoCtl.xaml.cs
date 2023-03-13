using System.Windows.Controls;
using HandyControlDemo.Data;

namespace HandyControlDemo.UserControl;

public partial class ListViewDemoCtl
{
    public ListViewDemoCtl()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine(((sender as Button).DataContext as DemoDataModel).Name);
    }
}

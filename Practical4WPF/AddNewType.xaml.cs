using System.Windows;

namespace Practical4WPF;

public partial class AddNewType : Window
{
    public static string newtype;
    public AddNewType()
    {
        InitializeComponent();
    }
    private void OkBtn_Click(object sender, RoutedEventArgs e)
    {
        DialogResult=true;
        newtype = newType.Text;
    }

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
}
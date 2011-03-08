using System.Windows.Controls;
using Microsoft.Phone.Controls;

namespace RandomNumberGenerator
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void TextBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            tb.SelectAll();
        }
    }
}
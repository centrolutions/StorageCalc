using System.Windows;

namespace StorageCalc
{
    public class MessageBoxHelper: IMessageBoxHelper
    {
        public void Show(string message)
        {
            MessageBox.Show(message);
        }
    }
}

using System.ComponentModel;
using System.Windows.Forms;

namespace Gui
{
    public class UiViewModel : INotifyPropertyChanged
    {
        private string _directory;
        public string Directory
        {
            get { return _directory; }
            set
            {
                _directory = value;
                var handler = PropertyChanged;
                if(handler != null)
                    handler(this, new PropertyChangedEventArgs("Directory"));
            }
        }
        public string ListName { get; set; }
        public string Creator { get; set; }
        public string Category { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

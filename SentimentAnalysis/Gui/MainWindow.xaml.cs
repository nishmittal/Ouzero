using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

namespace Gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private UiViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnPrepGo_Click(object sender, RoutedEventArgs e)
        {
            if(FailedValidation())
            {
                TxtPrepProgress.Text = "Enter a valid value for all inputs.";
                return;
            }
            _viewModel = new UiViewModel
            {
                Directory = TxtPrepDirectory.Text,
                ListName = TxtListName.Text,
                Creator = TxtCreatorName.Text,
                Category = TxtCategory.Text
            };
            TxtPrepProgress.Text = "Preparing files of handles...";
        }

        private bool FailedValidation()
        {
            var emptyDirectory = TxtPrepDirectory.Text == string.Empty;
            var invalidDirectory = !Directory.Exists(TxtPrepDirectory.Text);
            var emptyListName = TxtListName.Text == string.Empty;
            var emptyCreatorName = TxtCreatorName.Text == string.Empty;
            var emptyCategory = TxtCategory.Text.Any();

            return emptyDirectory || invalidDirectory || emptyListName || emptyCreatorName || emptyCategory;
        }

        private void BtnPrepFileChooser_Click(object sender, RoutedEventArgs e)
        {
            var directoryChooser = new FolderBrowserDialog();

            var dialogResult = directoryChooser.ShowDialog();

            if(dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                TxtPrepDirectory.Text = directoryChooser.SelectedPath;
            }
        }

        private void TxtListName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (TxtListName.Text == string.Empty)
            {
                TxtListName.Foreground = Brushes.Red;
                TxtListName.Text = "ENTER LIST NAME";
            }
        }
    }
}

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WoS.UI.Validation;

namespace WoS.UI.Controls
{
    /// <summary>
    /// Interaction logic for BrowseBox.xaml
    /// </summary>
    public partial class BrowseBox : UserControl
    {
        public BrowseBox()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty FileNameProperty = DependencyProperty.Register(nameof(FileName), typeof(string), typeof(BrowseBox), new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnFileNameChanged, null, true, UpdateSourceTrigger.PropertyChanged));

        public string FileName
        {
            get => Convert.ToString(GetValue(FileNameProperty));
            set => SetValue(FileNameProperty, value);
        }

        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register(nameof(Watermark), typeof(string), typeof(BrowseBox), new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnFileNameChanged, null, true, UpdateSourceTrigger.PropertyChanged));

        public string Watermark
        {
            get => Convert.ToString(GetValue(WatermarkProperty));
            set => SetValue(WatermarkProperty, value);
        }

        private static void OnFileNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is BrowseBox bb)) return;
            bb.LostFocus -= HandleLostFocus;
            bb.TextBox1.Text = bb.FileName;
            bb.LostFocus += HandleLostFocus;
        }

        private static void HandleLostFocus(object sender, RoutedEventArgs e)
        {
            if (!(sender is BrowseBox bb)) return;
            bb.FileName = bb.TextBox1.Text;
        }

        public enum BrowseTypeEnum
        {
            OpenFile,
            SaveFile,
            BrowseDirectory
        }

        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(nameof(Type), typeof(BrowseTypeEnum), typeof(BrowseBox));
        public BrowseTypeEnum Type
        {
            get => (BrowseTypeEnum)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }

        public static readonly DependencyProperty InitialDirectoryProperty = DependencyProperty.Register(nameof(InitialDirectory), typeof(string), typeof(BrowseBox));
        public string InitialDirectory
        {
            get => Convert.ToString(GetValue(InitialDirectoryProperty));
            set => SetValue(InitialDirectoryProperty, value);
        }

        public static readonly DependencyProperty BasePathProperty = DependencyProperty.Register(nameof(BasePath), typeof(string), typeof(BrowseBox));
        public string BasePath
        {
            get => Convert.ToString(GetValue(BasePathProperty));
            set => SetValue(BasePathProperty, value);
        }

        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(nameof(Filter), typeof(string), typeof(BrowseBox));
        public string Filter
        {
            get => Convert.ToString(GetValue(FilterProperty));
            set => SetValue(FilterProperty, value);
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(BrowseBox));
        public string Title
        {
            get => Convert.ToString(GetValue(TitleProperty));
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty IsValidProperty = DependencyProperty.Register(nameof(IsValid), typeof(bool), typeof(BrowseBox), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsValidPropertyChanged, IsValidPropertyCoerce, true, UpdateSourceTrigger.PropertyChanged));
        private static object IsValidPropertyCoerce(DependencyObject d, object baseValue)
        {
            if (!(d is BrowseBox bb)) return null;
            SetControlValid(bb, Convert.ToBoolean(baseValue));
            return baseValue;
        }

        private static void IsValidPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs) { }

        private static void SetControlValid(BrowseBox browseBox, bool isValid)
        {
            if (isValid)
                System.Windows.Controls.Validation.ClearInvalid(browseBox.TextBox1.GetBindingExpression(TextBox.TextProperty));
            else
            {
                var validationError = new ValidationError(new RegexValidationRule(), browseBox.TextBox1.GetBindingExpression(TextBox.TextProperty)) { ErrorContent = "File not found" };
                System.Windows.Controls.Validation.MarkInvalid(browseBox.TextBox1.GetBindingExpression(TextBox.TextProperty), validationError);
            }
        }

        public bool IsValid
        {
            get => System.Convert.ToBoolean(GetValue(IsValidProperty));
            set => SetValue(IsValidProperty, value);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ExecuteBrowseCommand();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ExecuteBrowseCommand()
        {
            switch (Type)
            {
                case BrowseTypeEnum.OpenFile:
                    ExecuteOpenFileDialog();
                    break;
                case BrowseTypeEnum.SaveFile:
                    ExecuteSaveFileDialog();
                    break;
                case BrowseTypeEnum.BrowseDirectory:
                    ExecuteBrowseDirectory();
                    break;
                default:
                    ExecuteOpenFileDialog();
                    break;
            }
        }

        private void ExecuteOpenFileDialog()
        {
            var dlg = new OpenFileDialog
            {
                InitialDirectory = InitialDirectory,
                Filter = Filter,
                Title = Title
            };
            if (!string.IsNullOrEmpty(BasePath) && System.IO.Directory.Exists(BasePath) && !string.IsNullOrEmpty(FileName))
            {
                var fname = System.IO.Path.Combine(BasePath, FileName);
                if (System.IO.File.Exists(fname))
                    dlg.FileName = fname;
            }
            else if (System.IO.File.Exists(FileName))
                dlg.FileName = FileName;
            else if (!string.IsNullOrEmpty(FileName) && System.IO.Directory.Exists(InitialDirectory))
            {
                var fName = System.IO.Path.Combine(InitialDirectory, FileName);
                if (System.IO.File.Exists(fName))
                    dlg.FileName = fName;
            }
            if (!string.IsNullOrEmpty(InitialDirectory) && System.IO.Directory.Exists(InitialDirectory) && !string.IsNullOrEmpty(dlg.FileName) && dlg.FileName.StartsWith(InitialDirectory))
                dlg.FileName = dlg.FileName.Replace(InitialDirectory, "").TrimStart('\\');

            if (dlg.ShowDialog() != true) return;
            //if (!string.IsNullOrEmpty(BasePath) && System.IO.Directory.Exists(BasePath) && !string.IsNullOrEmpty(dlg.FileName) && dlg.FileName.StartsWith(BasePath))
            //    FileName = dlg.FileName.Replace(BasePath.EnsureEndsWith("\\"), "");
            //else
                FileName = dlg.FileName;
        }

        private void ExecuteSaveFileDialog()
        {
            var dlg = new SaveFileDialog
            {
                InitialDirectory = InitialDirectory,
                Filter = Filter,
                Title = Title
            };
            if (!string.IsNullOrEmpty(FileName))
                dlg.FileName = FileName;

            if (dlg.ShowDialog() == true)
                FileName = dlg.FileName;
        }

        private void ExecuteBrowseDirectory()
        {
            //using var dlg = new FolderBrowserDialog()
            //{

            //}
        }

    }

}

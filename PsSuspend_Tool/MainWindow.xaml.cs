using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
namespace PsSuspend_Tool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Style PIDInput;
        Style NameInput;
        Style HintText;
        Style ButtonStyle;
    
        public MainWindow()
        {
            InitializeComponent();
  
            PIDInput = (Style)Application.Current.Resources["PIDInput"];
            NameInput = (Style)Application.Current.Resources["NameInput"];
            HintText = (Style)Application.Current.Resources["HintText"];
            ButtonStyle = (Style)Application.Current.Resources["Button"];
            Add.Click += (o, r) =>
            {
                MainPanal.Children.Add(GenProgramUnit());
            };
            Remove.Click += (o, r) =>
            {
                if (MainPanal.Children.Count > 2)
                {
                    MainPanal.Children.Remove(MainPanal.Children[MainPanal.Children.Count-1]);
                }
            };
        }
        StackPanel GenProgramUnit()
        {
            string id=Guid.NewGuid().ToString();
            StackPanel sp = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Left };
            var pid = new TextBox() { Style = PIDInput };
            sp.Children.Add(pid);

            var name = new TextBox() { Style = NameInput };
            sp.Children.Add(name);
            var s = new Button() { Style = ButtonStyle, Content = "Suspend" };
            s.Click += (o, r) =>
            {
                if (pid.Text != "")
                {
                    cmd($"pssuspend.exe {pid.Text}");
                }
                else
                {
                    cmd($"pssuspend.exe {name.Text}");
                }
            };
            sp.Children.Add(s);
            var r = new Button() { Style = ButtonStyle, Content = "-r" };
            r.Click += (o, r) =>
            {
                if (pid.Text != "")
                {
                    cmd($"pssuspend.exe -r {pid.Text}");
                }
                else
                {
                    cmd($"pssuspend.exe -r {name.Text}");
                }
            };
            
            sp.Children.Add(r);
            return sp;
        }
        string cmd(string c)
        {
            Debug.WriteLine(c);
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
            cmd.StandardInput.WriteLine(c);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            Debug.WriteLine(cmd.StandardOutput.ReadToEnd());
            return cmd.StandardOutput.ReadToEnd();
        }
    }
}
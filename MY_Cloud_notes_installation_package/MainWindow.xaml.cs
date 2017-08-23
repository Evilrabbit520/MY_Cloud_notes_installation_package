using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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

namespace MY_Cloud_notes_installation_package
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //test
        }

        bool installEnd = false;


        private void MYDLOGIN_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var pint = Mouse.GetPosition(this);
                if (pint.X < 50 || pint.Y < 120)
                    DragMove();
            }
        }

        private void buttonMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void buttonShutdown_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void buttonInstall_Click(object sender, RoutedEventArgs e)
        {
            this.textboxInstallPath.IsEnabled = false;
            this.progressbarInstall.Value = this.progressbarInstall.Minimum;

            string strDirectory = this.textboxInstallPath.Text+"\\";
            string password = "";
            bool overWrite = true;

            Task task = new Task(() =>
            {
                using (ZipInputStream s = new ZipInputStream(new MemoryStream(InstallResource.ResourceManager.GetObject("install") as byte[])))
                {
                    s.Password = password;
                    ZipEntry theEntry;

                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        string directoryName = "";
                        string pathToZip = "";
                        pathToZip = theEntry.Name;

                        if (pathToZip != "")
                        {
                            directoryName = System.IO.Path.GetDirectoryName(pathToZip) + "\\";
                        }

                        string fileName = System.IO.Path.GetFileName(pathToZip);

                        Directory.CreateDirectory(strDirectory + directoryName);

                        if (fileName != "")
                        {
                            if ((File.Exists(strDirectory + directoryName + fileName) && overWrite) || (!File.Exists(strDirectory + directoryName + fileName)))
                            {
                                using (FileStream streamWriter = File.Create(strDirectory + directoryName + fileName))
                                {
                                    int size = 2048;
                                    byte[] data = new byte[2048];
                                    while (true)
                                    {
                                        size = s.Read(data, 0, data.Length);

                                        if (size > 0)
                                            streamWriter.Write(data, 0, size);
                                        else
                                            break;
                                    }
                                    streamWriter.Close();
                                }
                            }
                        }

                        this.progressbarInstall.Dispatcher.Invoke(() => { this.progressbarInstall.Value += (new Random()).Next(Convert.ToInt32(this.progressbarInstall.Maximum - this.progressbarInstall.Value)); });
                    }

                    s.Close();
                    this.progressbarInstall.Dispatcher.Invoke(() => { this.progressbarInstall.Value =this.progressbarInstall.Maximum; });
                    MessageBox.Show("Over", "Over");
                    this.buttonExit.Dispatcher.Invoke(() => { this.buttonExit.IsEnabled = true; });
                }
            });

            task.Start();
            /*
            Directory.CreateDirectory(this.textboxInstallPath.Text);
            FileStream fs = new FileStream(this.textboxInstallPath.Text+@"\m.pdf", FileMode.Create);
            try
            {
                
                fs.BeginWrite(ba, 0, ba.Length,new AsyncCallback(asr=> {
                    using (Stream str = (Stream)asr.AsyncState)
                    {
                        fs.Close();
                        this.installEnd = true;
                        str.EndWrite(asr);
                        //Console.WriteLine("异步写入结束");
                    }
                }),fs);
                                
                //SevenZip.Compression.LZMA.Decoder d = new SevenZip.Compression.LZMA.Decoder();
                //d.Code(abstream, stream, abstream.Length, stream.Length, m_CodeProgress);
                //stream.Close();
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            */
            (sender as Button).Visibility = Visibility.Collapsed;
            this.buttonExit.Visibility = Visibility.Visible;
            this.buttonExit.IsEnabled = false;
            

            
        }

        private void buttonChoiceInstallPath_Click(object sender, RoutedEventArgs e)
        {
            var m_Dialog = new System.Windows.Forms.FolderBrowserDialog();
            var result = m_Dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            string m_Dir = m_Dialog.SelectedPath.Trim();
            this.textboxInstallPath.Text = m_Dir;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}

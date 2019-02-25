using BE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace PL_WPF.UI.TraineeInterface
{
    /// <summary>
    /// Interaction logic for DetailsAboutMyTest.xaml
    /// </summary>
    public partial class DetailsAboutMyTest : UserControl
    {
        protected BackgroundWorker timerWorker;
        protected TimeSpan timeSpanToTheTest;
        private readonly TimeSpan aSecond = new TimeSpan(0, 0, 1);


        public DetailsAboutMyTest()
        {
            InitializeComponent();
            timerWorker = new BackgroundWorker();
            timerWorker.WorkerReportsProgress = true;
            timerWorker.WorkerSupportsCancellation = true;
            timerWorker.DoWork += (sender, e) =>
            {
                BackgroundWorker backgroundWorker = sender as BackgroundWorker;
                timeSpanToTheTest = ((TimeSpan)e.Argument).Duration(); // the duration is for debugging

                while (timeSpanToTheTest.TotalSeconds > 0)
                {
                    backgroundWorker.ReportProgress(1);
                    Thread.Sleep(1000);
                    timeSpanToTheTest = timeSpanToTheTest.Subtract(aSecond);
                }

                e.Result = "Successfully!!!";
            };
            timerWorker.ProgressChanged += (sender, e) =>
            {
                //e.ProgressPercentage;
                timerTextBlock.Text = string.Format(@"{0}.{1:hh\:mm\:ss}", timeSpanToTheTest.Days, timeSpanToTheTest);
            };
            timerWorker.RunWorkerCompleted += (sender, e) =>
            {
                if (e.Error != null)
                    return; // TODO something
                else
                {
                    timerTextBlock.Text = e.Result as string;
                }
            };

            RefreshButtonClick += (sender, e) =>
            {
                if (timerWorker.IsBusy && !MyTestDadaGrid.HasItems && timerWorker.WorkerSupportsCancellation)
                    timerWorker.CancelAsync();
                if (!timerWorker.IsBusy && MyTestDadaGrid.HasItems)
                    timerWorker.RunWorkerAsync((MyTestDadaGrid.Items[0] as Test).Date - DateTime.Now);
            };

            //MyTestDadaGrid.AddingNewItem += (sender, e) => { e.}
            //BUG not change after failed
        }


        public event RoutedEventHandler RefreshButtonClick
        {
            add { Refresh_Button.Click += value; }
            remove { Refresh_Button.Click -= value; }
        }



        private void Refresh_Button_Click(object sender, RoutedEventArgs e)
        {
            // RefreshButtonClick(this, new RoutedEventArgs());
        }
    }
}

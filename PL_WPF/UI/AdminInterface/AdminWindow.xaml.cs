//BS"D

using BE;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL_WPF.UI.AdminInterface
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        BL.IBL bl = BL.FactorySingleton.Instance;
        protected bool ToSort { get; set; }

        public AdminWindow()
        {
            InitializeComponent();
            //new ObservableCollection<Tester>()
            ourGroup.DataContext = bl.TestersByExpertise(true);
            toSortToggleButton.DataContext = this;
            
            //List<Tester> res = instance.Get_testers_list_grouping_by_CarType(false).SelectMany(item => item).ToList();
        }

        private void TraineesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Testers_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string groupBy = (Testers_ListBox.SelectedItem as ListBoxItem).Content as string;
            groupBy = groupBy == "Expertise" ? "VehicleTypesExpertise" : groupBy == "Experience"? "YearsOfExperience" : "Gender";
            Testers_ListView.ItemsSource = bl.GetTesters();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(Testers_ListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription(groupBy);
            view.GroupDescriptions.Add(groupDescription);
        }

        private void Trainees_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string groupBy = (Trainees_ListBox.SelectedItem as ListBoxItem).Content as string;
            groupBy = groupBy == "Vehicle" ? "VehicleTypeTraining" : groupBy == "Gearbox" ? "GearboxTypeTraining" : groupBy == "Gender"? "Gender": groupBy == "Driving school" ? "DrivingSchool": "TeacherName";
            Trainees_ListView.ItemsSource = bl.GetTrainees();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(Trainees_ListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription(groupBy);
            view.GroupDescriptions.Add(groupDescription);
        }

        private void Tests_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Tests_ListView.ItemsSource = bl.GetTests();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(Tests_ListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Vehicle");
            view.GroupDescriptions.Add(groupDescription);
        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {

        }
    }
}

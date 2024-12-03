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
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для RefreshWindow.xaml
    /// </summary>
    public partial class RefreshWindow : Window
    {
        private Requests _currentRequest;

        public RefreshWindow(Requests request)
        {
            InitializeComponent();
            _currentRequest = request;

            StatusComboBox.ItemsSource = AutodetailsEntities.GetContext().RequestStatus.ToList();
            WorkersComboBox.ItemsSource = AutodetailsEntities.GetContext().Workers.ToList();
            NamePartsComboBox.ItemsSource = AutodetailsEntities.GetContext().NameParts.ToList();
            ClientsComboBox.ItemsSource = AutodetailsEntities.GetContext().Clients.ToList();
            // Презагрузкаданных

            StatusComboBox.SelectedItem = request.RequestStatus;
            WorkersComboBox.SelectedItem = request.Workers;
            NamePartsComboBox.SelectedItem = request.NameParts;
            ClientsComboBox.SelectedItem = request.Clients;

        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            // Здесь код для обновления данных в базе
            var context = AutodetailsEntities.GetContext();
            _currentRequest.status_id = ((RequestStatus)StatusComboBox.SelectedItem).status_id;
            _currentRequest.worker_id =
           ((Workers)WorkersComboBox.SelectedItem).worker_id;
            _currentRequest.name_part_id =
           ((NameParts)NamePartsComboBox.SelectedItem).name_part_id;
            _currentRequest.client_id =
           ((Clients)ClientsComboBox.SelectedItem).client_id;
            context.SaveChanges();
            MessageBox.Show("Данные заявки обновлены");
            this.Close();
        }

    }
}

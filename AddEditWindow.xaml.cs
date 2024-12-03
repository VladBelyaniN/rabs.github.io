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
    /// Логика взаимодействия для AddEditWindow.xaml
    /// </summary>
    public partial class AddEditWindow : Window
    {
        private Requests request = new Requests();
        private Clients client = new Clients();
        private NameParts NamePart = new NameParts();
        public AddEditWindow()
        {
            InitializeComponent();
            DataContext = request;
            ComboStatus.ItemsSource = AutodetailsEntities.GetContext().RequestStatus.ToList();
        }
        private void BtnSave_Click(Object sender, RoutedEventArgs e)
        {
            StringBuilder error = new StringBuilder();
            if (request.application_number == null)
                error.AppendLine("Введите номер заявки!");
            else if (!int.TryParse(request.application_number.ToString(), out int applicationNumber) || applicationNumber <= 0)
                error.AppendLine("Номер заявки должен иметь положительное и не отрицательное значение!");
            else if (AutodetailsEntities.GetContext().Requests.Any(row => row.application_number == request.application_number))
                error.AppendLine("Номер заявки уже существует!");
            if (request.request_date == null || request.request_date == DateTime.MinValue)
                error.AppendLine("Укажитедату!");
            if (ComboStatus.SelectedItem != null && ComboStatus.SelectedItem is RequestStatus selectedStatus)
                request.status_id = selectedStatus.status_id;
            else error.AppendLine("Выберитестатус!");
            if (string.IsNullOrWhiteSpace(EquipmentTextBox.Text))
                error.AppendLine("Укажитеоборудование!");
            if (string.IsNullOrWhiteSpace(ClientTextBox.Text))
                error.AppendLine("Укажитеклиента!");
            if (string.IsNullOrWhiteSpace(FaultTypeTextBox.Text))
                error.AppendLine("Укажите тип неисправности!");
            if (error.Length > 0)
            {
                MessageBox.Show(error.ToString(), "Предупреждение!", MessageBoxButton.OK,
                MessageBoxImage.Information);

                return;
            }
            try
            {
                var context = AutodetailsEntities.GetContext();
                client.client_name = ClientTextBox.Text;
                NamePart.name_part_name = FaultTypeTextBox.Text;

                context.Clients.Add(client);
                context.NameParts.Add(NamePart);
                context.SaveChanges();

                var clientId = client.client_id;
                var faultTypeId = NamePart.name_part_id;
                request.client_id = clientId;
                request.name_part_id = faultTypeId;
                context.Requests.Add(request);
                context.SaveChanges();
                MessageBox.Show("Информация сохранена");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}

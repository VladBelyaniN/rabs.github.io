using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Data.Entity;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1;
namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshdemoDataGrid();
            ComboStatus.ItemsSource = AutodetailsEntities.GetContext().RequestStatus.ToList();
            Box.Text = AutodetailsEntities.GetContext().Requests.Count(r => r.status_id == 3).ToString();
            Vis();
        }

        private void RefreshdemoDataGrid()
        {
            var context = AutodetailsEntities.GetContext();
            var requestsWithRelations = context.Requests
            .Include(r => r.NameParts)
            .Include(r => r.Clients)
            .Include(r => r.Workers)
            .ToList();
            dataGrid.ItemsSource = requestsWithRelations;
        }

        private void Vis()
        {
            switch (Authorization.authorizationRole)
            {
                case "Админ":
                    // Админ имеет доступ ко всем функциям, ничего скрывать не нужно
                    BtnAdd.Visibility = Visibility.Visible;
                    BtnDelet.Visibility = Visibility.Visible;
                    Template.Visibility = Visibility.Visible;
                    break;

                case "Модератор":
                    // Модератор не имеет доступа к удалению
                    BtnAdd.Visibility = Visibility.Visible;
                    BtnDelet.Visibility = Visibility.Collapsed;
                    Template.Visibility = Visibility.Visible;
                    break;

                case "Пользователь":
                    // Гость имеет доступ только к добавлению и обновлению
                    BtnAdd.Visibility = Visibility.Visible;
                    BtnDelet.Visibility = Visibility.Collapsed;
                    Template.Visibility = Visibility.Collapsed;
                    break;
            }
        }
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selectedRequests = (sender as Button)?.DataContext as Requests;
            if (selectedRequests != null)
            {
                RefreshWindow addEditWindow = new RefreshWindow(selectedRequests);
                if (addEditWindow.ShowDialog() == true)
                {
                    RefreshdemoDataGrid();
                }
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddEditWindow addEditWindow = new AddEditWindow();
            if (addEditWindow.ShowDialog() == true)
            {
                RefreshdemoDataGrid();

            }
        }
        private void BtnDelet_Click(object sender, RoutedEventArgs e)
        {
            var servisForRemoving = dataGrid.SelectedItems.Cast<Requests>().ToList();
            if (servisForRemoving.Any() && MessageBox.Show($"Вы точно хотите удалить следующее{servisForRemoving.Count()}элемент ? ", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var context = AutodetailsEntities.GetContext();
                context.Requests.RemoveRange(servisForRemoving);
                context.SaveChanges();
                MessageBox.Show("Данные удалены");
                RefreshdemoDataGrid();
            }
        }
        private void BtnUp_Click(object sender, RoutedEventArgs e)
        {

            RefreshdemoDataGrid();
        }
        private void BtnOut_Click(object sender, RoutedEventArgs e)
        {
            if (ComboStatus.SelectedItem is RequestStatus selectedStatus)
            {
                int selectedStatusId = selectedStatus.status_id;
                var context = AutodetailsEntities.GetContext();
                dataGrid.ItemsSource = context.Requests
                .Include(r => r.NameParts)
                .Include(r => r.Clients)
                .Include(r => r.Workers)
                .Where(r => r.status_id == selectedStatusId)
                .ToList();
            }
        }
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = SearchBox.Text.ToLower();
            var context = AutodetailsEntities.GetContext();
            try
            {
                dataGrid.ItemsSource = context.Requests
                .Include(r => r.NameParts)
                .Include(r => r.Clients)
                .Include(r => r.Workers)
                .Where(r =>
                r.application_number.ToString().Contains(searchText) ||
                r.NameParts.name_part_name.ToString().Contains(searchText) ||
                r.Clients.client_name.ToString().Contains(searchText) ||
                r.RequestStatus.status_name.ToString().Contains(searchText) ||
                r.Workers.worker_name.ToString().Contains(searchText))
                .ToList();
            }
            catch (System.Data.Entity.Core.EntityCommandExecutionException ex)
            {
                // Логированиеилиотладкаисключения
                Console.WriteLine(ex.InnerException?.Message);
            }
        }

        private void BtnAuthorization_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationWindow authorizationWindow = new AuthorizationWindow();
            authorizationWindow.Show();
            this.Close();
        }

    }


}

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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

namespace AlalykinKP
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Личный_состав _currentItem = new Личный_состав();
        public AddEditPage(Личный_состав SelectedItem)
        {
            InitializeComponent();
            if (SelectedItem != null)
            {
                _currentItem = SelectedItem;
                RotaCombo.SelectedIndex = _currentItem.id_rota.Value - 1;
                BirhDateDP.SelectedDate = _currentItem.data_rozhdeniya;
            }
            DataContext = _currentItem;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentItem.familya))
                errors.AppendLine("Введите фамилию");
            if (string.IsNullOrWhiteSpace(_currentItem.imya))
                errors.AppendLine("Введите имя");
            if (string.IsNullOrWhiteSpace(_currentItem.otchestvo))
                errors.AppendLine("Введите отчество");
            if (string.IsNullOrWhiteSpace(BirhDateDP.Text)) errors.AppendLine("Введите дату рождения");
            else
            {
                _currentItem.data_rozhdeniya = Convert.ToDateTime(BirhDateDP.Text);
                if (_currentItem.data_rozhdeniya > DateTime.Today.AddYears(-18) || _currentItem.data_rozhdeniya < DateTime.Today.AddYears(-100))
                    errors.AppendLine("Возраст военнослужащего должен быть старше 18 и младше 100");
            }
            if (string.IsNullOrWhiteSpace(RotaCombo.Text))
                errors.AppendLine("Выберите роту из списка");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(_currentItem.dolzhnost)) _currentItem.dolzhnost = "рядовой";
                if (string.IsNullOrWhiteSpace(_currentItem.nagrady)) _currentItem.nagrady = "нет";
                if (string.IsNullOrWhiteSpace(_currentItem.uchastie)) _currentItem.uchastie = "не участвовал";
                _currentItem.id_rota = RotaCombo.SelectedIndex + 1;
                if (_currentItem.id_soldat == 0)
                {
                    _currentItem.data_postupleniya = DateTime.Today;
                    MilitaryDistrictEntities.GetContext().Личный_состав.Add(_currentItem);
                }
                try
                {
                    MilitaryDistrictEntities.GetContext().SaveChanges();
                    MessageBox.Show("Информация сохранена!");
                    Manager.MainFrame.GoBack();
                }
                catch (DbUpdateException ex)
                {
                    Exception inner = ex;
                    string fullError = "";
                    while (inner != null)
                    {
                        fullError += inner.Message + "\n\n---\n\n";
                        inner = inner.InnerException;
                    }

                    MessageBox.Show(fullError, "Причина ошибки");
                }
            }
        }
        private void ChangePhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == true)
            {
                string fileName = System.IO.Path.GetFileName(dialog.FileName);

                _currentItem.image = "/Images/" + fileName;

                BigImage.Source = new BitmapImage(new Uri("." + _currentItem.image, UriKind.Relative));
            }
        }
    }
}

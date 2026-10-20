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

namespace AlalykinKP
{
    /// <summary>
    /// Логика взаимодействия для PersonalPage.xaml
    /// </summary>
    public partial class PersonalPage : Page
    {
        public PersonalPage()
        {
            InitializeComponent();
            var currentItem = MilitaryDistrictEntities.GetContext().Личный_состав.ToList();
            ListView.ItemsSource = currentItem;
            ComboType.SelectedIndex = 0;
            SortType.SelectedIndex = 0;
            UpdateItems();
            this.Loaded += PersonalPage_Loaded;
        }
        private void PersonalPage_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateItems();
        }
        private void UpdateItems()
        {
            var currentItem = MilitaryDistrictEntities.GetContext().Личный_состав.ToList();
            if (ComboType.SelectedIndex == 1)
            {
                currentItem = currentItem.Where(p => (p.id_rota==1)).ToList();
            }
            if (ComboType.SelectedIndex == 2)
            {
                currentItem = currentItem.Where(p => (p.id_rota == 2)).ToList();
            }
            if (ComboType.SelectedIndex == 3)
            {
                currentItem = currentItem.Where(p => (p.id_rota == 3)).ToList();
            }
            if (ComboType.SelectedIndex == 4)
            {
                currentItem = currentItem.Where(p => (p.id_rota == 4)).ToList();
            }
            if (ComboType.SelectedIndex == 5)
            {
                currentItem = currentItem.Where(p => (p.id_rota == 5)).ToList();
            }
            if (SortType.SelectedIndex == 1)
            {
                currentItem = currentItem.OrderBy(p => p.familya).ToList();
            }
            if (SortType.SelectedIndex == 2)
            {
                currentItem = currentItem.OrderByDescending(p => p.data_rozhdeniya).ToList();
            }
            if (SortType.SelectedIndex == 3)
            {
                currentItem = currentItem.OrderByDescending(p => p.data_postupleniya).ToList();
            }
            currentItem = currentItem.Where(p => (p.familya+ p.imya + p.otchestvo+p.nagrady+p.uchastie+p.dolzhnost).Replace(" ", string.Empty).ToLower().Contains(TBoxSearch.Text.Replace(" ",string.Empty).ToLower())).ToList();
            ListView.ItemsSource = currentItem.ToList();
            personalCount.Text = "Объектов на странице: " + currentItem.ToList().Count;
        }
        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateItems();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateItems();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var currentItem = (sender as Button).DataContext as Личный_состав;
            if (currentItem.nagrady!="нет" || currentItem.uchastie!="не участвовал")
                MessageBox.Show("Невозможно удалить героя России!\nДля удаления уберите запись о наградах и участии", "ЗАПРЕЩЕНО!");
            else
            {
                if (MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание!",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        MilitaryDistrictEntities.GetContext().Личный_состав.Remove(currentItem);
                        MilitaryDistrictEntities.GetContext().SaveChanges();
                        ListView.ItemsSource = MilitaryDistrictEntities.GetContext().Личный_состав.ToList();
                        UpdateItems();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as Личный_состав));
        }

        private void SortType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateItems();
        }
    }
}

 Padding на margin 
 шрифт заменить на segoe ui 
 
 
 
 App.xaml (ПУСТОЙ, только запуск)

<Application x:Class="OfficeEquipment.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             StartupUri="Windows/LoginWindow.xaml">
    <Application.Resources>
        <!-- ВСЁ ПУСТО, стили пишем в каждом окне -->
    </Application.Resources>
</Application>



2. App.xaml.cs
csharp
using System.Windows;

namespace OfficeEquipment
{
    public partial class App : Application
    {
    }
}





3. UserControls/ProductCard.xaml (ШАБЛОН КАРТОЧКИ)
xml
<UserControl x:Class="OfficeEquipment.UserControls.ProductCard"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             Height="150" Width="900">
    
    <Border x:Name="MainBorder" CornerRadius="5" Margin="0,0,0,5" 
            BorderBrush="Gray" BorderThickness="1" Background="White">
        <Grid>
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="150"/>
                <ColumnDefinition Width="*"/>
                <ColumnDefinition Width="120"/>
                <ColumnDefinition Width="80"/>
            </Grid.ColumnDefinitions>
            
            <!-- ФОТО -->
            <Image x:Name="ProductImage" Grid.Column="0" 
                   Width="130" Height="130" Stretch="Uniform" Margin="10"/>
            
            <!-- ИНФОРМАЦИЯ -->
            <StackPanel Grid.Column="1" Margin="10,5">
                <TextBlock x:Name="NameText" FontFamily="Comic Sans MS" 
                           FontSize="16" FontWeight="Bold" TextWrapping="Wrap"/>
                <TextBlock x:Name="CategoryText" FontFamily="Comic Sans MS" 
                           FontSize="12" Margin="0,5"/>
                <TextBlock x:Name="DescriptionText" FontFamily="Comic Sans MS" 
                           FontSize="12" TextWrapping="Wrap"/>
                <TextBlock x:Name="ManufacturerText" FontFamily="Comic Sans MS" 
                           FontSize="12"/>
                <TextBlock x:Name="SupplierText" FontFamily="Comic Sans MS" 
                           FontSize="12"/>
            </StackPanel>
            
            <!-- ЦЕНА И СКИДКА -->
            <StackPanel Grid.Column="2" VerticalAlignment="Center">
                <TextBlock x:Name="PriceText" FontFamily="Comic Sans MS" 
                           FontSize="14" HorizontalAlignment="Right"/>
                <TextBlock x:Name="DiscountText" FontFamily="Comic Sans MS" 
                           FontSize="12" Foreground="Red" HorizontalAlignment="Right"/>
            </StackPanel>
            
            <!-- КОЛИЧЕСТВО -->
            <TextBlock x:Name="QuantityText" Grid.Column="3" 
                       FontFamily="Comic Sans MS" FontSize="14" 
                       VerticalAlignment="Center" HorizontalAlignment="Center"/>
        </Grid>
    </Border>
</UserControl>




4. UserControls/ProductCard.xaml.cs (ЛОГИКА КАРТОЧКИ)
csharp
using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OfficeEquipment.UserControls
{
    public partial class ProductCard : UserControl
    {
        public ProductCard(dynamic product)  // dynamic - чтобы подходило под любую модель
        {
            InitializeComponent();
            
            // ЗАПОЛНЯЕМ ДАННЫЕ (меняй названия полей под свою БД)
            NameText.Text = product.Name;
            CategoryText.Text = "Категория: " + product.Category?.Name;
            DescriptionText.Text = product.Description;
            ManufacturerText.Text = "Производитель: " + product.Manufacturer?.Name;
            SupplierText.Text = "Поставщик: " + product.Supplier?.Name;
            PriceText.Text = product.Price.ToString("F2") + " руб.";
            QuantityText.Text = product.StockQuantity + " шт.";
            
            // СКИДКА
            int discount = product.DiscountPercent;
            if (discount > 0)
            {
                DiscountText.Text = "Скидка: " + discount + "%";
            }
            
            // ЦВЕТ ФОНА ПРИ СКИДКЕ > 20% (#C8A2C8)
            if (discount > 20)
            {
                MainBorder.Background = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#C8A2C8"));
            }
            
            // КАРТИНКА (заглушка если нет)
            string imagePath = product.ImagePath;
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, 
                "Assets", string.IsNullOrEmpty(imagePath) ? "picture.png" : imagePath);
            
            try
            {
                ProductImage.Source = new BitmapImage(new Uri(fullPath));
            }
            catch
            {
                ProductImage.Source = new BitmapImage(new Uri(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "picture.png")));
            }
        }
    }
}





5. Windows/LoginWindow.xaml (ОКНО ВХОДА)
xml
<Window x:Class="OfficeEquipment.Windows.LoginWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Вход в систему" Height="350" Width="300"
        WindowStartupLocation="CenterScreen" Background="White">
    <Grid>
        <StackPanel VerticalAlignment="Center" Margin="30">
            
            <!-- ЛОГОТИП -->
            <Image Source="/Assets/logo.png" Width="100" Height="100"/>
            
            <!-- ЗАГОЛОВОК (меняй название компании) -->
            <TextBlock Text="Офисная техника" FontFamily="Comic Sans MS" 
                       FontSize="20" FontWeight="Bold" Foreground="#498C51" 
                       HorizontalAlignment="Center" Margin="0,10"/>
            
            <!-- ЛОГИН -->
            <TextBlock Text="Логин:" FontFamily="Comic Sans MS" FontSize="14"/>
            <TextBox x:Name="LoginBox" FontFamily="Comic Sans MS" FontSize="14" 
                     Padding="5" Margin="0,0,0,10"/>
            
            <!-- ПАРОЛЬ -->
            <TextBlock Text="Пароль:" FontFamily="Comic Sans MS" FontSize="14"/>
            <PasswordBox x:Name="PasswordBox" FontFamily="Comic Sans MS" 
                         FontSize="14" Padding="5" Margin="0,0,0,15"/>
            
            <!-- КНОПКА ВХОДА (цвет #76E383) -->
            <Button Content="Войти" Click="Login_Click" FontFamily="Comic Sans MS" 
                    FontSize="14" Background="#76E383" Foreground="Black" 
                    Padding="10" BorderThickness="0" Cursor="Hand"/>
        </StackPanel>
    </Grid>
</Window>






6. Windows/LoginWindow.xaml.cs
csharp
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace OfficeEquipment.Windows
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text.Trim();
            string password = PasswordBox.Password;
            
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            // ЗАМЕНИ НА СВОЙ КОНТЕКСТ
            using (var db = new OfficeEquipmentContext())
            {
                // ЗАМЕНИ НАЗВАНИЯ ТАБЛИЦ И ПОЛЕЙ
                var user = db.Users
                    .Include(u => u.Role)
                    .FirstOrDefault(u => u.Login == login && u.Password == password);
                
                if (user == null)
                {
                    MessageBox.Show("Неверный логин или пароль", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                
                // ОТКРЫВАЕМ НУЖНОЕ ОКНО ПО РОЛИ
                Window window;
                if (user.Role.Name == "Администратор")
                    window = new AdminWindow(user);
                else
                    window = new ClientWindow(user);
                
                window.Show();
                this.Close();
            }
        }
    }
}





7. Windows/ClientWindow.xaml (ОКНО КЛИЕНТА)
xml
<Window x:Class="OfficeEquipment.Windows.ClientWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Просмотр товаров" Height="600" Width="950"
        WindowStartupLocation="CenterScreen" Background="White">
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
        </Grid.RowDefinitions>
        
        <!-- ШАПКА (цвет #76E383) -->
        <Border Grid.Row="0" Background="#76E383" Padding="10">
            <Grid>
                <Image Source="/Assets/logo.png" Width="40" Height="40" 
                       HorizontalAlignment="Left"/>
                <TextBlock Text="Офисная техника" FontFamily="Comic Sans MS" 
                           FontSize="18" FontWeight="Bold" Foreground="Black" 
                           HorizontalAlignment="Center" VerticalAlignment="Center"/>
                <StackPanel Orientation="Horizontal" HorizontalAlignment="Right">
                    <TextBlock x:Name="UserNameText" FontFamily="Comic Sans MS" 
                               FontSize="14" VerticalAlignment="Center" Margin="0,0,10,0"/>
                    <Button Content="Выход" Click="Exit_Click" FontFamily="Comic Sans MS" 
                            FontSize="14" Background="#498C51" Foreground="White" 
                            Padding="10,5" BorderThickness="0" Cursor="Hand"/>
                </StackPanel>
            </Grid>
        </Border>
        
        <!-- СПИСОК ТОВАРОВ -->
        <ScrollViewer Grid.Row="1" VerticalScrollBarVisibility="Auto">
            <StackPanel x:Name="ProductsPanel" Margin="10"/>
        </ScrollViewer>
    </Grid>
</Window>




8. Windows/ClientWindow.xaml.cs
csharp
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using OfficeEquipment.UserControls;

namespace OfficeEquipment.Windows
{
    public partial class ClientWindow : Window
    {
        private dynamic _user;  // dynamic - подходит под любую модель
        
        public ClientWindow(dynamic user)
        {
            InitializeComponent();
            _user = user;
            UserNameText.Text = user.FullName;  // ЗАМЕНИ НА СВОЁ ПОЛЕ ФИО
            LoadProducts();
        }
        
        private void LoadProducts()
        {
            ProductsPanel.Children.Clear();
            
            // ЗАМЕНИ НА СВОЙ КОНТЕКСТ
            using (var db = new OfficeEquipmentContext())
            {
                // ЗАГРУЖАЕМ ТОВАРЫ СО ВСЕМИ СВЯЗЯМИ
                var products = db.Products
                    .Include(p => p.Category)
                    .Include(p => p.Manufacturer)
                    .Include(p => p.Supplier)
                    .ToList();
                
                // СОЗДАЁМ КАРТОЧКУ ДЛЯ КАЖДОГО ТОВАРА
                foreach (var product in products)
                {
                    var card = new ProductCard(product);
                    ProductsPanel.Children.Add(card);
                }
            }
        }
        
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            this.Close();
        }
    }
}







9. Windows/AdminWindow.xaml (ОКНО АДМИНИСТРАТОРА)
xml
<Window x:Class="OfficeEquipment.Windows.AdminWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Управление товарами" Height="650" Width="1000"
        WindowStartupLocation="CenterScreen" Background="White">
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
        </Grid.RowDefinitions>
        
        <!-- ШАПКА -->
        <Border Grid.Row="0" Background="#76E383" Padding="10">
            <Grid>
                <Image Source="/Assets/logo.png" Width="40" Height="40" 
                       HorizontalAlignment="Left"/>
                <TextBlock Text="Офисная техника - Администратор" 
                           FontFamily="Comic Sans MS" FontSize="18" FontWeight="Bold" 
                           HorizontalAlignment="Center" VerticalAlignment="Center"/>
                <StackPanel Orientation="Horizontal" HorizontalAlignment="Right">
                    <TextBlock x:Name="UserNameText" FontFamily="Comic Sans MS" 
                               FontSize="14" VerticalAlignment="Center" Margin="0,0,10,0"/>
                    <Button Content="Заказы" Click="Orders_Click" 
                            FontFamily="Comic Sans MS" FontSize="14" Background="#498C51" 
                            Foreground="White" Padding="10,5" Margin="0,0,5,0" 
                            BorderThickness="0" Cursor="Hand"/>
                    <Button Content="Выход" Click="Exit_Click" 
                            FontFamily="Comic Sans MS" FontSize="14" Background="#498C51" 
                            Foreground="White" Padding="10,5" BorderThickness="0" Cursor="Hand"/>
                </StackPanel>
            </Grid>
        </Border>
        
        <!-- ПАНЕЛЬ ФИЛЬТРОВ -->
        <StackPanel Grid.Row="1" Margin="10">
            <Grid>
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="*"/>
                    <ColumnDefinition Width="150"/>
                    <ColumnDefinition Width="180"/>
                    <ColumnDefinition Width="Auto"/>
                </Grid.ColumnDefinitions>
                
                <!-- ПОИСК -->
                <TextBox x:Name="SearchBox" Grid.Column="0" FontFamily="Comic Sans MS" 
                         FontSize="14" Padding="5" Margin="0,0,5,0"
                         TextChanged="Filter_Changed"/>
                
                <!-- СОРТИРОВКА -->
                <ComboBox x:Name="SortBox" Grid.Column="1" FontFamily="Comic Sans MS" 
                          FontSize="14" Margin="0,0,5,0" SelectionChanged="Filter_Changed">
                    <ComboBoxItem Content="Без сортировки" IsSelected="True"/>
                    <ComboBoxItem Content="По наличию (возр.)"/>
                    <ComboBoxItem Content="По наличию (убыв.)"/>
                </ComboBox>
                
                <!-- ФИЛЬТР ПО ПРОИЗВОДИТЕЛЮ -->
                <ComboBox x:Name="ManufacturerBox" Grid.Column="2" 
                          FontFamily="Comic Sans MS" FontSize="14" Margin="0,0,5,0"
                          SelectionChanged="Filter_Changed"/>
                
                <!-- КНОПКА ДОБАВИТЬ -->
                <Button Content="Добавить товар" Click="Add_Click" Grid.Column="3"
                        FontFamily="Comic Sans MS" FontSize="14" Background="#76E383" 
                        Padding="10,5" BorderThickness="0" Cursor="Hand"/>
            </Grid>
        </StackPanel>
        
        <!-- СПИСОК ТОВАРОВ -->
        <ScrollViewer Grid.Row="2" VerticalScrollBarVisibility="Auto">
            <StackPanel x:Name="ProductsPanel" Margin="10"/>
        </ScrollViewer>
    </Grid>
</Window>









10. Windows/AdminWindow.xaml.cs
csharp
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using OfficeEquipment.UserControls;

namespace OfficeEquipment.Windows
{
    public partial class AdminWindow : Window
    {
        private dynamic _user;
        private List<dynamic> _allProducts;  // храним все товары
        private bool _editWindowOpen = false;  // запрет на 2 окна
        
        public AdminWindow(dynamic user)
        {
            InitializeComponent();
            _user = user;
            UserNameText.Text = user.FullName;
            LoadManufacturers();
            LoadProducts();
        }
        
        private void LoadManufacturers()
        {
            using (var db = new OfficeEquipmentContext())
            {
                var manufacturers = db.Manufacturers.ToList();
                ManufacturerBox.Items.Clear();
                ManufacturerBox.Items.Add("Все производители");
                foreach (var m in manufacturers)
                    ManufacturerBox.Items.Add(m.Name);  // ЗАМЕНИ НА СВОЁ ПОЛЕ
                ManufacturerBox.SelectedIndex = 0;
            }
        }
        
        private void LoadProducts()
        {
            using (var db = new OfficeEquipmentContext())
            {
                _allProducts = db.Products
                    .Include(p => p.Category)
                    .Include(p => p.Manufacturer)
                    .Include(p => p.Supplier)
                    .ToList();
            }
            ApplyFilter();
        }
        
        private void ApplyFilter()
        {
            if (_allProducts == null) return;
            
            var filtered = _allProducts.AsEnumerable();
            
            // ПОИСК
            string search = SearchBox.Text.ToLower();
            if (!string.IsNullOrEmpty(search))
            {
                filtered = filtered.Where(p =>
                    p.Name.ToLower().Contains(search) ||
                    (p.Description != null && p.Description.ToLower().Contains(search)) ||
                    p.Manufacturer.Name.ToLower().Contains(search));  // ЗАМЕНИ ПОЛЯ
            }
            
            // ФИЛЬТР ПО ПРОИЗВОДИТЕЛЮ
            if (ManufacturerBox.SelectedIndex > 0)
            {
                string selected = ManufacturerBox.SelectedItem.ToString();
                filtered = filtered.Where(p => p.Manufacturer.Name == selected);
            }
            
            // СОРТИРОВКА
            if (SortBox.SelectedIndex == 1)
                filtered = filtered.OrderBy(p => p.StockQuantity);
            else if (SortBox.SelectedIndex == 2)
                filtered = filtered.OrderByDescending(p => p.StockQuantity);
            
            // ОТОБРАЖАЕМ
            ProductsPanel.Children.Clear();
            foreach (var product in filtered)
            {
                var card = new ProductCard(product);
                
                // ДОБАВЛЯЕМ КНОПКУ УДАЛИТЬ В КАРТОЧКУ
                var deleteBtn = new Button
                {
                    Content = "Удалить",
                    Tag = product.ProductId,  // ЗАМЕНИ НА ID
                    FontFamily = new System.Windows.Media.FontFamily("Comic Sans MS"),
                    FontSize = 12,
                    Background = System.Windows.Media.Brushes.Red,
                    Foreground = System.Windows.Media.Brushes.White,
                    Padding = new Thickness(8, 3, 8, 3),
                    Margin = new Thickness(5),
                    BorderThickness = new Thickness(0),
                    Cursor = System.Windows.Input.Cursors.Hand
                };
                deleteBtn.Click += Delete_Click;
                
                // ДОБАВЛЯЕМ КНОПКУ В КАРТОЧКУ (в последнюю колонку)
                var grid = card.FindName("MainBorder") as Border;
                // УПРОЩЁННО: добавляем обработчик клика на всю карточку для редактирования
                card.MouseLeftButtonDown += (s, e) => EditProduct(product);
                
                ProductsPanel.Children.Add(card);
            }
        }
        
        private void Filter_Changed(object sender, TextChangedEventArgs e)
        {
            ApplyFilter();
        }
        
        private void Filter_Changed(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilter();
        }
        
        private void EditProduct(dynamic product)
        {
            if (_editWindowOpen)
            {
                MessageBox.Show("Окно редактирования уже открыто!", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            var editWindow = new ProductEditWindow(product);
            editWindow.Closed += (s, e) => 
            { 
                _editWindowOpen = false; 
                LoadProducts(); 
            };
            _editWindowOpen = true;
            editWindow.Show();
        }
        
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            EditProduct(null);  // null = новый товар
        }
        
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            int productId = (int)btn.Tag;
            
            // ПРОВЕРКА НА НАЛИЧИЕ В ЗАКАЗАХ
            using (var db = new OfficeEquipmentContext())
            {
                var inOrders = db.OrderItems.Any(oi => oi.ProductId == productId);
                if (inOrders)
                {
                    MessageBox.Show("Товар есть в заказах! Удалить нельзя.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                
                var result = MessageBox.Show("Удалить товар?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    var product = db.Products.Find(productId);
                    if (product != null)
                    {
                        db.Products.Remove(product);
                        db.SaveChanges();
                        LoadProducts();
                    }
                }
            }
        }
        
        private void Orders_Click(object sender, RoutedEventArgs e)
        {
            new OrderWindow(_user).Show();
            this.Close();
        }
        
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            this.Close();
        }
    }
}










11. Windows/ProductEditWindow.xaml (РЕДАКТИРОВАНИЕ ТОВАРА)
xml
<Window x:Class="OfficeEquipment.Windows.ProductEditWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Редактирование товара" Height="500" Width="500"
        WindowStartupLocation="CenterScreen" Background="White">
    <Grid Margin="20">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
            <RowDefinition Height="Auto"/>
        </Grid.RowDefinitions>
        
        <!-- ЗАГОЛОВОК -->
        <TextBlock x:Name="TitleText" Grid.Row="0" Text="Редактирование товара"
                   FontFamily="Comic Sans MS" FontSize="18" FontWeight="Bold"
                   Foreground="#498C51" Margin="0,0,0,15"/>
        
        <!-- ID (только чтение) -->
        <TextBlock Grid.Row="1" Text="ID:" FontFamily="Comic Sans MS" FontSize="14"/>
        <TextBox x:Name="IdBox" Grid.Row="2" FontFamily="Comic Sans MS" FontSize="14"
                 Padding="5" Margin="0,0,0,8" IsReadOnly="True"/>
        
        <!-- НАИМЕНОВАНИЕ -->
        <TextBlock Grid.Row="3" Text="Наименование:" FontFamily="Comic Sans MS" FontSize="14"/>
        <TextBox x:Name="NameBox" Grid.Row="4" FontFamily="Comic Sans MS" FontSize="14"
                 Padding="5" Margin="0,0,0,8"/>
        
        <!-- КАТЕГОРИЯ -->
        <TextBlock Grid.Row="5" Text="Категория:" FontFamily="Comic Sans MS" FontSize="14"/>
        <ComboBox x:Name="CategoryBox" Grid.Row="6" FontFamily="Comic Sans MS" FontSize="14"
                  Padding="5" Margin="0,0,0,8"/>
        
        <!-- ПРОИЗВОДИТЕЛЬ -->
        <TextBlock Grid.Row="7" Text="Производитель:" FontFamily="Comic Sans MS" FontSize="14"/>
        <ComboBox x:Name="ManufacturerBox" Grid.Row="8" FontFamily="Comic Sans MS" FontSize="14"
                  Padding="5" Margin="0,0,0,8"/>
        
        <!-- КНОПКИ -->
        <StackPanel Grid.Row="9" Orientation="Horizontal" HorizontalAlignment="Right">
            <Button Content="Сохранить" Click="Save_Click" FontFamily="Comic Sans MS"
                    FontSize="14" Background="#76E383" Padding="10,5" Margin="0,0,10,0"
                    BorderThickness="0" Cursor="Hand"/>
            <Button Content="Отмена" Click="Cancel_Click" FontFamily="Comic Sans MS"
                    FontSize="14" Background="Gray" Foreground="White" Padding="10,5"
                    BorderThickness="0" Cursor="Hand"/>
        </StackPanel>
    </Grid>
</Window>












12. Windows/ProductEditWindow.xaml.cs (ШАБЛОН)
csharp
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace OfficeEquipment.Windows
{
    public partial class ProductEditWindow : Window
    {
        private dynamic _product;
        private bool _isNew;
        
        public ProductEditWindow(dynamic product)
        {
            InitializeComponent();
            
            LoadCategories();
            LoadManufacturers();
            
            if (product == null)
            {
                _isNew = true;
                TitleText.Text = "Добавление товара";
                IdBox.Text = "Новый";
            }
            else
            {
                _isNew = false;
                _product = product;
                IdBox.Text = product.ProductId.ToString();
                NameBox.Text = product.Name;
                CategoryBox.SelectedValue = product.CategoryId;
                ManufacturerBox.SelectedValue = product.ManufacturerId;
            }
        }
        
        private void LoadCategories()
        {
            using (var db = new OfficeEquipmentContext())
            {
                CategoryBox.ItemsSource = db.Categories.ToList();
                CategoryBox.DisplayMemberPath = "Name";  // ЗАМЕНИ НА СВОЁ ПОЛЕ
                CategoryBox.SelectedValuePath = "Id";
            }
        }
        
        private void LoadManufacturers()
        {
            using (var db = new OfficeEquipmentContext())
            {
                ManufacturerBox.ItemsSource = db.Manufacturers.ToList();
                ManufacturerBox.DisplayMemberPath = "Name";
                ManufacturerBox.SelectedValuePath = "Id";
            }
        }
        
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // ПРОВЕРКИ
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                MessageBox.Show("Введите наименование!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            using (var db = new OfficeEquipmentContext())
            {
                if (_isNew)
                {
                    _product = new  // ЗАМЕНИ НА СВОЙ КЛАСС Product
                    {
                        Name = NameBox.Text,
                        CategoryId = (int)CategoryBox.SelectedValue,
                        ManufacturerId = (int)ManufacturerBox.SelectedValue,
                        Price = 0,
                        StockQuantity = 0,
                        DiscountPercent = 0
                    };
                    db.Products.Add(_product);
                }
                else
                {
                    var p = db.Products.Find(_product.ProductId);
                    p.Name = NameBox.Text;
                    p.CategoryId = (int)CategoryBox.SelectedValue;
                    p.ManufacturerId = (int)ManufacturerBox.SelectedValue;
                }
                db.SaveChanges();
            }
            
            MessageBox.Show("Сохранено!", "Успех",
                MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
        
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}









. Windows/OrderWindow.xaml (СПИСОК ЗАКАЗОВ)
xml
<Window x:Class="OfficeEquipment.Windows.OrderWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Заказы" Height="600" Width="830"
        WindowStartupLocation="CenterScreen"
        Background="White">
    <DockPanel>
        <!-- ВЕРХНЯЯ ПАНЕЛЬ С КНОПКОЙ НАЗАД -->
        <DockPanel DockPanel.Dock="Top" Margin="5">
            <Button Content="Назад" Click="Button_exit" 
                    FontFamily="Comic Sans MS" FontSize="14"
                    Background="#76E383" Padding="10,5" Cursor="Hand"/>
        </DockPanel>
        
        <!-- НИЖНЯЯ ПАНЕЛЬ С КНОПКАМИ (видна только админу) -->
        <StackPanel Name="PanelBottomButton" Orientation="Horizontal" 
                    DockPanel.Dock="Bottom" Visibility="Collapsed">
            <Button Content="Добавить" Width="80" Margin="5" Click="Button_add_order"
                    FontFamily="Comic Sans MS" FontSize="14"
                    Background="#76E383" Padding="8,4" Cursor="Hand"/>
            <Button Content="Удалить" Width="80" Margin="5" Click="Button_delete_order"
                    FontFamily="Comic Sans MS" FontSize="14"
                    Background="#e74c3c" Foreground="White" Padding="8,4" Cursor="Hand"/>
        </StackPanel>
        
        <!-- СПИСОК ЗАКАЗОВ -->
        <StackPanel>
            <ListBox Name="BoxOrder" Height="500" FontFamily="Comic Sans MS">
                <ListBox.ItemTemplate>
                    <DataTemplate>
                        <DockPanel Height="100">
                            <!-- ПРАВАЯ ЧАСТЬ - ДАТА ДОСТАВКИ -->
                            <Border DockPanel.Dock="Right"
                                    BorderBrush="Gray"
                                    BorderThickness="1"
                                    Margin="5"
                                    MinWidth="120"
                                    Background="#76E383">
                                <TextBlock Text="{Binding DeliveryDate, StringFormat='dd.MM.yyyy'}"
                                           VerticalAlignment="Center"
                                           HorizontalAlignment="Center"
                                           FontSize="14" FontWeight="Bold"/>
                            </Border>
                            
                            <!-- ЛЕВАЯ ЧАСТЬ - ИНФОРМАЦИЯ О ЗАКАЗЕ -->
                            <Border BorderBrush="Gray"
                                    BorderThickness="1"
                                    Margin="5"
                                    Width="640"
                                    Background="White">
                                <StackPanel Margin="5">
                                    <TextBlock FontWeight="Bold" FontSize="14">
                                        <Run Text="Заказ №"/>
                                        <Run Text="{Binding OrderId}"/>
                                    </TextBlock>
                                    <TextBlock Text="{Binding OrderStatus.StatusName}" FontSize="12"/>
                                    <TextBlock Text="{Binding PickupPoint.Address}" FontSize="12"/>
                                    <TextBlock Text="{Binding OrderDate, StringFormat='dd.MM.yyyy'}" FontSize="12"/>
                                </StackPanel>
                            </Border>
                        </DockPanel>
                    </DataTemplate>
                </ListBox.ItemTemplate>
            </ListBox>
        </StackPanel>
    </DockPanel>
</Window>











2. Windows/OrderWindow.xaml.cs (ЛОГИКА)
csharp
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using OfficeEquipment.ModelsDB;  // ЗАМЕНИ НА СВОЙ namespace

namespace OfficeEquipment.Windows
{
    public partial class OrderWindow : Window
    {
        private OfficeEquipmentContext _context;  // ЗАМЕНИ НА СВОЙ КОНТЕКСТ
        private dynamic _currentUser;
        
        public OrderWindow(dynamic user)
        {
            InitializeComponent();
            _context = new OfficeEquipmentContext();
            _currentUser = user;
            
            LoadOrders();
            
            // ПРОВЕРКА РОЛИ - кнопки видны только админу
            if (_currentUser.Role.RoleName == "Администратор")  // ЗАМЕНИ НА СВОЮ ПРОВЕРКУ
            {
                BoxOrder.MouseDoubleClick += BoxOrder_MouseDoubleClick;
                PanelBottomButton.Visibility = Visibility.Visible;
            }
        }
        
        private void LoadOrders()
        {
            var orders = _context.Orders
                .Include(o => o.PickupPoint)
                .Include(o => o.OrderStatus)
                .ToList();
            BoxOrder.ItemsSource = orders;
        }
        
        // ДВОЙНОЙ КЛИК ДЛЯ РЕДАКТИРОВАНИЯ (только админ)
        private void BoxOrder_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var order = BoxOrder.SelectedItem as Order;  // ЗАМЕНИ Order НА СВОЙ КЛАСС
            if (order != null)
            {
                var editWindow = new EditOrderWindow(order);
                if (editWindow.ShowDialog() == true)
                {
                    LoadOrders();
                }
            }
        }
        
        // ДОБАВИТЬ ЗАКАЗ
        private void Button_add_order(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddOrderWindow();
            if (addWindow.ShowDialog() == true)
            {
                LoadOrders();
            }
        }
        
        // УДАЛИТЬ ЗАКАЗ
        private void Button_delete_order(object sender, RoutedEventArgs e)
        {
            var order = BoxOrder.SelectedItem as Order;  // ЗАМЕНИ Order НА СВОЙ КЛАСС
            if (order == null)
            {
                MessageBox.Show("Выберите заказ для удаления", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            var result = MessageBox.Show("Удалить заказ?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
                
            if (result == MessageBoxResult.Yes)
            {
                // СНАЧАЛА УДАЛЯЕМ ПОЗИЦИИ ЗАКАЗА
                var orderItems = _context.OrderItems.Where(oi => oi.OrderId == order.OrderId);
                _context.OrderItems.RemoveRange(orderItems);
                
                // ПОТОМ УДАЛЯЕМ САМ ЗАКАЗ
                _context.Orders.Remove(order);
                _context.SaveChanges();
                
                LoadOrders();
            }
        }
        
        // КНОПКА НАЗАД
        private void Button_exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}











3. Windows/AddOrderWindow.xaml (ДОБАВЛЕНИЕ ЗАКАЗА)
xml
<Window x:Class="OfficeEquipment.Windows.AddOrderWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Добавление заказа" Height="350" Width="400"
        WindowStartupLocation="CenterScreen"
        Background="White">
    <StackPanel VerticalAlignment="Center" HorizontalAlignment="Center">
        <StackPanel Margin="10">
            
            <!-- АРТИКУЛ (КОД) -->
            <StackPanel Orientation="Horizontal" Margin="4">
                <TextBox Name="BoxCode" Width="150" FontFamily="Comic Sans MS" FontSize="14"/>
                <TextBlock Text="Артикул" Margin="5,0,0,0" FontFamily="Comic Sans MS" FontSize="14"/>
            </StackPanel>
            
            <!-- СТАТУС -->
            <StackPanel Orientation="Horizontal" Margin="4">
                <ComboBox Name="BoxStatus" Width="150" FontFamily="Comic Sans MS" FontSize="14"
                          SelectedIndex="0"/>
                <TextBlock Text="Статус заказа" Margin="5,0,0,0" FontFamily="Comic Sans MS" FontSize="14"/>
            </StackPanel>
            
            <!-- ПУНКТ ВЫДАЧИ -->
            <StackPanel Orientation="Horizontal" Margin="4">
                <TextBox Name="BoxDelivery" Width="150" FontFamily="Comic Sans MS" FontSize="14"/>
                <TextBlock Text="Пункт выдачи" Margin="5,0,0,0" FontFamily="Comic Sans MS" FontSize="14"/>
            </StackPanel>
            
            <!-- ДАТА ЗАКАЗА -->
            <StackPanel Orientation="Horizontal" Margin="4">
                <TextBox Name="BoxDateOrder" Width="150" FontFamily="Comic Sans MS" FontSize="14"/>
                <TextBlock Text="Дата заказа" Margin="5,0,0,0" FontFamily="Comic Sans MS" FontSize="14"/>
            </StackPanel>
            
            <!-- ДАТА ВЫДАЧИ -->
            <StackPanel Orientation="Horizontal" Margin="4">
                <TextBox Name="BoxDateDelivery" Width="150" FontFamily="Comic Sans MS" FontSize="14"/>
                <TextBlock Text="Дата выдачи" Margin="5,0,0,0" FontFamily="Comic Sans MS" FontSize="14"/>
            </StackPanel>
            
        </StackPanel>
        
        <!-- КНОПКИ -->
        <StackPanel Orientation="Horizontal" HorizontalAlignment="Center">
            <Button Content="Добавить" Width="80" Click="Button_add" Margin="5"
                    FontFamily="Comic Sans MS" FontSize="14"
                    Background="#76E383" Padding="8,4" Cursor="Hand"/>
            <Button Content="Отмена" Width="80" Click="Button_exit" Margin="5"
                    FontFamily="Comic Sans MS" FontSize="14"
                    Background="Gray" Foreground="White" Padding="8,4" Cursor="Hand"/>
        </StackPanel>
    </StackPanel>
</Window>














4. Windows/AddOrderWindow.xaml.cs
csharp
using System;
using System.Linq;
using System.Windows;
using OfficeEquipment.ModelsDB;  // ЗАМЕНИ НА СВОЙ namespace

namespace OfficeEquipment.Windows
{
    public partial class AddOrderWindow : Window
    {
        private OfficeEquipmentContext _context;  // ЗАМЕНИ НА СВОЙ КОНТЕКСТ
        
        public AddOrderWindow()
        {
            InitializeComponent();
            _context = new OfficeEquipmentContext();
            
            // ЗАГРУЖАЕМ СТАТУСЫ В COMBOBOX
            BoxStatus.ItemsSource = _context.OrderStatuses.ToList();
            BoxStatus.DisplayMemberPath = "StatusName";  // ЗАМЕНИ НА СВОЁ ПОЛЕ
        }
        
        private void Button_add(object sender, RoutedEventArgs e)
        {
            // ПРОВЕРКА ЗАПОЛНЕНИЯ
            if (string.IsNullOrWhiteSpace(BoxDateDelivery.Text) ||
                string.IsNullOrWhiteSpace(BoxDateOrder.Text) ||
                string.IsNullOrWhiteSpace(BoxCode.Text) ||
                string.IsNullOrWhiteSpace(BoxDelivery.Text))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            try
            {
                // СОЗДАЁМ НОВЫЙ ЗАКАЗ
                var order = new Order  // ЗАМЕНИ Order НА СВОЙ КЛАСС
                {
                    OrderDate = DateTime.Parse(BoxDateOrder.Text),
                    DeliveryDate = DateTime.Parse(BoxDateDelivery.Text),
                    PickupCode = int.Parse(BoxCode.Text),  // ЗАМЕНИ НА СВОЁ ПОЛЕ
                    PickupPoint = _context.PickupPoints
                        .FirstOrDefault(p => p.Address == BoxDelivery.Text),  // ЗАМЕНИ Address
                    OrderStatus = BoxStatus.SelectedItem as OrderStatus,  // ЗАМЕНИ OrderStatus
                    UserId = 1  // ЗАМЕНИ НА ID ТЕКУЩЕГО ПОЛЬЗОВАТЕЛЯ
                };
                
                _context.Orders.Add(order);
                _context.SaveChanges();
                
                DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void Button_exit(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}










5. Windows/EditOrderWindow.xaml (РЕДАКТИРОВАНИЕ)
xml
<Window x:Class="OfficeEquipment.Windows.EditOrderWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Изменение заказа" Height="350" Width="400"
        WindowStartupLocation="CenterScreen"
        Background="White">
    <StackPanel VerticalAlignment="Center" HorizontalAlignment="Center">
        <StackPanel Name="PanelOrder" Margin="10">
            
            <!-- АРТИКУЛ -->
            <StackPanel Orientation="Horizontal" Margin="4">
                <TextBox Name="BoxCode" Width="150" FontFamily="Comic Sans MS" FontSize="14"
                         Text="{Binding PickupCode}"/>
                <TextBlock Text="Артикул" Margin="5,0,0,0" FontFamily="Comic Sans MS" FontSize="14"/>
            </StackPanel>
            
            <!-- СТАТУС -->
            <StackPanel Orientation="Horizontal" Margin="4">
                <ComboBox Name="BoxStatus" Width="150" FontFamily="Comic Sans MS" FontSize="14"/>
                <TextBlock Text="Статус заказа" Margin="5,0,0,0" FontFamily="Comic Sans MS" FontSize="14"/>
            </StackPanel>
            
            <!-- ПУНКТ ВЫДАЧИ -->
            <StackPanel Orientation="Horizontal" Margin="4">
                <TextBox Name="BoxDelivery" Width="150" FontFamily="Comic Sans MS" FontSize="14"
                         Text="{Binding PickupPoint.Address}"/>
                <TextBlock Text="Пункт выдачи" Margin="5,0,0,0" FontFamily="Comic Sans MS" FontSize="14"/>
            </StackPanel>
            
            <!-- ДАТА ЗАКАЗА -->
            <StackPanel Orientation="Horizontal" Margin="4">
                <TextBox Name="BoxDateOrder" Width="150" FontFamily="Comic Sans MS" FontSize="14"
                         Text="{Binding OrderDate, StringFormat='dd.MM.yyyy'}"/>
                <TextBlock Text="Дата заказа" Margin="5,0,0,0" FontFamily="Comic Sans MS" FontSize="14"/>
            </StackPanel>
            
            <!-- ДАТА ВЫДАЧИ -->
            <StackPanel Orientation="Horizontal" Margin="4">
                <TextBox Name="BoxDateDelivery" Width="150" FontFamily="Comic Sans MS" FontSize="14"
                         Text="{Binding DeliveryDate, StringFormat='dd.MM.yyyy'}"/>
                <TextBlock Text="Дата выдачи" Margin="5,0,0,0" FontFamily="Comic Sans MS" FontSize="14"/>
            </StackPanel>
            
        </StackPanel>
        
        <!-- КНОПКИ -->
        <StackPanel Orientation="Horizontal" HorizontalAlignment="Center">
            <Button Content="Сохранить" Width="80" Click="Button_save" Margin="5"
                    FontFamily="Comic Sans MS" FontSize="14"
                    Background="#76E383" Padding="8,4" Cursor="Hand"/>
            <Button Content="Отмена" Width="80" Click="Button_exit" Margin="5"
                    FontFamily="Comic Sans MS" FontSize="14"
                    Background="Gray" Foreground="White" Padding="8,4" Cursor="Hand"/>
        </StackPanel>
    </StackPanel>
</Window>











6. Windows/EditOrderWindow.xaml.cs
csharp
using System;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using OfficeEquipment.ModelsDB;  // ЗАМЕНИ НА СВОЙ namespace

namespace OfficeEquipment.Windows
{
    public partial class EditOrderWindow : Window
    {
        private OfficeEquipmentContext _context;  // ЗАМЕНИ НА СВОЙ КОНТЕКСТ
        private Order _order;  // ЗАМЕНИ Order НА СВОЙ КЛАСС
        
        public EditOrderWindow(Order order)
        {
            InitializeComponent();
            _context = new OfficeEquipmentContext();
            _order = order;
            
            // УСТАНАВЛИВАЕМ КОНТЕКСТ ДАННЫХ
            PanelOrder.DataContext = _order;
            
            // ЗАГРУЖАЕМ СТАТУСЫ
            BoxStatus.ItemsSource = _context.OrderStatuses.ToList();
            BoxStatus.DisplayMemberPath = "StatusName";  // ЗАМЕНИ НА СВОЁ ПОЛЕ
            BoxStatus.SelectedItem = _order.OrderStatus;
        }
        
        private void Button_save(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(BoxDateDelivery.Text) ||
                string.IsNullOrWhiteSpace(BoxDateOrder.Text) ||
                string.IsNullOrWhiteSpace(BoxCode.Text) ||
                string.IsNullOrWhiteSpace(BoxDelivery.Text))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            try
            {
                _order.OrderDate = DateTime.Parse(BoxDateOrder.Text);
                _order.DeliveryDate = DateTime.Parse(BoxDateDelivery.Text);
                _order.PickupCode = int.Parse(BoxCode.Text);
                _order.PickupPoint = _context.PickupPoints
                    .FirstOrDefault(p => p.Address == BoxDelivery.Text);
                _order.OrderStatus = BoxStatus.SelectedItem as OrderStatus;
                
                _context.Entry(_order).State = EntityState.Modified;
                _context.SaveChanges();
                
                DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void Button_exit(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}






1с
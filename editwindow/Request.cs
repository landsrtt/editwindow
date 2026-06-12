
1. Login окно:
удали в окне логин начиная с Title, то что сверху Title удалять не надо и вот код начинается этот снизу с Title, его вставляй:
 
            Title="Вход в систему" Height="400" Width="350"
        WindowStartupLocation="CenterScreen" Background="White"
        Icon="/Assets/icon.ico">
    <Grid>
        <StackPanel VerticalAlignment="Center" Margin="30">
            
            <!-- ЛОГОТИП -->
            <Image Source="/Assets/logo.png" Width="120" Height="120"/>
            
            <TextBlock Text="Техника для больницы" FontFamily="Cooper Black" 
                       FontSize="22" Foreground="#E4C418" 
                       HorizontalAlignment="Center" Margin="0,15"/>
            
            <TextBlock Text="Логин:" FontFamily="Cooper Black" FontSize="14" 
                       Margin="0,10,0,0"/>
            <TextBox x:Name="LoginBox" FontFamily="Cooper Black" FontSize="14" 
                     Margin="0,0,0,10"/>
            
            <TextBlock Text="Пароль:" FontFamily="Cooper Black" FontSize="14"/>
            <PasswordBox x:Name="PasswordBox" FontFamily="Cooper Black" 
                         FontSize="14" Margin="0,0,0,15"/>
            
            <Button Content="Войти" Click="Login_Click" FontFamily="Cooper Black" 
                    FontSize="14" Background="#E4C418" Foreground="Black" 
                    Margin="0,10,0,0" Height="35"/>
        </StackPanel>
    </Grid>
</Window>
2. Login cs
короче в самом верху если у тебя нет таких using, то добавь, если есть, то не добавляй
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
 
потом короче вставляй код ПОСЛЕ  public LoginWindow()
        {
            InitializeComponent();
        } (ВСЕ ОСТАЛЬНЫЕ СКОБКИ ПОСЛЕ ЭТОЙ ОДНОЙ УДАЛИ)
вот как скобка закрылась вставляй дальше это
                       
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
            
            using (var db = new ТвойКонтекст())             {
                var user = db.Users
                    .Include(u => u.Role)
                    .FirstOrDefault(u => u.Login == login && u.Password == password);
                
                if (user == null)
                {
                    MessageBox.Show("Неверный логин или пароль", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                
                if (user.Role.RoleName == "Администратор")
                {
                    new AdminWindow(user).Show();
                }
                else
                {
                    new ClientWindow(user).Show();
                }
                
                this.Close();
            }
        }
    }
}
3. Потом создай папку в проекте и назови UserControls и в папке создай окно и назови ProductCard.xaml
удали там все и вставь 
<UserControl x:Class="EnchDE.UserControls.ProductCard"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             Height="130" Width="900">
    
    <Border x:Name="MainBorder" CornerRadius="5" Margin="0,0,0,5" 
            BorderBrush="#E4C418" BorderThickness="1" Background="White">
        <Grid Margin="8">
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="110"/>
                <ColumnDefinition Width="*"/>
                <ColumnDefinition Width="100"/>
            </Grid.ColumnDefinitions>
            
            <!-- ФОТО -->
            <Image x:Name="ProductImage" Grid.Column="0" 
                   Width="90" Height="90" Stretch="Uniform"/>
            
            <!-- ИНФОРМАЦИЯ -->
            <StackPanel Grid.Column="1" Margin="15,0">
                <TextBlock x:Name="NameText" FontFamily="Cooper Black" 
                           FontSize="15" FontWeight="Bold"/>
                <TextBlock x:Name="CategoryText" FontFamily="Cooper Black" 
                           FontSize="12" Margin="0,3"/>
                <TextBlock x:Name="DescriptionText" FontFamily="Cooper Black" 
                           FontSize="12" TextWrapping="Wrap"/>
                <TextBlock x:Name="ManufacturerText" FontFamily="Cooper Black" 
                           FontSize="12" Margin="0,3"/>
            </StackPanel>
            
            <!-- ЦЕНА И СКИДКА -->
            <StackPanel Grid.Column="2" VerticalAlignment="Center">
                <TextBlock x:Name="PriceText" FontFamily="Cooper Black" 
                           FontSize="15" FontWeight="Bold" HorizontalAlignment="Right"/>
                <TextBlock x:Name="DiscountText" FontFamily="Cooper Black" 
                           FontSize="13" Foreground="Red" HorizontalAlignment="Right" 
                           Margin="0,5,0,0"/>
            </StackPanel>
        </Grid>
    </Border>
</UserControl>
4.ProductCard cs
короче в самом верху если у тебя нет таких using, то добавь, если есть, то не добавляй
using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

потом вот вставляй
    public partial class ProductCard : UserControl
    {
        public ProductCard(dynamic product)
        {
            InitializeComponent();
            
            NameText.Text = product.Name;
            CategoryText.Text = "Категория: " + (product.Category?.Name ?? "");
            DescriptionText.Text = product.Description;
            ManufacturerText.Text = "Производитель: " + (product.Manufacturer?.Name ?? "");
            PriceText.Text = product.Price.ToString("F2") + " руб.";
            
            int discount = product.DiscountPercent;
            if (discount > 0)
            {
                DiscountText.Text = "Скидка " + discount + "%";
            }
            
            if (discount > 10)
            {
                MainBorder.Background = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#C8A2C8"));
            }
            
            string imageName = product.ImagePath;
            if (string.IsNullOrEmpty(imageName))
            {
                imageName = "picture.png";
            }
            
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + "Assets\\" + imageName;
            
            try
            {
                if (File.Exists(fullPath))
                {
                    ProductImage.Source = new BitmapImage(new Uri(fullPath));
                }
            }
            catch { }
        }
    }
}


5.  вставляй так же как и окно логина
     Title="Просмотр товаров" Height="600" Width="950"
        WindowStartupLocation="CenterScreen" Background="White"
        Icon="/Assets/icon.ico">
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
        </Grid.RowDefinitions>
        
        <!-- ШАПКА (цвет #E4C418) -->
        <Border Grid.Row="0" Background="#E4C418" Margin="0,0,0,5">
            <Grid Margin="10">
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="Auto"/>
                    <ColumnDefinition Width="*"/>
                    <ColumnDefinition Width="Auto"/>
                </Grid.ColumnDefinitions>
                
                <Image Grid.Column="0" Source="/Assets/logo.png" 
                       Width="35" Height="35"/>
                
                <TextBlock Grid.Column="1" Text="Техника для больницы" 
                           FontFamily="Cooper Black" FontSize="18" FontWeight="Bold" 
                           VerticalAlignment="Center" Margin="10,0"/>
                
                <StackPanel Grid.Column="2" Orientation="Horizontal">
                    <TextBlock x:Name="UserNameText" FontFamily="Cooper Black" 
                               FontSize="14" VerticalAlignment="Center" Margin="0,0,10,0"/>
                    <Button Content="Выход" Click="Exit_Click" FontFamily="Cooper Black" 
                            FontSize="14" Background="#00FA9A" Padding="10,5"/>
                </StackPanel>
            </Grid>
        </Border>
        
        <!-- СПИСОК ТОВАРОВ -->
        <ScrollViewer Grid.Row="1">
            <StackPanel x:Name="ProductsPanel" Margin="10"/>
        </ScrollViewer>
    </Grid>
</Window>


6. ClientWindow cs

проверь сверху
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using EnchDE.UserControls;

и это
    public partial class ClientWindow : Window
    {
        public ClientWindow(dynamic user)
        {
            InitializeComponent();
            UserNameText.Text = user.FullName;
            LoadProducts();
        }
        
        private void LoadProducts()
        {
            ProductsPanel.Children.Clear();
            
            using (var db = new ТвойКонтекст())
            {
                var products = db.Products
                    .Include(p => p.Category)
                    .Include(p => p.Manufacturer)
                    .ToList();
                
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
7. админ
как логин вставляй
         
        Title="Управление товарами" Height="600" Width="950"
        WindowStartupLocation="CenterScreen" Background="White"
        Icon="/Assets/icon.ico">
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
        </Grid.RowDefinitions>
        
        <!-- ШАПКА -->
        <Border Grid.Row="0" Background="#E4C418" Margin="0,0,0,5">
            <Grid Margin="10">
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="Auto"/>
                    <ColumnDefinition Width="*"/>
                    <ColumnDefinition Width="Auto"/>
                </Grid.ColumnDefinitions>
                
                <Image Grid.Column="0" Source="/Assets/logo.png" 
                       Width="35" Height="35"/>
                
                <TextBlock Grid.Column="1" Text="Техника для больницы - Администратор" 
                           FontFamily="Cooper Black" FontSize="18" FontWeight="Bold" 
                           VerticalAlignment="Center" Margin="10,0"/>
                
                <StackPanel Grid.Column="2" Orientation="Horizontal">
                    <TextBlock x:Name="UserNameText" FontFamily="Cooper Black" 
                               FontSize="14" VerticalAlignment="Center" Margin="0,0,10,0"/>
                    <Button Content="Выход" Click="Exit_Click" FontFamily="Cooper Black" 
                            FontSize="14" Background="#00FA9A" Padding="10,5"/>
                </StackPanel>
            </Grid>
        </Border>
        
        <Grid Grid.Row="1" Margin="10">
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="*"/>
                <ColumnDefinition Width="Auto"/>
            </Grid.ColumnDefinitions>
            
            <TextBox x:Name="SearchBox" Grid.Column="0" FontFamily="Cooper Black" 
                     FontSize="14" Margin="0,0,10,0" TextChanged="Search_Changed"/>
            
            <Button Content="Добавить товар" Click="Add_Click" Grid.Column="1"
                    FontFamily="Cooper Black" FontSize="14" 
                    Background="#00FA9A" Padding="10,5"/>
        </Grid>
        
        <ScrollViewer Grid.Row="2">
            <StackPanel x:Name="ProductsPanel" Margin="10"/>
        </ScrollViewer>
    </Grid>
</Window>
8. админ cs
сверху проверь
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using EnchDE.UserControls;

потом  это
    public partial class AdminWindow : Window
    {
        private dynamic _user;
        private List<dynamic> _allProducts;
        
        public AdminWindow(dynamic user)
        {
            InitializeComponent();
            _user = user;
            UserNameText.Text = user.FullName;
            LoadProducts();
        }
        
        private void LoadProducts()
        {
            using (var db = new ТвойКонтекст())
            {
                _allProducts = db.Products
                    .Include(p => p.Category)
                    .Include(p => p.Manufacturer)
                    .ToList();
            }
            ApplySearch();
        }
        
        private void ApplySearch()
        {
            if (_allProducts == null) return;
            
            var filtered = _allProducts.AsEnumerable();
            
            string search = SearchBox.Text.ToLower();
            if (!string.IsNullOrEmpty(search))
            {
                filtered = filtered.Where(p =>
                    p.Name.ToLower().Contains(search) ||
                    (p.Description != null && p.Description.ToLower().Contains(search)) ||
                    p.Category.Name.ToLower().Contains(search) ||
                    p.Manufacturer.Name.ToLower().Contains(search));
            }
            
            ProductsPanel.Children.Clear();
            foreach (var product in filtered)
            {
                var card = new ProductCard(product);
                card.MouseLeftButtonDown += (s, e) => EditProduct(product);
                ProductsPanel.Children.Add(card);
            }
        }
        
        private void Search_Changed(object sender, TextChangedEventArgs e)
        {
            ApplySearch();
        }
        
        private void EditProduct(dynamic product)
        {
            var editWindow = new ProductEditWindow(product);
            editWindow.ShowDialog();  // БЛОКИРУЕТ АДМИНКУ ПОКА ОКНО ОТКРЫТО
            LoadProducts();           // ОБНОВЛЯЕМ ПОСЛЕ ЗАКРЫТИЯ
        }
        
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new ProductEditWindow(null);
            editWindow.ShowDialog();  // БЛОКИРУЕТ АДМИНКУ
            LoadProducts();           // ОБНОВЛЯЕМ
        }
        
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            this.Close();
        }
    }
}
9.окно producteditwindow
        Title="Редактирование товара" Height="550" Width="500"
        WindowStartupLocation="CenterScreen" Background="White"
        Icon="/Assets/icon.ico">
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
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
            <RowDefinition Height="Auto"/>
        </Grid.RowDefinitions>
        
        <!-- ЗАГОЛОВОК -->
        <TextBlock x:Name="TitleText" Grid.Row="0" 
                   Text="Редактирование товара" FontFamily="Cooper Black" 
                   FontSize="18" Foreground="#E4C418" Margin="0,0,0,15"/>
        
        <!-- ФОТО -->
        <TextBlock Grid.Row="1" Text="Фото товара:" FontFamily="Cooper Black" FontSize="14"/>
        <Grid Grid.Row="2" Margin="0,0,0,8">
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="Auto"/>
                <ColumnDefinition Width="*"/>
            </Grid.ColumnDefinitions>
            <Image x:Name="ProductImage" Width="120" Height="100" Stretch="Uniform"/>
            <Button Grid.Column="1" Content="Выбрать фото" 
                    FontFamily="Cooper Black" FontSize="12" Background="#E4C418"
                    Width="100" Height="30" Margin="10,0" Click="SelectImage_Click"/>
        </Grid>
        
        <!-- ID -->
        <TextBlock Grid.Row="3" Text="ID:" FontFamily="Cooper Black" FontSize="14"/>
        <TextBox x:Name="IdBox" Grid.Row="4" FontFamily="Cooper Black" FontSize="14" 
                 IsReadOnly="True" Margin="0,0,0,8"/>
        
        <!-- НАИМЕНОВАНИЕ -->
        <TextBlock Grid.Row="5" Text="Наименование:" FontFamily="Cooper Black" FontSize="14"/>
        <TextBox x:Name="NameBox" Grid.Row="6" FontFamily="Cooper Black" FontSize="14" 
                 Margin="0,0,0,8"/>
        
        <!-- КАТЕГОРИЯ -->
        <TextBlock Grid.Row="7" Text="Категория:" FontFamily="Cooper Black" FontSize="14"/>
        <ComboBox x:Name="CategoryBox" Grid.Row="8" FontFamily="Cooper Black" 
                  FontSize="14" Margin="0,0,0,8"/>
        
        <!-- ОПИСАНИЕ -->
        <TextBlock Grid.Row="9" Text="Описание:" FontFamily="Cooper Black" FontSize="14"/>
        <TextBox x:Name="DescriptionBox" Grid.Row="10" FontFamily="Cooper Black" 
                 FontSize="14" TextWrapping="Wrap" Height="50" Margin="0,0,0,8"/>
        
        <!-- ПРОИЗВОДИТЕЛЬ + ЦЕНА + СКИДКА в одной строке -->
        <Grid Grid.Row="11" Margin="0,0,0,8">
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="*"/>
                <ColumnDefinition Width="10"/>
                <ColumnDefinition Width="*"/>
                <ColumnDefinition Width="10"/>
                <ColumnDefinition Width="*"/>
            </Grid.ColumnDefinitions>
            
            <StackPanel Grid.Column="0">
                <TextBlock Text="Производитель:" FontFamily="Cooper Black" FontSize="12"/>
                <ComboBox x:Name="ManufacturerBox" FontFamily="Cooper Black" FontSize="12"/>
            </StackPanel>
            
            <StackPanel Grid.Column="2">
                <TextBlock Text="Цена:" FontFamily="Cooper Black" FontSize="12"/>
                <TextBox x:Name="PriceBox" FontFamily="Cooper Black" FontSize="12"/>
            </StackPanel>
            
            <StackPanel Grid.Column="4">
                <TextBlock Text="Скидка %:" FontFamily="Cooper Black" FontSize="12"/>
                <TextBox x:Name="DiscountBox" FontFamily="Cooper Black" FontSize="12"/>
            </StackPanel>
        </Grid>
        
        <!-- КНОПКИ -->
        <StackPanel Grid.Row="14" Orientation="Horizontal" HorizontalAlignment="Right" 
                    Margin="0,10,0,0">
            <Button Content="Сохранить" Click="Save_Click" FontFamily="Cooper Black" 
                    FontSize="14" Background="#00FA9A" Padding="10,5" Margin="0,0,10,0"/>
            <Button Content="Отмена" Click="Cancel_Click" FontFamily="Cooper Black" 
                    FontSize="14" Background="Gray" Foreground="White" Padding="10,5"/>
        </StackPanel>
    </Grid>
</Window>

10/ сверху проверь
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

потом вот:
    public partial class ProductEditWindow : Window
    {
        private dynamic _product;
        private bool _isNew;
        private string _selectedImagePath;
        
        public ProductEditWindow(dynamic product)
        {
            InitializeComponent();
            
            LoadCategory();
            LoadManufacturer();
            
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
                DescriptionBox.Text = product.Description;
                PriceBox.Text = product.Price.ToString();
                DiscountBox.Text = product.DiscountPercent.ToString();
                CategoryBox.SelectedValue = product.CategoryId;
                ManufacturerBox.SelectedValue = product.ManufacturerId;
                
                // Загружаем фото
                LoadImage(product.ImagePath);
            }
        }
        
        private void LoadCategory()
        {
            using (var db = new ТвойКонтекст())
            {
                CategoryBox.ItemsSource = db.Categories.ToList();
                CategoryBox.DisplayMemberPath = "Name";
                CategoryBox.SelectedValuePath = "CategoryId";
            }
        }
        
        private void LoadManufacturer()
        {
            using (var db = new ТвойКонтекст())
            {
                ManufacturerBox.ItemsSource = db.Manufacturers.ToList();
                ManufacturerBox.DisplayMemberPath = "Name";
                ManufacturerBox.SelectedValuePath = "ManufacturerId";
            }
        }
        
        private void LoadImage(string imageName)
        {
            if (string.IsNullOrEmpty(imageName))
            {
                imageName = "picture.png";
            }
            
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + "Assets\\" + imageName;
            
            try
            {
                if (File.Exists(fullPath))
                {
                    ProductImage.Source = new BitmapImage(new Uri(fullPath));
                }
            }
            catch { }
        }
        
        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Изображения|*.jpg;*.jpeg;*.png";
            
            if (dialog.ShowDialog() == true)
            {
                _selectedImagePath = dialog.FileName;
                try
                {
                    ProductImage.Source = new BitmapImage(new Uri(_selectedImagePath));
                }
                catch { }
            }
        }
        
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Проверки
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                MessageBox.Show("Введите наименование!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            decimal price;
            if (!decimal.TryParse(PriceBox.Text, out price) || price < 0)
            {
                MessageBox.Show("Цена не может быть отрицательной!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            int discount;
            if (!int.TryParse(DiscountBox.Text, out discount) || discount < 0 || discount > 100)
            {
                MessageBox.Show("Скидка от 0 до 100!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            if (CategoryBox.SelectedValue == null)
            {
                MessageBox.Show("Выберите категорию!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            if (ManufacturerBox.SelectedValue == null)
            {
                MessageBox.Show("Выберите производителя!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            try
            {
                using (var db = new ТвойКонтекст())
                {
                    if (_isNew)
                    {
                        // Сохраняем фото
                        string imageName = "";
                        if (!string.IsNullOrEmpty(_selectedImagePath))
                        {
                            imageName = Guid.NewGuid().ToString() + 
                                       Path.GetExtension(_selectedImagePath);
                            string newPath = AppDomain.CurrentDomain.BaseDirectory + 
                                           "Assets\\" + imageName;
                            File.Copy(_selectedImagePath, newPath);
                        }
                        
                        var newProduct = new
                        {
                            Name = NameBox.Text,
                            Description = DescriptionBox.Text,
                            Price = price,
                            DiscountPercent = discount,
                            CategoryId = (int)CategoryBox.SelectedValue,
                            ManufacturerId = (int)ManufacturerBox.SelectedValue,
                            ImagePath = imageName,
                            StockQuantity = 0
                        };
                        db.Products.Add(newProduct);
                    }
                    else
                    {
                        var p = db.Products.Find(_product.ProductId);
                        if (p != null)
                        {
                            // Сохраняем новое фото
                            if (!string.IsNullOrEmpty(_selectedImagePath))
                            {
                                string imageName = Guid.NewGuid().ToString() + 
                                                   Path.GetExtension(_selectedImagePath);
                                string newPath = AppDomain.CurrentDomain.BaseDirectory + 
                                               "Assets\\" + imageName;
                                File.Copy(_selectedImagePath, newPath);
                                p.ImagePath = imageName;
                            }
                            
                            p.Name = NameBox.Text;
                            p.Description = DescriptionBox.Text;
                            p.Price = price;
                            p.DiscountPercent = discount;
                            p.CategoryId = (int)CategoryBox.SelectedValue;
                            p.ManufacturerId = (int)ManufacturerBox.SelectedValue;
                        }
                    }
                    db.SaveChanges();
                }
                
                MessageBox.Show("Сохранено!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

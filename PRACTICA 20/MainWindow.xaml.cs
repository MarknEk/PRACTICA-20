using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace PRACTICA_20
{
    public class DataManager
    {
        public void SaveToXml(Account account, string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Account));
            using (TextWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, account);
            }
        }
    }
    public partial class MainWindow : Window
    {
        private List<Account> accounts = new List<Account>();

        
        private ComboBox accountTypeComboBox;
        private TextBox numberTextBox;
        private TextBox balanceTextBox; 
        private DatePicker openingDatePicker;
        private TextBox fullNameTextBox;
        private TextBox passportTextBox;
        private DatePicker dobDatePicker;
        private CheckBox smsCheckBox;
        private CheckBox internetCheckBox;


        public MainWindow()
        {
            InitializeComponent();

            
            accountTypeComboBox = new ComboBox();
            numberTextBox = new TextBox();
            balanceTextBox = new TextBox(); 
            openingDatePicker = new DatePicker();
            fullNameTextBox = new TextBox();
            passportTextBox = new TextBox();
            dobDatePicker = new DatePicker();
            smsCheckBox = new CheckBox();
            internetCheckBox = new CheckBox();


            // Заполнение поля выбора типа вклада
            List<string> accountTypes = new List<string>
            {
                "Текущий",
                "Сберегательный",
                "Депозитный"
            };

            accountTypeComboBox.ItemsSource = accountTypes;

        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
{
    // Создаем новый объект Account из введенных данных
    Account newAccount = new Account
    {
        Number = numberTextBox.Text,
        AccountType = accountTypeComboBox.Text,
        Balance = balanceTextBox.Text,
        OpeningDate = openingDatePicker.SelectedDate ?? DateTime.Now,
        Owner = new Owner
        {
            FullName = fullNameTextBox.Text,
            PassportData = passportTextBox.Text,
            DateOfBirth = dobDatePicker.SelectedDate ?? DateTime.Now
        },
        IsSmsNotificationEnabled = smsCheckBox.IsChecked ?? false,
        IsInternetBankingEnabled = internetCheckBox.IsChecked ?? false
    };

    // Добавляем новый аккаунт в список аккаунтов
    accounts.Add(newAccount);

    // Очищаем поля ввода
    ClearInputFields();

    // Создаем экземпляр класса для управления данными
    DataManager dataManager = new DataManager();

    // Сохраняем новый аккаунт в файл XML
    dataManager.SaveToXml(newAccount, "account.xml");

    // Выводим сообщение об успешном сохранении
    MessageBox.Show("Данные были успешно сохранены");
}







private void ShowDataButton_Click(object sender, RoutedEventArgs e)
    {
        dataListBox.Items.Clear();

        // Проверяем, существует ли файл
        if (File.Exists("account.xml"))
        {
            // Создаем экземпляр XmlSerializer для десериализации данных
            XmlSerializer serializer = new XmlSerializer(typeof(Account));

            // Открываем файл для чтения
            using (FileStream fs = new FileStream("account.xml", FileMode.Open))
            {
                // Десериализуем данные из файла
                Account account = (Account)serializer.Deserialize(fs);

                // Добавляем данные в dataListBox
                dataListBox.Items.Add($"{account.Number} - {account.Owner.FullName}");
            }
        }
        else
        {
            MessageBox.Show("Файл с данными не найден.");
        }
    }


    private void ClearInputFields()
        {
            numberTextBox.Text = "";
            accountTypeComboBox.SelectedIndex = -1;
            balanceTextBox.Text = "";
            openingDatePicker.SelectedDate = DateTime.Now;
            fullNameTextBox.Text = "";
            passportTextBox.Text = "";
            dobDatePicker.SelectedDate = DateTime.Now;
            smsCheckBox.IsChecked = false;
            internetCheckBox.IsChecked = false;
        }
    }

    public class Account
    {
        public string Number { get; set; }
        public string AccountType { get; set; }
        public string Balance { get; set; }
        public DateTime OpeningDate { get; set; }
        public Owner Owner { get; set; }
        public bool IsSmsNotificationEnabled { get; set; }
        public bool IsInternetBankingEnabled { get; set; }
    }

    public class Owner
    {
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PassportData { get; set; }
    }
}

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SarGoldACC.WpfApp.Views.OperationsForm;

public partial class ReceiveGold : Window
{
    public ReceiveGold()
    {
        InitializeComponent();
    }
    private void MainTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (MainTypeComboBox.SelectedItem is ComboBoxItem selectedItem)
        {
            string selectedType = selectedItem.Content.ToString();

            if (selectedType == "آبشده")
            {
                RingNumberPanel.Visibility = Visibility.Visible;
                DescriptionTextBox.Text = "دریافت متفرقه"; // پیش فرض برای هر دو یکیه
            }
            else // متفرقه
            {
                RingNumberPanel.Visibility = Visibility.Collapsed;
                DescriptionTextBox.Text = "دریافت متفرقه";
            }
        }
    }

    private void DealTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DealTypeComboBox.SelectedItem is ComboBoxItem selectedItem)
        {
            string dealType = selectedItem.Content.ToString();

            if (dealType == "شرطی")
            {
                GoldPurityTextBox.Text = "750";
                GoldPurityTextBox.IsReadOnly = true; // غیر قابل تغییر
                GoldPurityTextBox.Background = Brushes.LightGray; // مثلا برای فهم بیشتر کاربر
            }
            else if (dealType == "قطعی")
            {
                GoldPurityTextBox.Text = "";
                GoldPurityTextBox.IsReadOnly = false; // قابل ویرایش
                GoldPurityTextBox.Background = Brushes.White;
            }
        }
    }

    private void SubmitButton_Click(object sender, RoutedEventArgs e)
    {
        // در اینجا میتونی داده‌ها رو ذخیره یا ارسال کنی
        MessageBox.Show("فرم با موفقیت ثبت شد!");
    }
}
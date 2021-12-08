using Chernovik.db;
using Chernovik.Views;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Chernovik
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window   
    {  
        static MainWindow window;

        public static void Navigate(Page page)
            {
                window.mainFrame.Navigate(page);
            }

            private void BackPage(object sender, RoutedEventArgs e)
            {
                if (mainFrame.CanGoBack)
                    mainFrame.GoBack();
            }

            private void ForwardPage(object sender, RoutedEventArgs e)
            {
                if (mainFrame.CanGoForward)
                    mainFrame.GoForward();
            }
      
        public MainWindow()
        {
            InitializeComponent();
            window = this;
            Navigate(new MaterialList());

          

            //var connection = DBInstance.Get();
            //string path = @"C:\Users\User\Desktop\Практика\Сессия 1\materialsupplier_b_import.csv";
            //var materials = connection.Material.ToList();
            //var types = connection.MaterialType.ToList();
            //var supliers = connection.Supplier.ToList();
            //var rows = File.ReadAllLines(path);

            //connection.MaterialType.Add(new MaterialType { Title = "Пресс" });
            //    connection.MaterialType.Add(new MaterialType { Title = "Нарезка" });
            //    connection.MaterialType.Add(new MaterialType { Title = "Рулон" });
            //    connection.MaterialType.Add(new MaterialType { Title = "Гранулы" });

            //for (int i = 1; i < rows.Length; i++)
            //{
            //    var cols = rows[i].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            //    connection.Material.Add(new Material
            //    {
            //        Title = cols[0],
            //        MaterialType = types.First(s => s.Title == cols[1]),
            //        Image = cols[2],
            //        Cost = decimal.Parse(cols[3]),
            //        CountInStock = int.Parse(cols[4]),
            //        MinCount = int.Parse(cols[5]),
            //        CountInPack = int.Parse(cols[6]),
            //        Unit = cols[7]
            //    }) ;

            //}
            //for (int i = 1; i < rows.Length; i++)
            //{
            //    var cols = rows[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            //    connection.Supplier.Add(new Supplier
            //    {
            //       Title = cols[0],
            //       INN = cols[2],
            //       StartDate = DateTime.Parse(cols[4]),
            //       SupplierType = cols[1],
            //       QualityRating = int.Parse(cols[3])
            //    }) ;

            //}
            //    for (int i = 1; i < rows.Length; i++)
            //    {
            //        var cols = rows[i].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            //        Material material = materials.First(s => s.Title == cols[0]);

            //        Supplier supplier = supliers.First(s => s.Title == cols[1]);

            //        supplier.Material.Add(material);


            //}

            //    connection.SaveChanges();
        }
    }
}

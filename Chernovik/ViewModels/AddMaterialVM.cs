using Chernovik.db;
using Chernovik.mvvm;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Chernovik.ViewModels
{
    public class AddMaterialVM : BaseViewModel
    {
        private string searchText = "";
        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                Search();
            }
        }

        private BitmapImage imageMaterial;
        public BitmapImage ImageMaterial
        {
            get => imageMaterial;
            set
            {
                imageMaterial = value;
                SignalChanged();
            }
        }
        List<Supplier> searchResult;

        public Supplier SelectedMaterialSupplier { get; set; }
        private ObservableCollection<Supplier> selectedMaterialSuppliers = new ObservableCollection<Supplier>();
        public ObservableCollection<Supplier> SelectedMaterialSuppliers
        {
            get => selectedMaterialSuppliers;
            set
            {
                selectedMaterialSuppliers = value;
                SignalChanged();
            }
        }

        public Material AddMaterial { get;set; }
  

        public Supplier SelectedSupplier { get; set; }
        private List<Supplier> supplier;
        public List<Supplier> Supplier {
            get => supplier; 
            set
            {
                supplier = value;
                SignalChanged();
            }

        }

        public List<MaterialType> MaterialTypes { get; set; }
      public MaterialType SelectedMaterialType { get; set; }

        public CustomCommand SelectImage { get; set; }
        public CustomCommand RemoveSupplier { get; set; }
        public CustomCommand AddSupplier { get; set; }
        public CustomCommand Save { get; set; }
        public CustomCommand Cancel { get; set; }

        public AddMaterialVM(Material material)
        {
            var connection = DBInstance.Get();
            Supplier = connection.Supplier.ToList();
            MaterialTypes = connection.MaterialType.ToList();

            if (material == null)
            {
                AddMaterial = new Material { Image = @"\materials\picture.png", MaterialType = MaterialTypes.First()};
            }
            else
            {
                AddMaterial = new Material
                {
                    ID = material.ID,
                    Title = material.Title,
                    MaterialType = material.MaterialType,
                    MinCount = material.MinCount,
                    Cost = material.Cost,
                    MaterialTypeID = material.MaterialTypeID,
                    MaterialCountHistory = material.MaterialCountHistory,
                    ProductMaterial = material.ProductMaterial,
                    CountInPack = material.CountInPack,
                    CountInStock = material.CountInStock,
                    Supplier = material.Supplier,
                    Image = material.Image,
                    Description = material.Description,
                    Unit = material.Unit
                };
                if (material.Supplier != null)
                {
                     SelectedMaterialSuppliers = new ObservableCollection<Supplier>(material.Supplier);
                }
            } 
            SelectedMaterialType = AddMaterial.MaterialType;
            string directory = Environment.CurrentDirectory;
            ImageMaterial = GetImageFromPath(directory.Substring(0, directory.Length - 10) + "\\" + AddMaterial.Image);

            SelectImage = new CustomCommand(() =>
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == true)
                {
                    try
                    {
                        var info = new FileInfo(ofd.FileName);
                        ImageMaterial = GetImageFromPath(ofd.FileName);
                        AddMaterial.Image = $"/materials/{info.Name}";
                        var newPath = directory.Substring(0, directory.Length - 10) + AddMaterial.Image;
                        File.Copy(ofd.FileName, newPath);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            });

            AddSupplier = new CustomCommand(() =>
            {
                if (SelectedSupplier == null)
                {
                    MessageBox.Show("Нужно выбрать поставщика из выпадающего списка!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                } 
                else if (SelectedMaterialSuppliers.Contains(SelectedSupplier))
                {
                    MessageBox.Show("Материал уже содержит выбранный элемент!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                SelectedMaterialSuppliers.Add(SelectedSupplier);
                SignalChanged("SelectedMaterialSuppliers");
                }
              
            });

            RemoveSupplier = new CustomCommand(() =>
            {
                if (SelectedSupplier == null)
                {
                    MessageBox.Show("Нужно выбрать поставщика из списка !", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                else
                {
                     SelectedMaterialSuppliers.Remove(SelectedMaterialSupplier);
                     SignalChanged("SelectedMaterialSuppliers");
                }
                
            });

            Save = new CustomCommand(() =>
            {
                if (AddMaterial.Cost < 0 || AddMaterial.CountInStock < 0)
                {
                    MessageBox.Show("Проверьте правильность заполнения данных!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                MessageBoxResult result = MessageBox.Show("Сохранить изменения?", "Подтвердите действие", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        AddMaterial.Supplier = SelectedMaterialSuppliers;
                        if (AddMaterial.ID == 0)
                            connection.Material.Add(AddMaterial);
                        else
                        {
                        connection.Entry(material).CurrentValues.SetValues(AddMaterial);
                        material.Supplier = AddMaterial.Supplier;
                        } 
                        connection.SaveChanges();

                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.DataContext == this)  CloseWin(window);
                        }

                        SignalChanged("Material");
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    };
                }
                else return;
               
            });

            Cancel = new CustomCommand(() =>
            {
                MessageBoxResult result = MessageBox.Show("Отменить изменения?", "Подтвердите действие", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.DataContext == this)
                        {
                            CloseWin(window);
                        }
                    }
                else return;
            });

            searchResult = connection.Supplier.ToList();
        }

        private BitmapImage GetImageFromPath(string url)
        {
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.UriSource = new Uri(url, UriKind.Absolute);
            img.EndInit();
            return img;
        }

        private void Search()
        {
            var search = SearchText.ToLower();
            searchResult = DBInstance.Get().Supplier
                        .Where(c => c.Title.ToLower().Contains(search)).ToList();
            Supplier = searchResult;
            SignalChanged("Supplier");
        }

        public void CloseWin(object obj)
        {
            Window win = obj as Window;
            win.Close();
        }
    }
}

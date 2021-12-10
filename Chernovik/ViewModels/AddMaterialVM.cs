using Chernovik.db;
using Chernovik.mvvm;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

        public Material AddMaterial { get;set; }
        public MaterialType SelectedMaterialType { get; set; }

        public List<Supplier> Supplier { get; set; }
        public List<MaterialType> MaterialType { get; set; }

        public CustomCommand SelectImage { get; set; }

        public AddMaterialVM(Material material)
        {
            Supplier = DBInstance.Get().Supplier.ToList();
            MaterialType = DBInstance.Get().MaterialType.ToList();

            if (material == null)
            {
                AddMaterial = new Material();
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
                    CountInPack = material.CountInPack,
                    CountInStock = material.CountInStock,
                    Supplier = material.Supplier,
                    Image = material.Image,
                    Description = material.Description,
                    Unit = material.Unit
                };
                SelectedMaterialType = material.MaterialType;
                string directory = Environment.CurrentDirectory;
                ImageMaterial = GetImageFromPath(directory.Substring(0,directory.Length - 10) + "\\" + material.Image);
            }
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
                        var newPath = Environment.CurrentDirectory + AddMaterial.Image;
                        File.Copy(ofd.FileName, newPath);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            });




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
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Outlands_Adventure_Launcher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Populate(196, 256);
        }

        private void ListView_Click(object sender, EventArgs e)
        {
            string selected = listView.SelectedItems[0].SubItems[0].Text;
            MessageBox.Show(selected);
        }

        private void Rellenar_Click(object sender, EventArgs e)
        {
            Populate(206, 256);
        }

        private void Limpiar_Click(object sender, EventArgs e)
        {
            MessageBox.Show(listView.Items[0].Text);
            MessageBox.Show(listView.Items[1].Text);
            MessageBox.Show(listView.Items[2].Text);
        }

        private void LargeImages_Click(object sender, EventArgs e)
        {
            //ResizeImageView(206, 256);
            ResizeImageView(206, 256);
        }

        private void SmallImages_Click(object sender, EventArgs e)
        {
            //ResizeImageView(100, 140);
            ResizeImageView(100, 140);
        }

        private void Populate(int xSize, int ySize)
        {
            listView.Items.Clear();
            imageList.Images.Clear();

            List<string> nombreImagenes = new List<string>();
            string folderPath = "C:/Users/thena/Desktop/GTX 1060/Imágenes";

            imageList.ImageSize = new Size(xSize, ySize);
            imageList.ColorDepth = ColorDepth.Depth32Bit;

            string[] paths;
            paths = Directory.GetFiles(folderPath);

            try
            {
                foreach (string path in paths)
                {
                    imageList.Images.Add(Image.FromFile(path));
                    nombreImagenes.Add(Path.GetFileNameWithoutExtension(path));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }

            for (int imagenActual = 0; imagenActual < nombreImagenes.Count; imagenActual++)
            {
                listView.Items.Add(nombreImagenes[imagenActual], imagenActual);
            }

            listView.LargeImageList = imageList;
        }

        private void ResizeImageView(int xSize, int ySize)
        {
            List<Image> imagesBackUp = new List<Image>();
            List<string> imagesName = new List<string>();

            foreach (Image currentImage in imageList.Images)
            {
                imagesBackUp.Add(currentImage);
            }

            for (int currentImage = 0; currentImage < listView.Items.Count; currentImage++)
            {
                imagesName.Add(listView.Items[currentImage].Text);
            }

            listView.Items.Clear();
            imageList.Images.Clear();

            imageList.ImageSize = new Size(xSize, ySize);
            imageList.ColorDepth = ColorDepth.Depth32Bit;

            for (int currentImage = 0; currentImage < imagesBackUp.Count; currentImage++)
            {
                imageList.Images.Add(imagesBackUp[currentImage]);
                listView.Items.Add(imagesName[currentImage], currentImage);
            }

            listView.LargeImageList = imageList;
        }





        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Pen greenPen = new Pen(Color.Green, 2);
            Point p1 = new Point(20, 20);
            Point p2 = new Point(20, 200);
            e.Graphics.DrawLine(greenPen, p1, p2);
            greenPen.Dispose();
        }
    }
}

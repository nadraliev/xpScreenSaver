
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageBounce
{

    using Winforms = System.Windows.Forms;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Width = SystemParameters.VirtualScreenWidth;
            this.Height = SystemParameters.VirtualScreenHeight;
            this.Left = 0;
            this.Top = 0;
        }



        ThicknessAnimation animation;
        

        public double y(double x)
        {
            return k * x + b;   //change movement path
        }

        double offset = 2;
        double k = 2;
        double b = -1;

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Image im;
            Winforms.OpenFileDialog openFileDialog = new Winforms.OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF";
            openFileDialog.FilterIndex = 2;
            var result = openFileDialog.ShowDialog();
            if (result == Winforms.DialogResult.OK)
            {

                canvas.Children.Clear();
                if (animation != null)
                    animation.Freeze();
                k = 2;
                b = -1;

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(openFileDialog.FileName, UriKind.Absolute);
                bitmap.EndInit();

                im = new Image();

                im.Source = bitmap;

                im.Margin = new Thickness(0, 0, 0, 0);

                im.MaxHeight = 300;
                im.MaxWidth = 300;
                canvas.Children.Add(im);

                im.Loaded += Im_Loaded;
            }

        }

        private void Im_Loaded(object sender, RoutedEventArgs e)
        {
            Image im = (Image)canvas.Children[0];
            animation = new ThicknessAnimation();
            animation.To = new Thickness(im.Margin.Left + offset, y(im.Margin.Left + offset), 0, 0);
            animation.Duration = TimeSpan.FromTicks(1);
            animation.Completed += Animation_Completed;

            im.BeginAnimation(MarginProperty, animation);
        }

        private void Animation_Completed(object sender, EventArgs e)
        {
            if (!animation.IsFrozen)
            {
                Image im = (Image)canvas.Children[0];
                if (im.Margin.Left <= 0)
                {
                    k *= -1;
                    offset *= -1;
                }
                if (im.Margin.Left >= canvas.ActualWidth - im.ActualWidth)
                {
                    k *= -1;
                    offset *= -1;
                }
                if (im.Margin.Top <= 0)
                {
                    k *= -1;
                }
                if (im.Margin.Top >= canvas.ActualHeight - im.ActualHeight)
                {
                    k *= -1;
                }
                b = im.Margin.Top - im.Margin.Left * k;
                animation.To = new Thickness(im.Margin.Left + offset, y(im.Margin.Left + offset), 0, 0);
                im.BeginAnimation(MarginProperty, animation);
            }
        }
    }
}

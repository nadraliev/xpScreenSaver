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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }



        ThicknessAnimation animation;
        Image im;

        public double y(double x)
        {
            return k*x + b;
        }

        double offset = 2;
        double k = 2;
        double b = -1;

        private void button_Click(object sender, RoutedEventArgs e)
        {
            
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("image.jpg", UriKind.Relative);
            bitmap.EndInit();

            im = new Image();
            im.Source = bitmap;
            im.Margin = new Thickness(0, y(0), 0, 0);
            im.Stretch = Stretch.None;
            canvas.Children.Add(im);

            im.Loaded += Im_Loaded;


            
            
        }

        private void Im_Loaded(object sender, RoutedEventArgs e)
        {
            animation = new ThicknessAnimation();
            animation.To = new Thickness(im.Margin.Left + offset, y(im.Margin.Left + offset), 0, 0);
            animation.Duration = TimeSpan.FromTicks(1);
            animation.Completed += Animation_Completed;

            im.BeginAnimation(MarginProperty, animation);
        }

        private void Animation_Completed(object sender, EventArgs e)
        {
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

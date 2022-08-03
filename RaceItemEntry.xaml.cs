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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace F1_Desktop_Calendar
{
    /// <summary>
    /// Interaction logic for RaceItemEntry.xaml
    /// </summary>
    public partial class RaceItemEntry : UserControl
    {
        public RaceItemEntry()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //FadeInText();
        }

        public void FadeInText()
        {
            DoubleAnimation ani = new DoubleAnimation(1, TimeSpan.FromSeconds(0.5));
            StackPanelGrid.BeginAnimation(StackPanel.OpacityProperty, ani);
        }

        public async void FadeInDate()
        {
            await Task.Delay(2500);
            DoubleAnimation ani = new DoubleAnimation(1, TimeSpan.FromSeconds(0.5));
            Race_Date.BeginAnimation(TextBlock.OpacityProperty, ani);
        }

        private void WidgetControl_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Race_Date.Opacity <= 0 && P1Grid.Opacity <=0)
            {
                DoubleAnimation ani = new DoubleAnimation(1, TimeSpan.FromSeconds(0.25));
                ShowResultsText.BeginAnimation(TextBlock.OpacityProperty, ani);
            }
        }

        private void WidgetControl_MouseLeave(object sender, MouseEventArgs e)
        {
            DoubleAnimation ani = new DoubleAnimation(0, TimeSpan.FromSeconds(0.25));
            ShowResultsText.BeginAnimation(TextBlock.OpacityProperty, ani);
        }

        private void WidgetControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Race_Date.Opacity <= 0 && P1Grid.Opacity <= 0)
            {
                DoubleAnimation ani = new DoubleAnimation(0, TimeSpan.FromSeconds(0.15));
                ShowResultsText.BeginAnimation(TextBlock.OpacityProperty, ani);
                FadeResultInOut();
            }
        }

        public async void FadeResultInOut()
        {
            DoubleAnimation ani = new DoubleAnimation(1, TimeSpan.FromSeconds(0.25));
            P1Grid.BeginAnimation(Grid.OpacityProperty, ani);
            P2Grid.BeginAnimation(Grid.OpacityProperty, ani);
            P3Grid.BeginAnimation(Grid.OpacityProperty, ani);

            await Task.Delay(5000);

            ani = new DoubleAnimation(0, TimeSpan.FromSeconds(0.25));
            P1Grid.BeginAnimation(Grid.OpacityProperty, ani);
            P2Grid.BeginAnimation(Grid.OpacityProperty, ani);
            P3Grid.BeginAnimation(Grid.OpacityProperty, ani);

        }

        public void FadeResultIn()
        {
            DoubleAnimation ani = new DoubleAnimation(1, TimeSpan.FromSeconds(0.25));
            P1Grid.BeginAnimation(Grid.OpacityProperty, ani);
            P2Grid.BeginAnimation(Grid.OpacityProperty, ani);
            P3Grid.BeginAnimation(Grid.OpacityProperty, ani);
        }
        public void FadeResultOut()
        {
            DoubleAnimation ani = new DoubleAnimation(0, TimeSpan.FromSeconds(0.25));
            P1Grid.BeginAnimation(Grid.OpacityProperty, ani);
            P2Grid.BeginAnimation(Grid.OpacityProperty, ani);
            P3Grid.BeginAnimation(Grid.OpacityProperty, ani);
        }


    }
}

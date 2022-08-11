using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace F1_Desktop_Calendar
{
    /// <summary>
    /// Interaction logic for DriverStandings.xaml
    /// </summary>
    public partial class DriverStandings : Window
    {

		#region Driver Standings API Call Variables
		[XmlRoot(ElementName = "Driver")]
		public class Driver
		{

			[XmlElement(ElementName = "PermanentNumber")]
			public int PermanentNumber { get; set; }

			[XmlElement(ElementName = "GivenName")]
			public string GivenName { get; set; }

			[XmlElement(ElementName = "FamilyName")]
			public string FamilyName { get; set; }

			[XmlElement(ElementName = "DateOfBirth")]
			public DateTime DateOfBirth { get; set; }

			[XmlElement(ElementName = "Nationality")]
			public string Nationality { get; set; }

			[XmlAttribute(AttributeName = "driverId")]
			public string DriverId { get; set; }

			[XmlAttribute(AttributeName = "code")]
			public string Code { get; set; }

			[XmlAttribute(AttributeName = "url")]
			public string Url { get; set; }

			[XmlText]
			public string Text { get; set; }
		}

		[XmlRoot(ElementName = "Constructor")]
		public class Constructor
		{

			[XmlElement(ElementName = "Name")]
			public string Name { get; set; }

			[XmlElement(ElementName = "Nationality")]
			public string Nationality { get; set; }

			[XmlAttribute(AttributeName = "constructorId")]
			public string ConstructorId { get; set; }

			[XmlAttribute(AttributeName = "url")]
			public string Url { get; set; }

			[XmlText]
			public string Text { get; set; }
		}

		[XmlRoot(ElementName = "DriverStanding")]
		public class DriverStanding
		{

			[XmlElement(ElementName = "Driver")]
			public Driver Driver { get; set; }

			[XmlElement(ElementName = "Constructor")]
			public Constructor Constructor { get; set; }

			[XmlAttribute(AttributeName = "position")]
			public int Position { get; set; }

			[XmlAttribute(AttributeName = "positionText")]
			public int PositionText { get; set; }

			[XmlAttribute(AttributeName = "points")]
			public int Points { get; set; }

			[XmlAttribute(AttributeName = "wins")]
			public int Wins { get; set; }

			[XmlText]
			public string Text { get; set; }
		}

		[XmlRoot(ElementName = "StandingsList")]
		public class StandingsList
		{

			[XmlElement(ElementName = "DriverStanding")]
			public List<DriverStanding> DriverStanding { get; set; }

			[XmlAttribute(AttributeName = "season")]
			public int Season { get; set; }

			[XmlAttribute(AttributeName = "round")]
			public int Round { get; set; }

			[XmlText]
			public string Text { get; set; }
		}

		[XmlRoot(ElementName = "StandingsTable")]
		public class StandingsTable
		{

			[XmlElement(ElementName = "StandingsList")]
			public StandingsList StandingsList { get; set; }

			[XmlAttribute(AttributeName = "season")]
			public int Season { get; set; }

			[XmlText]
			public string Text { get; set; }
		}

		[XmlRoot(ElementName = "MRData")]
		public class MRData
		{

			[XmlElement(ElementName = "StandingsTable")]
			public StandingsTable StandingsTable { get; set; }

			[XmlAttribute(AttributeName = "xmlns")]
			public string Xmlns { get; set; }

			[XmlAttribute(AttributeName = "series")]
			public string Series { get; set; }

			[XmlAttribute(AttributeName = "url")]
			public string Url { get; set; }

			[XmlAttribute(AttributeName = "limit")]
			public int Limit { get; set; }

			[XmlAttribute(AttributeName = "offset")]
			public int Offset { get; set; }

			[XmlAttribute(AttributeName = "total")]
			public int Total { get; set; }

			[XmlText]
			public string Text { get; set; }
		}

		#endregion

		public MRData DriverStandingsData;

		public bool LockMovement = false;

        public DriverStandings()
        {
            InitializeComponent();
			APICallDriverStandingsAsync();

		}

		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (!LockMovement)
			{
				if (e.ChangedButton == MouseButton.Left && e.ButtonState == MouseButtonState.Pressed)
					this.DragMove();
			}

		}

		/// <summary>
		/// On launch place window on bottom centre of screen with offset
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			//EnableBlur();
			var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
			this.Top = desktopWorkingArea.Bottom - this.Height - 50;
		}

		private async void APICallDriverStandingsAsync()
        {
			var url = "http://ergast.com/api/f1/current/driverStandings";

			HttpClient client = new HttpClient();
			HttpResponseMessage response = await client.GetAsync(url);
			response.EnsureSuccessStatusCode();
			var responseBody = await response.Content.ReadAsStringAsync();

			XmlRootAttribute xRoot = new XmlRootAttribute();
			xRoot.ElementName = "MRData";
			xRoot.Namespace = "http://ergast.com/mrd/1.5";
			xRoot.IsNullable = true;

			XmlSerializer serializer = new XmlSerializer(typeof(MRData), xRoot);
			using (StringReader reader = new StringReader(responseBody))
			{
				DriverStandingsData = (MRData)serializer.Deserialize(reader);
				PopulateGrid();
			}
		}

		private void PopulateGrid()
        {
            foreach (var item in DriverStandingsData.StandingsTable.StandingsList.DriverStanding)
            {
				DriverStandingEntry test = new DriverStandingEntry();
				test.Position.Text = item.Position.ToString();
                switch (item.Position)
                {
                    case 1:
						test.Position.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 204, 0));
						break;
					case 2:
						test.Position.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
						break;
					case 3:
						test.Position.Foreground = new SolidColorBrush(Color.FromArgb(255, 204, 102, 0));
						break;
					default:
						test.Position.Foreground = new SolidColorBrush(Color.FromArgb(100, 255, 255, 255));
						break;
                }
				switch (item.Constructor.ConstructorId)
				{
					case "red_bull":
						test.TeamIcon.Source = new BitmapImage(new Uri(@"pack://application:,,,/F1 Desktop Calendar;component/Images/TeamIcons/redBull.png", UriKind.RelativeOrAbsolute));
						test.TeamColor.Background = new SolidColorBrush(Color.FromArgb(255, 30, 91, 198));
						break;
					case "ferrari":
						test.TeamIcon.Source = new BitmapImage(new Uri(@"pack://application:,,,/F1 Desktop Calendar;component/Images/TeamIcons/Ferrari.png", UriKind.RelativeOrAbsolute));
						test.TeamColor.Background = new SolidColorBrush(Color.FromArgb(255, 237, 28, 36));
						break;
					case "mercedes":
						test.TeamIcon.Source = new BitmapImage(new Uri(@"pack://application:,,,/F1 Desktop Calendar;component/Images/TeamIcons/Merc.png", UriKind.RelativeOrAbsolute));
						test.TeamColor.Background = new SolidColorBrush(Color.FromArgb(255, 108, 211, 191));
						break;
					case "williams":
						test.TeamColor.Background = new SolidColorBrush(Color.FromArgb(255, 55, 190, 221));
						break;
					case "haas":
						test.TeamColor.Background = new SolidColorBrush(Color.FromArgb(255, 182, 186, 189));
						break;
					case "mclaren":
						test.TeamColor.Background = new SolidColorBrush(Color.FromArgb(255, 245, 128, 32));
						break;
					case "aston_martin":
						test.TeamColor.Background = new SolidColorBrush(Color.FromArgb(255, 45, 130, 109));
						break;
					case "alphatauri":
						test.TeamColor.Background = new SolidColorBrush(Color.FromArgb(255, 78, 124, 155));
						break;
					case "alfa":
						test.TeamColor.Background = new SolidColorBrush(Color.FromArgb(255, 172, 32, 57));
						break;
					case "alpine":
						test.TeamColor.Background = new SolidColorBrush(Color.FromArgb(255, 34, 147, 209));
						break;
					default:
						test.TeamIcon.Source = new BitmapImage(new Uri(@"pack://application:,,,/F1 Desktop Calendar;component/Images/TeamIcons/Merc.png", UriKind.RelativeOrAbsolute));
						test.TeamColor.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
						break;
				}

				/*
				Mercedes			#6CD3BF	108,211,191
				Red Bull Racing		#1E5BC6	30,91,198
				Ferrari				#ED1C24	237,28,36
				McLaren				#F58020	245,128,32
				Alpine				#2293D1	34,147,209
				AlphaTauri			#4E7C9B	78,124,155
				Aston Martin		#2D826D	45,130,109
				Williams			#37BEDD	55,190,221

				Alfa Romeo Racing	#B12039	172,32,57

				Haas				#B6BABD	182,186,189
				 */
				test.Name.Text = item.Driver.GivenName.ToString() + " " + item.Driver.FamilyName.ToString();
				test.Constructor.Text = item.Constructor.Name.ToString();
				test.Points.Text = item.Points.ToString();

				Stack.Children.Add(test);
			}
        }

        private void CloseBtn_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
			this.Close();
        }
    }
}

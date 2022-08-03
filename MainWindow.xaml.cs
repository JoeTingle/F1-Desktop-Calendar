using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System;
using System.Windows.Media;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.Win32;
using System.Windows.Media.Animation;
using System.Windows.Forms;
using System.Reflection;

namespace F1_Desktop_Calendar
{
	/// <summary>
	/// Main Entry Point || Loads all race entries for curent season
	/// </summary>
	public partial class MainWindow : Window
	{
		//Xml data variables & classes
		#region Variables
		[XmlRoot(ElementName = "Location")]
		public class Location
		{

			[XmlElement(ElementName = "Locality")]
			public string Locality { get; set; }

			[XmlElement(ElementName = "Country")]
			public string Country { get; set; }

			[XmlAttribute(AttributeName = "lat")]
			public double Lat { get; set; }

			[XmlAttribute(AttributeName = "long")]
			public double Long { get; set; }

			[XmlText]
			public string Text { get; set; }
		}

		[XmlRoot(ElementName = "Circuit")]
		public class Circuit
		{

			[XmlElement(ElementName = "CircuitName")]
			public string CircuitName { get; set; }

			[XmlElement(ElementName = "Location")]
			public Location Location { get; set; }

			[XmlAttribute(AttributeName = "circuitId")]
			public string CircuitId { get; set; }

			[XmlAttribute(AttributeName = "url")]
			public string Url { get; set; }

			[XmlText]
			public string Text { get; set; }
		}

		[XmlRoot(ElementName = "FirstPractice")]
		public class FirstPractice
		{

			[XmlElement(ElementName = "Date")]
			public DateTime Date { get; set; }

			[XmlElement(ElementName = "Time")]
			public DateTime Time { get; set; }
		}

		[XmlRoot(ElementName = "SecondPractice")]
		public class SecondPractice
		{

			[XmlElement(ElementName = "Date")]
			public DateTime Date { get; set; }

			[XmlElement(ElementName = "Time")]
			public DateTime Time { get; set; }
		}

		[XmlRoot(ElementName = "ThirdPractice")]
		public class ThirdPractice
		{

			[XmlElement(ElementName = "Date")]
			public DateTime Date { get; set; }

			[XmlElement(ElementName = "Time")]
			public DateTime Time { get; set; }
		}

		[XmlRoot(ElementName = "Qualifying")]
		public class Qualifying
		{

			[XmlElement(ElementName = "Date")]
			public DateTime Date { get; set; }

			[XmlElement(ElementName = "Time")]
			public DateTime Time { get; set; }
		}

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

		[XmlRoot(ElementName = "Status")]
		public class Status
		{

			[XmlAttribute(AttributeName = "statusId")]
			public int StatusId { get; set; }

			[XmlText]
			public string Text { get; set; }
		}

		[XmlRoot(ElementName = "Time")]
		public class RaceTime
		{

			[XmlAttribute(AttributeName = "millis")]
			public int Millis { get; set; }

			[XmlText]
			public string Text { get; set; }
		}

		[XmlRoot(ElementName = "AverageSpeed")]
		public class AverageSpeed
		{

			[XmlAttribute(AttributeName = "units")]
			public string Units { get; set; }

			[XmlText]
			public double Text { get; set; }
		}

		[XmlRoot(ElementName = "FastestLap")]
		public class FastestLap
		{

			[XmlElement(ElementName = "Time")]
			public string Time { get; set; }

			[XmlElement(ElementName = "AverageSpeed")]
			public AverageSpeed AverageSpeed { get; set; }

			[XmlAttribute(AttributeName = "rank")]
			public int Rank { get; set; }

			[XmlAttribute(AttributeName = "lap")]
			public int Lap { get; set; }

			[XmlText]
			public string Text { get; set; }
		}

		[XmlRoot(ElementName = "Result")]
		public class Result
		{

			[XmlElement(ElementName = "Driver")]
			public Driver Driver { get; set; }

			[XmlElement(ElementName = "Constructor")]
			public Constructor Constructor { get; set; }

			[XmlElement(ElementName = "Grid")]
			public int Grid { get; set; }

			[XmlElement(ElementName = "Laps")]
			public int Laps { get; set; }

			[XmlElement(ElementName = "Status")]
			public Status Status { get; set; }

			[XmlElement(ElementName = "Time")]
			public RaceTime Time { get; set; }

			[XmlElement(ElementName = "FastestLap")]
			public FastestLap FastestLap { get; set; }

			[XmlAttribute(AttributeName = "number")]
			public int Number { get; set; }

			[XmlAttribute(AttributeName = "position")]
			public int Position { get; set; }

			[XmlAttribute(AttributeName = "positionText")]
			public string PositionText { get; set; }

			[XmlAttribute(AttributeName = "points")]
			public int Points { get; set; }

			[XmlText]
			public string Text { get; set; }
		}

		[XmlRoot(ElementName = "ResultsList")]
		public class ResultsList
		{

			[XmlElement(ElementName = "Result")]
			public List<Result> Result { get; set; }
		}

		[XmlRoot(ElementName = "Race")]
		public class Race
		{

			[XmlElement(ElementName = "RaceName")]
			public string RaceName { get; set; }

			[XmlElement(ElementName = "Circuit")]
			public Circuit Circuit { get; set; }

			[XmlElement(ElementName = "Date")]
			public DateTime Date { get; set; }

			[XmlElement(ElementName = "Time")]
			public DateTime Time { get; set; }

			[XmlElement(ElementName = "ResultsList")]
			public ResultsList ResultsList { get; set; }

			[XmlElement(ElementName = "FirstPractice")]
			public FirstPractice FirstPractice { get; set; }

			[XmlElement(ElementName = "SecondPractice")]
			public SecondPractice SecondPractice { get; set; }

			[XmlElement(ElementName = "ThirdPractice")]
			public ThirdPractice ThirdPractice { get; set; }

			[XmlElement(ElementName = "Qualifying")]
			public Qualifying Qualifying { get; set; }

			[XmlAttribute(AttributeName = "season")]
			public int Season { get; set; }

			[XmlAttribute(AttributeName = "round")]
			public int Round { get; set; }

			[XmlAttribute(AttributeName = "url")]
			public string Url { get; set; }

			[XmlText]
			public string Text { get; set; }

			[XmlElement(ElementName = "Sprint")]
			public Sprint Sprint { get; set; }
		}

		[XmlRoot(ElementName = "Sprint")]
		public class Sprint
		{

			[XmlElement(ElementName = "Date")]
			public DateTime Date { get; set; }

			[XmlElement(ElementName = "Time")]
			public DateTime Time { get; set; }
		}

		[XmlRoot(ElementName = "RaceTable")]
		public class RaceTable
		{

			[XmlElement(ElementName = "Race")]
			public List<Race> Race { get; set; }

			[XmlAttribute(AttributeName = "season")]
			public int Season { get; set; }

			[XmlText]
			public string Text { get; set; }
		}

		[XmlRoot(ElementName = "MRData")]
		public class MRData
		{

			[XmlElement(ElementName = "RaceTable")]
			public RaceTable RaceTable { get; set; }

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

		// Variables for windows aero blur effect on border background
		#region Aero Glass Effect
		internal enum AccentState
		{
			ACCENT_DISABLED = 0,
			ACCENT_ENABLE_GRADIENT = 1,
			ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
			ACCENT_ENABLE_BLURBEHIND = 3,
			ACCENT_INVALID_STATE = 4
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct AccentPolicy
		{
			public AccentState AccentState;
			public int AccentFlags;
			public int GradientColor;
			public int AnimationId;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct WindowCompositionAttributeData
		{
			public WindowCompositionAttribute Attribute;
			public IntPtr Data;
			public int SizeOfData;
		}

		internal enum WindowCompositionAttribute
		{
			// ...
			WCA_ACCENT_POLICY = 19
			// ...
		}
		[DllImport("user32.dll")]
		internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);
		#endregion

		// Variables for internet adapter status
		#region Internet Check
		[DllImport("wininet.dll")]
		private extern static bool InternetGetConnectedState(out int description, int reservedValue);

		#endregion

		/// <summary>
		/// Stores XML data for all races in current season
		/// </summary>
		MRData RaceData;
		/// <summary>
		/// Stores XML data for the current round
		/// </summary>
		MRData CurrentRaceData;
		/// <summary>
		/// Stores XML data for all race results (~200kb Needs time to load at start)
		/// </summary>
		MRData ResultsData;

		/// <summary>
		/// Sets whether the user allowed to move window
		/// </summary>
		private bool LockMovement = true;

		/// <summary>
		/// All Race entries displayed on the window
		/// </summary>
		public RaceItemEntry[] RacesList;

		//Countdown variables for next race upcoming
		DispatcherTimer Countdown = new DispatcherTimer();
		DateTime Start = DateTime.Now;
		DateTime EndDay = new DateTime(2022, 08, 01, 9, 55, 00);
		TimeSpan Time = new TimeSpan();

		/// <summary>
		/// The next round number
		/// </summary>
		public int CurrentRound;


		public MainWindow()
		{
			//Before launching window check if user has internet.
			//Need internet to start for API call / download
			CheckInternet();

			InitializeComponent();
			RacesList = new RaceItemEntry[] { Race1, Race2, Race3, Race4, Race5, Race6, Race7, Race8, Race9, Race10, Race11, Race12, Race13, Race14,
												Race15,Race16,Race17,Race18,Race19,Race20,Race21,Race22};

			//Call and download all race data, requires slight delay to populate all classes/variables
			APICallAsync();
			//Allow the API call to download data for races and then launch window
			WaitThenShowWindow();
			//Mouse click handler event
			AddHandler(FrameworkElement.MouseDownEvent, new MouseButtonEventHandler(Window_MouseDown), true);
			SetStatupMain();
		}

		/// <summary>
		/// Slowly fades the main window in from bottom right corner of screen
		/// </summary>
		public async void FadeInAnim()
		{
			rectScale.ScaleX = 0;
			rectScale.ScaleY = 0;
			DoubleAnimation ani = new DoubleAnimation(1, TimeSpan.FromSeconds(1));
			ani.Completed += new EventHandler(myanim_Completed);
			rectScale.BeginAnimation(ScaleTransform.ScaleXProperty, ani);
			rectScale.BeginAnimation(ScaleTransform.ScaleYProperty, ani);
			
			
		}

		/// <summary>
		/// Delays then shows window, then delays again to fade in race entries
		/// </summary>
		public async void WaitThenShowWindow()
		{
			await Task.Delay(1250);
			this.Visibility = Visibility.Visible;
			FadeInAnim();
			await Task.Delay(250);
			FadeInRaceEntries();
			AdjustPrevRounds();


		}

		/// <summary>
		/// Once animation is completed, enable the aero blur on background
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void myanim_Completed(object sender, EventArgs e)
		{
			EnableBlur();
		}

		/// <summary>
		/// Slowly fades in opacity of all race entries with delay between each
		/// </summary>
		public async void FadeInRaceEntries()
		{
            foreach (var race in RacesList)
            {
				race.FadeInText();
				await Task.Delay(100);
			}


		}

		/// <summary>
		/// Return the type of a child object
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="depObj"></param>
		/// <returns></returns>
		public T GetChildOfType<T>(DependencyObject depObj)
			where T : DependencyObject
		{
			if (depObj == null) return null;

			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
			{
				var child = VisualTreeHelper.GetChild(depObj, i);

				var result = (child as T) ?? GetChildOfType<T>(child);
				if (result != null) return result;
			}
			return null;
		}

		/// <summary>
		/// Checks internet adapter status, if false a dialog box is displayed and application closed
		/// </summary>
		public void CheckInternet()
        {
            if (!IsInternetAvailable())
            {
				string messageBoxText = "No Internet Connection Detected ! \n\nTo Get The Latest Race Info\nWe Need Internet At Least Once On Startup\nPlease Try Again...";
				string caption = "F1 Calendar - No Internet Connection Detected";
				MessageBoxButton button = MessageBoxButton.OK;
				MessageBoxImage icon = MessageBoxImage.Error;
				MessageBoxResult result;

				result = System.Windows.MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.OK);

				switch (result)
                {
                    case MessageBoxResult.OK:
						System.Windows.Application.Current.Shutdown();
						break;
				}
			}
        }

		public static bool IsInternetAvailable()
		{
			int description;
			return InternetGetConnectedState(out description, 0);
		}

		/// <summary>
		/// Allows user to drag/move window if enabled
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
            if (!LockMovement)
            {
				if (e.ChangedButton == MouseButton.Left && e.ButtonState == MouseButtonState.Pressed)
					this.DragMove();
			}

		}

		/// <summary>
		/// On launch place window on bottom right of screen with offset
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			//EnableBlur();
			var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
			this.Left = desktopWorkingArea.Right - this.Width;
			this.Top = desktopWorkingArea.Bottom - this.Height + 2;
		}

		/// <summary>
		/// Main API call to download race data
		/// </summary>
		public async void APICallAsync()
		{
			var url = "http://ergast.com/api/f1/current";

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
				var test = (MRData)serializer.Deserialize(reader);
				RaceData = test;
				GetRaceResults();
				UpdateRaces();
				HidePreviousRounds();
			}


		}

		/// <summary>
		/// API call to get all season results (~200kb Largest call)
		/// </summary>
		public async void GetRaceResults()
		{
			//Takes Time, Lots Of Data ~200kb
			var url = "http://ergast.com/api/f1/" + RaceData.RaceTable.Season + "/results/?limit=100000";

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
				var test = (MRData)serializer.Deserialize(reader);
				ResultsData = test;
			}


		}

		/// <summary>
		/// Exits application on click
		/// || Disposes of system tray icon
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			AppIcon.Icon = null;
			AppIcon.Dispose();
			System.Windows.Application.Current.Shutdown();
		}

		/// <summary>
		/// Checks if RaceData is populated with successfull API call then updates round info
		/// </summary>
		public void UpdateRaces()
        {
			if (RaceData != null)
			{

				Rounds();

			}
		}

		/// <summary>
		/// Returns correct suffix for DateTime Parses
		/// </summary>
		/// <param name="day"></param>
		/// <returns></returns>
		string GetDaySuffix(int day)
		{
			switch (day)
			{
				case 1:
				case 21:
				case 31:
					return "st ";
				case 2:
				case 22:
					return "nd ";
				case 3:
				case 23:
					return "rd ";
				default:
					return "th ";
			}
		}

		/// <summary>
		/// Updates round / circuit / date / flag for each race entry
		/// </summary>
		public void Rounds()
        {

            for (int i = 0; i < RacesList.Length; i++)
            {
                RacesList[i].Race_Number.Text = RaceData.RaceTable.Race[i].Round.ToString() + ".";
				RacesList[i].Race_Name.Text = RaceData.RaceTable.Race[i].RaceName.ToString();
				RacesList[i].Race_Date.Text = RaceData.RaceTable.Race[i].Date.ToString("M").Insert(2,GetDaySuffix(RaceData.RaceTable.Race[i].Date.Day));


				string name = ReplaceGrandPrixNames(RaceData.RaceTable.Race[i].RaceName);
				RacesList[i].Race_Img.Source = new BitmapImage(new Uri(@"https://countryflagsapi.com/png/" + name, UriKind.Absolute));
			}


		}

		/// <summary>
		/// API call for current round info
		/// </summary>
		public async void HidePreviousRounds()
        {
			var url = "http://ergast.com/api/f1/current/next";

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
				var test = (MRData)serializer.Deserialize(reader);
				CurrentRaceData = test;
				CurrentRound = CurrentRaceData.RaceTable.Race[0].Round;

			}
		}

		/// <summary>
		/// Slowly fades out race results for each entry
		/// </summary>
		public void HideResults()
		{
			for (int i = 0; i < CurrentRound - 1; i++)
			{
				if (i < RacesList.Length)
				{
					RacesList[i].FadeResultOut();
				}
			}
		}

		/// <summary>
		/// Slowly fade in each race result entry
		/// </summary>
		public void ShowResults()
		{
			for (int i = 0; i < CurrentRound - 1; i++)
			{
				if (i < RacesList.Length)
				{
					RacesList[i].FadeResultIn();
				}
			}
		}

		/// <summary>
		/// For each completed race, adds flag background and lowers opacity
		/// </summary>
		public async void AdjustPrevRounds()
        {
			//Loop through each Listed Race Entry Up To Current Round
			for (int i = 0; i < CurrentRound - 1; i++)
			{
				if (i < RacesList.Length)
				{
					ImageBrush imgBrush = new ImageBrush();
					imgBrush.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/F1 Desktop Calendar;component/Images/flagwide.png", UriKind.RelativeOrAbsolute));
					imgBrush.Stretch = Stretch.UniformToFill;
					imgBrush.TileMode = TileMode.Tile;
					imgBrush.Opacity = 0.045;
					RacesList[i].Background = imgBrush;
					RacesList[i].Race_Number.Opacity = 0.3;
					RacesList[i].Race_Name.Opacity = 0.3;
					RacesList[i].Race_Img.Opacity = 0.3;
					RacesList[i].Race_Date.Opacity = 0;

                    if (ResultsData != null)
                    {
                        if (i < ResultsData.RaceTable.Race.Count)
                        {
							RacesList[i].P1Name.Text = ResultsData.RaceTable.Race[i].ResultsList.Result[0].Driver.Code;
							RacesList[i].P2Name.Text = ResultsData.RaceTable.Race[i].ResultsList.Result[1].Driver.Code;
							RacesList[i].P3Name.Text = ResultsData.RaceTable.Race[i].ResultsList.Result[2].Driver.Code;
						}
					}
                    else
                    {
						Debug.WriteLine("\n\nERROR : Results Data Null (API Call Not Completed ?\n\n)");
                    }

				}

			}

            //Highlight Next Race // Green Background
            if (CurrentRound >= 0) // Fixing Array out of bounds error (API not loaded current round yet ??)
            {
				RacesList[CurrentRound - 1].Race_Number.FontWeight = FontWeights.ExtraBold;
				RacesList[CurrentRound - 1].Race_Name.FontWeight = FontWeights.ExtraBold;
				RacesList[CurrentRound - 1].Background = new SolidColorBrush(Color.FromArgb(25, 0, 255, 0));
			}


			for (int i = CurrentRound -1; i < RacesList.Length; i++)
			{
				RacesList[i].FadeInDate();
				await Task.Delay(100);
			}

			//Next Race Countdown // Title
			DoubleAnimation ani = new DoubleAnimation(1, TimeSpan.FromSeconds(1.5));
			NextRaceItem.RoundText.Text = "Round " + CurrentRound;
			//NextRaceItem.RaceName.Text = CurrentRaceData.RaceTable.Race[0].RaceName;
			NextRaceItem.RaceName.Text = RaceData.RaceTable.Race[CurrentRound -1].RaceName;
			//NextRaceItem.RaceNameImgMaskText.Text = CurrentRaceData.RaceTable.Race[0].RaceName;
			NextRaceItem.RaceNameImgMaskText.Text = RaceData.RaceTable.Race[CurrentRound - 1].RaceName;
			NextRaceItem.RoundText.Opacity = 0;
			NextRaceItem.RoundText.BeginAnimation(System.Windows.Controls.TextBlock.OpacityProperty, ani);


			NextRaceItem.RaceNameImgMask.Opacity = 0;
			NextRaceItem.RaceNameImgMask.BeginAnimation(System.Windows.Controls.Image.OpacityProperty,ani);
			//string name = ReplaceGrandPrixNames(CurrentRaceData.RaceTable.Race[0].RaceName);
			string name = ReplaceGrandPrixNames(RaceData.RaceTable.Race[CurrentRound - 1].RaceName);
			NextRaceItem.RaceNameImgMask.Source = new BitmapImage(new Uri(@"https://countryflagsapi.com/png/" + name, UriKind.Absolute));





			EndDay = CurrentRaceData.RaceTable.Race[0].Date;
			EndDay = new DateTime(EndDay.Year, EndDay.Month, EndDay.Day, CurrentRaceData.RaceTable.Race[0].Time.Hour + 1, CurrentRaceData.RaceTable.Race[0].Time.Minute, 0);
			Time = EndDay - Start;
			NextRaceItem.CountdownText.Text = Time.Days.ToString() + " Day(s) " + Time.Hours.ToString() + " Hour(s) " + Time.Minutes.ToString() + " Min(s) ";

			NextRaceItem.CountdownText.Opacity = 0;
			NextRaceItem.CountdownText.BeginAnimation(System.Windows.Controls.TextBlock.OpacityProperty, ani);

			Countdown.Interval = new TimeSpan(0, 0, 0, 1, 0);
			Countdown.Tick += new EventHandler(Countdown_Tick);
			Countdown.Start();


		}

		/// <summary>
		/// Updates countdown to next race text
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Countdown_Tick(object sender, EventArgs e)
		{
			Time = Time.Subtract(new TimeSpan(0, 0, 1));
			NextRaceItem.CountdownText.Text = Time.Days.ToString() + " Day(s) " + Time.Hours.ToString() + " Hour(s) "+ Time.Minutes.ToString() + " Min(s) ";
            
			if (Time.TotalSeconds <= 0)
            {
				//Restart();
				CurrentRound++;

				//Next Day Test
				EndDay = RaceData.RaceTable.Race[CurrentRound - 1].Date;
				Countdown.Stop();

				AdjustPrevRounds();
			}
			
		}

		/// <summary>
		/// Regex for API information
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string RemoveWords(string text)
		{
			text = text.Replace("&nbsp;", " ").Replace("<br>", "\n").Replace("description", "").Replace("INFRA:CORE:", "")
				.Replace("RESERVED", "")
				.Replace(":", "")
				.Replace(";", "")
				.Replace("-0/3/0", "")
				.Replace("Grand", "")
				.Replace("Prix", "");
			var oRegEx = new System.Text.RegularExpressions.Regex("<[^>]+>");
			return oRegEx.Replace(text, string.Empty);
		}

		/// <summary>
		/// Regex for API information
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string ReplaceGrandPrixNames(string text)
		{
			text = text
				.Replace("RESERVED", "")
				.Replace("Bahrain ", "Bahrain")
				.Replace("Saudi Arabian ", "Saudi Arabia")
				.Replace("Australian ", "Australia")
				.Replace("Emilia Romagna ", "Italy")
				.Replace("Miami ", "United States")
				.Replace("Spanish ", "Spain")
				.Replace("Monaco ", "Monaco")
				.Replace("Azerbaijan ", "Azerbaijan")
				.Replace("Canadian ", "Canada")
				.Replace("British ", "United Kingdom")
				.Replace("Austrian ", "Austria")
				.Replace("French ", "France")
				.Replace("Hungarian ", "Hungary")
				.Replace("Belgian ", "Belgium")
				.Replace("Dutch ", "Denmark")
				.Replace("Italian ", "Italy")
				.Replace("Singapore ", "Singapore")
				.Replace("Japanese ", "Japan")
				.Replace("United States ", "United States")
				.Replace("Mexico City ", "Mexico")
				.Replace("Brazilian ", "Brazil")
				.Replace("Abu Dhabi ", "United Arab Emirates")
				.Replace("Grand ", "")
				.Replace("Prix", "");
			var oRegEx = new System.Text.RegularExpressions.Regex("<[^>]+>");
			return oRegEx.Replace(text, string.Empty);
		}

		/// <summary>
		/// On check locks movement to current position
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MenuItem_Checked(object sender, RoutedEventArgs e)
        {
			LockMovement = true;

		}

		/// <summary>
		/// On uncheck allows user to move / drag window
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void MenuItem_Unchecked(object sender, RoutedEventArgs e)
        {
			LockMovement = false;

		}

		/// <summary>
		/// Enables aero blur for background
		/// </summary>
		internal void EnableBlur()
		{
			var windowHelper = new WindowInteropHelper(this);

			var accent = new AccentPolicy();
			accent.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;

			var accentStructSize = Marshal.SizeOf(accent);

			var accentPtr = Marshal.AllocHGlobal(accentStructSize);
			Marshal.StructureToPtr(accent, accentPtr, false);

			var data = new WindowCompositionAttributeData();
			data.Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY;
			data.SizeOfData = accentStructSize;
			data.Data = accentPtr;

			SetWindowCompositionAttribute(windowHelper.Handle, ref data);

			Marshal.FreeHGlobal(accentPtr);
		}

		/// <summary>
		/// Disables aero blur for background
		/// </summary>
		internal void DisableBlur()
		{
			var windowHelper = new WindowInteropHelper(this);

			var accent = new AccentPolicy();
			accent.AccentState = AccentState.ACCENT_DISABLED;

			var accentStructSize = Marshal.SizeOf(accent);

			var accentPtr = Marshal.AllocHGlobal(accentStructSize);
			Marshal.StructureToPtr(accent, accentPtr, false);

			var data = new WindowCompositionAttributeData();
			data.Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY;
			data.SizeOfData = accentStructSize;
			data.Data = accentPtr;

			SetWindowCompositionAttribute(windowHelper.Handle, ref data);

			Marshal.FreeHGlobal(accentPtr);
		}

		/// <summary>
		/// On application exit dispose of system tray icon
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_Closed(object sender, EventArgs e)
        {
			AppIcon.Icon = null;
			AppIcon.Dispose();	
        }

		/// <summary>
		/// On application exit dispose of system tray icon
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
			AppIcon.Icon = null;
			AppIcon.Dispose();

		}

		/// <summary>
		/// Allow application to start on PC boot
		/// || applies registry key
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void SetStartup(object sender, RoutedEventArgs e)
        {
			SetStatupMain();
		}

		private void SetStatupMain()
        {
			var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
				"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

			//key.DeleteValue("F1 Desktop Calendar", false);

			Assembly curAssembly = Assembly.GetEntryAssembly();
			var test = Process.GetCurrentProcess().MainModule.FileName;
			if (key != null)
			{
				var dir = AppDomain.CurrentDomain.BaseDirectory;
				var existed = key.GetValue("F1 Desktop Calendar", test);
				if (StartupCheckBox.IsChecked)
					key.SetValue("F1 Desktop Calendar", test);
				else
					key.DeleteValue("F1 Desktop Calendar", false);
			}
		}

		/// <summary>
		/// Reset window position to bottom right of screen with offset
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
			var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
			this.Left = desktopWorkingArea.Right - this.Width;
			this.Top = desktopWorkingArea.Bottom - this.Height + 2;
		}

		/// <summary>
		/// If checked show all race results
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void ShowResultsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
			ShowResults();
        }

		/// <summary>
		/// If checked hide all race results
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ShowResultsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
			HideResults();
        }

		/// <summary>
		/// Restart the application
		/// </summary>
		public void Restart()
		{
			System.Diagnostics.Process.Start(Process.GetCurrentProcess().MainModule.FileName);
            System.Windows.Application.Current.Shutdown();
		}
	}
}

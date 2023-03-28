using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace RBEarthquake
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// This method pulls data from our API, and selects a random quake to share details from.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="minMagnitude"></param>
        private void LoadEarthquakeDetails(string startDate, string endDate, double minMagnitude)
        {
            using (WebClient wc = new WebClient())
            {
                
                try
                {
                    string jsonData = wc.DownloadString($"https://earthquake.usgs.gov/fdsnws/event/1/query?format=geojson&starttime={startDate}&endtime={endDate}&minmagnitude={minMagnitude}");
                    // To parse the jsonData, we use the JObject class
                    JObject parsedJson = JObject.Parse(jsonData);

                    
                    // Deserialize the json data into a custom class
                    AllEarthquakeDetails quakeData = JsonConvert.DeserializeObject<AllEarthquakeDetails>(jsonData);
                    int quakeCount = Int32.Parse(quakeData.metadata.count.ToString());
                    Random rnd = new Random(quakeCount -1);
                    int selectedIndex = rnd.Next(quakeCount -1);
                    
                    LblResults.Text = "There were "+ quakeData.metadata.count.ToString() + " earthquakes during this time. \n \n Details of one of them: \n" + "Place: " + quakeData.features[selectedIndex].properties.place + "\n Magnitude: " + quakeData.features[selectedIndex].properties.mag;

                  
                }
                catch (Exception ex)
                {

                    DisplayAlert("Oh no", $"{ex.Message}", "Close"); 
                }
            }
        }
        /// <summary>
        /// This method takes user data and uses them as parameters to call the LoadEarthquakeDetails method with.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnFind_Clicked(object sender, EventArgs e)
        {
            string startDate = DPStartDate.Date.ToString();
            string endDate = DPEndDate.Date.ToString();
            double minMagnitude = Double.Parse(EntMinimumMagnitude.Text);



            LoadEarthquakeDetails(startDate, endDate, minMagnitude);
        }
    }
}

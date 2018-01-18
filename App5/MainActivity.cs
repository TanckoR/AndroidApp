using Android.App;
using Android.Widget;
using Android.OS;
using Android.Locations;
using System.Linq;
using Newtonsoft.Json.Linq;
using System;
using Android.Content;
//using Plugin.Geolocator;

namespace App5
{
    public class AppUsers
    {
        public Users[] User { get; set; }
    }
    public class Users
    {
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    [Activity(Label = "App5", MainLauncher = true)]
    public class MainActivity : Activity
    {
        //LocationManager locationManager;
        //string locationProvider;

        //Location currentLocation;
        TextView txtlatitu;
        TextView txtlong;
        //Location _currentLocation;
        //LocationManager locMgr;
        
        public int defaultKm = 0;
        public double myLat = 0;
        public double myLng = 0;
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);            
            SetContentView(Resource.Layout.Main);

           // locMgr = GetSystemService(Context.LocationService) as LocationManager;
            txtlatitu = FindViewById<TextView>(Resource.Id.txtlatitude);
            txtlong = FindViewById<TextView>(Resource.Id.txtlong);

            var txtRange = FindViewById<TextView>(Resource.Id.textView1);
            var SeekBar = FindViewById<SeekBar>(Resource.Id.seekBar1);

        
            SeekBar.ProgressChanged += (s, e)=>{
                txtlatitu.Text = "";
                defaultKm = e.Progress;
                txtRange.Text = string.Format("Range:{0}", defaultKm);
                
               myLat = 48.008548;
               myLng = 37.812889;

                int count = 0;

                var json = JObject.Parse(/*File.ReadAllText("App5.Users.Json"));*/"{ 'User':[{'Name':'name1','Lat':'48.008615','Lng':'37.811911'},{'Name':'name2','Lat':'48.008828','Lng':'37.826799'},{'Name':'name3','Lat':'48.009212','Lng':'37.841055'},{'Name':'name4','Lat':'48.009027','Lng':'37.826798'},{'Name':'name5','Lat':'48.021201','37.809426'},{'Name':'name6','Lat':'48.028058','Lng':'37.795949'},{'Name':'name7','Lat':'48.037406','Lng':'37.771815'}]}");
                var pairs = json.Properties().First().Value.ToObject<Users[]>();

                int endRange = defaultKm * 1000;
                foreach (var user in pairs)
                {
                    double userLat = user.Lat;
                    double userLng = user.Lng;
                    double range = rangeBetweenCoords(myLat, myLng, userLat, userLng);
                    if (range <= endRange)
                    {
                        count++;
                        txtlatitu.Text += "User near you:" + user.Name + "Cords:(" + user.Lat + ":" + user.Lng + ") on ragne: " + range + " meters\n";
                        txtlong.Text = "Radius:" + endRange;
                    }
                }
            };               
            
        }
        
        public static double radians(double Cord)
        {
            return Cord * Math.PI/ 180;
        }
        public static double rangeBetweenCoords(double lat1, double lng1, double lat2, double lng2)
        {
            double range = 0;
            //перевод в радианы
            double startLat = radians(lat1);
            double startLng = radians(lng1);
            double endLat = radians(lat2);
            double endLng = radians(lng2);
            //косинусы и синусы широт и долгот
            double cos1 = Math.Cos(startLat),
                cos2 = Math.Cos(endLat),
                sin1 = Math.Sin(startLat),
                sin2 = Math.Sin(endLat),
                delta = endLng - startLng,
                delta1 = Math.Cos(delta),
                delta2 = Math.Sin(delta);
            //вычисление длинны большого круга
            double y = Math.Sqrt(Math.Pow(cos2 * delta2, 2) + Math.Pow(cos1 * sin2 - sin1 * cos2 * delta1, 2));
            double x = sin1 * sin2 + cos1 * cos2 * delta1;
            double atan = Math.Atan2(y, x);
            range = atan * 6372795;
            return range;
        }
    }
}


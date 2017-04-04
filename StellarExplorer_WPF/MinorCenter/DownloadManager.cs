using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Newtonsoft.Json;
using StellarExplorer_WPF.Annotations;
using SwissEphNet;
using WpfDBpruef.modelle;

namespace StellarExplorer_WPF.MinorCenter
{
    class DownloadManager : INotifyPropertyChanged
    {
        //Main Query Browser Prefix for Data Obtain
        public String AsterankQuery = "http://www.asterank.com/api/asterank?";

        private ICommand _downloadCommand;
        public ICommand DownloadCommand
        {
            get
            {
                if (_downloadCommand == null)
                    _downloadCommand =
                        new DelegateCommand(SaveExecuted
                                            );
                return _downloadCommand;
            }
        }


        private String _toQueryAsteroids;
        public String ToQueryAsteroid
        {
            get { return _toQueryAsteroids; }
            set { _toQueryAsteroids = value; }
        }

        private String _queriedAsteroids;

        public String QueriedAsteroids
        {
            get { return _queriedAsteroids; }
            set { _queriedAsteroids = value; }
        }


        public void SaveExecuted(object param)
        {
            try
            {
                List<AsteroidJson> queriedAsteroids = new List<AsteroidJson>();
                //Correct integration of query string from EDIT_ASTNO
                if (!ToQueryAsteroid.Equals(String.Empty))
                {
                    List<String> asteroidList = ToQueryAsteroid.Replace(" ", "").Split(',').ToList();
                    DownloadManager downloadManager = new DownloadManager();
                    foreach (String asteroid in asteroidList)
                    {
                        String query = downloadManager.createQuery(new AsteroidJson { name = asteroid });
                        queriedAsteroids.Add(downloadManager.DownloadAsteroidJson(query, 1).First());
                    }
                }

               

                StringBuilder stringBuilder = new StringBuilder();

                foreach (AsteroidJson asteroid in queriedAsteroids)
                {
                    // Determine type of AsteroidJson
                    Type type = asteroid.GetType();
                    //Query all attributes that are contained within AsteroidJson instance
                    PropertyInfo[] propertyInfos = type.GetProperties();
                    bool first = true;
                    //Loop through all variables that should define the search
                    stringBuilder.Append("Name : " + asteroid.name);
                    foreach (var property in propertyInfos)
                    {
                        if (property.GetValue(asteroid, null) != null && !property.GetValue(asteroid, null).Equals(""))
                        {
                            stringBuilder.Append(Environment.NewLine + property.Name + " : " +
                                                 property.GetValue(asteroid, null));
                        }
                    }

                    stringBuilder.Append(Environment.NewLine + Environment.NewLine);
                    //TEXT_LIVE.Text = "Size" + queriedAsteroids.Count.ToString();
                }
                QueriedAsteroids = stringBuilder.ToString();
                if (PropertyChanged != null)      // feuere Event
                    PropertyChanged(this,
                        new PropertyChangedEventArgs("QueriedAsteroids"));
                stringBuilder.Clear();
            }
            catch (Exception exception)
            {
                //MessageBox.Show("Seems like some of the extra bodies were not found which caused an error.", "StellarExplorer", MessageBoxButtons.OK);
            }

        }




        /// <summary>
        /// Method Requires parameters which are used in order to query the JSON File from URL.
        /// </summary>
        /// <param name="query">Specifies query parameters which are used to select stelar bodies</param>
        /// <param name="limit">Amount of objects to be returned - determined by accuracy of parameters</param>
        /// <returns>List of JSON AsteroidJson Objects</returns>
        public IEnumerable<AsteroidJson> DownloadAsteroidJson(String query, int limit)
        {
            //Preparing the URL String for Query
            
            String callLimit = "&limit=" + limit;
            String asterTankRequest = String.Concat(query, callLimit);
            //String to store entire Json once obtained
            var obtainedJsonString = String.Empty;

            //Downloading the JSON String from URL
            using (var webClient = new System.Net.WebClient())
            {
                obtainedJsonString = webClient.DownloadString(asterTankRequest);
            }
            //Initializing settings for JSON to C# Object transformations
            var serializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
            };
            //Receiving a List containing C# objects as AsteroidJson
            var result = JsonConvert.DeserializeObject<List<AsteroidJson>>(obtainedJsonString, serializerSettings);
            //Converting result into an I enumerable
            return result;
        }

        /// <summary>
        /// Method prepares a query string in order to feed the webclient from mongodb instance
        /// </summary>
        /// <param name="parameterJson">AsteroidJson instance where all attributes are set which should affect the search</param>
        /// <returns>Query segment as string</returns>
        public String createQuery(AsteroidJson parameterJson)
        {
            //Define Stringbuilder Instance to improve performance on many values
            StringBuilder stringBuilder = new StringBuilder();
            //Initialise the beginning of instance since it is not repetitive
            stringBuilder.Append(AsterankQuery + "query={");
            //Determine type of AsteroidJson
            Type type = parameterJson.GetType();
            //Query all attributes that are contained within AsteroidJson instance
            PropertyInfo[] propertyInfos = type.GetProperties();
            bool first = true;
            //Loop through all variables that should define the search
            foreach (var property in propertyInfos)
            {
                //Determine value of variable
                if (property.GetValue(parameterJson, null) != null)
                {
                    //If it's a set boolean value perform operation
                    if (property.GetValue(parameterJson, null).GetType() == typeof(bool)){//Perform operation on bool argument
                    }else
                    {
                        //Prepend commas
                        if (!first){stringBuilder.Append(",");}else{first = false;}

                        //Perform operation on string argument
                        Console.WriteLine("Name : {0} Value:{1}", property.Name, property.GetValue(parameterJson, null));
                        //Example template Query

                        //http://www.asterank.com/api/asterank?query={%22name%22:%22Vesta%22}&limit=10
                        if (property.GetValue(parameterJson, null).ToString().Contains(" "))
                        stringBuilder.Append("%22" + property.Name + "%22:\"" +
                                                 property.GetValue(parameterJson, null) + "\"");
                        else
                        {
                            stringBuilder.Append("%22" + property.Name + "%22:\"" +
                                                 property.GetValue(parameterJson, null) + "\"");
                        }
                    }
                }
            }
            //Return finished Query part 
            return stringBuilder.ToString() + "}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

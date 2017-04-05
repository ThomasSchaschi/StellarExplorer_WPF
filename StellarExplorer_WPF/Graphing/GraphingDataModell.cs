using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LiveCharts;
using LiveCharts.Wpf;
using StellarExplorer_WPF.Annotations;
using StellarExplorer_WPF.DataViewer.Database;
using SweWin.LiveQuery;

namespace StellarExplorer_WPF.Graphing
{
    class GraphingDataModell : INotifyPropertyChanged
    {
        public stellardbEntities Stellardb = new stellardbEntities();
        private LineSeries lsEclLat, lsEclLong, lsDist, lsSpeed, lsHouse, lsNutation, lsEclObl;
        public SeriesCollection SeriesCollection { get; set; }
        public Func<double, string> XFormatter { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public String[] Labels { get; set; }

        List<String> chartMonths = new List<String>();
        ChartValues<double> chartEclLong = new ChartValues<double>();
        ChartValues<double> chartEclLat = new ChartValues<double>();
        ChartValues<double> chartDist = new ChartValues<double>();
        ChartValues<double> chartSpeed = new ChartValues<double>();
        ChartValues<double> chartHouse = new ChartValues<double>();
        ChartValues<double> chartEclObl = new ChartValues<double>();
        ChartValues<double> chartNutation = new ChartValues<double>();

        private List<String> _selectAbleDatabasesList;
        private String _selectedDatabase;
        public List<string> SelectAbleDatabasesList
        {
            get { return _selectAbleDatabasesList; }
            set { _selectAbleDatabasesList = value; }
        }

        public string SelectedDatabase
        {
            get { return _selectedDatabase; }
            set
            {
                _selectedDatabase = value;
                var databaseId = (from entry in Stellardb.Crude_Entries
                    where entry.ce_ephemerisname.Equals(value)
                    select entry.ce_id).First().ToString(); 
                List<Table_Template> valsDb = new DBOperations().QueryTableTemplateFromTableGuid(databaseId);
                chartMonths.Clear();
                chartEclLong.Clear();
                chartEclLat.Clear();
                chartDist.Clear();
                chartSpeed.Clear();
                chartHouse.Clear();
                chartEclObl.Clear();
                foreach (Table_Template tableTemplate in valsDb)
                {
                    chartMonths.Add(tableTemplate.t_date.ToLongDateString());
                    chartEclLong.Add(double.Parse(tableTemplate.t_ecllong.ToString()));
                    chartEclLat.Add(double.Parse(tableTemplate.t_ecllat.ToString()));
                    chartDist.Add(double.Parse(tableTemplate.t_dist.ToString()));
                    chartSpeed.Add(double.Parse(tableTemplate.t_speed.ToString()));
                    chartHouse.Add(double.Parse(tableTemplate.t_house.ToString()));
                    chartEclObl.Add(double.Parse(tableTemplate.t_eclobl.ToString()));
                }


            }
        }


        public GraphingDataModell()
        {
            
            var databaseEntries = Stellardb.Crude_Entries.ToList();
            var listNames = (from entry in databaseEntries select entry.ce_ephemerisname).ToList();
            SelectAbleDatabasesList = listNames;
            

            //af with few
            var ed = from d in Stellardb.cd82d04d_07d0_4041_b4ad_bd7e29094ec3
                     select d;

         
            foreach (cd82d04d_07d0_4041_b4ad_bd7e29094ec3 a in ed)
            {

                chartMonths.Add(a.t_date.ToLongDateString());
                chartEclLong.Add(double.Parse(a.t_ecllong.ToString()));
                chartEclLat.Add(double.Parse(a.t_ecllat.ToString()));
                chartDist.Add(double.Parse(a.t_dist.ToString()));
                chartSpeed.Add(double.Parse(a.t_speed.ToString()));
                chartHouse.Add(double.Parse(a.t_house.ToString()));
                chartEclObl.Add(double.Parse(a.t_eclobl.ToString()));
            }

            lsEclLat = new LineSeries
            {
                Title = "Eclyptical Latitude",
                Values = chartEclLat,
                PointGeometrySize = 3,
                LineSmoothness = 0
            };
            lsEclLong = new LineSeries
            {
                Title = "Eclyptical Longitude",
                Values = chartEclLong,
                PointGeometrySize = 3,
                LineSmoothness = 0
            };
            lsDist = new LineSeries
            {
                Title = "Distance",
                Values = chartDist,
                PointGeometrySize = 3,
                LineSmoothness = 0
            };
            lsSpeed = new LineSeries
            {
                Title = "Speed",
                Values = chartSpeed,
                PointGeometrySize = 3,
                LineSmoothness = 0
            };
            lsHouse = new LineSeries
            {
                Title = "House",
                Values = chartHouse,
                PointGeometrySize = 3,
                LineSmoothness = 0
            };
            lsEclObl = new LineSeries
            {
                Title = "Eclyptical Obl.",
                Values = chartEclObl,
                PointGeometrySize = 3,
                LineSmoothness = 0
            };
            lsNutation = new LineSeries
            {
                Title = "Nutation",
                Values = chartNutation,
                PointGeometrySize = 3,
                LineSmoothness = 0
            };


            SeriesCollection = new SeriesCollection();

            XFormatter = val => new DateTime((long)val).ToString();
            YFormatter = val => val.ToString();
            Labels = chartMonths.ToArray();
        }

        private Boolean _eclLongChecked = false;
        private Boolean _eclLatChecked = false;
        private Boolean _distanceChecked = false;
        private Boolean _houseChecked = false;
        private Boolean _speedChecked = false;
        private Boolean _eclOblChecked = false;

        public bool EclLongChecked
        {
            get { return _eclLongChecked; }
            set
            {
                _eclLongChecked = value;
                if (value == false)
                {
                    SeriesCollection.Remove(lsEclLong);
                }
                else
                {
                    SeriesCollection.Add(lsEclLong);
                }
                PropChangedSeriesCollection();
            }
        }

        public bool EclLatChecked
        {
            get { return _eclLatChecked; }
            set
            {
                _eclLatChecked = value;
                if (value == false)
                {
                    SeriesCollection.Remove(lsEclLat);
                }
                else
                {
                    SeriesCollection.Add(lsEclLat);
                }
                PropChangedSeriesCollection();
            }
        }

        public bool DistanceChecked
        {
            get { return _distanceChecked; }
            set
            {
                _distanceChecked = value;
                if (value == false)
                {
                    SeriesCollection.Remove(lsDist);
                }
                else
                {
                    SeriesCollection.Add(lsDist);
                }
                PropChangedSeriesCollection();
            }
        }

        public bool HouseChecked
        {
            get { return _houseChecked; }
            set
            {
                _houseChecked = value;
                if (value == false)
                {
                    SeriesCollection.Remove(lsHouse);
                }
                else
                {
                    SeriesCollection.Add(lsHouse);
                }
                PropChangedSeriesCollection();
            }
        }

        public bool SpeedChecked
        {
            get { return _speedChecked; }
            set
            {
                _speedChecked = value;
                if (value == false)
                {
                    SeriesCollection.Remove(lsSpeed);
                }
                else
                {
                    SeriesCollection.Add(lsSpeed);
                }
                PropChangedSeriesCollection();
            }
        }

        public bool EclOblChecked
        {
            get { return _eclOblChecked; }
            set
            {
                _eclOblChecked = value;
                if (value == false)
                {
                    SeriesCollection.Remove(lsEclObl);
                }
                else
                {
                    SeriesCollection.Add(lsEclObl);
                }
                PropChangedSeriesCollection();
            }
        }


        private void PropChangedSeriesCollection()
        {
            if (PropertyChanged != null)      // feuere Event
                PropertyChanged(this,
                    new PropertyChangedEventArgs("SeriesCollection"));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

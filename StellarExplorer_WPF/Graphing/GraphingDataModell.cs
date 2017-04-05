using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using LiveCharts;
using LiveCharts.Wpf;
using StellarExplorer_WPF.Annotations;
using StellarExplorer_WPF.DataViewer.Database;
using SweWin.LiveQuery;

namespace StellarExplorer_WPF.Graphing
{
    internal class GraphingDataModell : INotifyPropertyChanged
    {
        public stellardbEntities Stellardb = new stellardbEntities();
        private readonly LineSeries lsEclLat;
        private readonly LineSeries lsEclLong;
        private readonly LineSeries lsDist;
        private readonly LineSeries lsSpeed;
        private readonly LineSeries lsHouse;
        private LineSeries lsNutation;
        private readonly LineSeries lsEclObl;
        public SeriesCollection SeriesCollection { get; set; }
        public Func<double, string> XFormatter { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public string[] Labels { get; set; }

        private readonly List<string> chartMonths = new List<string>();
        private readonly ChartValues<double> chartEclLong = new ChartValues<double>();
        private readonly ChartValues<double> chartEclLat = new ChartValues<double>();
        private readonly ChartValues<double> chartDist = new ChartValues<double>();
        private readonly ChartValues<double> chartSpeed = new ChartValues<double>();
        private readonly ChartValues<double> chartHouse = new ChartValues<double>();
        private readonly ChartValues<double> chartEclObl = new ChartValues<double>();
        private readonly ChartValues<double> chartNutation = new ChartValues<double>();

        private string _selectedDatabase;
        public List<string> SelectAbleDatabasesList { get; set; }

        public string SelectedDatabase
        {
            get { return _selectedDatabase; }
            set
            {
                _selectedDatabase = value;
                var databaseId = (from entry in Stellardb.Crude_Entries
                    where entry.ce_ephemerisname.Equals(value)
                    select entry.ce_id).First().ToString();
                var valsDb = new DBOperations().QueryTableTemplateFromTableGuid(databaseId);
                chartMonths.Clear();
                chartEclLong.Clear();
                chartEclLat.Clear();
                chartDist.Clear();
                chartSpeed.Clear();
                chartHouse.Clear();
                chartEclObl.Clear();
                foreach (var tableTemplate in valsDb)
                {
                    chartMonths.Add(tableTemplate.t_date.ToLongDateString());
                    chartEclLong.Add(double.Parse(tableTemplate.t_ecllong));
                    chartEclLat.Add(double.Parse(tableTemplate.t_ecllat));
                    chartDist.Add(double.Parse(tableTemplate.t_dist));
                    chartSpeed.Add(double.Parse(tableTemplate.t_speed));
                    chartHouse.Add(double.Parse(tableTemplate.t_house));
                    chartEclObl.Add(double.Parse(tableTemplate.t_eclobl));
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


            foreach (var a in ed)
            {
                chartMonths.Add(a.t_date.ToLongDateString());
                chartEclLong.Add(double.Parse(a.t_ecllong));
                chartEclLat.Add(double.Parse(a.t_ecllat));
                chartDist.Add(double.Parse(a.t_dist));
                chartSpeed.Add(double.Parse(a.t_speed));
                chartHouse.Add(double.Parse(a.t_house));
                chartEclObl.Add(double.Parse(a.t_eclobl));
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

            XFormatter = val => new DateTime((long) val).ToString();
            YFormatter = val => val.ToString();
            Labels = chartMonths.ToArray();
        }

        private bool _eclLongChecked;
        private bool _eclLatChecked;
        private bool _distanceChecked;
        private bool _houseChecked;
        private bool _speedChecked;
        private bool _eclOblChecked;

        public bool EclLongChecked
        {
            get { return _eclLongChecked; }
            set
            {
                _eclLongChecked = value;
                if (value == false)
                    SeriesCollection.Remove(lsEclLong);
                else
                    SeriesCollection.Add(lsEclLong);
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
                    SeriesCollection.Remove(lsEclLat);
                else
                    SeriesCollection.Add(lsEclLat);
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
                    SeriesCollection.Remove(lsDist);
                else
                    SeriesCollection.Add(lsDist);
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
                    SeriesCollection.Remove(lsHouse);
                else
                    SeriesCollection.Add(lsHouse);
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
                    SeriesCollection.Remove(lsSpeed);
                else
                    SeriesCollection.Add(lsSpeed);
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
                    SeriesCollection.Remove(lsEclObl);
                else
                    SeriesCollection.Add(lsEclObl);
                PropChangedSeriesCollection();
            }
        }


        private void PropChangedSeriesCollection()
        {
            if (PropertyChanged != null) // feuere Event
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

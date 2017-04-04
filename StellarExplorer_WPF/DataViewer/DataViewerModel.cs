using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using StellarExplorer_WPF.DataViewer.Database;
using StellarExplorer_WPF.MinorCenter;
using StellarExplorer_WPF.Modelle;
using SweWin.LiveQuery;
using WpfDBpruef.modelle;

namespace StellarExplorer_WPF.DataViewer
{
    public partial class DataViewerModel
    {

        private List<String> _dataBaseBodiesList = new List<string>() {"Sun","Moon","Mercury","Venus","Mars","Jupiter","Saturn","Uranus","Neptune","Pluto"};
        private List<String> _selectableIntervalsList  = new List<string>() {"Days", "Months","Years"};

        private DateTime _dateTimeFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        private DateTime _dateTimeUntil = new DateTime(DateTime.Now.Year+2, DateTime.Now.Month, DateTime.Now.Day);
        private String _selectedBody;
        private String _saveToDatabaseAs;
        private int _selectedInterval = 1;
        private String _selectedIntervalString;
         
        private DBOperations dbOperations = new DBOperations();
        private FormData _formData = new FormData();

        public DateTime DateTimeFrom
        {
            get { return _dateTimeFrom; }
            set { _dateTimeFrom = value; }
        }

        public DateTime DateTimeUntil
        {
            get { return _dateTimeUntil; }
            set { _dateTimeUntil = value; }
        }

        public string SelectedBody
        {
            get { return _selectedBody; }
            set { _selectedBody = value; }
        }

        public string SaveToDatabaseAs
        {
            get { return _saveToDatabaseAs; }
            set { _saveToDatabaseAs = value; }
        }

        public int SelectedInterval
        {
            get { return _selectedInterval; }
            set { _selectedInterval = value; }
        }

        public List<string> DataBaseBodiesList
        {
            get { return _dataBaseBodiesList; }
            set { _dataBaseBodiesList = value; }
        }

        public List<string> SelectableIntervalsList
        {
            get { return _selectableIntervalsList; }
            set { _selectableIntervalsList = value; }
        }

        public string SelectedIntervalString
        {
            get { return _selectedIntervalString; }
            set { _selectedIntervalString = value; }
        }

        private ICommand _calculateCommand;
        public ICommand CalculateCommand
        {
            get
            {
                if (_calculateCommand == null)
                    _calculateCommand =
                        new DelegateCommand(SaveExecuted
                                            );
                return _calculateCommand;
            }
        }

        public void SaveExecuted(object param)
        {
            
            
            //var counter = dbOperations.QueryEphemerisNamesList().Count;
            _formData = new FormData();
            _formData.MonitorBody = SelectedBody;


            DateTime dzero = new DateTime(1, 1, 1);
            StringBuilder buf = new StringBuilder();
            _formData.swisseph(buf);
            

            List<TableEphemerisEntry> calculatedEntries = new List<TableEphemerisEntry>();

            //DateTime from = new DateTime(int.Parse(EDIT_FROM_YEAR.Text), int.Parse(EDIT_FROM_MONTH.Text), int.Parse(EDIT_FROM_DAY.Text));
            //DateTime until = new DateTime(int.Parse(EDIT_UNTIL_YEAR.Text), int.Parse(EDIT_UNTIL_MONTH.Text), int.Parse(EDIT_UNTIL_DAY.Text));

            //int interval = int.Parse(textBox1.Text);

            //Crude_Entries ce = new Crude_Entries
            //{
            //    ce_id = Guid.NewGuid(),
            //    ce_ephemerisname = EDIT_OBJECT.Text,
            //    ce_from = from,
            //    ce_until = until,
            //    ce_geomatricallatitude = "GeoLat",
            //    ce_geometricallongitude = "GeoLon",
            //    ce_system = "LeSystem",
            //    ce_withasteroids = new byte[] { 1 },
            //    ce_withhyperbolicbodies = new byte[] { 0 },
            //    ce_service = "LeService",
            //    ce_object = "Sun",
            //    ce_abovesea = 399,
            //    ce_intervals = "intervals"
            //};

            if (SelectedIntervalString.Equals("Days"))
            {
            //Days
                while ((DateTimeFrom - dzero).TotalDays < (DateTimeUntil - dzero).TotalDays)
                {

                    _formData.DateTime = DateTimeFrom;
                    _formData.swisseph(buf);
                    calculatedEntries.Add(_formData.ExtractTemplateFromStringBuilder());

                    DateTimeFrom = DateTimeFrom.AddDays(SelectedInterval);
                }
            }
            else if (SelectedIntervalString.Equals("Months"))
            {
            //    //Months
                while (MonthDifference(DateTimeUntil, DateTimeFrom) > 0)
                {

                    _formData.DateTime = DateTimeFrom;
                    _formData.swisseph(buf);
                    calculatedEntries.Add(_formData.ExtractTemplateFromStringBuilder());

                    DateTimeFrom = DateTimeFrom.AddMonths(SelectedInterval);
                }

            }
            else if (SelectedIntervalString.Equals("Years"))
            {
            //    //Years
               while (YearDifference(DateTimeUntil, DateTimeFrom) > 0)
               {

                   _formData.DateTime = DateTimeFrom;
                   _formData.swisseph(buf);
                   calculatedEntries.Add(_formData.ExtractTemplateFromStringBuilder());

                         DateTimeFrom = DateTimeFrom.AddYears(SelectedInterval);
               }
            }


            ////MessageBox.Show(calculatedEntries.Count.ToString(), "asdf");

            //Crude_Entries crude = CreateNewCrudeEntries();

            //insertCrudeEntry(crude);
            //CreateTable(crude.ce_id);

            //InsertObjectIntoTable(crude.ce_id, calculatedEntries);

            //MessageBox.Show("Done", "ai");



        }

        public Crude_Entries CreateNewCrudeEntries()
        {
            Crude_Entries parseEntity = new Crude_Entries
            {
                ce_id = Guid.NewGuid(),
                ce_ephemerisname = SaveToDatabaseAs,
                ce_from = DateTimeFrom,
                ce_until = DateTimeUntil, int.Parse(EDIT_UNTIL_DAY.Text)),
                ce_geometricallongitude = EDIT_LONG.Text + EDIT_LONGM.Text + EDIT_LONGS.Text,
                ce_geomatricallatitude = EDIT_LAT.Text + EDIT_LAT.Text + EDIT_LAT.Text,
                ce_system = COMBO_CENTER.Text,
                ce_service = COMBO_EPHE.Text,
                ce_abovesea = int.Parse(EDIT_ALT.Text),
                ce_object = EDIT_OBJECT.Text,
                ce_intervals = textBox1.Text + "-" + COMBO_INTS.SelectedItem.ToString()
            };
            //{ "main planets", "with asteroids", "with hyp. bodies" };
            parseEntity.ce_withasteroids = new byte[] { 0 };
            parseEntity.ce_withhyperbolicbodies = new byte[] { 0 };
            if (COMBO_PLANSEL.Text.Equals(plansel[1])) parseEntity.ce_withasteroids = new byte[] { 1 };
            if (COMBO_PLANSEL.Text.Equals(plansel[2])) parseEntity.ce_withhyperbolicbodies = new byte[] { 1 };

            //stellarEntity.Crude_Entries.

            return parseEntity;
        }

        public static int MonthDifference(DateTime lValue, DateTime rValue)
        {
            return (lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year);
        }
        
        public static int YearDifference(DateTime lValue, DateTime rValue)
        {
            return lValue.Year - rValue.Year;
        }

    }
}

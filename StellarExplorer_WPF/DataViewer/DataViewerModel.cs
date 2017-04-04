using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
            DBOperations dbOperations = new DBOperations();
            var counter = dbOperations.QueryEphemerisNamesList().Count;
        }
    }
}

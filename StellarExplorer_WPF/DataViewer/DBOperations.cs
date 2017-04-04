using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using StellarExplorer_WPF.DataViewer.Database;
using StellarExplorer_WPF.MinorCenter;
using StellarExplorer_WPF.Modelle;

namespace SweWin.LiveQuery
{
    public class DBOperations
    {
        stellardbEntities _entities = new stellardbEntities();

        public List<Crude_Entries> QueryEphemerisNamesList()
        {
            String command = "SELECT [ce_id],[ce_ephemerisname],[ce_from],[ce_until],[ce_geometricallongitude],[ce_geomatricallatitude],[ce_system],[ce_withasteroids],[ce_withhyperbolicbodies],[ce_service],[ce_abovesea],[ce_intervals],[ce_object] FROM[dbo].[tablename]";
            command = command.Replace("tablename", "Crude_Entries");


            List<Crude_Entries> entryList = null;
            
                SqlParameter @TableName = new SqlParameter()
                {
                    ParameterName = "@TableName",
                    DbType = DbType.String,
                    Value = "Crude_Entries"
                };

                entryList = _entities.Database.SqlQuery<Crude_Entries>(command).ToList<Crude_Entries>();
            
            return entryList;
        }

        public List<Crude_Entries> QueryEphemerisList()
        {
            var erg = from entry in _entities.Crude_Entries
                select new Crude_Entries();
            return (List<Crude_Entries>) erg;
        }

        public List<Table_Template> QueryTableTemplateFromTableGuid(String guid)
        {
            String command = "SELECT [t_date],[t_ecllong],[t_ecllat],[t_dist],[t_speed],[t_house],[t_eclobl],[t_nutation] FROM[dbo].[tablename]";
            command = command.Replace("tablename", guid);


            List<Table_Template> entryList = null;
            
                SqlParameter @TableName = new SqlParameter()
                {
                    ParameterName = "@TableName",
                    DbType = DbType.String,
                    Value = guid
                };

                entryList = _entities.Database.SqlQuery<Table_Template>(command).ToList<Table_Template>();
            
            return entryList;
        }





        public bool InsertObjectIntoTable(Guid guid, List<TableEphemerisEntry> e)
        {
            StringBuilder sql = new StringBuilder().Append(@"INSERT INTO [dbo].[tablename] 
                ([t_date], [t_ecllong], [t_ecllat], [t_dist], [t_speed], [t_house], [t_eclobl], [t_nutation]) 
                VALUES ");
            //(N'rep_t_date', N'rep_t_ecllong', N'rep_t_ecllat', N'rep_t_dist', 
            //N'rep_t_speed', N'rep_t_house', N'rep_t_eclobl', N'rep_t_nutation')
            List<String> insertStringList = new List<string>();
            int counter = 0;
            int max = 999;
            foreach (TableEphemerisEntry entry in e)
            {
                if (counter < max)                  {
                    sql.Append("(N'" + entry.custom_date + " 00:00:00" + "',");
                    sql.Append("N'" + entry.custom_ecllong + "',");
                    sql.Append("N'" + entry.custom_ecllat + "',");
                    sql.Append("N'" + entry.custom_dist + "',");
                    sql.Append("N'" + entry.custom_speed + "',");
                    sql.Append("N'" + entry.custom_house + "',");
                    sql.Append("N'" + entry.custom_ecllong + "',");
                    sql.Append("N'" + entry.custom_nutation.Replace("'", "#") + "')");
                    sql.Append(",");
                    counter++;
                }
                else 
                {
                    String entire = sql.ToString();
                    entire = entire.Remove(entire.Length - 1, 1);

                    entire = entire.Replace("tablename", guid.ToString());
                    entire = entire.Replace("\r", String.Empty);
                    entire = entire.Replace("\n", String.Empty);
                    _entities.Database.ExecuteSqlCommand(
                    entire);
                    _entities.SaveChanges();
                    counter = 0;
                    sql = sql.Clear();
                    sql.Append(@"INSERT INTO [dbo].[tablename] 
                ([t_date], [t_ecllong], [t_ecllat], [t_dist], [t_speed], [t_house], [t_eclobl], [t_nutation]) 
                VALUES ");
                }
            }
            String entire1 = sql.ToString();
            entire1 = entire1.Remove(entire1.Length - 1, 1);
            entire1 = entire1.Replace("tablename", guid.ToString());
            entire1 = entire1.Replace("\r", String.Empty);
            entire1 = entire1.Replace("\n", String.Empty);
            _entities.Database.ExecuteSqlCommand(
            entire1);
            _entities.SaveChanges();
            return true;
        }



        


    }
}

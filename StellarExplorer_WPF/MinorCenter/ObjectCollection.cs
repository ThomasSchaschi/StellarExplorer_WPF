using System;

namespace StellarExplorer_WPF.MinorCenter
{
    class ObjectCollection
    {

    }

   
    class AsteroidJson
    {
        public string sigma_tp { get; set; }
        public string diameter { get; set; }
        public string tp { get; set; }
        public string epoch_mjd { get; set; }
        public string ad { get; set; }
        public string producer { get; set; }
        public string rms { get; set; }
        public string H_sigma { get; set; }
        public string closeness { get; set; }
        public string spec { get; set; }
        public string K2 { get; set; }
        public string K1 { get; set; }
        public string M1 { get; set; }
        public string n_dop_obs_used { get; set; }
        public string full_name { get; set; }
        public string M2 { get; set; }
        public string prov_des { get; set; }
        public string equinox { get; set; }
        public string DT { get; set; }
        public string diameter_sigma { get; set; }
        public string saved { get; set; }
        public string albedo { get; set; }
        public string moid_ld { get; set; }
        public string two_body { get; set; }
        public string neo { get; set; }
        public string prefix { get; set; }
        public string sigma_ad { get; set; }
        public string score { get; set; }
        public string data_arc { get; set; }
        public string profit { get; set; }
        public string sigma_q { get; set; }
        public string sigma_w { get; set; }
        public string epoch { get; set; }
        public string per { get; set; }
        public string id { get; set; }
        public string A1 { get; set; }
        public string PC { get; set; }
        public string A3 { get; set; }
        public string A2 { get; set; }
        public string per_y { get; set; }
        public string n_opp { get; set; }
        public string epoch_cal { get; set; }
        public string orbit_id { get; set; }
        public string sigma_a { get; set; }
        public string sigma_om { get; set; }
        public string price { get; set; }
        public string sigma_e { get; set; }
        public string condition_code { get; set; }
        public string rot_per { get; set; }
        public string G { get; set; }
        public string last_obs { get; set; }
        public string H { get; set; }
        public string pha { get; set; }
        public string IR { get; set; }
        public bool inexact { get; set; }
        public string spec_T { get; set; }
        public string tp_cal { get; set; }
        public string n_obs_used { get; set; }
        public string moid { get; set; }
        public string extent { get; set; }
        public string dv { get; set; }
        public string ma { get; set; }
        public string GM { get; set; }
        public string pdes { get; set; }
        public string @class { get; set; }
        public string a { get; set; }
        public string t_jup { get; set; }
        public string om { get; set; }
        public string e { get; set; }
        public string name { get; set; }
        public string i { get; set; }
        public string sigma_per { get; set; }
        public string BV { get; set; }
        public string n { get; set; }
        public string q { get; set; }
        public string sigma_i { get; set; }
        public string w { get; set; }
        public string sigma_ma { get; set; }
        public string first_obs { get; set; }
        public string n_del_obs_used { get; set; }
        public string sigma_n { get; set; }
        public string spkid { get; set; }
        public string UB { get; set; }
        
    }



    public class TableEphemerisEntry
    {
        public String custom_nutation,
                custom_ecllong,
                custom_ecllat,
                custom_dist,
                custom_speed,
                custom_house,
                custom_eclobl,
                custom_date;

        public TableEphemerisEntry(string customDate, string customDist, string customEcllat, string customEcllong, string customEclobl, string customHouse, string customNutation, string customSpeed)
        {
            custom_date = customDate;
            custom_dist = customDist;
            custom_ecllat = customEcllat;
            custom_ecllong = customEcllong;
            custom_eclobl = customEclobl;
            custom_house = customHouse;
            custom_nutation = customNutation;
            custom_speed = customSpeed;
        }

        public String returnAsCommaList()
        {
            return String.Concat(custom_date, ",", custom_ecllong, ",", custom_ecllat, ",", custom_dist, ",",
                custom_speed, ",", custom_house, ",", custom_eclobl, ",", custom_nutation);
        }
    }

   

}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CaribbeanSailboat.Database;
using Oracle.ManagedDataAccess.Client;

namespace CaribbeanSailboat.Pages.Report
{
    public partial class Report
    {
        public List<Charter> CustomerIDCharters { get; private set; } = new List<Charter>();
        public List<Charter> DateRangeCharters { get; private set; } = new List<Charter>();
        private string customerID;
        private bool isCustomerIDView;
        private bool searchPerformed = false;
        private DateTime? startDateFilter;
        private DateTime? endDateFilter;

        private async Task LoadFilteredCharters()
        {
            CustomerIDCharters.Clear();
            if (!string.IsNullOrEmpty(customerID))
            {
                await LoadChartersAsync(CustomerIDCharters, customerID: customerID);
            }
            searchPerformed = true;
        }

        private async Task LoadChartersByDateRange()
        {

            DateRangeCharters.Clear();
            if (startDateFilter.HasValue && endDateFilter.HasValue)
            {
                await LoadChartersAsync(DateRangeCharters, startDate: startDateFilter, endDate: endDateFilter);
            }
            searchPerformed = true; // Ensure this is set to true after the search
        }


        private async Task LoadChartersAsync(List<Charter> targetList, string customerID = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var dbConfig = DBConfigReader.ReadConfig("dbConfig.json");
            var connectionString = $"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={dbConfig.Host})(PORT={dbConfig.Port}))) (CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={dbConfig.ServiceName})));User Id={dbConfig.UserId};Password={dbConfig.Password};";

            using (var connection = new OracleConnection(connectionString))
            {
                var command = new OracleCommand();
                command.Connection = connection;

                if (!string.IsNullOrEmpty(customerID))
                {
                    command.CommandText = @"
                SELECT 
                    c.CUST_FNAME, c.CUST_LNAME, c.CUST_EMAIL, 
                    b.BOAT_NAME, b.BOAT_SIZE, b.BOAT_RENTAL_COST,
                    w.WEATHER_DESC, 
                    i.ITINERARY_NAME
                FROM CHARTER ch
                JOIN CUSTOMER c ON ch.CUST_ID = c.CUST_ID
                JOIN BOAT b ON ch.BOAT_ID = b.BOAT_ID
                JOIN WEATHER w ON ch.WEATHER_ID = w.WEATHER_ID
                JOIN ITINERARY i ON ch.ITINERARY_ID = i.ITINERARY_ID
                WHERE ch.CUST_ID = :customerID";
                    command.Parameters.Add(new OracleParameter("customerID", customerID));
                }
                else if (startDate.HasValue && endDate.HasValue)
                {
                    Console.WriteLine($"Date range search parameters - StartDate: {startDate}, EndDate: {endDate}");

                    command.CommandText = @"
                SELECT 
                    cu.CUST_FNAME, cu.CUST_LNAME, 
                    b.BOAT_NAME, 
                    cr.CREW_FNAME, cr.CREW_LNAME, 
                    i.ITINERARY_NAME, 
                    w.WEATHER_DESC,
                    ch.CHARTER_START_DATE, ch.CHARTER_END_DATE, ch.CHARTER_RETURN_DATE
                FROM CHARTER ch
                JOIN CUSTOMER cu ON ch.CUST_ID = cu.CUST_ID
                JOIN BOAT b ON ch.BOAT_ID = b.BOAT_ID
                JOIN CREW cr ON ch.CREW_ID = cr.CREW_ID
                JOIN ITINERARY i ON ch.ITINERARY_ID = i.ITINERARY_ID
                JOIN WEATHER w ON ch.WEATHER_ID = w.WEATHER_ID
                WHERE ch.CHARTER_START_DATE >= :startDate AND ch.CHARTER_END_DATE <= :endDate";
                    command.Parameters.Add(new OracleParameter("startDate", startDate.Value));
                    command.Parameters.Add(new OracleParameter("endDate", endDate.Value));
                }
                else
                {
                    // Default query if no parameters are specified
                    command.CommandText = @"/* Your default SQL query */";
                }

                connection.Open();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        
                        var charter = new Charter();
                        // Populate Charter object based on the query executed (customerID or date range)
                        if (!string.IsNullOrEmpty(customerID))
                        {
                            charter.CustomerFirstName = reader["CUST_FNAME"] != DBNull.Value ? reader["CUST_FNAME"].ToString() : "null";
                            charter.CustomerLastName = reader["CUST_LNAME"] != DBNull.Value ? reader["CUST_LNAME"].ToString() : "null";
                            charter.CustomerEmail = reader["CUST_EMAIL"] != DBNull.Value ? reader["CUST_EMAIL"].ToString() : "null";
                            charter.BoatName = reader["BOAT_NAME"] != DBNull.Value ? reader["BOAT_NAME"].ToString() : "null";
                            charter.BoatSize = reader["BOAT_SIZE"] != DBNull.Value ? reader["BOAT_SIZE"].ToString() : "null";
                            charter.WeatherDescription = reader["WEATHER_DESC"] != DBNull.Value ? reader["WEATHER_DESC"].ToString() : "null";
                            charter.ItineraryName = reader["ITINERARY_NAME"] != DBNull.Value ? reader["ITINERARY_NAME"].ToString() : "null";
                            charter.BoatRentalCost = reader["BOAT_RENTAL_COST"] != DBNull.Value ? Convert.ToDecimal(reader["BOAT_RENTAL_COST"]) : 0;
                        }
                        else if (startDate.HasValue && endDate.HasValue)
                        {
                            charter.CustomerFirstName = reader["CUST_FNAME"] as string ?? "null";
                            charter.CustomerLastName = reader["CUST_LNAME"] as string ?? "null";
                            charter.BoatName = reader["BOAT_NAME"] as string ?? "null";
                            charter.CrewFirstName = reader["CREW_FNAME"] as string ?? "null";
                            charter.CrewLastName = reader["CREW_LNAME"] as string ?? "null";
                            charter.ItineraryName = reader["ITINERARY_NAME"] as string ?? "null";
                            charter.WeatherDescription = reader["WEATHER_DESC"] as string ?? "null";
                            charter.CharterStartDate = reader["CHARTER_START_DATE"]?.ToString() ?? "null";
                            charter.CharterEndDate = reader["CHARTER_END_DATE"]?.ToString() ?? "null";
                            charter.CharterReturnDate = reader["CHARTER_RETURN_DATE"]?.ToString() ?? "null";
                        }
                        targetList.Add(charter);
                    }
                }
            }
            Console.WriteLine($"Total records fetched: {targetList.Count}");
        }
    }
    public class Charter
    {
        public string CharterID { get; set; }
        public string BoatID { get; set; }
        public string CustomerID { get; set; }
        public string CrewID { get; set; }
        public string ItineraryID { get; set; }
        public string WeatherID { get; set; }
        public string CharterStartDate { get; set; }
        public string CharterEndDate { get; set; }
        public string CharterReturnDate { get; set; }

        // New Shtuff

        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerEmail { get; set; }
        public string BoatName { get; set; }
        public string BoatSize { get; set; }
        public string WeatherDescription { get; set; }
        public string ItineraryName { get; set; }
        public decimal BoatRentalCost { get; set; }
        public string CrewFirstName { get; set; }
        public string CrewLastName { get; set; }
    }
}


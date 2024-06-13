﻿using System;
namespace Stackoverflow.Web.Models
{
    public static class WebConstants
    {
        //check your API where it runs then store here
        //public const string ApiUrl = "http://localhost:5293/v3/";
        //public const string ApiUrl = "https://localhost:7278/v3/";
        //public static string ApiUrl = System.Environment.GetEnvironmentVariable("API_URL") ?? "http://localhost:5293/v3/";
        public static string ApiUrl = System.Environment.GetEnvironmentVariable("API_URL") ?? "https://kecosa7377.bsite.net/v3/";
        public static string DefaultStackoverflowConnection = System.Environment.GetEnvironmentVariable("DefaultStackoverflowConnection") ?? "Server=sql.bsite.net\\MSSQL2016;Database=kartik_StackOverflow;User Id=kartik_StackOverflow;Password=hsn#2*#YW9Q%!M5;Trust Server Certificate=True;";
    }

}

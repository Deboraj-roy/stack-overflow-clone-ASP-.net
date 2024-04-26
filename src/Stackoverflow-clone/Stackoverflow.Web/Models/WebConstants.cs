﻿using System;
namespace Stackoverflow.Web.Models
{
    public static class WebConstants
    {
        //check your API where it runs then store here
        //public const string ApiUrl = "http://localhost:5293/v3/";
        //public const string ApiUrl = "https://localhost:7278/v3/";
        public static string ApiUrl = System.Environment.GetEnvironmentVariable("API_URL") ?? "http://localhost:5293/v3/";
    }

}

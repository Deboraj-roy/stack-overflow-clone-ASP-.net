﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stackoverflow.Infrastructure
{
    public interface IApplicationDbContext
    {
    }
//dotnet ef migrations add initial --project Stackoverflow.Web --context ApplicationDbContext
}
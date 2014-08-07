using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ExpatManager.Models;

namespace ExpatManager.DAL
{
    public class HRInitializer : CreateDatabaseIfNotExists<HRContext>
    {

    }
}
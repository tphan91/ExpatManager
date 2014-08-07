using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Data.Objects.SqlClient;
using ExpatManager.Models;
using ExpatManager.DAL;
using System.Web.Mvc;

namespace ExpatManager.Helper
{
    public class dbHelper
    {
        private static ExpatriateManagementContext HR = new ExpatriateManagementContext();
        //private static HRContext HR = new HRContext();

        public static ExpatriateHistory CreateExpatriateHistory(Expatriate expatriate)
        {
            ExpatriateHistory expatriateHistory = new ExpatriateHistory();
            expatriateHistory.CreateBy = expatriate.CreateBy;
            expatriateHistory.CreateDateTime = DateTime.Now;
            expatriateHistory.ModifiedBy = expatriate.ModifiedBy;
            expatriateHistory.ModifiedDateTime = DateTime.Now;
            expatriateHistory.CeilingValue = expatriate.CeilingValue;
            expatriateHistory.CostCode = expatriate.CostCode;
            expatriateHistory.DateOfPromotion = expatriate.DateOfPromotion;
            expatriateHistory.ExpatriateID = expatriate.ExpatriateID;
            expatriateHistory.FamilyStatus = expatriate.FamilyStatus;
            expatriateHistory.JobGrade = expatriate.JobGrade;
            expatriateHistory.Promotion = expatriate.Promotion;
            expatriateHistory.Status = expatriate.Status;
            expatriateHistory.Title = expatriate.Title;

            return expatriateHistory;
        }

        public static vPersonJob vPersonJobFind(String CIF)
        {
            var _pj = HR.PersonJobs
            .Where(d => d.PayRollNo == CIF)
            .AsNoTracking()
            .FirstOrDefault();
            return _pj;
        }
    }
}
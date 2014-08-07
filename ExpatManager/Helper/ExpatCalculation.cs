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
    public class ExpatCalculation
    {
        private static ExpatriateManagementContext EM = new ExpatriateManagementContext();

        public static decimal CeilingValue(int JobGradeId, int FamilyStatusId)
        {
            if (FamilyStatusId > 2) { FamilyStatusId = 2; }
            var _CeilingTable = EM.CeilingTables
                .Where(d => d.JobGradeId.Equals(JobGradeId) && d.FamilyStatusId.Equals(FamilyStatusId))
                .AsNoTracking()
                .FirstOrDefault();
            
            return _CeilingTable.CeilingValue;
        }

        public static decimal CeilingBreach(Decimal CeilingValue, Decimal MonthlyPayment)
        {
            return MonthlyPayment - CeilingValue;
        }

        public static decimal ProRataPayment(DateTime StartDate, Decimal MonthlyPayment)
        {
            int NumberOfDays;
            int DayOfMonth = StartDate.Day;
            DateTime dt = new DateTime(StartDate.Year, StartDate.Month, 10);

            if (DayOfMonth > 10)
            {
                NumberOfDays = (dt.AddMonths(1) - StartDate).Days;  
            }
            else if (DayOfMonth < 10)
            {
                NumberOfDays = (dt - StartDate).Days;
            }
            else
            {
                NumberOfDays = 0; 
            }

            decimal ProRataPayment = Decimal.Round((TotalAmount(MonthlyPayment) / 365 * NumberOfDays),2);
            return ProRataPayment;
        }

        public static decimal ProRataPaymentAndMonthly(DateTime StartDate, Decimal MonthlyPayment)
        {
            int NumberOfDays;
            int DayOfMonth = StartDate.Day;
            DateTime dt = new DateTime(StartDate.Year, StartDate.Month, 10);

            if (DayOfMonth > 10)
            {
                NumberOfDays = (dt.AddMonths(1) - StartDate).Days;
            }
            else if (DayOfMonth < 10)
            {
                NumberOfDays = (dt - StartDate).Days;
            }
            else
            {
                NumberOfDays = 0;
            }

            decimal ProRataPayment = Decimal.Round((TotalAmount(MonthlyPayment) / 365 * NumberOfDays) + MonthlyPayment, 2);
            return ProRataPayment;
        }

        public static decimal FinalPayment(Decimal MonthlyPayment, Decimal ProRataPayment)
        {
            decimal FinalPayment = TotalAmount(MonthlyPayment) - ((MonthlyPayment * 10) + ProRataPayment);
            return FinalPayment;
        }

        public static decimal FinalPaymentRenewal(Decimal MonthlyPayment, Decimal ProRataPayment)
        {
            decimal FinalPayment = TotalAmount(MonthlyPayment) - ((MonthlyPayment * 11) + ProRataPayment);
            return FinalPayment;
        }

        public static decimal TotalAmount(Decimal MonthlyPayment)
        {
             return MonthlyPayment * 12;
        }
    }
}
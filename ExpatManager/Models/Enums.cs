using System;
using System.ComponentModel;
using ExpatManager.Helper;

namespace ExpatManager.Models
{
    public class Enums
    {
        public enum EnumDocumentType
        {
            Other = 0,
            CouncilTax = 1,
            CouncilTaxRefund = 2
        }

        //[TypeConverter(typeof(HtmlDropDownExtensions.PascalCaseWordSplittingEnumConverter))]
        public enum EnumFamilyStatus
        {
            [Description("Single")]
            SINGLE = 0,

            [Description("Spouse")]
            SPOUSE = 1,

            [Description("Spouse And Child")]
            SPOUSEANDCHILD = 2,

            [Description("Spouse And 2 Children")]
            SPOUSEANDCHILDx2 = 3,

            [Description("Spouse And 3 Children")]
            SPOUSEANDCHILDx3 = 4,

            [Description("Spouse And 4 Children")]
            SPOUSEANDCHILDx4 = 5,

            [Description("Spouse And 5 Children")]
            SPOUSEANDCHILDx5 = 6
        }

        public enum EnumFamilyType
        {
            SPOUSE = 0,
            CHILD = 1
        }

        public enum EnumJobGrade
        {

            [Description("")]
            O = 0,

            [Description("")]
            M = 1,

            [Description("")]
            L = 2,

            [Description("")]
            SL = 3,

            [Description("")]
            F = 4,

            [Description("")]
            RIJI = 5,

            [Description("")]
            GM = 6,

            [Description("")]
            YAKUIN = 7
        }

        public enum EnumTitle
        {
            MR = 0,
            MISS = 1,
            MS = 2,
            MRS = 3,
        }

        public enum EnumModelName
        {
            Agent = 1,
            AgreementDetail = 2,
            AgreementPayment = 3,
            CeilingTable = 4,
            Expartriate = 5,
            ExpatriateHistory = 6,
            Family = 7,
            LandlordBankDetail = 8
        }

        public enum EnumDepartureReason
        {
            [Description("Unselected")]
            Unselected = 0,
            [Description("Recalled")]
            Recalled = 1,
            [Description("Choice To Move")]
            ChoiceToMove = 2,
            [Description("Forced Move")]
            ForcedMove = 3,
            [Description("Family To Remain")]
            FamilyToRemain = 4,
            [Description("Resigned")]
            Resigned = 5 
        }

        public enum AuditAction
        {
            [Description("Insert")]
            I,
            [Description("Update")]
            U,
            [Description("Delete")]
            D
        }
    }
}
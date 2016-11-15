﻿using NIMBOLE.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace NIMBOLE.Models.Models
{
    public class AccountModel  
    {
        #region Properties
        public Int64 Id { get; set; }
        public string AccountCode { get; set; }
        public Guid TenantId { get; set; }
        public string AccountName { get; set; }
        public string AccountOwner { get; set; }
        public string AccountClassification { get; set; }
        public string ParentAccount { get; set; }
        public Int64 Employees { get; set; }
        public string OwnerShip { get; set; }
        public string Industry { get; set; }
        public string AccountType { get; set; }
        public bool IsParentAccount { get; set; }
        public string Subsidiary { get; set; }
        public string Distributor { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Rating { get; set; }
        public string SICCode { get; set; }
        public Int64 AnnualRevenue { get; set; }
        public string Website { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Status { get; set; }

        public string Region { get; set; }

        #endregion

        #region Constructor
        public AccountModel()
        {

        }
        #endregion
    }

    //public class AExcelImport
    //{
    //    //   public HttpPostedFileBase pfiles { get; set; }
    //    public int Id { get; set; }
    //    public bool IsSelected { get; set; }
    //    public string AccountCode { get; set; }
    //    public string AccountName { get; set; }
    //    public string AccountOwner { get; set; }
    //    public string AccountClassification { get; set; }
    //    public string ParentAccount { get; set; }
    //    public Int32 NoofEmployees { get; set; }
    //    public string Ownership { get; set; }
    //    public string Industry { get; set; }
    //    public string AccountType { get; set; }
    //    public bool IsParentAccount { get; set; }
    //    public string Subsidary { get; set; }
    //    public string Region { get; set; }
    //    public string Phone { get; set; }
    //    public string Fax { get; set; }
    //    public string Email { get; set; }
    //    public Int32 Rating { get; set; }
    //    public string SICCode { get; set; }
    //    public decimal AnnualRevenue { get; set; }
    //    public string Website { get; set; }
     
    //}

    //public class AccountExcelImport
    //{
    //    public AccountExcelImport()
    //    {
    //        AExcelImport = new List<AExcelImport>();
    //    }
    //    [Required(ErrorMessage = "Please Upload File.")]
    //    [ValidateFile]
    //    public HttpPostedFileBase ImportFile { get; set; }

    //    public List<AExcelImport> AExcelImport { get; set; }

    //    public string AInvalidHeaders { get; set; }
    //}

    //public class ValidateFileAttribute : ValidationAttribute
    //{
    //    public override bool IsValid(object value)
    //    {
    //        int MaxContentLength = 1024 * 1024 * 2;//3MB
    //        string[] AllowedFileExtensions = new string[] { ".xlsx" };
    //        var file = value as HttpPostedFileBase;

    //        if (file == null)
    //            return false;
    //        else if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
    //        {
    //            ErrorMessage = "Please Upload your xlsx of type:" + string.Join(",", AllowedFileExtensions);
    //            return false;
    //        }
    //        else if (file.ContentLength > MaxContentLength)
    //        {
    //            ErrorMessage = "Uploaded xlsx is too large, maximum allowed size is: " + (MaxContentLength / 1024).ToString() + "MB";
    //            return false;
    //        }
    //        else
    //            return true;
    //    }
    //}


}


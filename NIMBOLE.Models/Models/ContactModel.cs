using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace NIMBOLE.Models.Models
{
    public class ContactModel
    {
        #region Properties
        public Int64 Id { get; set; }
        public Guid TenantId { get; set; }
        public string ContactCode { get; set; }
        public string ContactImageURL { get; set; }
        public Int64 LeadSourceId { get; set; }

        //[Display(Name = "FirstName", ResourceType = typeof(NIMBOLE.GlobalResources.Resources))]
        //[Required(ErrorMessageResourceType = typeof(NIMBOLE.GlobalResources.Resources), ErrorMessageResourceName = "Required")]
        public string FirstName { get; set; }
		
        public string MiddleName { get; set; }

        //[Display(Name = "LastName", ResourceType = typeof(NIMBOLE.GlobalResources.Resources))]
        //[Required(ErrorMessageResourceType = typeof(NIMBOLE.GlobalResources.Resources), ErrorMessageResourceName = "Required")]
        public string LastName { get; set; }

        //[Display(Name = "Email", ResourceType = typeof(NIMBOLE.GlobalResources.Resources))]
        //[Required(ErrorMessageResourceType = typeof(NIMBOLE.GlobalResources.Resources), ErrorMessageResourceName = "Required")]
        public string ContactEmail { get; set; }
        public string WorkEmail { get; set; }
		
        public Int64 DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ContactRoleId { get; set; }
        public bool Status { get; set; }
        public string FullName { get; set; }

        #endregion
    }

    public class ExcelImport
    {
        public int Id { get; set; }
        public bool IsSelected { get; set; }
        public string ContactEmail { get; set; }
        public string WorkEmail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string LeadSource { get; set; }
        public string Designation { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string ZipCode { get; set; }
        public string Mobile { get; set; }
        public string OfficePhone { get; set; }
        public string Fax { get; set; }
        public string HomePhone { get; set; }
        public string SkypeName { get; set; }

        public string Comments { get; set; }
        public string ContactEmailId { get; set; }
        public string Department { get; set; }
        public bool Status { get; set; }
        public string AccountName { get; set; }
        public string DepartmentName { get; set; }
    }

    public class ContactExcelImport
    {
        public ContactExcelImport()
        {
            ExcelImport = new List<ExcelImport>();
        }
        //[Required(ErrorMessage = "Please Upload File.")]
        //[ValidateFile]
        public HttpPostedFileBase ImportFile { get; set; }

        public List<ExcelImport> ExcelImport { get; set; }

        public string InvalidHeaders { get; set; }
    }

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


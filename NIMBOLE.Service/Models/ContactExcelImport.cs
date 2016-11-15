using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NIMBOLE.Service.Models
{
   public class ContactExcelImport
    {
        public ContactExcelImport()
        {
            ExcelImport = new List<CExcelImport>();
        }
        [Required(ErrorMessage = "Please Upload File.")]
        [EValidateFile]
        public HttpPostedFileBase ImportFile { get; set; }
        public List<CExcelImport> ExcelImport { get; set; }
        public string InvalidHeaders { get; set; }

    }
    public class CExcelImport:NIMBOLE.Models.Models.ContactModel
    {
        public bool IsSelected { get; set; }
    }
    public class EValidateFileAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int MaxContentLength = 1024 * 1024 * 2; //3 MB
            string[] AllowedFileExtensions = new string[] { ".xls", ".xlsx" };

            var file = value as HttpPostedFileBase;

            if (file == null)
                return false;

            else if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
            {
                ErrorMessage = "Please upload your Excel of type: " + string.Join(", ", AllowedFileExtensions);
                return false;
            }

            else if (file.ContentLength > MaxContentLength)
            {
                ErrorMessage = "Uploaded excel is too large, maximum allowed size is : " + (MaxContentLength / 1024).ToString() + "MB";
                return false;
            }

            else
                return true;
        }
    }

}

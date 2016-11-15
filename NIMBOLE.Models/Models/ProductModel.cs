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

    public class ProductNamesModel
    {
        public Int64 ProductNamesId { get; set; }
        public string ProductName { get; set; }
    }

    public class PExcelImport
    {
        //   public HttpPostedFileBase pfiles { get; set; }
        public int Id { get; set; }
        public bool IsSelected { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public decimal Price { get; set; }
        public string ProductType { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Comments { get; set; }
        public string ManufacturerName { get; set; }

    }

    public class ProductExcelImport
    {
        public ProductExcelImport()
        {
            PExcelImport = new List<PExcelImport>();
        }
        [Required(ErrorMessage = "Please Upload File.")]
        [ValidateFile]
        public System.Web.HttpPostedFileBase ImportFile { get; set; }

        public List<PExcelImport> PExcelImport { get; set; }

        public string InvalidHeaders { get; set; }
    }


    public class ProductModel  
    {
        #region Properties

        public Int64 Id { get; set; }
        public Guid TenantId { get; set; }
        //[Required(ErrorMessage=GlobalResources.Resources.ProductName,ErrorMessageResourceName=NIMBOLE.GlobalResources.Resources.ProductName,ErrorMessageResourceType=typeof(string))]
        //[Required(ErrorMessageResourceType = typeof(GlobalResources.Resources), ErrorMessageResourceName = "ProductName")]
        //[Display(ResourceType = typeof(GlobalResources.Resources), Name = "ProductName")]
        [Display(Name = "ProductName", ResourceType = typeof(NIMBOLE.GlobalResources.Resources))]
        [Required(ErrorMessageResourceType = typeof(NIMBOLE.GlobalResources.Resources), ErrorMessageResourceName = "Required")]
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        [Display(Name = "Price", ResourceType = typeof(NIMBOLE.GlobalResources.Resources))]
        [Required(ErrorMessageResourceType = typeof(NIMBOLE.GlobalResources.Resources), ErrorMessageResourceName = "Required")]
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public Int64 Quantity { get; set; }
        public Int64 ProdId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        [Display(Name = "ProductType", ResourceType = typeof(NIMBOLE.GlobalResources.Resources))]
        [Required(ErrorMessageResourceType = typeof(NIMBOLE.GlobalResources.Resources), ErrorMessageResourceName = "Required")]
        public string ProductType { get; set; }
        public string ProductTypeId { get; set; }
        public string ProductTypeDes { get; set; }
        public string Comments { get; set; }
        public bool Status { get; set; }
        public string Active { get; set; }
        public string Change_status { get; set; }
        public string ManufacturerName { get; set; }
        public DateTime ManufacturerDate { get; set; }
        #endregion

        #region Constructor
        public ProductModel()
        {

        }
        #endregion


    }
}

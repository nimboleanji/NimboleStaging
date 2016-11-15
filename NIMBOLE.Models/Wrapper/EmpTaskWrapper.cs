using NIMBOLE.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NIMBOLE.Common;
using NIMBOLE.Entities;
using NIMBOLE.Models.Models.Transactions;
using NIMBOLE.Entities.Core;

namespace NIMBOLE.Models.Mappers
{
    public class EmpTaskWrapper
    {
        GenericRepository<TblEmpTask> repo;
        DTO dto;
        public EmpTaskWrapper()
        {
            repo= new GenericRepository<TblEmpTask>();
            dto = new DTO();
        }

        public IEnumerable<EmployeeTaskModel> Get()
        {
            var query = repo.Get();
            List<EmployeeTaskModel> lstEmployeeTaskModel = dto.MapTable2Model(query);
            return lstEmployeeTaskModel;
        }

        public virtual EmployeeTaskModel GetByID(object id)
        {
            var query=repo.GetByID(id);
            EmployeeTaskModel objEmployeeTaskModel=dto.MapTable2Model(query);
            return objEmployeeTaskModel;
        }

        public virtual void Insert(EmployeeTaskModel model)
        {
            var tbl = dto.MapModel2Table(model);
            repo.Insert(tbl);
        }

        public virtual void Delete(object id)
        {
            var tbl = repo.GetByID(id);
            var model = dto.MapTable2Model(tbl);
            Delete(model);
        }

        public virtual void Delete(EmployeeTaskModel model)
        {
            var tbl = dto.MapModel2Table(model);
            repo.Delete(tbl);
        }

        public virtual void Update(EmployeeTaskModel entityToUpdate)
        {
            var tbl = dto.MapModel2Table(entityToUpdate);
            repo.Update(tbl);
        }
    }
}

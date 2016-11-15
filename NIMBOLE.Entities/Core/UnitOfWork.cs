using System;
using NIMBOLE.Entities;

namespace NIMBOLE.Entities.Core
{
    public class UnitOfWorkEmpTask  : IDisposable
    {
        private NIMBOLEContext context = new NIMBOLEContext();
        private GenericRepository<TblEmpTask> _entityRepository;

        public GenericRepository<TblEmpTask> EntityRepository
        {
            get
            {
                if (this._entityRepository == null)
                    this._entityRepository = new GenericRepository<TblEmpTask>(context);
                return _entityRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
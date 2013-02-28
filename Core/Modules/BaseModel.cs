using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Modules
{
    public abstract class BaseModel<T> where T : DataObject
    {
        protected readonly BaseDao<T> dao;

        public BaseModel(BaseDao<T> dao)
        {
            this.dao = dao;
        }

        /// <summary>
        /// Gets the data object with the specified id.</summary>
        /// <param name="id">The id of the data object to get</param>
        /// <returns>
        /// A DataObject object representing the data object if found, null otherwise</returns>
        public T GetById(int id)
        {
            return dao.GetById(id);
        }

        /// <summary>
        /// Gets all data objects in the database.</summary>
        /// <returns>
        /// A non-null, possibly empty list of filled DataObject objects</returns>
        public List<T> GetAll()
        {
            return dao.GetAll();
        }
    }
}

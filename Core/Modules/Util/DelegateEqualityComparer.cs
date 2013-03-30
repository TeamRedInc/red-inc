using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Modules.Util
{
    class DelegateEqualityComparer<T> : EqualityComparer<T>
    {
        private readonly Func<T, T, bool> _comparer;

        public DelegateEqualityComparer(Func<T, T, bool> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            _comparer = comparer;
        }

        public override bool Equals(T x, T y)
        {
            return _comparer(x, y);
        }

        public override int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
    }
}

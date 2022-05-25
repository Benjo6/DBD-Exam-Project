using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib.Models;
public interface EntityWithId<T>
{
    public T Id { get; set; }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Workers.Data
{
    public interface ISeedData
    {
        void EnsurePopulated();
    }
}

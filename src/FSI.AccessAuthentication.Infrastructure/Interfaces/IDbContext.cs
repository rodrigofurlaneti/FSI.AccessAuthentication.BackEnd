﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.AccessAuthentication.Infrastructure.Interfaces
{
    public interface IDbContext
    {
        IDbConnection CreateConnection();
    }
}

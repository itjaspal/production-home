﻿using System.Data.Entity;

namespace api.DataAccess
{
    public class MyDbConfiguration : DbConfiguration
    {
        public MyDbConfiguration()
        {
            this.AddInterceptor(new NVarcharInterceptor()); //add this line to existing config.
        }
    }
}
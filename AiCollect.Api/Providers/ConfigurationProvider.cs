using AiCollect.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AiCollect.Api.Providers
{
    public class ConfigurationProvider
    {
        internal DbClass _dbClass;
        public ConfigurationProvider()
        {
            _dbClass = new DbClass();
        }

        public bool SaveConfiguration(Configuration configuration)
        {
            return _dbClass.ExecuteNonQuery(configuration.SaveQuery) > -1;
        }

        public string GetConfigurationString(string query)
        {
            return _dbClass.ExecuteScalar(query).ToString();
        }

    }
}
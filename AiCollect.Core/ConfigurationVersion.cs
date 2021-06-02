using Newtonsoft.Json.Linq;
using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace AiCollect.Core
{
    public enum ConfigurationVersionResults
    {
        UptoDate,
        OutDated,
        Newer
    }

    public enum AssemblyVersionStatus
    {
        Same,
        Older,
        Newer,
        Unknown
    }

    [DataContract]
    public class ConfigurationVersion : IComparable
    {
        private Version _assemblyVersion;

        private Configuration _parent;
        [IgnoreDataMember]
        public Configuration Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        private int _major;

        [DataMember]
        public int Major
        {
            get { return _major; }
            internal set { _major = value; }
        }

        private int _minor;

        [DataMember]
        public int Minor
        {
            get { return _minor; }
            internal set { _minor = value; }
        }

        private int _build;

        [DataMember]
        public int Build
        {
            get { return _build; }
            internal set { _build = value; }
        }

        private int _ConfigfileVersion; // should be incremented everytime the configuration is saved

        [DataMember]
        public int ConfigfileVersion
        {
            get { return _ConfigfileVersion; }
            set { _ConfigfileVersion = value; }
        }

        public AssemblyVersionStatus AssemblyVersionStatus { get; private set; }

        public ConfigurationVersion(Configuration configuration)
        {
            _assemblyVersion = GetCurrentAssemblyVersion();
            _ConfigfileVersion = 0;
            Parent = configuration;
            //Parent.BeforeSave += new ConfigurationBeforeSaveHandler(Parent_BeforeSave);
            //Parent.Saved += new ConfigurationSavedHandler(configuration_ConfigurationSaved);
        }

        private void Parent_BeforeSave(Configuration configuration)
        {
            _ConfigfileVersion++;
        }

        private void configuration_ConfigurationSaved(Configuration config)
        {
            //_ConfigfileVersion++;
        }
        internal void ReadJson(JObject obj)
        {
            Minor = int.Parse(((JValue)obj["Minor"]).Value.ToString());
            Major = int.Parse(((JValue)obj["Major"]).Value.ToString());
            Build = int.Parse(((JValue)obj["Build"]).Value.ToString());
            ConfigfileVersion = int.Parse(((JValue)obj["ConfigfileVersion"]).Value.ToString());
        }

      
        public ConfigurationVersionResults CheckVersion(ConfigurationVersion configVersion)
        {
            //ConfigurationVersionResults result = ConfigurationVersionResults.UptoDate;
            if (this.ConfigfileVersion == configVersion.ConfigfileVersion)
                return ConfigurationVersionResults.UptoDate;
            else if (this.ConfigfileVersion < configVersion.ConfigfileVersion)
                return ConfigurationVersionResults.OutDated;
            else if (this.ConfigfileVersion > configVersion.ConfigfileVersion)
                return ConfigurationVersionResults.Newer;


            /*Version v1 = new Version(Major, Minor, Build,ConfigfileVersion);
            Version v2 = new Version(configVersion.Major, configVersion.Minor, configVersion.Build,configVersion.ConfigfileVersion);
            switch (v1.CompareTo(v2))
            {
                case 0:
                    return ConfigurationVersionResults.UptoDate;//AssemblyVersionStatus.Same;

                case 1:
                    return ConfigurationVersionResults.Newer;//AssemblyVersionStatus.Newer;

                case -1:
                    return ConfigurationVersionResults.OutDated;//AssemblyVersionStatus.Older;
            }*/

           return ConfigurationVersionResults.UptoDate;
        }

        public AssemblyVersionStatus CheckAssemblyVersionStatus()
        {
            Version v1 = new Version(Major, Minor, Build);
            Version v2 = new Version(_assemblyVersion.Major, _assemblyVersion.Minor, _assemblyVersion.Build);
            switch (v1.CompareTo(v2))
            {
                case 0:
                    return AssemblyVersionStatus.Same;

                case 1:
                    return AssemblyVersionStatus.Newer;

                case -1:
                    return AssemblyVersionStatus.Older;
            }
            return AssemblyVersionStatus.Unknown;
        }

        public string VersionString { get; set; }
        public override string ToString()
        {
            return string.Format("v.{0}.{1}.{2}.{3}", Major.ToString(), Minor.ToString(), Build.ToString(), ConfigfileVersion.ToString());
        }

        public int CompareTo(object obj)
        {
            ConfigurationVersion version = (ConfigurationVersion)obj;
            if (this.Major == version.Major && this.Minor == version.Minor && this.Build == version.Build && this.ConfigfileVersion == version.ConfigfileVersion)
                return 1;
            else
                return 0;
        }

        private Version GetCurrentAssemblyVersion()
        {
            Assembly assem = typeof(Configuration).GetTypeInfo().Assembly;
            AssemblyName assemName = assem.GetName();
            return assemName.Version;
        }
    }
}
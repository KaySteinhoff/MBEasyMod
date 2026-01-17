using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MBEasyMod.Interfaces;

namespace MBEasyMod.Services
{
	public class ConfigManager : IConfigManager
	{
		private class Config
		{
			internal string identifier { get; set; }
			internal string value { get; set; }
			internal string comment { get; set; }
		}
		private Dictionary<Type, dynamic> convertersTo = new Dictionary<Type, dynamic>();
		private Dictionary<Type, dynamic> convertersFrom = new Dictionary<Type, dynamic>();
		private Config[] configLines = new Config[0];
		private Dictionary<string, Config> configs = new Dictionary<string, Config>();

		private string configPath = "";
		private const string regex = @"^\s*(?<IDENT>\w+)*\s*={1}\s*(?<VALUE>[^#]+)*(?<OPTIONAL_COMMENT>\s*[#]+.*$)*$";

		public ConfigManager(string configPath)
		{
			this.configPath = configPath;

			// Default converters
			AddConfigConverter((string str, out bool value)=>{return bool.TryParse(str, out value);}, (bool val, out string str)=>{str = val.ToString(); return true;});
			AddConfigConverter((string str, out int value)=>{return int.TryParse(str, out value);}, (int val, out string str)=>{str = val.ToString(); return true;});
			AddConfigConverter((string str, out float value)=>{return float.TryParse(str, out value);}, (float val, out string str)=>{str = val.ToString(); return true;});
			AddConfigConverter((string str, out string value)=>{value = str; return true;}, (string val, out string str)=>{str = val; return true;});
		}

		public void AddConfigConverter<T>(ConfigConvertToFunc<T> converterTo, ConfigConvertFromFunc<T> converterFrom)
		{
			if(convertersTo.ContainsKey(typeof(T)))
				convertersTo[typeof(T)] = converterTo;
			else
				convertersTo.Add(typeof(T), converterTo);

			if(convertersFrom.ContainsKey(typeof(T)))
				convertersFrom[typeof(T)] = converterFrom;
			else
				convertersFrom.Add(typeof(T), converterFrom);
		}

		public void ReadConfigs()
		{
			if(ServiceManager.TryGetService(out ILogger logger))
				logger.Log("Reading configs...");
			
			StreamReader reader = new StreamReader(configPath, Encoding.UTF8);

			string line = null;
			int lineNumber = 0;
			List<Config> configLines = new List<Config>();
			ServiceManager.TryGetService(out logger);
			while((line = reader.ReadLine()) != null)
			{
				lineNumber++;
	
				/* Parse config lines */
				Match configLine = Regex.Match(line, regex);
				Config config = new Config { identifier = configLine.Groups["IDENT"].Value, value = configLine.Groups["VALUE"].Value, comment = configLine.Groups["OPTIONAL_COMMENT"].Value };
				configLines.Add(config);

				/* If the current line is a valid config line add it to the configs dictionary */
				bool anySet = configLine.Groups["IDENT"].Success || configLine.Groups["VALUE"].Success;
				if( anySet && 
					!(configLine.Groups["IDENT"].Success && configLine.Groups["VALUE"].Success))
				{
					if(logger != null)
						logger.Log($"Malformed config line detected ({lineNumber}): {line}");
					continue;
				}
				configs.Add(configLine.Groups["IDENT"].Value, config);
			}
			this.configLines = configLines.ToArray();

			reader.Close();
		}

		public void SaveConfigs()
		{
			if(ServiceManager.TryGetService(out ILogger logger))
				logger.Log("Saving configs...");

			StreamWriter writer = new StreamWriter(configPath, false, Encoding.UTF8);

			/* Save already existing configs */
			foreach(var config in configLines)
				writer.WriteLine($"{config.identifier}={config.value}{config.comment}");
			
			/* Append newly added configs */
			foreach(var config in configs)
			{
				if(configLines.Any(c => c == config.Value))
					continue;

				// comment normally isn't set here however should someone use reflection to grab 
				// and modify the dictionary it may be so we'll have to consider that possibility
				writer.WriteLine($"{config.Value.identifier}={config.Value.value}{config.Value.comment}");
			}

			writer.Close();
		}

		public bool TryGetConfigValue<T>(string identifier, out T value)
		{
			value = default;
			if(!convertersTo.ContainsKey(typeof(T)))
				return false;
			
			ConfigConvertToFunc<T> convertFunc = convertersTo[typeof(T)];
			return convertFunc(configs[identifier].value, out value);
		}

		public bool TrySetConfigValue<T>(string identifier, T value)
		{
			if(!convertersFrom.ContainsKey(typeof(T)))
				return false;

			ConfigConvertFromFunc<T> convertFunc = convertersFrom[typeof(T)];
			if(!convertFunc(value, out string convertedValue))
				return false;

			if(ServiceManager.TryGetService(out ILogger logger))
				logger.Log($"Setting config '{identifier}' to '{convertedValue}'");

			if(!configs.ContainsKey(identifier))
				configs.Add(identifier, new Config { identifier = identifier, value = convertedValue, comment = "" });
			else
				configs[identifier].value = convertedValue;
			return true;
		}
	}
}

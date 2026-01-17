namespace MBEasyMod.Interfaces
{
	public delegate bool ConfigConvertToFunc<T>(string value, out T result);
	public delegate bool ConfigConvertFromFunc<T>(T value, out string result);
	public interface IConfigManager
	{
		void AddConfigConverter<T>(ConfigConvertToFunc<T> converterTo, ConfigConvertFromFunc<T> converterFrom);
		void ReadConfigs();
		void SaveConfigs();
		bool TryGetConfigValue<T>(string identifier, out T value);
		bool TrySetConfigValue<T>(string identifier, T value);
	}
}

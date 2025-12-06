using System.Reflection;

namespace MBEasyMod.Helpers
{
    public static class ConstantsHelper
    {
        private static string moduleFolder = $"../../Modules/{Assembly.GetCallingAssembly().GetName().Name}";
        private static string name = Assembly.GetCallingAssembly().GetName().Name;
        private static string version = Assembly.GetCallingAssembly().GetName().Version?.ToString(3);

        public static string Name => name;
        public static string Version => version;
        public static string ModuleFolder => moduleFolder;
    }
}
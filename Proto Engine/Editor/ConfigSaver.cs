using IniParser;
using IniParser.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proto_Engine.Editor
{
    public class ConfigSaver
    {

        public static void SaveConfig()
        {
            IniData newConfig = new IniData();
            newConfig.Sections.Add("Tilesets");
            newConfig.Sections["Tilesets"].Add("test", "pathToTileset");

        }
    }
}

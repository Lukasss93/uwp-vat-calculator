using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuraRT.Globalization;
using Lukasss93.JsonClasses;

namespace VAT_Discount_Calculator.Classes
{
    public class MyConstants
    {
        public static InfoParameter GenerateParameter()
        {
            InfoParameter parameter = new InfoParameter();
            parameter.Icon = new Uri("ms-appx:///Assets/tile/colored/360x360.png");
            parameter.Name = LocalizedString.Get("AppName");
            parameter.Description = LocalizedString.Get("appdescription");
            parameter.Features = new List<string>();

            parameter.ExistPRO = false;

            parameter.Changelog = new List<InfoChangelog>()
            {
                new InfoChangelog(new Version(1,3,0,0),new DateTime(2016,8,11),new List<InfoLog>()
                {
                    new InfoLog(LocalizedString.Get("changelog3"),LogType.Added),
                    new InfoLog(LocalizedString.Get("changelog4"),LogType.Fixed)
                }),
                new InfoChangelog(new Version(1,2,0,0),new DateTime(2016,3,6),new List<InfoLog>()
                {
                    new InfoLog(LocalizedString.Get("changelog2"),LogType.Fixed)
                }),
                new InfoChangelog(new Version(1,1,0,0),new DateTime(2016,2,17),new List<InfoLog>()
                {
                    new InfoLog(LocalizedString.Get("changelog2"),LogType.Fixed)
                }),
                new InfoChangelog(new Version(1,0,0,0),new DateTime(2016,1,23),new List<InfoLog>()
                {
                    new InfoLog(LocalizedString.Get("changelog1"),LogType.Default)
                })
            };

            parameter.StoreUrl = "https://www.microsoft.com/store/apps/9nblggh5gscp";
            parameter.Disclaimer = null;

            return parameter;
        }
    }
}

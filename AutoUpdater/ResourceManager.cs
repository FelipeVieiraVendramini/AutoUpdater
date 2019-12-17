#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoUpdater - ResourceManager.cs
// 
// Description: <Write a description for this file>
// 
// Colaborators who worked in this file:
// Felipe Vieira Vendramini
// 
// Developed by:
// Felipe Vieira Vendramini <service@ftwmasters.com.br>
// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;

namespace AutoUpdater
{
    public class LanguageManager
    {
        public static List<Languages> AvailableLanguages = new List<Languages>
        {
            //new Languages {LanguageFullName = "English", LanguageCultureName = "en-US"},
            new Languages {LanguageFullName = "Português Brasileiro", LanguageCultureName = "pt-BR"}
        };

        public static bool IsLanguageAvailable(string lang)
        {
            return AvailableLanguages.FirstOrDefault(a => a.LanguageCultureName.Equals(lang)) != null;
        }

        public static string GetDefaultLanguage()
        {
            return AvailableLanguages[0].LanguageCultureName;
        }

        public void SetLanguage(string lang)
        {
            try
            {
                if (!IsLanguageAvailable(lang)) lang = GetDefaultLanguage();
                var cultureInfo = new CultureInfo(lang);
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);

                CurrentlySelectedLanguage = cultureInfo.IetfLanguageTag;
                LanguageResource.Initialize();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public static string CurrentlySelectedLanguage;

        public static string GetString(string name, params object[] strs)
        {
            string result = LanguageResource.ResourceManager.GetString(name);
            return !string.IsNullOrEmpty(result) ? string.Format(result, strs) : name;
        }
    }

    public class LanguageResource
    {
        public static ResourceManager ResourceManager;

        public static void Initialize()
        {
            ResourceManager = new ResourceManager($"AutoUpdater.Language_{LanguageManager.CurrentlySelectedLanguage}", Assembly.GetExecutingAssembly());
        }
    }

    public class Languages
    {
        public string LanguageFullName { get; set; }
        public string LanguageCultureName { get; set; }
    }
}
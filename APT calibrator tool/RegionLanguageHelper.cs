using System;
using System.Globalization;
using System.Windows.Forms;

public class RegionLanguageHelper
{
    /// <summary>
    /// Checks whether the current system region is Germany (DE).
    /// Displays a message box with detailed info (culture, region, etc.).
    /// </summary>
    /// <returns>
    /// true if the region is DE (e.g., de-DE, en-DE, etc.), otherwise false.
    /// </returns>
    public static bool CheckIfRegionIsDE()
    {
        // Get the current culture
        CultureInfo currentCulture = CultureInfo.CurrentCulture;
        // Get the associated RegionInfo
        RegionInfo currentRegion = new RegionInfo(currentCulture.Name);

        // Prepare a string with info
        string message =
            "Culture Name: {currentCulture.Name}\r\n + Display Name: {currentCulture.DisplayName}\r\n + TwoLetterISOLanguageName: {currentCulture.TwoLetterISOLanguageName}\r\n Region Name: {currentRegion.Name}\r\n + Region English Name: {currentRegion.EnglishName}\r\n";

        // Check if the region is "DE"GOg
        bool isDE = currentRegion.TwoLetterISORegionName.Equals("DE", StringComparison.OrdinalIgnoreCase);

        if (isDE)
        {
            message += "\r\nThe current region is DE (Germany).";
        }
        else
        {
            message += "\r\nThe current region is not DE (Germany).";
        }

        // Show a message box with the information
        //MessageBox.Show(message, "Region & Language Check",
           // MessageBoxButtons.OK, MessageBoxIcon.Information);

        return isDE;
    }
}

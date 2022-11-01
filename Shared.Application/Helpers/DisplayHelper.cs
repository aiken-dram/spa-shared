using System.ComponentModel;
using System.Text;
using Shared.Application.Interfaces;

namespace Shared.Application.Helpers;

/// <summary>
/// Static class for displaying values as strings
/// </summary>
public static class DisplayHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Family"></param>
    /// <param name="Name"></param>
    /// <param name="Father"></param>
    /// <returns></returns>
    public static string FIO(string Family, string Name, string Father)
    {
        return Family
               + (string.IsNullOrEmpty(Name) ? "" : " " + Name)
               + (string.IsNullOrEmpty(Father) ? "" : " " + Father);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Family"></param>
    /// <param name="Name"></param>
    /// <param name="Father"></param>
    /// <returns></returns>
    public static string FIOShort(string Family, string Name, string Father)
    {
        return Family
            + (string.IsNullOrEmpty(Name) ? "" : " " + Name[0] + ".")
            + (string.IsNullOrEmpty(Father) ? "" : " " + Father[0] + ".");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="Father"></param>
    /// <returns></returns>
    public static string NameFather(string Name, string Father)
    { return Name + (string.IsNullOrEmpty(Father) ? "" : " " + Father); }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Index"></param>
    /// <param name="City"></param>
    /// <param name="Street"></param>
    /// <param name="House"></param>
    /// <param name="Block"></param>
    /// <param name="App"></param>
    /// <returns></returns>
    public static string Address(string Index, string City, string Street, string House, string Block, string App)
    {
        StringBuilder sb = new StringBuilder();
        if (!string.IsNullOrEmpty(Index))
            sb.Append(Index + ", ");
        sb.Append(City);
        sb.Append(Environment.NewLine);
        sb.Append(Street);
        if (!string.IsNullOrEmpty(House))
            sb.Append(Messages.AddressHouse(House));
        if (!string.IsNullOrEmpty(Block))
            sb.Append(Messages.AddressBlock(Block));
        if (!string.IsNullOrEmpty(App))
            sb.Append(Messages.AddressAppartment(App));
        return sb.ToString();
    }

    /// <summary>
    /// DisplayName attribute for class
    /// </summary>
    /// <typeparam name="T">Class</typeparam>
    /// <returns>DisplayName attribute</returns>
    public static string GetDisplayName<T>()
    {
        var displayName = typeof(T)
          .GetCustomAttributes(typeof(DisplayNameAttribute), true)
          .FirstOrDefault() as DisplayNameAttribute;

        if (displayName != null)
            return displayName.DisplayName;

        return "";
    }
}

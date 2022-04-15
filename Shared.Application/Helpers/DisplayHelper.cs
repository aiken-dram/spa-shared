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
    /// <param name="val"></param>
    /// <returns></returns>
    public static string ToString(DateTime val)
    { return val.ToString(Messages.DateTimeFormat); }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="val"></param>
    /// <param name="NullValue"></param>
    /// <returns></returns>
    public static string ToString(DateTime? val, string NullValue = Messages.NullValue)
    { return (val.HasValue) ? ToString(val.Value) : NullValue; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static string ToString(long val)
    { return val.ToString(); }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="val"></param>
    /// <param name="NullValue"></param>
    /// <returns></returns>
    public static string ToString(long? val, string NullValue = Messages.NullValue)
    { return (val.HasValue) ? ToString(val.Value) : NullValue; }

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
    /// <param name="val"></param>
    /// <param name="NullValue"></param>
    /// <returns></returns>
    public static string Percentage(decimal? val, string NullValue = Messages.NullValue)
    { return val.HasValue ? val.Value.ToString("0.00") + "%" : NullValue; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="val"></param>
    /// <param name="NullValue"></param>
    /// <returns></returns>
    public static string Currency(decimal? val, string NullValue = Messages.NullValue)
    { return val.HasValue ? val.Value.ToString("0.00") + Messages.Currency : NullValue; }

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
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dict"></param>
    /// <param name="NullValue"></param>
    /// <returns></returns>
    public static string FromDictionary(long? id, IList<IDictionary> dict, string NullValue = Messages.NullValue)
    { return id.HasValue ? dict.First(p => p.Value == id.Value).Text : NullValue; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="val"></param>
    /// <param name="NullValue"></param>
    /// <returns></returns>
    public static string? CurrencyText(decimal? val, string? NullValue = Messages.NullValue)
    { return val.HasValue ? RuDateAndMoneyConverter.SumText(val.Value) : NullValue; }
}

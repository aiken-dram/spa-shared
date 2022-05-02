using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Shared.Infrastructure.Helpers;

//THIS IS WIP
/*internal class Sample
{
    public class SampleDto
    {
        public int Number { get; set; }
        public string Text { get; set; }
        public DateTime? Date { get; set; }
    }

    public IEnumerable<SampleDto> records = new[] {
        new SampleDto { Number = 1, Text = "Test" }
    };

    public Sample(int count)
    {
        HSSFWorkbook wb;
        using (FileStream file = new FileStream(@"Forms\Sample.xls", FileMode.Open, FileAccess.Read))
            wb = new HSSFWorkbook(file);

        ISheet sh = wb.GetSheetAt(0);

        //need some style configuration, but by default this should be enough
        var excel = new Excel(wb, sh);

        //date in string format
        excel.Cell(0, 0).Write($"{DateTime.Now:dd.MM.yyyy}"); //done

        //title
        excel.Cell(1, 2).Write("Title"); //done

        //records
        foreach (var r in records)
        {
            var row = excel.AddRow(); //done

            //Number - 1
            row.Add(r.Number); //done

            //Text - 2
            row.Add(r.Text); //done

            //empty column 
            row.Add(); //done

            //birth date - 3
            row.Add(r.Date); //done
        }

        //2D alternatively, use predefined mapping
        //2D: also need configuration of map (like in CsvHelper ish)
        excel.Map(records);

        if (count > 60000)
            excel.AddRow().Add($"Number of records exceeded maximum allowed number of rows in Excel. Only first 60000 from {count.ToString()} total records were saved.");
    }
}*/

/// <summary>
/// Helper class to work with Excel spreadsheet
/// </summary>
/// <remarks>
/// Example:
/// 
/// HSSFWorkbook wb;
/// ISheet sh = wb.GetSheetAt(0);
/// var excel = new Excel(wb, sh);
/// 
/// //Writing to existing cell
/// excel.Cell(1, 2).Write("Title");
/// 
/// //Writing IEnumerable of records
/// foreach (var r in records)
/// {
///     var row = excel.AddRow();
///     row.Add(r.Number); //int number
///     row.Add(r.Text); //string text
///     row.Add(); //empty column
///     row.Add(r.Date); //datetime, can be nullable
/// }
/// </remarks>
public class Excel
{
    private IWorkbook _book;
    private ISheet _sheet;

    private ExcelStyles Styles { get; set; }

    /// <summary>
    /// Constructor for Excel helper class
    /// </summary>
    /// <param name="book">NPOI IWorkbook</param>
    /// <param name="sheet">NPOI ISheet</param>
    public Excel(IWorkbook book, ISheet sheet)
    {
        this._book = book;
        this._sheet = sheet;
        this.Styles = new ExcelStyles(this);
    }

    /// <summary>
    /// Returns excel cell helper for [i,j] position in excel sheet
    /// </summary>
    /// <param name="i">Row number (starting from 0)</param>
    /// <param name="j">Column number (starting from 0)</param>
    /// <returns>Excel cell helper class</returns>
    public ExcelCell Cell(int i, int j)
    {
        return new ExcelCell(this, i, j);
    }

    /// <summary>
    /// Append a row to current sheet
    /// </summary>
    public ExcelRow AddRow()
    {
        int rn = _sheet.LastRowNum + 1;
        var row = _sheet.CreateRow(rn);
        return new ExcelRow(this, row, rn, 0);
    }

    /// <summary>
    /// Appends mapped records to current sheet
    /// </summary>
    /// <param name="records">List of records</param>
    /// <typeparam name="T">Record class</typeparam>
    public void Map<T>(IEnumerable<T> records)
    {

    }

    /// <summary>
    /// Write value into cell and apply style
    /// </summary>
    /// <param name="val">Value</param>
    /// <param name="i">Row number</param>
    /// <param name="j">Column number</param>
    /// <param name="NewCell">Is this a new cell</param>
    /// <typeparam name="T">Value type</typeparam>
    internal void Write<T>(T val, int i, int j, bool NewCell = false)
    {
        var c = _sheet.GetRow(i).GetCell(j);

        if (NewCell)
            c.CellStyle = Styles.GetStyle("default");

        if (val == null)
            return;

        switch (val)
        {
            case string sVal:
                c.SetCellValue(sVal);
                break;
            case int iVal:
                c.SetCellValue((double)iVal);
                break;
            case decimal dVal:
                c.SetCellValue((double)dVal);
                c.CellStyle = Styles.GetStyle("defaultSum");
                break;
            case DateTime dDate:
                c.SetCellValue(dDate);
                c.CellStyle = Styles.GetStyle("defaultDate");
                break;
        }
    }

    /// <summary>
    /// Creates a ICellStyle in workbook based on ExcelStyle parameters
    /// </summary>
    /// <param name="style"></param>
    /// <returns></returns>
    internal ICellStyle AddStyle(ExcelStyle style)
    {
        var res = (HSSFCellStyle)_book.CreateCellStyle();
        if (style.AllBorder)
        {
            res.BorderBottom = style.Border;
            res.BorderLeft = style.Border;
            res.BorderRight = style.Border;
            res.BorderTop = style.Border;
        }
        else
        {
            res.BorderBottom = style.BorderBottom;
            res.BorderLeft = style.BorderLeft;
            res.BorderRight = style.BorderRight;
            res.BorderTop = style.BorderTop;
        }

        if (style.ForegroundIndexColor.HasValue)
        {
            res.FillForegroundColor = style.ForegroundIndexColor.Value;
            res.FillPattern = FillPattern.SolidForeground;
        }

        if (style.Font != null)
            res.SetFont(style.Font);

        res.VerticalAlignment = style.Vertical;
        res.Alignment = style.Horizontal;

        if (style.WrapText)
            res.WrapText = true;

        if (style.DataFormat.HasValue)
            res.DataFormat = style.DataFormat.Value;

        return res;
    }
}

/// <summary>
/// Structure for defining excel style
/// </summary>
public struct ExcelStyle
{
    /// <summary>
    /// Border style
    /// </summary>
    public BorderStyle Border { get; set; }

    /// <summary>
    /// Use Border for all border styles
    /// </summary>
    public bool AllBorder { get; set; }

    /// <summary>
    /// Top border style
    /// </summary>
    public BorderStyle BorderTop { get; set; }

    /// <summary>
    /// Bottom border style
    /// </summary>
    public BorderStyle BorderBottom { get; set; }

    /// <summary>
    /// Left border style
    /// </summary>
    public BorderStyle BorderLeft { get; set; }

    /// <summary>
    /// Right border style
    /// </summary>
    public BorderStyle BorderRight { get; set; }

    /// <summary>
    /// Foreground color
    /// </summary>
    public short? ForegroundIndexColor { get; set; }

    /// <summary>
    /// Vertical alignment
    /// </summary>
    public VerticalAlignment Vertical { get; set; }

    /// <summary>
    /// Horizontal alignment
    /// </summary>
    public HorizontalAlignment Horizontal { get; set; }

    /// <summary>
    /// Font
    /// </summary>
    public IFont? Font { get; set; }

    /// <summary>
    /// Align center
    /// </summary>
    public bool AlighCenter { get; set; }

    /// <summary>
    /// Wrap text
    /// </summary>
    public bool WrapText { get; set; }

    /// <summary>
    /// Data format
    /// </summary>
    public short? DataFormat { get; set; }

    /// <summary>
    /// Default structure
    /// </summary>
    public static ExcelStyle Default => new ExcelStyle
    {
        Border = BorderStyle.Thin,
        AllBorder = true,
        BorderTop = BorderStyle.None,
        BorderBottom = BorderStyle.None,
        BorderLeft = BorderStyle.None,
        BorderRight = BorderStyle.None,
        ForegroundIndexColor = null,
        Font = null,
        Vertical = VerticalAlignment.Top,
        Horizontal = HorizontalAlignment.Left,
        AlighCenter = false,
        WrapText = true,
        DataFormat = null,
    };
}

/// <summary>
/// Helper class for excel styles in workbook
/// </summary>
public class ExcelStyles
{
    private Excel _excel;

    private IDictionary<string, ICellStyle> _styles;

    /// <summary>
    /// Default cell style
    /// </summary>
    public ExcelStyle _default = new ExcelStyle
    {
        Border = BorderStyle.Thin,
        AllBorder = true,
        ForegroundIndexColor = null,
        Font = null,
        Horizontal = HorizontalAlignment.Center,
        Vertical = VerticalAlignment.Top,
        AlighCenter = false,
        WrapText = true,
        DataFormat = null,
    };

    /// <summary>
    /// Default cell style for DateTime values
    /// </summary>
    public ExcelStyle _defaultDate = new ExcelStyle
    {
        Border = BorderStyle.Thin,
        AllBorder = true,
        ForegroundIndexColor = null,
        Font = null,
        Horizontal = HorizontalAlignment.Left,
        Vertical = VerticalAlignment.Top,
        AlighCenter = false,
        WrapText = true,
        DataFormat = 14
    };

    /// <summary>
    /// Default cell style for decimal values
    /// </summary>
    public ExcelStyle _defaultSum = new ExcelStyle
    {
        Border = BorderStyle.Thin,
        AllBorder = true,
        ForegroundIndexColor = null,
        Font = null,
        Horizontal = HorizontalAlignment.Right,
        Vertical = VerticalAlignment.Top,
        AlighCenter = false,
        WrapText = true,
        DataFormat = 4,
    };

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="excel">Parent Excel helper instance</param>
    public ExcelStyles(Excel excel)
    {
        this._excel = excel;
        _styles = new Dictionary<string, ICellStyle>();
        _styles.Add("default", _excel.AddStyle(_default));
        _styles.Add("defaultDate", _excel.AddStyle(_defaultDate));
        _styles.Add("defaultSum", _excel.AddStyle(_defaultSum));
    }

    /// <summary>
    /// Get style based on key
    /// </summary>
    /// <returns>ICellStyle</returns>
    public ICellStyle GetStyle(string key)
    {
        return _styles[key];
    }
}

/// <summary>
/// Helper class for working with excel cell
/// </summary>
public class ExcelCell
{
    private Excel _excel;

    private int _i;
    private int _j;

    /// <summary>
    /// Constructor for excel cell helper
    /// </summary>
    /// <param name="excel">Parent object</param>
    /// <param name="i">Row number</param>
    /// <param name="j">Column number</param>
    public ExcelCell(Excel excel, int i, int j)
    {
        _excel = excel;
        _i = i;
        _j = j;
    }

    /// <summary>
    /// Write value to cell
    /// </summary>
    /// <param name="val"></param>
    /// <typeparam name="T"></typeparam>
    public void Write<T>(T val)
    {
        //2D - this should have some style configuration
        _excel.Write(val, _i, _j);
    }
}

/// <summary>
/// Helper class for working with excel row
/// </summary>
public class ExcelRow
{
    private Excel _excel;

    private IRow _row;
    private int _rn;
    private int _lastCell;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="excel">Parent Excel helper instance</param>
    /// <param name="row">NPOI IRow</param>
    /// <param name="rn">Row number</param>
    /// <param name="lastCell">Last cell index in the row</param>
    public ExcelRow(Excel excel, IRow row, int rn, int lastCell)
    {
        _excel = excel;
        _row = row;
        _rn = rn;
        _lastCell = lastCell;
    }

    /// <summary>
    /// Append cell to row with provided value
    /// </summary>
    /// <param name="val">Value</param>
    /// <typeparam name="T">Type of value</typeparam>
    public void Add<T>(T val)
    {
        //2D - this should have some style configuration
        _row.CreateCell(_lastCell);
        _excel.Write(val, _rn, _lastCell, NewCell: true);
        _lastCell++;
    }


    /// <summary>
    /// Add empty cell with default style to row
    /// </summary>
    public void Add()
    { this.Add<string?>(null); }
}

/// <summary>
/// Helper for working with rich text excel cell values
/// </summary>
public class RichTextHelper
{
    /// <summary>
    /// Rich text string value
    /// </summary>
    public IRichTextString Value { get; set; }

    /// <summary>
    /// Rich text helper constructor
    /// </summary>
    public RichTextHelper()
    {
        Value = new HSSFRichTextString();
    }

    /// <summary>
    /// Creates rich text string from string value
    /// </summary>
    /// <param name="str">String value</param>
    public RichTextHelper(string str)
    {
        Value = new HSSFRichTextString(str);
    }

    /// <summary>
    /// Creates rich text string from string value and applies font to specified characters
    /// </summary>
    /// <param name="str">String value</param>
    /// <param name="iStart">Starting position for applying font</param>
    /// <param name="iEnd">End position for applying font</param>
    /// <param name="font">Font to apply</param>
    public RichTextHelper(string str, int iStart, int iEnd, IFont? font)
    {
        Value = new HSSFRichTextString(str);
        Value.ApplyFont(iStart, iEnd, font);
    }

    /// <summary>
    /// Creates rich text string from array of string and applies font to specified line
    /// </summary>
    /// <param name="arr">Array of string values</param>
    /// <param name="iFont">Line number for applying font</param>
    /// <param name="font">Font to apply</param>
    public static RichTextHelper FromArray(string[] arr, int? iFont = null, IFont? font = null)
    {
        string res = "";
        int i = 0;
        int iStart = 0;
        int iEnd = 0;
        foreach (var s in arr)
        {
            if (!string.IsNullOrEmpty(s))
            {
                if (iFont != null && iFont == i)
                {
                    iStart = res.Length;
                    iEnd = iStart + s.Length;
                }
                res += s + Environment.NewLine;
            }
            i++;
        }
        if (iFont.HasValue && iEnd > 0)
            return new RichTextHelper(res, iStart, iEnd, font);
        else
            return new RichTextHelper(res);
    }
}

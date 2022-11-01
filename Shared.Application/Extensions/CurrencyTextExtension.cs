namespace Shared.Application.Extensions;

/// <summary>
/// Russian text cases
/// </summary>
public enum TextCase
{
    /// <summary>
    /// Кто? Что?
    /// </summary>
    Nominative,

    /// <summary>
    /// Кого? Чего?
    /// </summary>
    Genitive,

    /// <summary>
    /// Кому? Чему?
    /// </summary>
    Dative,

    /// <summary>
    /// Кого? Что?
    /// </summary>
    Accusative,

    /// <summary>
    /// Кем? Чем?
    /// </summary>
    Instrumental,

    /// <summary>
    /// О ком? О чём?
    /// </summary>
    Prepositional
};

/// <summary>
/// Static class for russian date and money converter
/// </summary>
public static class RuDateAndMoneyConverterExtension
{
    static string zero = "ноль";
    static string firstMale = "один";
    static string firstFemale = "одна";
    static string firstFemaleAccusative = "одну";
    static string firstMaleGenetive = "одно";
    static string secondMale = "два";
    static string secondFemale = "две";
    static string secondMaleGenetive = "двух";
    static string secondFemaleGenetive = "двух";

    static string[] from3till19 =
    {
        "", "три", "четыре", "пять", "шесть",
        "семь", "восемь", "девять", "десять", "одиннадцать",
        "двенадцать", "тринадцать", "четырнадцать", "пятнадцать",
        "шестнадцать", "семнадцать", "восемнадцать", "девятнадцать"
    };
    static string[] from3till19Genetive =
    {
        "", "трех", "четырех", "пяти", "шести",
        "семи", "восеми", "девяти", "десяти", "одиннадцати",
        "двенадцати", "тринадцати", "четырнадцати", "пятнадцати",
        "шестнадцати", "семнадцати", "восемнадцати", "девятнадцати"
    };
    static string[] tens =
    {
        "", "двадцать", "тридцать", "сорок", "пятьдесят",
        "шестьдесят", "семьдесят", "восемьдесят", "девяносто"
    };
    static string[] tensGenetive =
    {
        "", "двадцати", "тридцати", "сорока", "пятидесяти",
        "шестидесяти", "семидесяти", "восьмидесяти", "девяноста"
    };
    static string[] hundreds =
    {
        "", "сто", "двести", "триста", "четыреста",
        "пятьсот", "шестьсот", "семьсот", "восемьсот", "девятьсот"
    };
    static string[] hundredsGenetive =
    {
        "", "ста", "двухсот", "трехсот", "четырехсот",
        "пятисот", "шестисот", "семисот", "восемисот", "девятисот"
    };
    static string[] thousands =
    {
        "", "тысяча", "тысячи", "тысяч"
    };
    static string[] thousandsAccusative =
    {
        "", "тысячу", "тысячи", "тысяч"
    };
    static string[] millions =
    {
        "", "миллион", "миллиона", "миллионов"
    };
    static string[] billions =
    {
        "", "миллиард", "миллиарда", "миллиардов"
    };
    static string[] trillions =
    {
        "", "трилион", "трилиона", "триллионов"
    };
    static string[] rubles =
    {
        "", "рубль", "рубля", "рублей"
    };
    static string[] copecks =
    {
        "", "копейка", "копейки", "копеек"
    };

    /// <summary>
    /// 
    /// </summary>
    public static string? CurrencyText(this decimal? val, bool FirstCapital = false, string? NullValue = Messages.NullValue)
    { return val.HasValue ? val.Value.CurrencyText(FirstCapital) : NullValue; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sum"></param>
    /// <param name="NullValue"></param>
    /// <param name="FirstCapital"></param>
    /// <returns></returns>
    public static string? CurrencyText(this decimal sum, bool FirstCapital = false)
    {
        //Десять тысяч рублей 67 копеек
        long rublesAmount = (long)Math.Floor(sum);
        long copecksAmount = ((long)Math.Round(sum * 100)) % 100;
        int lastRublesDigit = lastDigit(rublesAmount);
        int lastCopecksDigit = lastDigit(copecksAmount);

        //string s = String.Format("{0:N0} ({1}) ", rublesAmount, NumeralsToTxt(rublesAmount, TextCase.Nominative, true, FirstCapital));
        string s = String.Format("{0} ", NumeralsToTxt(rublesAmount, TextCase.Nominative, true, FirstCapital));

        if (IsPluralGenitive(lastRublesDigit))
        {
            s += rubles[3] + " ";
        }
        else if (IsSingularGenitive(lastRublesDigit))
        {
            s += rubles[2] + " ";
        }
        else
        {
            s += rubles[1] + " ";
        }

        s += String.Format("{0:00} ", copecksAmount);

        if (IsPluralGenitive(lastCopecksDigit))
        {
            s += copecks[3] + " ";
        }
        else if (IsSingularGenitive(lastCopecksDigit))
        {
            s += copecks[2] + " ";
        }
        else
        {
            s += copecks[1] + " ";
        }

        return s.Trim();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_digits"></param>
    /// <returns></returns>
    static bool IsPluralGenitive(int _digits)
    {
        if (_digits >= 5 || _digits == 0)
            return true;

        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_digits"></param>
    /// <returns></returns>
    static bool IsSingularGenitive(int _digits)
    {
        if (_digits >= 2 && _digits <= 4)
            return true;

        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_amount"></param>
    /// <returns></returns>
    static int lastDigit(long _amount)
    {
        long amount = _amount;

        if (amount >= 100)
            amount = amount % 100;

        if (amount >= 20)
            amount = amount % 10;

        return (int)amount;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_digits"></param>
    /// <param name="_hundreds"></param>
    /// <param name="_tens"></param>
    /// <param name="_from3till19"></param>
    /// <param name="_second"></param>
    /// <param name="_first"></param>
    /// <param name="_power"></param>
    /// <returns></returns>
    static string MakeText(int _digits, string[] _hundreds, string[] _tens, string[] _from3till19, string _second, string _first, string[] _power)
    {
        string s = "";
        int digits = _digits;

        if (digits >= 100)
        {
            s += _hundreds[digits / 100] + " ";
            digits = digits % 100;
        }
        if (digits >= 20)
        {
            s += _tens[digits / 10 - 1] + " ";
            digits = digits % 10;
        }

        if (digits >= 3)
        {
            s += _from3till19[digits - 2] + " ";
        }
        else if (digits == 2)
        {
            s += _second + " ";
        }
        else if (digits == 1)
        {
            s += _first + " ";
        }

        if (_digits != 0 && _power.Length > 0)
        {
            digits = lastDigit(_digits);

            if (IsPluralGenitive(digits))
            {
                s += _power[3] + " ";
            }
            else if (IsSingularGenitive(digits))
            {
                s += _power[2] + " ";
            }
            else
            {
                s += _power[1] + " ";
            }
        }

        return s;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_sourceNumber"></param>
    /// <param name="_case"></param>
    /// <param name="_isMale"></param>
    /// <param name="_firstCapital"></param>
    /// <returns></returns>
    public static string NumeralsToTxt(long _sourceNumber, TextCase _case, bool _isMale, bool _firstCapital)
    {
        string s = "";
        long number = _sourceNumber;
        int remainder;
        int power = 0;

        if ((number >= (long)Math.Pow(10, 15)) || number < 0)
        {
            return "";
        }

        while (number > 0)
        {
            remainder = (int)(number % 1000);
            number = number / 1000;

            switch (power)
            {
                case 12:
                    s = MakeText(remainder, hundreds, tens, from3till19, secondMale, firstMale, trillions) + s;
                    break;
                case 9:
                    s = MakeText(remainder, hundreds, tens, from3till19, secondMale, firstMale, billions) + s;
                    break;
                case 6:
                    s = MakeText(remainder, hundreds, tens, from3till19, secondMale, firstMale, millions) + s;
                    break;
                case 3:
                    switch (_case)
                    {
                        case TextCase.Accusative:
                            s = MakeText(remainder, hundreds, tens, from3till19, secondFemale, firstFemaleAccusative, thousandsAccusative) + s;
                            break;
                        default:
                            s = MakeText(remainder, hundreds, tens, from3till19, secondFemale, firstFemale, thousands) + s;
                            break;
                    }
                    break;
                default:
                    string[] powerArray = { };
                    switch (_case)
                    {
                        case TextCase.Genitive:
                            s = MakeText(remainder, hundredsGenetive, tensGenetive, from3till19Genetive, _isMale ? secondMaleGenetive : secondFemaleGenetive, _isMale ? firstMaleGenetive : firstFemale, powerArray) + s;
                            break;
                        case TextCase.Accusative:
                            s = MakeText(remainder, hundreds, tens, from3till19, _isMale ? secondMale : secondFemale, _isMale ? firstMale : firstFemaleAccusative, powerArray) + s;
                            break;
                        default:
                            s = MakeText(remainder, hundreds, tens, from3till19, _isMale ? secondMale : secondFemale, _isMale ? firstMale : firstFemale, powerArray) + s;
                            break;
                    }
                    break;
            }

            power += 3;
        }

        if (_sourceNumber == 0)
        {
            s = zero + " ";
        }

        if (s != "" && _firstCapital)
            s = s.Substring(0, 1).ToUpper() + s.Substring(1);

        return s.Trim();
    }
}

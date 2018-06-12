using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BerlinClock
{
  public class TimeConverter : ITimeConverter
  {
    //The module is set to 5 because every lamp in 1th of hours/minutes value 5 h/m 
    const int MODULE = 5;
    //This is the default value for a quarter of hour
    const char INTERVALMINUTE = 'R';
    public string convertTime(string aTime)
    {
      //check if the time has a valid format
      Regex reg = new Regex(@"([01]?[0-9]|2[0-3]):[0-5][0-9]");
      if (!reg.IsMatch(aTime))
      {
        throw new Exception("Invalid Time");
      }

      StringBuilder _berlinClock = new StringBuilder();
      int hh, mm, ss;

      //i've validated with regular expression 
      hh = Int32.Parse(aTime.Substring(0, 2));
      mm = Int32.Parse(aTime.Substring(3, 2));
      ss = Int32.Parse(aTime.Substring(6, 2));

      //Calculate berlinClock's 1th lamp (second)
      _berlinClock.AppendLine((ss % 2 == 1) ? "O" : "Y");

      //Calculate berlinClock's 2th row lamps (hh)
      _berlinClock.AppendLine(GetRowOfBerlinClock(hh, 4, 'O', 'R', true));

      //Calculate berlinClock's 3th row lamps (hh)
      _berlinClock.AppendLine(GetRowOfBerlinClock(hh, 4, 'O', 'R', false));

      //Calculate berlinClock's 4th row lamps (mm)
      _berlinClock.AppendLine(GetRowOfBerlinClock(mm, 11, 'O', 'Y', true));

      //Calculate berlinClock's 5th row lamps (mm)
      _berlinClock.Append(GetRowOfBerlinClock(mm, 4, 'O', 'Y', false));

      return _berlinClock.ToString();

    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="val">value(hh or mm)</param>
    /// <param name="n">number of lamp for current row</param>
    /// <param name="old">starter character setting</param>
    /// <param name="newval">character for turn on lamp of row</param>
    /// <param name="isQuot">choise between quotient/remainder</param>
    /// <returns>Conversion of row in BerlinClock format</returns>
    private string GetRowOfBerlinClock(int val, int n, char old, char newval, bool isQuot)
    {
      int remainder, quotREM;
      StringBuilder _berClockPartial = new StringBuilder(new string(old, n));
      int quotient = Math.DivRem(val, MODULE, out remainder);
      quotREM = isQuot ? quotient : remainder;
      _berClockPartial = _berClockPartial.Replace(old, newval, 0, quotREM);
      //The 3th row contain 11 lamps, so if are turned on 3th,6th and 9th lamps i need to set Red color
      if (n == 11 && isQuot)
      {
        for (int i = 2; i < quotient; i = i + 3)
        {
          _berClockPartial = _berClockPartial.Replace(newval, INTERVALMINUTE, i, 1);
        }
      }
      return _berClockPartial.ToString();
    }

  }
}

//Rextester.Program.Main is the entry point for your code. Don't change it.
//Compiler version 4.0.30319.17929 for Microsoft (R) .NET Framework 4.5

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading;

namespace Rextester
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var r1 = "ѕредставитель торговый в компанию этк ферреро";
            var r2 = "“орговый представитель ";
            
            var t = new Fuzzy();
            
            for(int i = 1; i <= 5; i++)
            {
                Console.WriteLine("ƒлина подстроки " + i + ": " + t.IndistinctMatching(i, r1, r2));
            }
        }
    }
    
    public class Fuzzy
	{
		//------------------------------------------------------------------------------
		//MaxMatching - максимальна€ длина подстроки (достаточно 3-4)
		//strInputMatching - сравниваема€ строка
		//strInputStandart - строка-образец
		// —равнивание без учета регистра
		// if IndistinctMatching(4, "поискова€ строка", "оригинальна€ строка  - эталон") > 40 then ...
		struct RetCount
		{
			public long lngSubRows;
			public long lngCountLike;
		}
		RetCount Matching(string strInputA, string strInputB, int lngLen, bool isCase)
		{
			RetCount TempRet;
			int PosStrA;
			int PosStrB;
			string strTempA;
			string strTempB;
			TempRet.lngCountLike = 0;
			TempRet.lngSubRows = 0;
            
			for (PosStrA = 0; PosStrA <= strInputA.Length - lngLen; PosStrA++)
			{
				strTempA = strInputA.Substring(PosStrA, lngLen);

				for (PosStrB = 0; PosStrB <= strInputB.Length - lngLen; PosStrB++)
				{
					strTempB = strInputB.Substring(PosStrB, lngLen);
					if ((string.Compare(strTempA, strTempB, isCase) == 0))
					{
						TempRet.lngCountLike += 1;
						break;
					}
				}
				TempRet.lngSubRows += 1;
			}
			return TempRet;
		}

		public float IndistinctMatching(int MaxMatching, string strInputMatching, string strInputStandart, bool isCase = true)
		{
			if (strInputMatching == null || strInputStandart == null)
			{
				return 0;
			}

			if (strInputMatching == strInputStandart)
			{
				return 100;
			}
				
			strInputMatching = Regex.Replace(strInputMatching, @"[\W\s]", "");
			strInputStandart = Regex.Replace(strInputStandart, @"[\W\s]", "");
							
			RetCount gret;
			RetCount tret;
			int lngCurLen; //текуща€ длина подстроки
			
			//если не передан какой-либо параметр, то выход
			//if (MaxMatching == 0 || strInputMatching.Length == 0 || strInputStandart.Length == 0) return 0;

			gret.lngCountLike = 0;
			gret.lngSubRows = 0;
			// ÷икл прохода по длине сравниваемой фразы
			for (lngCurLen = 1; lngCurLen <= MaxMatching; lngCurLen++)
			{
				//—равниваем строку A со строкой B
				tret = Matching(strInputMatching, strInputStandart, lngCurLen, isCase);
				gret.lngCountLike += tret.lngCountLike;
				gret.lngSubRows += tret.lngSubRows;
				//—равниваем строку B со строкой A
				tret = Matching(strInputStandart, strInputMatching, lngCurLen, isCase);
				gret.lngCountLike += tret.lngCountLike;
				gret.lngSubRows += tret.lngSubRows;
			}
			if (gret.lngSubRows == 0) return 0;
			return (float)(gret.lngCountLike * 100.0 / gret.lngSubRows);
		}

	}
}
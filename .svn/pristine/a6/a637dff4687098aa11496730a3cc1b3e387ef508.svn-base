using UnityEngine;
using System;

public class MathUtil
{

    public static double calculate(string numA, string numB, char sign)
    {
        if (sign == '+')
        {
            int r1 = 0, r2 = 0, c = 0;
            double m = 0, _numA, _numB;
            try
            {
                r1 = numA.Split('.')[1].Length;
            }
            catch (Exception e)
            {
                r1 = 0;
            }
            try
            {
                r2 = numB.Split('.')[1].Length;
            }
            catch (Exception e)
            {
                r2 = 0;
            }
            c = Math.Abs(r1 - r2);
            m = Math.Pow(10, Math.Max(r1, r2));
            if (c > 0)
            {
                var cm = Math.Pow(10, c);
                if (r1 > r2)
                {
                    _numA = double.Parse(numA.Replace(".", ""));
                    _numB = double.Parse(numB.Replace(".", "")) * cm;
                }
                else
                {
                    _numA = double.Parse(numA.Replace(".", "")) * cm;
                    _numB = double.Parse(numB.Replace(".", ""));
                }
            }
            else
            {
                _numA = double.Parse(numA.Replace(".", ""));
                _numB = double.Parse(numB.Replace(".", ""));
            }
            return (_numA + _numB) / m;
        }
        else if (sign == '-')
        {
            int r1 = 0, r2 = 0, n = 0;
            double m = 0;
            try
            {
                r1 = numA.Split('.')[1].Length;
            }
            catch (Exception e)
            {
                r1 = 0;
            }
            try
            {
                r2 = numB.Split('.')[1].Length;
            }
            catch (Exception e)
            {
                r2 = 0;
            }
            m = Math.Pow(10, Math.Max(r1, r2)); //last modify by deeka //动态控制精度长度
            n = (r1 >= r2) ? r1 : r2;
            return Math.Round(((double.Parse(numA) * m - double.Parse(numB) * m) / m), n);
        }
        else if (sign == '*')
        {
            string s1 = numA, s2 = numB;
            int m = 0;
            try
            {
                m += s1.Split('.')[1].Length;
            }
            catch (Exception e)
            {
            }
            try
            {
                m += s2.Split('.')[1].Length;
            }
            catch (Exception e)
            {
            }
            return double.Parse(s1.Replace(".", "")) * double.Parse(s2.Replace(".", "")) / Math.Pow(10, m);
        }
        else if (sign == '/')
        {
            int t1 = 0, t2 = 0;
            double r1, r2;
            string s1 = numA;
            string s2 = numB;
            try
            {
                t1 = s1.Split('.')[1].Length;
            }
            catch (Exception e)
            {
            }
            try
            {
                t2 = s2.Split('.')[1].Length;
            }
            catch (Exception e)
            {
            }
            r1 = double.Parse(s1.Replace(".", ""));
            r2 = double.Parse(s2.Replace(".", ""));
            return (r1 / r2) * Math.Pow(10, t2 - t1);
        }
        return 0;
    }


}


using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Text;

namespace SocketTest.Shared.Utils
{
    public static class ObectsExtensions
    {
        public static T TryParseTo<T>(this object value, string dateTimeFormat = "dd.MM.yyyy")
        {
            if (value.IsEmpty())
            {
                return default(T);
            }

            T val = default(T);
            if ((val is int || val is int?) && int.TryParse(value.ToStr(), out var result))
            {
                return (T)(object)result;
            }

            if ((val is long || val is long?) && long.TryParse(value.ToStr(), out var result2))
            {
                return (T)(object)result2;
            }

            if ((val is decimal || val is decimal?) && decimal.TryParse(value.ToStr(), out var result3))
            {
                return (T)(object)result3;
            }

            if ((val is double || val is double?) && double.TryParse(value.ToStr(), out var result4))
            {
                return (T)(object)result4;
            }

            if (val is string)
            {
                return (T)(object)value?.ToStr();
            }

            if ((val is DateTime || val is DateTime?) && DateTime.TryParseExact(value.ToStr().Replace("XX.XX.", "01.01."), dateTimeFormat, null, DateTimeStyles.None, out var result5))
            {
                return (T)(object)result5;
            }

            return default(T);
        }

        public static bool IsEmpty(this object value)
        {
            return string.IsNullOrWhiteSpace(value?.ToString());
        }

        public static string ToJson(this object inParam, Formatting format = Formatting.None, NullValueHandling nullValueHandling = NullValueHandling.Ignore)
        {
            if (inParam == null)
            {
                return "{}";
            }

            return JsonConvert.SerializeObject(inParam, format, new JsonSerializerSettings
            {
                NullValueHandling = nullValueHandling
            });
        }

        public static object Check(object v, int t)
        {
            if (v == null)
            {
                return DBNull.Value;
            }

            return t switch
            {
                1 => Convert.ToInt64(v),
                2 => v.ToString(),
                3 => Convert.ToDateTime(v, CultureInfo.InvariantCulture),
                4 => Convert.ToDateTime(v, CultureInfo.InvariantCulture).Date,
                _ => DBNull.Value,
            };
        }

        public static T FromJson<T>(this string inParam)
        {
            if (string.IsNullOrWhiteSpace(inParam))
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(inParam);
        }

        public static string ToStr(this object inParam, string dateTimeFormat = "dd.MM.yyyy")
        {
            if (inParam == null)
            {
                return "";
            }

            if (inParam is bool)
            {
                return (inParam.ToString().ToUpper() == "TRUE") ? "1" : "0";
            }

            if (inParam is decimal)
            {
                return inParam.ToString().Replace('.', ',');
            }

            if (inParam is DateTime)
            {
                return Convert.ToDateTime(inParam, CultureInfo.InvariantCulture).ToString(dateTimeFormat);
            }

            if (inParam is byte[])
            {
                return Encoding.Default.GetString(inParam as byte[]);
            }

            return inParam.ToString().Trim();
        }

        public static long ToInt64(this object inVal)
        {
            if (DBNull.Value == inVal || inVal == null || inVal.ToStr() == "")
            {
                return -1L;
            }

            return Convert.ToInt64(inVal);
        }

        public static long? ToNullableInt64(this object inVal)
        {
            if (DBNull.Value == inVal || inVal == null || inVal.ToStr() == "")
            {
                return null;
            }

            return Convert.ToInt64(inVal);
        }

        public static int? ToNullableInt(this object inVal)
        {
            if (DBNull.Value == inVal || inVal == null || inVal.ToStr() == "")
            {
                return null;
            }

            return Convert.ToInt32(inVal);
        }

        public static int ToInt(this object inVal)
        {
            if (DBNull.Value == inVal || inVal == null || inVal.ToStr() == "")
            {
                return -1;
            }

            return Convert.ToInt32(inVal);
        }

        public static DateTime? ToDateTimeV2(this string source, string format = "dd.MM.yyyy HH:mm:ss")
        {
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("ru-RU", useUserOverride: false).DateTimeFormat;
            if (DateTime.TryParseExact(source.Replace("XX.XX", "01.01"), format, dateTimeFormat, DateTimeStyles.None, out var result))
            {
                return result;
            }

            return null;
        }

        public static DateTime ToDateTime(this object source)
        {
            if (DBNull.Value == source || source == null || source.ToString() == "")
            {
                return DateTime.MinValue;
            }

            return Convert.ToDateTime(source, CultureInfo.InvariantCulture);
        }

        public static DateTime ToDateTime(this object source, CultureInfo cultureInfo)
        {
            if (DBNull.Value == source || source == null || source.ToString() == "")
            {
                return DateTime.MinValue;
            }

            return Convert.ToDateTime(source.ToString().Replace("XX.XX", "01.01"), cultureInfo);
        }

        public static decimal ToDecimal(this object inVal)
        {
            if (DBNull.Value == inVal || inVal == null || string.IsNullOrWhiteSpace(inVal.ToStr()))
            {
                return -1m;
            }

            return decimal.Parse(inVal.ToStr(), CultureInfo.InvariantCulture);
        }

        public static decimal? ToNullableDecimal(this object inVal)
        {
            if (DBNull.Value == inVal || inVal == null || string.IsNullOrWhiteSpace(inVal.ToStr()))
            {
                return null;
            }

            return decimal.Parse(inVal.ToStr(), CultureInfo.InvariantCulture);
        }

        public static double? ToNullableDouble(this object inVal)
        {
            if (DBNull.Value == inVal || inVal == null || inVal.ToStr() == "")
            {
                return null;
            }

            return Convert.ToDouble(inVal);
        }

        public static bool IsNullorEmpty(this object inParam)
        {
            return string.IsNullOrEmpty(inParam.ToStr().Trim());
        }

        public static bool IsDateTime(this object inParam)
        {
            if (inParam == null)
            {
                return false;
            }

            try
            {
                DateTime dateTime = Convert.ToDateTime(inParam, CultureInfo.InvariantCulture);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsDateTime(this object inParam, CultureInfo cultureInfo)
        {
            if (inParam == null)
            {
                return false;
            }

            try
            {
                DateTime dateTime = Convert.ToDateTime(inParam.ToString().Replace("XX.XX", "01.01"), cultureInfo);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static ulong ToUInt64(this object inVal)
        {
            if (DBNull.Value == inVal || inVal == null || inVal.ToStr() == "")
            {
                return 0uL;
            }

            return Convert.ToUInt64(inVal);
        }

        public static ulong? ToNullableUInt64(this object inVal)
        {
            if (DBNull.Value == inVal || inVal == null || inVal.ToStr() == "")
            {
                return null;
            }

            return Convert.ToUInt64(inVal);
        }

        public static DateTime? ToNullableDateTime(this object source)
        {
            if (DBNull.Value == source || source == null || source.ToString() == "")
            {
                return null;
            }

            return Convert.ToDateTime(source, CultureInfo.InvariantCulture);
        }

        public static DateTime? ToNullableDateTime(this object source, CultureInfo cultureInfo)
        {
            if (DBNull.Value == source || source == null || source.ToString() == "")
            {
                return null;
            }

            return Convert.ToDateTime(source.ToString().Replace("XX.XX", "01.01"), cultureInfo);
        }
    }
}

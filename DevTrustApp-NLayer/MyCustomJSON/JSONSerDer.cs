using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace MyCustomJSON
{
    public static class JSONSerDer
    {
        //Serialize
        public static string Serialize(this object item)
        {
            StringBuilder stringBuilder = new StringBuilder();
            AddValue(stringBuilder, item);
            return stringBuilder.ToString();
        }

        private static void AddValue(StringBuilder stringBuilder, object item)
        {
            if (item == null)
            {
                stringBuilder.Append("null");
                return;
            }

            Type type = item.GetType();
            if (type == typeof(string) || type == typeof(char))
            {
                stringBuilder.Append('"');
                string str = item.ToString();
                for (int i = 0; i < str.Length; ++i)
                    if (str[i] < ' ' || str[i] == '"' || str[i] == '\\')
                    {
                        stringBuilder.Append('\\');
                        int j = "\"\\\n\r\t\b\f".IndexOf(str[i]);
                        if (j >= 0)
                            stringBuilder.Append("\"\\nrtbf"[j]);
                        else
                            stringBuilder.AppendFormat("u{0:X4}", (UInt32)str[i]);
                    }
                    else
                        stringBuilder.Append(str[i]);
                stringBuilder.Append('"');
            }
            else if (type == typeof(byte) || type == typeof(sbyte) ||
                type == typeof(short) || type == typeof(ushort) ||
                type == typeof(int) || type == typeof(uint) ||
                type == typeof(long) || type == typeof(ulong)
                )
            {
                stringBuilder.Append(item.ToString());
            }
            else if (type == typeof(float))
            {
                stringBuilder.Append(((float)item).ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
            else if (type == typeof(double))
            {
                stringBuilder.Append(((double)item).ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
            else if (type == typeof(decimal))
            {
                stringBuilder.Append(((decimal)item).ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
            else if (type == typeof(bool))
            {
                stringBuilder.Append(((bool)item) ? "true" : "false");
            }
            else if (type == typeof(DateTime))
            {
                stringBuilder.Append('"');
                stringBuilder.Append(((DateTime)item).ToString(System.Globalization.CultureInfo.InvariantCulture));
                stringBuilder.Append('"');
            }
            else if (type.IsEnum)
            {
                stringBuilder.Append('"');
                stringBuilder.Append(item.ToString());
                stringBuilder.Append('"');
            }
            else if (item is IList)
            {
                stringBuilder.Append('[');
                bool isFirst = true;
                IList list = item as IList;
                for (int i = 0; i < list.Count; i++)
                {
                    if (isFirst)
                        isFirst = false;
                    else
                        stringBuilder.Append(',');
                    AddValue(stringBuilder, list[i]);
                }
                stringBuilder.Append(']');
            }
            else
            {
                stringBuilder.Append('{');

                bool isFirst = true;
                FieldInfo[] fieldDatas = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
                for (int i = 0; i < fieldDatas.Length; i++)
                {
                    if (fieldDatas[i].IsDefined(typeof(IgnoreDataMemberAttribute), true))
                        continue;

                    object value = fieldDatas[i].GetValue(item);
                    if (value != null)
                    {
                        if (isFirst)
                            isFirst = false;
                        else
                            stringBuilder.Append(',');
                        stringBuilder.Append('\"');
                        stringBuilder.Append(ObtainNameOfMember(fieldDatas[i]));
                        stringBuilder.Append("\":");
                        AddValue(stringBuilder, value);
                    }
                }
                PropertyInfo[] propertyDatas = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
                for (int i = 0; i < propertyDatas.Length; i++)
                {
                    if (!propertyDatas[i].CanRead || propertyDatas[i].IsDefined(typeof(IgnoreDataMemberAttribute), true))
                        continue;

                    object value = propertyDatas[i].GetValue(item, null);
                    if (value != null)
                    {
                        if (isFirst)
                            isFirst = false;
                        else
                            stringBuilder.Append(',');
                        stringBuilder.Append('\"');
                        stringBuilder.Append(ObtainNameOfMember(propertyDatas[i]));
                        stringBuilder.Append("\":");
                        AddValue(stringBuilder, value);
                    }
                }

                stringBuilder.Append('}');
            }
        }

        private static string ObtainNameOfMember(MemberInfo member)
        {
            if (member.IsDefined(typeof(DataMemberAttribute), true))
            {
                DataMemberAttribute dataMemberAttribute = (DataMemberAttribute)Attribute.GetCustomAttribute(member, typeof(DataMemberAttribute), true);
                if (!string.IsNullOrEmpty(dataMemberAttribute.Name))
                    return dataMemberAttribute.Name;
            }

            return member.Name;
        }

        //Deserialize
        [ThreadStatic] static Stack<List<string>> splitArrayPool;
        [ThreadStatic] static StringBuilder stringBuilder;
        [ThreadStatic] static Dictionary<Type, Dictionary<string, FieldInfo>> fieldDataCache;
        [ThreadStatic] static Dictionary<Type, Dictionary<string, PropertyInfo>> propertyDataCache;

        public static T Deserialize<T>(this string json)
        {
            
            if (propertyDataCache == null) propertyDataCache = new Dictionary<Type, Dictionary<string, PropertyInfo>>();
            if (fieldDataCache == null) fieldDataCache = new Dictionary<Type, Dictionary<string, FieldInfo>>();
            if (stringBuilder == null) stringBuilder = new StringBuilder();
            if (splitArrayPool == null) splitArrayPool = new Stack<List<string>>();

            //Remove all whitespace
            stringBuilder.Length = 0;
            for (int i = 0; i < json.Length; i++)
            {
                char c = json[i];
                if (c == '"')
                {
                    i = AddUntilEndOfString(true, i, json);
                    continue;
                }
                if (char.IsWhiteSpace(c))
                    continue;

                stringBuilder.Append(c);
            }

           
            return (T)ParseValue(typeof(T), stringBuilder.ToString());
        }

        static int AddUntilEndOfString(bool appendEscapeCharacter, int startIdx, string json)
        {
            stringBuilder.Append(json[startIdx]);
            for (int i = startIdx + 1; i < json.Length; i++)
            {
                if (json[i] == '\\')
                {
                    if (appendEscapeCharacter)
                        stringBuilder.Append(json[i]);
                    stringBuilder.Append(json[i + 1]);
                    i++;//Skip next character as it is escaped
                }
                else if (json[i] == '"')
                {
                    stringBuilder.Append(json[i]);
                    return i;
                }
                else
                    stringBuilder.Append(json[i]);
            }
            return json.Length - 1;
        }

        //Splits { <value>:<value>, <value>:<value> } and [ <value>, <value> ] into a list of <value> strings
        static List<string> Split(string json)
        {
            List<string> splitArray = splitArrayPool.Count > 0 ? splitArrayPool.Pop() : new List<string>();
            splitArray.Clear();
            if (json.Length == 2)
                return splitArray;
            int parseDepth = 0;
            stringBuilder.Length = 0;
            for (int i = 1; i < json.Length - 1; i++)
            {
                switch (json[i])
                {
                    case '[':
                    case '{':
                        parseDepth++;
                        break;
                    case ']':
                    case '}':
                        parseDepth--;
                        break;
                    case '"':
                        i = AddUntilEndOfString(true, i, json);
                        continue;
                    case ',':
                    case ':':
                        if (parseDepth == 0)
                        {
                            splitArray.Add(stringBuilder.ToString());
                            stringBuilder.Length = 0;
                            continue;
                        }
                        break;
                }

                stringBuilder.Append(json[i]);
            }

            splitArray.Add(stringBuilder.ToString());

            return splitArray;
        }

        internal static object ParseValue(Type type, string json)
        {
            if (type == typeof(string))
            {
                StringBuilder parseStringBuilder = new StringBuilder(json.Length);
                if (json.Length <= 2 && json[0] == '"' && json[json.Length - 1] == '"')
                    return string.Empty;
                else if (json.Length >= 0 && !json.StartsWith("\"") && !json.EndsWith("\""))
                {
                    throw new FormatException("JSON value format is not valid.");
                }
                else
                {
                    for (int i = 1; i < json.Length - 1; ++i)
                    {
                        if (json[i] == '\\' && i + 1 < json.Length - 1)
                        {
                            int j = "\"\\nrtbf/".IndexOf(json[i + 1]);
                            if (j >= 0)
                            {
                                parseStringBuilder.Append("\"\\\n\r\t\b\f/"[j]);
                                ++i;
                                continue;
                            }
                            if (json[i + 1] == 'u' && i + 5 < json.Length - 1)
                            {
                                UInt32 c = 0;
                                if (UInt32.TryParse(json.Substring(i + 2, 4), System.Globalization.NumberStyles.AllowHexSpecifier, null, out c))
                                {
                                    parseStringBuilder.Append((char)c);
                                    i += 5;
                                    continue;
                                }
                            }
                        }
                        parseStringBuilder.Append(json[i]);
                    }
                }
                return parseStringBuilder.ToString();
            }
            if (type.IsPrimitive)
            {
                var result = Convert.ChangeType(json, type, System.Globalization.CultureInfo.InvariantCulture);
                return result;
            }
            if (type == typeof(decimal))
            {
                decimal result;
                decimal.TryParse(json, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out result);
                return result;
            }
            if (type == typeof(DateTime))
            {
                DateTime result;
                DateTime.TryParse(json.Replace("\"", ""), System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out result);
                return result;
            }
            if (json == "null")
            {
                return null;
            }
            if (type.IsEnum)
            {
                if (json[0] == '"')
                    json = json.Substring(1, json.Length - 2);
                try
                {
                    return Enum.Parse(type, json, false);
                }
                catch
                {
                    return 0;
                }
            }
            if (json[0] == '{' && json[json.Length - 1] == '}')
            {
                return ObjectParsing(type, json);
            }

            return null;
        }

        static Dictionary<string, T> CreateNameForMember<T>(T[] members) where T : MemberInfo
        {
            Dictionary<string, T> nameToMember = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < members.Length; i++)
            {
                T member = members[i];
                if (member.IsDefined(typeof(IgnoreDataMemberAttribute), true))
                    continue;

                string name = member.Name;
                if (member.IsDefined(typeof(DataMemberAttribute), true))
                {
                    DataMemberAttribute dataMemberAttribute = (DataMemberAttribute)Attribute.GetCustomAttribute(member, typeof(DataMemberAttribute), true);
                    if (!string.IsNullOrEmpty(dataMemberAttribute.Name))
                        name = dataMemberAttribute.Name;
                }

                nameToMember.Add(name, member);
            }

            return nameToMember;
        }

        static object ObjectParsing(Type type, string json)
        {
            object instance = FormatterServices.GetUninitializedObject(type);

            // divisible by 2 means valid JSON
            List<string> elems = Split(json);
            if (elems.Count % 2 != 0)
                return instance;

            Dictionary<string, FieldInfo> nameToField;
            Dictionary<string, PropertyInfo> nameToProperty;
            if (!fieldDataCache.TryGetValue(type, out nameToField))
            {
                nameToField = CreateNameForMember(type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy));
                fieldDataCache.Add(type, nameToField);
            }
            if (!propertyDataCache.TryGetValue(type, out nameToProperty))
            {
                nameToProperty = CreateNameForMember(type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy));
                propertyDataCache.Add(type, nameToProperty);
            }

            for (int i = 0; i < elems.Count; i += 2)
            {
                if (elems[i].Length <= 2)
                    continue;
                string key = elems[i].Substring(1, elems[i].Length - 2);
                string value = elems[i + 1];

                FieldInfo fieldInfo;
                PropertyInfo propertyInfo;
                if (nameToField.TryGetValue(key, out fieldInfo))
                    fieldInfo.SetValue(instance, ParseValue(fieldInfo.FieldType, value));
                else if (nameToProperty.TryGetValue(key, out propertyInfo))
                    propertyInfo.SetValue(instance, ParseValue(propertyInfo.PropertyType, value), null);
            }

            return instance;
        }
    }

}

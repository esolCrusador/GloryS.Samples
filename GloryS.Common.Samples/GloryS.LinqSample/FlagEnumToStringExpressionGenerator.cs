using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GloryS.Common.Extensions;

namespace GloryS.LinqSample
{
    public static class FlagEnumToStringExpressionGenerator
    {
        public static Expression<Func<TEnum, string>> GetEnumToStringExpression<TEnum>()
        {
            Type enumType = typeof (TEnum);
            Type nullableEnumType = Nullable.GetUnderlyingType(enumType);
            if (nullableEnumType != null)
            {
                enumType = nullableEnumType;
            }

            ParameterExpression enumParam = Expression.Parameter(typeof(TEnum), enumType.Name.ToLower());
            Expression bodyExpression = Expression.Constant(null, typeof(String));

            foreach (var enumKvp in Combinations(enumType))
            {
                ConstantExpression enumValue = Expression.Constant(enumKvp.Key, typeof(TEnum));
                ConstantExpression enumString = Expression.Constant(enumKvp.Value, typeof(String));

                bodyExpression = Expression.Condition(Expression.Equal(enumParam, enumValue), enumString, bodyExpression);
            }

            return Expression.Lambda<Func<TEnum, string>>(bodyExpression, enumParam);
        }

        private static IEnumerable<KeyValuePair<Enum, string>> Combinations(Type enumType)
        {
            var allMembers = EnumExtensions.GetAllMembers(enumType);

            EnumInfo[] allValues = new EnumInfo[allMembers.Count + 1];
            int i = 0;
            foreach (var member in allMembers)
            {
                //TODO Replace with Readable.
                allValues[i++] = new EnumInfo(member, Convert.ToInt32(member), member.ToString());
            }

            allValues[i] = new EnumInfo((Enum)Enum.ToObject(enumType, 0), 0, null);

            return Combinations(0, enumType, allValues).Select(e => new KeyValuePair<Enum, string>(e.Member, e.MemberName));
        }

        private static IEnumerable<EnumInfo> Combinations(int startIndex, Type enumType, EnumInfo[] enumMembers)
        {
            for (int i = startIndex; i < enumMembers.Length; i++)
            {
                EnumInfo member = enumMembers[i];
                if (i + 1 == enumMembers.Length)
                {
                    yield return new EnumInfo(member.Member, member.MemberValue, member.MemberName);
                }

                IEnumerable<EnumInfo> subCombinations = Combinations(i + 1, enumType, enumMembers);

                foreach (var subCombination in subCombinations)
                {
                    int resultFlagValue = member.MemberValue + subCombination.MemberValue;
                    string resultString = member.MemberName;

                    if (subCombination.MemberName != null)
                    {
                        resultString = resultString + ", " + subCombination.MemberName;
                    }

                    yield return new EnumInfo((Enum) Enum.ToObject(enumType, resultFlagValue), resultFlagValue, resultString);
                }
            }
        }


        private struct EnumInfo
        {
            public EnumInfo(Enum member, int memberValue, string memberName)
            {
                Member = member;
                MemberValue = memberValue;
                MemberName = memberName;
            }

            public readonly Enum Member;
            public readonly int MemberValue;
            public readonly string MemberName;
        }
    }
}

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

            ParameterExpression gradeParam = Expression.Parameter(typeof(TEnum), enumType.Name.ToLower());
            Expression gradeBodyExpression = Expression.Constant(null, typeof(String));

            foreach (var grade in Combinations(enumType))
            {
                ConstantExpression gradeValue = Expression.Constant(grade.Key, typeof(TEnum));
                ConstantExpression gradeResultString = Expression.Constant(grade.Value, typeof(String));

                gradeBodyExpression = Expression.Condition(Expression.Equal(gradeParam, gradeValue), gradeResultString, gradeBodyExpression);
            }

            return Expression.Lambda<Func<TEnum, string>>(gradeBodyExpression, gradeParam);
        }

        private static IEnumerable<KeyValuePair<Enum, string>> Combinations(Type enumType)
        {
            var allMembers = EnumExtensions.GetAllMembers(enumType);

            EnumInfo[] allValues = new EnumInfo[allMembers.Count + 1];
            int i = 0;
            foreach (var member in allMembers)
            {
                //TODO Replace with Readable.
                allValues[i++] = new EnumInfo(member, Convert.ToInt32(member), member.Readable());
            }

            allValues[i] = new EnumInfo((Enum)Enum.ToObject(enumType, 0), 0, null);

            return Combinations(0, enumType, allValues).Select(e => new KeyValuePair<Enum, string>(e.Member, e.MemberName));
        }

        private static IEnumerable<EnumInfo> Combinations(int startIndex, Type enumType, EnumInfo[] grades)
        {
            for (int i = startIndex; i < grades.Length; i++)
            {
                EnumInfo member = grades[i];
                if (i + 1 == grades.Length)
                {
                    yield return new EnumInfo(member.Member, member.MemberValue, null);
                }

                IEnumerable<EnumInfo> subCombinations = Combinations(i + 1, enumType, grades);

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

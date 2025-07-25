﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.GenericExtensions
{
    public static class  EnumExtenction
    {
        public static string GetDescription<TEnum>(this TEnum enumerationValue) where TEnum : Enum
        {
            var type = enumerationValue.GetType();
            if (!type.IsEnum) throw new ArgumentException($"{nameof(enumerationValue)} must be of Enum type");

            var memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo.Length > 0)
            {
                var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return enumerationValue.ToString();
        }
    }
}

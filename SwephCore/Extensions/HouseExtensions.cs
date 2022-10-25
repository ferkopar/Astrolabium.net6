﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwephCore.Houses;

namespace SwephCore.Extensions
{
    /// <summary>
    /// House extensions
    /// </summary>
    public static class HouseExtensions
    {

        /// <summary>
        /// Convert an house système to Char
        /// </summary>
        public static char ToChar(this HouseSystem hs)
        {
            return SweHouse.HouseSystemToChar(hs);
        }

    }
}

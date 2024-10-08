﻿//*********************************************************************
//xCAD
//Copyright(C) 2024 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://xcad.xarial.com/license/
//*********************************************************************

using System;
using System.Collections.Generic;
using System.Text;
using Xarial.XCad.Base;

namespace Xarial.XCad.Data
{
    /// <summary>
    /// Specifies that this entity has properties
    /// </summary>
    public interface IPropertiesOwner : IXTransaction
    {
        /// <summary>
        /// Collection of properties
        /// </summary>
        IXPropertyRepository Properties { get; }
    }
}

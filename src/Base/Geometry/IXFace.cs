﻿using System;
using System.Collections.Generic;
using System.Text;
using Xarial.XCad.Geometry.Structures;

namespace Xarial.XCad.Geometry
{
    public interface IXFace : IXEntity
    {
        double Area { get; }
    }

    public interface IXPlanarFace : IXFace 
    {
        Vector Normal { get; }
    }

    public interface IXCylindricalFace : IXFace 
    {
        Point Origin { get; }
        Vector Axis { get; }
        double Radius { get; }
    }
}

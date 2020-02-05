﻿//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://xcad.xarial.com/license/
//*********************************************************************

using SolidWorks.Interop.sldworks;
using Xarial.XCad.SolidWorks.Annotations;
using Xarial.XCad.SolidWorks.Features;
using Xarial.XCad.SolidWorks.Geometry;
using Xarial.XCad.SolidWorks.Sketch;

namespace Xarial.XCad.SolidWorks
{
    public class SwObject : IXObject
    {
        public static SwObject FromDispatch(object disp, IModelDoc2 model = null)
        {
            switch (disp)
            {
                //TODO: make this automatic
                case IEdge edge:
                    var edgeCurve = edge.IGetCurve();
                    if (edgeCurve.IsCircle())
                    {
                        return new SwCircularEdge(edge);
                    }
                    else
                    {
                        return new SwEdge(edge);
                    }

                case IFace2 face:
                    var faceSurf = face.IGetSurface();
                    if (faceSurf.IsPlane())
                    {
                        return new SwPlanarFace(face);
                    }
                    else if (faceSurf.IsCylinder())
                    {
                        return new SwCylindricalFace(face);
                    }
                    else
                    {
                        return new SwFace(face);
                    }

                case IFeature feat:
                    return new SwFeature(feat, true);

                case IBody2 body:
                    if (!body.IsTemporaryBody())
                    {
                        return new SwBody(body);
                    }
                    else 
                    {
                        return new SwTempBody(body);
                    }

                case ISketchLine skLine:
                    return new SwSketchLine(model, skLine, true);

                case ISketchPoint skPt:
                    return new SwSketchPoint(model, skPt, true);

                case IDisplayDimension dispDim:
                    return new SwDimension(dispDim);

                default:
                    return new SwObject(disp);
            }
        }

        public virtual object Dispatch { get; }

        internal SwObject(object dispatch)
        {
            Dispatch = dispatch;
        }

        public virtual bool IsSame(IXObject other)
        {
            if (other is SwObject)
            {
                return Dispatch == (other as SwObject).Dispatch;
            }
            else
            {
                return false;
            }
        }
    }
}
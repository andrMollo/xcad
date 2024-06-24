﻿//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://xcad.xarial.com/license/
//*********************************************************************

using System.Runtime.InteropServices;
using Xarial.XCad;
using Xarial.XCad.Base.Attributes;
using Xarial.XCad.Geometry.Structures;
using Xarial.XCad.Features.CustomFeature.Structures;
using Xarial.XCad.Features.CustomFeature;
using Xarial.XCad.Geometry;
using Xarial.XCad.SolidWorks.Features.CustomFeature;
using Xarial.XCad.SolidWorks;
using Xarial.XCad.SolidWorks.Documents;
using Xarial.XCad.Features.CustomFeature.Attributes;
using System.Linq;
using Xarial.XCad.SolidWorks.Geometry.Primitives;
using Xarial.XCad.Geometry.Primitives;
using Xarial.XCad.SolidWorks.Geometry;
using Xarial.XCad.Base;
using Xarial.XCad.Annotations;
using SwAddIn.Properties;

namespace SwAddInExample
{
    [ComVisible(true)]
    [MissingDefinitionErrorMessage("xCAD. Download the add-in")]
    public class SimpleMacroFeature : SwMacroFeatureDefinition 
    {
        public override CustomFeatureRebuildResult OnRebuild(ISwApplication app, ISwDocument model, ISwMacroFeature feature)
        {
            return base.OnRebuild(app, model, feature);
        }
    }

    [ComVisible(true)]
    [Icon(typeof(Resources), nameof(Resources.xarial))]
    [MissingDefinitionErrorMessage("xCAD. Download the add-in")]
    public class SampleMacroFeature : SwMacroFeatureDefinition<PmpMacroFeatData>
    {
        public override CustomFeatureRebuildResult OnRebuild(ISwApplication app, ISwDocument model, ISwMacroFeature<PmpMacroFeatData> feature)
        {
            var parameters = feature.Parameters;

            var sweepArc = app.MemoryGeometryBuilder.WireBuilder.PreCreateCircle();
            sweepArc.Geometry = new Circle(new Axis(new Point(0, 0, 0), new Vector(0, 0, 1)), 0.01);
            sweepArc.Commit();

            var sweepLine = app.MemoryGeometryBuilder.WireBuilder.PreCreateLine();
            sweepLine.Geometry = new Line(new Point(0, 0, 0), new Point(1, 1, 1));
            sweepLine.Commit();

            var sweep = (ISwTempSweep)app.MemoryGeometryBuilder.SolidBuilder.PreCreateSweep();
            sweep.Profiles = new ISwPlanarRegion[] { app.MemoryGeometryBuilder.CreatePlanarSheet(
                app.MemoryGeometryBuilder.CreateRegionFromSegments(sweepArc)).Bodies.OfType<ISwTempPlanarSheetBody>().First() };
            sweep.Path = sweepLine;
            sweep.Commit();

            parameters.Number = parameters.Number + 1;
            return new CustomFeatureBodyRebuildResult() { Bodies = sweep.Bodies };
        }

        public override void OnAlignDimension(IXCustomFeature<PmpMacroFeatData> feat, string paramName, IXDimension dim)
        {
            switch (paramName)
            {
                case nameof(PmpMacroFeatData.Number):
                    this.AlignLinearDimension(dim, new Point(0, 0, 0), new Vector(0, 1, 0));
                    break;

                case nameof(PmpMacroFeatData.Angle):
                    this.AlignAngularDimension(dim, new Point(0, 0, 0), new Point(-0.1, 0, 0), new Vector(0, 1, 0));
                    break;
            }
        }
    }
}

using AssemblyLib.AssemblyModels;
using DrawCalculationLib.FunctionServices;
using DrawSettingLib.SettingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawSettingLib.SettingServices
{
    public class StructureService
    {
        public GeometryService geoService;

        public StructureService()
        {
            geoService = new GeometryService();
        }


        public StructureModel CreateStructureCRTColumn(List<StructureCRTRafterInputModel> rafterInputList,
                                                 List<StructureCRTColumnInputModel> columnInputList,
                                                 List<StructureCRTGirderInputModel> girderInputList,


                                                 List<HBeamModel> girderHbeamList,
                                                 NColumnCenterTopSupportModel columnCenterTopSupport,
                                                 PipeModel columnCenterPipe,

                                                 List<NColumnRafterModel> rafterOutputList,

                                                 double selTankID, double selTankHeight, double selAnnularInnerWidth, double selRoofOD, double selBottomThk,
                                                 double selRoofSlope,double selBottoSlope,
                                                 double selShellReduce =0
                                                 )
        {

            // Layer 에 맞게끔 수정 작업

            // Input

            List<StructureCRTRafterInputModel> newRafterInputList = new List<StructureCRTRafterInputModel>();
            newRafterInputList.Add(new StructureCRTRafterInputModel());
            newRafterInputList.AddRange(rafterInputList);

            List<StructureCRTColumnInputModel> newColumnInputList = new List<StructureCRTColumnInputModel>();
            newColumnInputList.AddRange(columnInputList);
            newColumnInputList.Add(new StructureCRTColumnInputModel());

            List<StructureCRTGirderInputModel> newGirderInputList = new List<StructureCRTGirderInputModel>();
            newGirderInputList.Add(new StructureCRTGirderInputModel());
            newGirderInputList.AddRange(girderInputList);
            newGirderInputList.Add(new StructureCRTGirderInputModel());

            List<HBeamModel> newGirderHBeamList = new List<HBeamModel>();
            newGirderHBeamList.Add(new HBeamModel());
            newGirderHBeamList.AddRange(girderHbeamList);
            newGirderHBeamList.Add(new HBeamModel());



            // Output
            List<NColumnRafterModel> newRafterOutputList = new List<NColumnRafterModel>();
            newRafterOutputList.Add(new NColumnRafterModel());
            newRafterOutputList.AddRange(rafterOutputList);







            //기울기, 밑변 길이
            double TankID = selTankID;
            double TankH = selTankHeight;
            double TankHalfID = TankID / 2;
            double AnnularInnerWidth = selAnnularInnerWidth;
            double RoofOD = selRoofOD;
            double BottomThickness = selBottomThk;

           // double RoofSlopeDegree = RadianToDegree(selRoofSlope);
           // double BottomSlopDegree = RadianToDegree(selBottoSlope);
            double RoofSlopeDegree = selRoofSlope;
            double BottomSlopDegree =selBottoSlope;


            // 고정 값
            double columnSpace = 25;
            double RafterOffset = 85; //Clip Point로부터 늘어나는 길이
            double ShellReduce = 70 + selShellReduce; // Shell ID로부터 안쪽으로 줄어드는 길이 -> Angle Type 따라서 달라짐 : k Type에서 조금 안쪽으로 들어감
            double GirderReduce = 200; //Column Point로부터 줄어드는 길이


            StructureModel newStrModel = new StructureModel();

            // 1. Create Layer : Column, Rafter, Girder
            for(int layerIndex = 0; layerIndex < newColumnInputList.Count; layerIndex++)
            {
                // Layer
                StructureLayerModel newLayer = new StructureLayerModel();
                newLayer.Number = 0;
                
                newLayer.StartAngle = 0;

                // Column
                StructureCRTColumnInputModel eachColumn = newColumnInputList[layerIndex];
                double columnQty = GetDoubleValue(eachColumn.Qty);
                double columnRadius = GetDoubleValue(eachColumn.Radius);
                double columnSize = GetDoubleValue(eachColumn.Size);
                double columnAngleOne = 0;
                if (columnQty > 0)
                    columnAngleOne = 360 / columnQty;
                for (int columnIndex = 0; columnIndex < columnQty; columnIndex++)
                {
                    StructureColumnModel newColumn = new StructureColumnModel();
                    newColumn.Radius = columnRadius;
                    newColumn.AngleOne = columnAngleOne;
                    newColumn.Height = 0;   // 나중에 구함
                    newColumn.Size = columnSize;

                    newLayer.ColumnList.Add(newColumn);
                }

                // Layer : Radius
                newLayer.Radius = columnRadius;



                // Rafter
                NColumnRafterModel eachOutputRafter = newRafterOutputList[layerIndex];
                double rafterHeight = GetDoubleValue(eachOutputRafter.A);

                StructureCRTRafterInputModel eachRafter = newRafterInputList[layerIndex];
                double rafterQty = GetDoubleValue(eachRafter.Qty);
                double rafterRadius = GetDoubleValue(eachRafter.Radius);
                string rafterSize = eachRafter.Size;
                double rafterAngleOne = 0;
                if (rafterQty > 0)
                    rafterAngleOne = 360 / rafterQty;
                for (int rafterIndex = 0; rafterIndex < rafterQty; rafterIndex++)
                {
                    StructureRafterModel newRafter = new StructureRafterModel();
                    newRafter.AngleOne = rafterAngleOne;
                    newRafter.Length = 0; // 나중에 구함
                    newRafter.Height = rafterHeight; 
                    newRafter.Size = rafterSize;
                    newLayer.RafterList.Add(newRafter);
                }


                // Girder + Clip
                HBeamModel eachGriderHBeam = newGirderHBeamList[layerIndex];
                double girderWidth = GetDoubleValue(eachGriderHBeam.B);
                double girderHeight = GetDoubleValue(eachGriderHBeam.A);

                StructureCRTGirderInputModel eachGirder = newGirderInputList[layerIndex];
                double girderQty = GetDoubleValue(eachGirder.Qty);
                double girderRadius = GetDoubleValue(eachGirder.Radius);
                string girderSize = eachGirder.Size;
                double girderAngleOne = 0;
                if (girderQty > 0)
                    girderAngleOne = 360 / girderQty;
                for (int girderIndex = 0; girderIndex < girderQty; girderIndex++)
                {
                    StructureGirderModel newGirder = new StructureGirderModel();
                    newGirder.Radius = girderRadius;
                    newGirder.AngleOne = girderAngleOne;
                    newGirder.Size = girderSize;


                    newGirder.Length = 0; // 나중에 구함
                    newGirder.Width = girderWidth;
                    newGirder.Height = girderHeight;

                    newLayer.GirderList.Add(newGirder);
                }



                newStrModel.LayerList.Add(newLayer);
            }

            // 2. Grider : Length
            foreach(StructureLayerModel eachLayer in newStrModel.LayerList)
            {
                double eachLayerRadius = eachLayer.Radius;
                for(int girderIndex = 0; girderIndex < eachLayer.GirderList.Count; girderIndex++)
                {
                    StructureColumnModel eachColumn = eachLayer.ColumnList[girderIndex];

                    StructureGirderModel eachGirder = eachLayer.GirderList[girderIndex];
                    eachGirder.Length = geoService.GetStringLengthByArcAngle(eachLayerRadius, eachColumn.AngleOne) - ( GirderReduce * 2); //Radius와 Angle로 현의 길이 구한 후 - (200 * 2)
                }
            }

            // 3. Rafter : Height
            foreach (StructureLayerModel eachLayer in newStrModel.LayerList)
            {
                double eachLayerRadius = eachLayer.Radius;
                for (int girderIndex = 0; girderIndex < eachLayer.GirderList.Count; girderIndex++)
                {
                    StructureColumnModel eachColumn = eachLayer.ColumnList[girderIndex];

                    StructureGirderModel eachGirder = eachLayer.GirderList[girderIndex];
                    eachGirder.Length = geoService.GetStringLengthByArcAngle(eachLayerRadius, eachColumn.AngleOne) - (GirderReduce * 2); //Radius와 Angle로 현의 길이 구한 후 - (200 * 2)
                }
            }

            // 4. AngleFormCenter
            foreach (StructureLayerModel eachLayer in newStrModel.LayerList)
            {
                double layerStartAngle = eachLayer.StartAngle;

                double girderStartAngle = layerStartAngle;
                for (int girderIndex = 0; girderIndex < eachLayer.GirderList.Count; girderIndex++)
                {
                    StructureGirderModel eachGirder = eachLayer.GirderList[girderIndex];
                    eachGirder.AngleFromCenter = girderStartAngle;
                    girderStartAngle += eachGirder.AngleOne;
                }

                double columnStartAngle = layerStartAngle;
                for (int columnIndex = 0; columnIndex < eachLayer.ColumnList.Count; columnIndex++)
                {
                    StructureColumnModel eachColumn = eachLayer.ColumnList[columnIndex];
                    eachColumn.AngleFromCenter = columnStartAngle;
                    columnStartAngle += eachColumn.AngleOne;
                }

                double rafterStartAngle = layerStartAngle;
                // 절반으로 시작
                if (eachLayer.RafterList.Count > 0)
                    rafterStartAngle += eachLayer.RafterList[0].AngleOne / 2;
                for (int rafterIndex = 0; rafterIndex < eachLayer.RafterList.Count; rafterIndex++)
                {
                    StructureRafterModel eachRafter= eachLayer.RafterList[rafterIndex];
                    eachRafter.AngleFromCenter = rafterStartAngle;
                    rafterStartAngle += eachRafter.AngleOne;
                }
            }

            // 5. Girder : Clip
            for(int layerIndex =0; layerIndex<newStrModel.LayerList.Count-1;layerIndex++)
            {
                StructureLayerModel innerLayer = newStrModel.LayerList[layerIndex];
                StructureLayerModel outerLayer = newStrModel.LayerList[layerIndex+1];

                double innerLayerRadius = innerLayer.Radius;
                for (int girderIndex = 0; girderIndex < innerLayer.GirderList.Count; girderIndex++)
                {
                    StructureGirderModel eachGirder = innerLayer.GirderList[girderIndex];

                    List<StructureRafterModel> innerRafterList = GetRafterByAngleRange(innerLayer.RafterList, eachGirder.AngleFromCenter, eachGirder.AngleFromCenter + eachGirder.AngleOne);
                    List<StructureRafterModel> outerRafterList = GetRafterByAngleRange(outerLayer.RafterList, eachGirder.AngleFromCenter, eachGirder.AngleFromCenter + eachGirder.AngleOne);

                    //Girder Point는 InnerRafter, OuterRafter 두가지가 반영되어야 함 -> Inner Rafter각도와 outer Rafter 각도를 고려하여 Clip의 포인트가 필요요
                    for(int innerRafterIndex=0; innerRafterIndex<innerRafterList.Count; innerRafterIndex++)
                    {
                        StructureRafterModel eachRafter = innerRafterList[innerRafterIndex];

                        StructureClipModel newClipModel = new StructureClipModel();
                        newClipModel.InOut = 0;// In = 0
                        newClipModel.Number = innerRafterIndex; // 0부터
                        newClipModel.AngleFromCenter = eachRafter.AngleFromCenter;

                        newClipModel.ColumnSideAngle = (180 - eachGirder.AngleOne) / 2; // 각도 B;
                        newClipModel.CenterSideAngle = eachRafter.AngleFromCenter - eachGirder.AngleFromCenter; // 각도 C
                        newClipModel.ClipSideAngle = 180 - newClipModel.ColumnSideAngle - newClipModel.CenterSideAngle; // 각도 A
                        newClipModel.GirderClipAngle = newClipModel.ClipSideAngle - 90; //Girder의 수직과 Clip의 각도;
                        newClipModel.GirderClipAngleABS = Math.Abs(newClipModel.GirderClipAngle);

                        //Girder Point는 InnerRafter, OuterRafter 두가지가 반영되어야 함 -> Inner Rafter각도와 outer Rafter 각도를 고려하여 Clip의 포인트가 필요요
                        newClipModel.PointLengthFormColumn = innerLayerRadius * Math.Sin(DegreeToRadian( newClipModel.CenterSideAngle)) / Math.Sin(DegreeToRadian(newClipModel.ClipSideAngle));
                        newClipModel.HorizontalLengthFromCenter = innerLayerRadius * Math.Sin(DegreeToRadian(newClipModel.ColumnSideAngle)) / Math.Sin(DegreeToRadian(newClipModel.ClipSideAngle));

                        eachGirder.ClipList.Add(newClipModel);
                    }
                    for (int outerRafterIndex = 0; outerRafterIndex < outerRafterList.Count; outerRafterIndex++)
                    {
                        StructureRafterModel eachRafter = outerRafterList[outerRafterIndex];

                        StructureClipModel newClipModel = new StructureClipModel();
                        newClipModel.InOut = 1;// Out = 1
                        newClipModel.Number = outerRafterIndex; // 0부터
                        newClipModel.AngleFromCenter = eachRafter.AngleFromCenter;

                        newClipModel.ColumnSideAngle = (180 - eachGirder.AngleOne) / 2; // 각도 B;
                        newClipModel.CenterSideAngle = eachRafter.AngleFromCenter - eachGirder.AngleFromCenter; // 각도 C
                        newClipModel.ClipSideAngle = 180 - newClipModel.ColumnSideAngle - newClipModel.CenterSideAngle; // 각도 A
                        newClipModel.GirderClipAngle = newClipModel.ClipSideAngle - 90; //Girder의 수직과 Clip의 각도;
                        newClipModel.GirderClipAngleABS = Math.Abs(newClipModel.GirderClipAngle);

                        //Girder Point는 InnerRafter, OuterRafter 두가지가 반영되어야 함 -> Inner Rafter각도와 outer Rafter 각도를 고려하여 Clip의 포인트가 필요요
                        newClipModel.PointLengthFormColumn = innerLayerRadius * Math.Sin(DegreeToRadian(newClipModel.CenterSideAngle)) / Math.Sin(DegreeToRadian(newClipModel.ClipSideAngle));
                        newClipModel.HorizontalLengthFromCenter = innerLayerRadius * Math.Sin(DegreeToRadian(newClipModel.ColumnSideAngle)) / Math.Sin(DegreeToRadian(newClipModel.ClipSideAngle));

                        eachGirder.ClipList.Add(newClipModel);
                    }

                    // 작은 각도 기준으로 정렬
                    eachGirder.ClipList= eachGirder.ClipList.OrderBy(x => x.AngleFromCenter).ToList();
                    
                }
            }

            // 6. Column : Height
            for (int layerIndex=0; layerIndex<newStrModel.LayerList.Count-1;layerIndex++)
            {
                StructureLayerModel innerLayer = newStrModel.LayerList[layerIndex];
                StructureLayerModel outerLayer = newStrModel.LayerList[layerIndex+1];
                StructureRafterModel OuterRafterOne = outerLayer.RafterList[0];

                double eachLayerRadius = innerLayer.Radius;
                for (int columnIndex = 0; columnIndex < innerLayer.ColumnList.Count; columnIndex++)
                {



                    StructureColumnModel innerColumn = innerLayer.ColumnList[columnIndex];
                    if (layerIndex == 0)
                    {
                        double centerColumnTopRadius = (GetDoubleValue(columnCenterPipe.OD) / 2) +
                                GetDoubleValue(columnCenterTopSupport.G) +
                                GetDoubleValue(columnCenterTopSupport.B1);

                        // Center : Height
                        double RoofCenterHeight = geoService.GetOppositeByAdjacent(RoofSlopeDegree, RoofOD);
                        double BottomHalfID = TankHalfID - AnnularInnerWidth;
                        double BottomCenterHeight = geoService.GetOppositeByAdjacent(BottomSlopDegree, BottomHalfID);
                        double BottomThicknessSlopeH = geoService.GetHypotenuseByWidth(BottomSlopDegree, BottomThickness);
                        double CenterColumnHalfOD = centerColumnTopRadius;
                        double CenterElevationPointLgH = geoService.GetOppositeByAdjacent(RoofSlopeDegree, CenterColumnHalfOD);

                        double columnSpaceHeight = geoService.GetOppositeByAdjacent(RoofSlopeDegree, columnSpace);
                        double RafterAA = geoService.GetOppositeByAdjacent(RoofSlopeDegree, OuterRafterOne.Height);

                        innerColumn.Height = RoofCenterHeight + (TankH - BottomCenterHeight - BottomThicknessSlopeH) - CenterElevationPointLgH - RafterAA - columnSpaceHeight;
                    }
                    else
                    {
                        StructureGirderModel innerGirder = innerLayer.GirderList[columnIndex];
                        StructureClipModel innerGirderClip = new StructureClipModel();
                        if (innerGirder.ClipList.Count > 0)
                            innerGirderClip = innerGirder.ClipList[0];

                        // Side : Height
                        double ColumnPointHLg = TankID - eachLayerRadius;
                        double BottomHalfID = TankHalfID - AnnularInnerWidth;
                        double BottomThicknessSlopeH = geoService.GetHypotenuseByWidth(BottomSlopDegree, BottomThickness);
                        double ColumnPointRoofH = geoService.GetOppositeByAdjacent(RoofSlopeDegree, ColumnPointHLg);
                        double ColumnPointBottomLg = BottomHalfID - eachLayerRadius;
                        double ColumnPointBottomHeight = geoService.GetOppositeByAdjacent(BottomSlopDegree, BottomSlopDegree);

                        double ElavationPointToClipPointLength = geoService.GetHypotenuseByWidth(innerGirderClip.GirderClipAngleABS, innerGirder.Width / 2);
                        double ElevationPointLgH = geoService.GetOppositeByAdjacent(RoofSlopeDegree, ElavationPointToClipPointLength);

                        double columnSpaceHeight = geoService.GetOppositeByAdjacent(RoofSlopeDegree, columnSpace);
                        double RafterAA = geoService.GetOppositeByAdjacent(RoofSlopeDegree, OuterRafterOne.Height);

                        innerColumn.Height = ColumnPointRoofH + (TankH - ColumnPointBottomHeight - BottomThicknessSlopeH) + ElevationPointLgH - RafterAA - innerGirder.Height - columnSpaceHeight;

                    }



                }
            }

            // 7. Rafter : Length
            for (int layerIndex = 0; layerIndex < newStrModel.LayerList.Count - 1; layerIndex++)
            {
                StructureLayerModel innerLayer = newStrModel.LayerList[layerIndex];
                StructureLayerModel outerLayer = newStrModel.LayerList[layerIndex + 1];

                double eachLayerRadius = innerLayer.Radius;
                StructureRafterModel firstRafter = new StructureRafterModel();
                if (outerLayer.RafterList.Count > 0)
                    firstRafter = outerLayer.RafterList[0];

                if (newStrModel.LayerList.Count == 1) //CenterColumn만 있을 경우 계산
                {

                    double centerColumnB = GetDoubleValue(columnCenterTopSupport.B);
                    double RafterHLg = (TankHalfID - centerColumnB - ShellReduce);
                    double RafterLg = geoService.GetHypotenuseByWidth(RoofSlopeDegree, RafterHLg) - geoService.GetOppositeByAdjacent(RoofSlopeDegree, firstRafter.Height / 2);

                    // 1개 구해서 전체 배정
                    foreach (StructureRafterModel eachRafter in outerLayer.RafterList)
                        eachRafter.Length = RafterLg;
                }
                else if(newStrModel.LayerList.Count > 1) // Side Column이 있을 경우
                {
                    if (layerIndex == 1) // 첫단일 경우 계산
                    {
                        double centerColumnB = GetDoubleValue(columnCenterTopSupport.B);
                        foreach (StructureRafterModel eachRafter in outerLayer.RafterList)
                        {
                            StructureGirderModel eachGirder = GetGirderByRafterAngle(outerLayer.GirderList, eachRafter.AngleFromCenter);
                            StructureClipModel eachClip = GetClipByRafterAngle(eachGirder.ClipList, eachRafter.AngleFromCenter, 0); // Inner Clip
                            
                            double Layer1RafterHLg = eachClip.HorizontalLengthFromCenter - centerColumnB;
                            eachRafter.Length = geoService.GetHypotenuseByWidth(RoofSlopeDegree, Layer1RafterHLg) + RafterOffset*2;
                        }
                    }
                    else if(layerIndex == newStrModel.LayerList.Count - 1) // 끝단일 경우 계산
                    {
                        foreach (StructureRafterModel eachRafter in outerLayer.RafterList)
                        {
                            StructureGirderModel eachGirder = GetGirderByRafterAngle(innerLayer.GirderList, eachRafter.AngleFromCenter);
                            StructureClipModel eachClip = GetClipByRafterAngle(eachGirder.ClipList, eachRafter.AngleFromCenter, 1); // Outer Clip

                            double tempEndHLg = TankHalfID - eachClip.HorizontalLengthFromCenter - ShellReduce;
                            eachRafter.Length = geoService.GetHypotenuseByWidth(RoofSlopeDegree, tempEndHLg) - geoService.GetOppositeByAdjacent(RoofSlopeDegree, firstRafter.Height / 2);
                        }


                    }
                    else //ColumnLayer < ColumnLayerCount) // 중간 Rafter Length
                    {
                        foreach (StructureRafterModel eachRafter in outerLayer.RafterList)
                        {
                            StructureGirderModel eachOuterGirder = GetGirderByRafterAngle(outerLayer.GirderList, eachRafter.AngleFromCenter);
                            StructureClipModel eachInnerClip = GetClipByRafterAngle(eachOuterGirder.ClipList, eachRafter.AngleFromCenter, 0); // Inner Clip

                            StructureGirderModel eachInnerGirder = GetGirderByRafterAngle(innerLayer.GirderList, eachRafter.AngleFromCenter);
                            StructureClipModel eachOuterClip = GetClipByRafterAngle(eachInnerGirder.ClipList, eachRafter.AngleFromCenter, 1); // Outer Clip

                            double RafterHLg = eachInnerClip.HorizontalLengthFromCenter - eachOuterClip.HorizontalLengthFromCenter;
                            eachRafter.Length = geoService.GetHypotenuseByWidth(RoofSlopeDegree, RafterHLg) + RafterOffset * 2;

                        }



                    }


                }


            }

            // 8. Clip : Height
            for (int layerIndex = 0; layerIndex < newStrModel.LayerList.Count - 1; layerIndex++)
            {
                StructureLayerModel innerLayer = newStrModel.LayerList[layerIndex];
                StructureLayerModel outerLayer = newStrModel.LayerList[layerIndex+1];

                double eachLayerRadius = innerLayer.Radius;
                StructureRafterModel firstRafter = new StructureRafterModel();
                if (outerLayer.RafterList.Count > 0)
                    firstRafter = outerLayer.RafterList[0];

                double centerColumnTopRadius = (GetDoubleValue(columnCenterPipe.OD) / 2) +
                                                GetDoubleValue(columnCenterTopSupport.G) +
                                                GetDoubleValue(columnCenterTopSupport.B1);
                double CenterClipE = GetDoubleValue(columnCenterTopSupport.E);          //ClipPoint에서 위로 길이
                double CenterDistance = GetDoubleValue(columnCenterTopSupport.B);
                double RafterHalfAA = geoService.GetOppositeByAdjacent(RoofSlopeDegree, firstRafter.Height / 2);
                double CenterAddClipLg = centerColumnTopRadius - CenterDistance;
                double CenterAddClipH = geoService.GetOppositeByAdjacent(RoofSlopeDegree, CenterAddClipLg);
                double columnSpaceHeight = geoService.GetOppositeByAdjacent(RoofSlopeDegree, columnSpace);



                if (layerIndex == 0) 
                {
                    double CenterClipHeight = CenterClipE + RafterHalfAA + columnSpaceHeight + CenterAddClipH;

                    StructureGirderModel newGirder = new StructureGirderModel();
                    
                    for(int rafterIndex = 0; rafterIndex < outerLayer.RafterList.Count; rafterIndex++)
                    {
                        StructureClipModel newClip = new StructureClipModel();
                        newClip.ClipHeight = CenterClipHeight;
                        newGirder.ClipList.Add(newClip);
                    }
                    innerLayer.GirderList.Add(newGirder);

                }
                else
                {

                    for(int girderIndex = 0; girderIndex < innerLayer.GirderList.Count; girderIndex++)
                    {
                        StructureGirderModel innerGirder = innerLayer.GirderList[girderIndex];
                        StructureClipModel firstClip = new StructureClipModel();
                        if (innerGirder.ClipList.Count > 0)
                            firstClip = innerGirder.ClipList[0];

                        double ElavationPointToClipPointLength = geoService.GetHypotenuseByWidth(firstClip.GirderClipAngleABS, innerGirder.Width / 2);
                        double ElevationPointLgH = geoService.GetOppositeByAdjacent(RoofSlopeDegree, ElavationPointToClipPointLength);
                        double SideAddClipH = ElevationPointLgH;

                        for(int clipIndex = 0; clipIndex < innerGirder.ClipList.Count; clipIndex++)
                        {
                            StructureClipModel eachClip = innerGirder.ClipList[clipIndex];
                            double EachAddHeight = geoService.GetOppositeByAdjacent(RoofSlopeDegree, firstClip.HorizontalLengthFromCenter - eachClip.HorizontalLengthFromCenter);

                            //Side Clip Height
                            double SideClipHeight = CenterClipE + RafterHalfAA + columnSpaceHeight + SideAddClipH; //SideColumn 첫번째 Clip의 높이
                            double EachClipHeight = SideClipHeight + EachAddHeight; // 나머지 Clip의 높이 = 첫번째  클립의 Horizental Length와 각 클립의 Horizental Length
                            eachClip.ClipHeight = EachClipHeight;
                        }



                    }


                }


            }






            return newStrModel;
        }

        public List<StructureRafterModel> GetRafterByAngleRange(List<StructureRafterModel> selRafterList, double startAngle, double endAngle)
        {
            List<StructureRafterModel> newList = new List<StructureRafterModel>();
            foreach(StructureRafterModel eachRafter in selRafterList)
            {
                if (startAngle <= eachRafter.AngleFromCenter && eachRafter.AngleFromCenter <= endAngle)
                {
                    newList.Add(eachRafter);
                }
            }

            return newList;
        }
        
        public StructureGirderModel GetGirderByRafterAngle(List<StructureGirderModel> selGirderList, double rafterAngle)
        {
            StructureGirderModel returnModel = new StructureGirderModel();
            foreach (StructureGirderModel eachGirder in selGirderList)
            {
                double startAngle = eachGirder.AngleFromCenter;
                double endAngle = eachGirder.AngleFromCenter + eachGirder.AngleOne;
                if (startAngle <= rafterAngle && rafterAngle <= endAngle)
                {
                    returnModel = eachGirder;
                    break;
                }
            }

            return returnModel;
        }

        public StructureClipModel GetClipByRafterAngle(List<StructureClipModel> selClipList, double rafterAngle, double inOut=0)
        {
            StructureClipModel returnModel = new StructureClipModel();
            foreach (StructureClipModel eachClip in selClipList)
            {
                if (eachClip.InOut == 0)
                {
                    if (eachClip.AngleFromCenter == rafterAngle)
                    {
                        returnModel = eachClip;
                        break;
                    }
                }
            }
            return returnModel;
        }





        public double GetDoubleValue(string selValue)
        {
            // Default Value
            double doubleValue = 0;

            if (!double.TryParse(selValue, out doubleValue))
                doubleValue = 0;
            return doubleValue;
        }

        private double DegreeToRadian(double angle) { return Math.PI * angle / 180.0; }


       private double RadianToDegree(double angle) { return angle * (180.0 / Math.PI); }

    }
}

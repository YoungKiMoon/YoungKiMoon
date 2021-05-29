using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using devDept.Eyeshot;
using devDept.Graphics;
using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
using devDept.Serialization;

using AssemblyLib.AssemblyModels;
using DrawWork.DrawModels;
using DrawWork.ValueServices;
using DrawWork.Commons;


namespace DrawWork.DrawWorkingPointSevices
{
    public class DRTWorkingPointModel
    {

        public Point3D RoofCenterPoint
        {
            get {return _RoofCenterPoint;}
            set { _RoofCenterPoint = value; }
        }
        private Point3D _RoofCenterPoint;

        public Circle circleRoofUpper
        {
            get { return _circleRoofUpper; }
            set { _circleRoofUpper = value; }
        }
        private Circle _circleRoofUpper;

        public Circle circleRoofLower
        {
            get { return _circleRoofLower; }
            set { _circleRoofLower = value; }
        }
        private Circle _circleRoofLower;



        public Point3D PointLeftRoofUp
        {
            get { return _PointLeftRoofUp; }
            set { _PointLeftRoofUp = value; }
        }
        private Point3D _PointLeftRoofUp;
        public Point3D PointLeftRoofDown
        {
            get { return _PointLeftRoofDown; }
            set { _PointLeftRoofDown = value; }
        }
        private Point3D _PointLeftRoofDown;

        public Point3D PointCenterTopUp
        {
            get { return _PointCenterTopUp; }
            set { _PointCenterTopUp = value; }
        }
        private Point3D _PointCenterTopUp;

        public Point3D PointCenterTopDown
        {
            get { return _PointCenterTopDown; }
            set { _PointCenterTopDown = value; }
        }
        private Point3D _PointCenterTopDown;


        
        public Vector3D CompressionRingDirection
        {
            get { return _CompressionRingDirection; }
            set { _CompressionRingDirection = value; }
        }
        private Vector3D _CompressionRingDirection;

        public double CompressionRingDegree
        {
            get { return _CompressionRingDegree; }
            set { _CompressionRingDegree = value; }
        }
        private double _CompressionRingDegree;

        public DRTWorkingPointModel()
        {
            //RoofCenterPoint = GetCenterPoint();
        }

    }
}

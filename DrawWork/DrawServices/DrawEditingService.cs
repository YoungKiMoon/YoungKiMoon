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

namespace DrawWork.DrawServices
{
    public class DrawEditingService
    {
        public DrawEditingService()
        {

        }


        #region Extend
        public  Line GetExtendLine(Line selLine, double Length, bool endPoint = true)
        {
            if (endPoint)
            {
                Line tempLine = new Line(selLine.StartPoint, selLine.EndPoint);
                Vector3D direction = new Vector3D(selLine.StartPoint, selLine.EndPoint);

                direction.Normalize();
                tempLine.EndPoint = tempLine.EndPoint + direction * Length;

                return tempLine;
            }
            else
            {
                Line tempLine = new Line(selLine.StartPoint, selLine.EndPoint);
                Vector3D direction = new Vector3D(selLine.EndPoint, selLine.StartPoint);

                direction.Normalize();
                tempLine.StartPoint = tempLine.StartPoint + direction * Length;

                return tempLine;
            }
        }
        #endregion

        #region Mirror
        public List<Entity> GetMirrorEntity(Plane selPlane, List<Entity> selList, double X, double Y, bool copyCmd = false)
        {
            Plane pl1 = selPlane;
            pl1.Origin.X = X;
            pl1.Origin.Y = Y;
            Mirror customMirror = new Mirror(pl1);
            List<Entity> mirrorList = new List<Entity>();
            if (copyCmd)
            {
                foreach (Entity eachEntity in selList)
                {
                    Entity newEntity = (Entity)eachEntity.Clone();
                    newEntity.TransformBy(customMirror);
                    mirrorList.Add(newEntity);
                }
            }
            else
            {
                foreach (Entity eachEntity in selList)
                {
                    eachEntity.TransformBy(customMirror);
                }
            }


            return mirrorList;
        }

        public List<Entity> GetEntityByMirror(Plane selPlane, List<Entity> selEntity)
        {
            List<Entity> newEntity = new List<Entity>();
            Mirror customMirror = new Mirror(selPlane);
            foreach (Entity eachEntity in selEntity)
            {
                Entity newEachEntity = (Entity)eachEntity.Clone();
                newEachEntity.TransformBy(customMirror);
                newEntity.Add(newEachEntity);
            }
            return newEntity;
        }
        #endregion

    }
}

using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Geometry;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CadDll.CADCommand
{
    public class CmdAnchor
    {
        private static List<string> _commandName = new List<string> { "MOVE", "GRIP_STRETCH" };
        private Document _doc;
        private Dictionary<ObjectId, List<ObjectId>> _mapHostToAnchored;
        private Dictionary<ObjectId, ObjectId> _mapAnchoredToHost;
        private static ObjectIdCollection _ids;
        private static List<double> _pos;
        Editor Ed 
        { 
            get 
            { 
                return _doc.Editor; 
            } 
        }

        // CONSTRUCTOR
        public CmdAnchor()
        {
            _doc = Application.DocumentManager.MdiActiveDocument;
            _doc.CommandWillStart += _doc_CommandWillStart;

            _mapHostToAnchored = new Dictionary<ObjectId, List<ObjectId>>();
            _mapAnchoredToHost = new Dictionary<ObjectId, ObjectId>();
            _ids = new ObjectIdCollection();
            _pos = new List<double>();
            Ed.WriteMessage("Anchors initialised for '{0}'. ", _doc.Name);

        }

        private void _doc_CommandWillStart(object sender, CommandEventArgs e)
        {
            if (_commandName.Contains(e.GlobalCommandName))
            {
                _ids.Clear();
                _pos.Clear();
                _doc.Database.ObjectOpenedForModify += Database_ObjectOpenedForModify;
                _doc.CommandCancelled += _doc_CommandEnded;
                _doc.CommandEnded += _doc_CommandEnded;
                _doc.CommandFailed += _doc_CommandEnded;
            }
        }
        private void removeEventHandlers()
        {
            _doc.CommandCancelled -= _doc_CommandEnded;
            _doc.CommandEnded -= _doc_CommandEnded;
            _doc.CommandFailed -= _doc_CommandEnded;
            _doc.Database.ObjectOpenedForModify -= Database_ObjectOpenedForModify;

        }


        private void _doc_CommandEnded(object sender, CommandEventArgs e)
        {
            removeEventHandlers();
            rollbackLocations();

        }

        private void Database_ObjectOpenedForModify(object sender, ObjectEventArgs e)
        {
            ObjectId id = e.DBObject.Id;
            if (_mapAnchoredToHost.ContainsKey(id))
            {
                Debug.Assert(e.DBObject is Circle, "Expected anchored object to be circle");
                saveLocation(_mapAnchoredToHost[id], id, false);
            }
            else if (_mapHostToAnchored.ContainsKey(id))
            {
                Debug.Assert(e.DBObject is Line, "Expected host object to be a line");
                foreach(ObjectId id2 in _mapHostToAnchored[id])
                {
                    saveLocation(id, id2, true);
                }
            }

        }

        private void saveLocation(ObjectId hostId, ObjectId anchoredId, bool hostModified)
        {
            if (!_ids.Contains(anchoredId))
            {
                double tValue = double.NaN;
                if (hostModified)
                {
                    Transaction tr = _doc.Database.TransactionManager.StartTransaction();
                    using (tr)
                    {
                        Line line = tr.GetObject(hostId, OpenMode.ForRead) as Line;
                        Circle circle = tr.GetObject(anchoredId, OpenMode.ForRead) as Circle;
                        {
                            Point3d p = circle.Center;
                            Point3d ps = line.StartPoint;
                            Point3d pe = line.EndPoint;
                            double lineLength = ps.DistanceTo(pe);
                            double circleOffset = ps.DistanceTo(p);
                            tValue = circleOffset / lineLength;
                        }
                        tr.Commit();
                    }
                }
                _ids.Add(anchoredId);
                _pos.Add(tValue);
            }
        }
        private void rollbackLocations()
        {
            Debug.Assert(_ids.Count == _pos.Count, "Expected same number of ids and locations");
            Transaction tr = _doc.Database.TransactionManager.StartTransaction();
            using (tr)
            {
                int i = 0;
                foreach(ObjectId id in _ids)
                {
                    Circle circle = tr.GetObject(id, OpenMode.ForWrite) as Circle;
                    Line line = tr.GetObject(_mapAnchoredToHost[id], OpenMode.ForRead) as Line;
                    Point3d ps = line.StartPoint;
                    Point3d pe = line.EndPoint;
                    double a = _pos[i++];
                    if (a.Equals(double.NaN))
                    {
                        LineSegment3d segment = new LineSegment3d(ps, pe);
                        Point3d p = circle.Center;
                        circle.Center = segment.GetClosestPointTo(p).Point;
                    }
                    else
                    {
                        circle.Center = ps + a * (pe - ps);
                    }
                }
                tr.Commit();
            }
        }

        // Entry Point : ANCHOR
        [CommandMethod("ANCHOR")]
        public void Anchor()
        {
            ObjectId hostId;
            ObjectId anchoredId;
            if(selectEntity(typeof(Line),out hostId) && selectEntity(typeof(Circle),out anchoredId))
            {

                // Check for previously stored anchors;
                if (_mapAnchoredToHost.ContainsKey(anchoredId))
                {
                    Ed.WriteMessage("Previous anchor removed.");
                    ObjectId oldHostId = _mapAnchoredToHost[anchoredId];
                    _mapAnchoredToHost.Remove(anchoredId);
                    _mapHostToAnchored[oldHostId].Remove(anchoredId);
                }

                // Add new anchor data;
                if (!_mapHostToAnchored.ContainsKey(hostId))
                {
                    _mapHostToAnchored[hostId] = new List<ObjectId>();
                }
                _mapHostToAnchored[hostId].Add(anchoredId);
                _mapAnchoredToHost.Add(anchoredId, hostId);

                // Ensure that anchored object is located on host:
                Transaction tr = _doc.Database.TransactionManager.StartTransaction();
                using (tr)
                {
                    Line line = tr.GetObject(hostId, OpenMode.ForRead) as Line;
                    Circle circle = tr.GetObject(anchoredId, OpenMode.ForWrite) as Circle;
                    Point3d pStart = line.StartPoint;
                    Point3d pEnd = line.EndPoint;
                    LineSegment3d lineSeg = new LineSegment3d(pStart, pEnd);
                    Point3d p = circle.Center;
                    circle.Center = lineSeg.GetClosestPointTo(p).Point;
                    tr.Commit();
                }
            }

        }
        
        public bool selectEntity(Type t, out ObjectId id)
        {
            id = ObjectId.Null;
            string name = t.Name.ToLower();
            string prompt = string.Format("Please select a {0}:", name);
            string msg = string.Format("Selected entity is not a {0}, please try again...", name);
            PromptEntityOptions optEnt = new PromptEntityOptions(prompt);
            optEnt.SetRejectMessage(msg);
            optEnt.AddAllowedClass(t, true);
            PromptEntityResult resEnt = Ed.GetEntity(optEnt);
            if (resEnt.Status == PromptStatus.OK)
            {
                id = resEnt.ObjectId;
            }
            return !id.IsNull;
        }
        

        

    }
}

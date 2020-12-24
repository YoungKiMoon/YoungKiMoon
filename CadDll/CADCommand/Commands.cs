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

namespace CadDll.CADCommand
{
    public class Commands
    {

        #region testdll
        [CommandMethod("testdll")]
        public void NestedTransactions()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            Transaction tr = db.TransactionManager.StartTransaction();
            using (tr)
            {
                ObjectIdCollection ids = CreateLotsOfCircles(tr, db, 0.5, 1.2, 30);
                tr.TransactionManager.QueueForGraphicsFlush();

                Transaction tr2 = tr.TransactionManager.StartTransaction();
                using (tr2)
                {
                    ChangeColor(tr2, ids, 1);
                    tr2.TransactionManager.QueueForGraphicsFlush();

                    CommitOrAbort(ed, tr2, "transaction to make the circles red");
                }

                Transaction tr3 = tr.TransactionManager.StartTransaction();
                using (tr3)
                {
                    ObjectIdCollection alternates = new ObjectIdCollection();
                    for(int i = 0; i < ids.Count; i++)
                    {
                        if (i % 2 == 0)
                            alternates.Add(ids[i]);
                    }
                    ChangeColor(tr3, alternates, 2);
                    tr3.TransactionManager.QueueForGraphicsFlush();

                    CommitOrAbort(ed, tr3, "transaction to make alternate circles yellow");    
                }
                Transaction tr4 = tr.TransactionManager.StartTransaction();
                using (tr4)
                {
                    SineWave(tr4, ids, 6);
                    tr4.TransactionManager.QueueForGraphicsFlush();
                    CommitOrAbort(ed, tr4, "transaction to draw a magenta sine ware");
                }

                CommitOrAbort(ed, tr, "top-level transaction");
            }
        }

        private void CommitOrAbort(Editor ed, Transaction tr, string desc)
        {
            PromptKeywordOptions pko = new PromptKeywordOptions("\nCommit or Abort the " + desc + "?");
            pko.AllowNone = true;
            pko.Keywords.Add("Commit");
            pko.Keywords.Add("Abort");
            pko.Keywords.Default = "Commit";

            PromptResult pkr = ed.GetKeywords(pko);
            if (pkr.StringResult == "Abort")
            {
                tr.Abort();
            }
            else
            {
                tr.Commit();
            }
        }

        public ObjectIdCollection CreateLotsOfCircles(Transaction tr, Database db, double radius, double offset, int numOnSide)
        {
            ObjectIdCollection ids = new ObjectIdCollection();
            BlockTable bt = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
            BlockTableRecord btr = tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
            for (int i = 0; i < numOnSide; i++)
            {
                for(int j = 0; j < numOnSide; j++)
                {
                    Hatch hat = new Hatch();
                    hat.SetDatabaseDefaults();
                    hat.SetHatchPattern(HatchPatternType.PreDefined, "SOLID");

                    ObjectId id = btr.AppendEntity(hat);
                    tr.AddNewlyCreatedDBObject(hat, true);
                    ids.Add(id);

                    Circle cir = new Circle();
                    cir.Radius = radius;
                    cir.Center = new Point3d(i * offset, j * offset, 0);

                    ObjectId lid = btr.AppendEntity(cir);
                    tr.AddNewlyCreatedDBObject(cir, true);

                    ObjectIdCollection loops = new ObjectIdCollection();
                    loops.Add(lid);
                    hat.AppendLoop(HatchLoopTypes.Default, loops);
                    hat.EvaluateHatch(true);
                    

                    cir.Erase();
                }
            }

            return ids;
        }

        private void ChangeColor(Transaction tr, ObjectIdCollection ids, int col)
        {
            foreach(ObjectId id in ids)
            {
                Entity ent = tr.GetObject(id, OpenMode.ForRead) as Entity;
                if (ent != null)
                {
                    if (!ent.IsWriteEnabled)
                        ent.UpgradeOpen();
                    ent.ColorIndex = col;
                }
            }
        }

        private void SineWave(Transaction tr, ObjectIdCollection ids, int col)
        {
            int numOnSide = (int)Math.Sqrt(ids.Count);
            for (int i =0; i < numOnSide; i++)
            {
                double res = Math.Sin(2 * Math.PI * i / (numOnSide - 1));
                res = (res + 1) / 2;
                int j = (int)(res * numOnSide);
                int idx = i * numOnSide + j;
                Entity ent = tr.GetObject(ids[idx], OpenMode.ForWrite) as Entity;
                ent.ColorIndex = col;
            }
        }

        #endregion


    }
}

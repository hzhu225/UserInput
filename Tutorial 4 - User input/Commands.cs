// Written by Felix Zhu @ CMS Surveyors

/********************************************************************************************************************************************
     In this tutorial, you will see how to take user input into your program. This program will create a Line based on 3 clicks from user. 
     The first point and second point define a Line in 2D, and the third point defines height of the Line.
     
     So if 3 points are:
     Point1: (x1, y1, z1)            Point2: (x2, y2, z2)           Point3: (x3, y3, z3)

     What is the coordinate of Line Startpoint and Endpoint?
     Startpoint: (x1, y1, z3)              Endpoint: (x2, y2, z3)

     You need to write a little bit of code in this tutorial.
*********************************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DatabaseServices;

namespace Tutorial_4
{
    public class Commands
    {
        [CommandMethod("Tut4")]
        public void Tutorial_4_User_input()
        {
            CivilDocument doc = CivilApplication.ActiveDocument;
            Document MdiActdoc = Application.DocumentManager.MdiActiveDocument;
            Editor editor = MdiActdoc.Editor;
            Database currentDB = MdiActdoc.Database;


            //PromptOption need to be used when you want to take user input. Because we are taking point inputs, we need to use PromptPointOptions. 
            //PromptPointOptions has a constructor which needs a string (a sentence), this string is the message user will see in Civil3D prompts.
            //The \n at begining means "Start a new line". It works exactly same as press Enter in Word.

            PromptPointOptions ppo_1 = new PromptPointOptions("\nPlease pick Point 1:");

            PromptPointResult ppr_1 = editor.GetPoint(ppo_1);         //PromptPointResult is used to store user input. 
                                                                      //Use editor.GetPoint() to get the result, and use ppo_1 above as the parameter.
                                                                      //Now we establish a relationship between ppr_1 and ppo_1. So ppr_1 stores the input result of ppo_1.


            //Do exactly same for Point2 and Point3

            PromptPointOptions ppo_2 = new PromptPointOptions("\nPlease pick Point 2:");
            PromptPointResult ppr_2 = editor.GetPoint(ppo_2);

            PromptPointOptions ppo_3 = new PromptPointOptions("\nPlease pick Point 3:");
            PromptPointResult ppr_3 = editor.GetPoint(ppo_3);



            //Now we have 3 user input of points which stored in ppr_1, ppr_2 and ppr_3.
            //Why this part is outside transaction? Because we didn't touch anything in database, we just take user inputs (basically the cursor position when clicking).


            using (Transaction trans = HostApplicationServices.WorkingDatabase.TransactionManager.StartOpenCloseTransaction())
            {
                BlockTable blockTab = trans.GetObject(currentDB.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord modelSpaceBTR = trans.GetObject(blockTab[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                //ppr.Status shows the user's action. OK means user follows the rules and picked a point successfully.
                //User might press ESC at any time, in that case, ppr won't have any value in it. Program will throw an exception if you try to access ppr values.
                //So we must check the Status before accessing ppr values.

                if (ppr_1.Status == PromptStatus.OK && ppr_2.Status == PromptStatus.OK && ppr_3.Status == PromptStatus.OK)
                {

                                                        //Now we have ppr_1, ppr_2 and ppr_3, but how to access the coordinatas of points?
                                                        //If you type ppr_1 and a dot ( ppr_1. ), you will see there is a ppr_1.Value, which is an instance of Point3d.
                                                        //So now you should realise those clicks from user are stored as a Point3d type.


                    Point3d pt_1 = ppr_1.Value;          //Think: why we don't use  Point3d pt1 = new Point3d(ppr_1.Value)  as in Tutorial 3? 
                    Point3d pt_2 = ppr_2.Value;          //Because ppr_1.Value is already an instance of Point3d, we don't need to declare another space in RAM to store it.
                    Point3d pt_3 = ppr_3.Value;          //So we don't even need to use these 3 lines of codes. What these do is just adding another "Name" or "Label" to those values. 



                    //Now you can access coordinates of user clicks throuth pt_1.X, or ppr_1.Value.X. As mentioned above, they are exactly same thing.

                    //Task: You learned how to create an instance of Line in last tutorial. We need to create a line in this tutorial as well. 
                    //The only difference is we need to use values stored in ppr to create that line.

                    //I copied codes from last tutorial, try to make some change to finish the task.
                    //Hint: have a look at very top of this tutorial. 


                    Point3d startPt = new Point3d(0, 0, 0);
                    Point3d endPt = new Point3d(5, 5, 5);

                    Line myline = new Line(startPt, endPt);

                    modelSpaceBTR.AppendEntity(myline);
                    trans.AddNewlyCreatedDBObject(myline, true);

                }

                trans.Commit();
            }
        }
    }
}

//After finish, build your code and load dll file into Civil3D, check if it works.

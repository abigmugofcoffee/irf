using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace ExcelExport
{
    public partial class Form1 : Form
    {
        RealEstateEntities1 context = new RealEstateEntities1();
        List<Flat> Flats;

        Excel.Application xlApp;
        Excel.Workbook xlWB;
        Excel.Worksheet xlSheet;
        public Form1()
        {
            InitializeComponent();
            LoadData();
            CreateExcel();
        }

        private void LoadData()
        {
            Flats = context.Flats.ToList();
        }

        private void CreateExcel()
        {
            try
            {
                xlApp = new Excel.Application();
                xlWB = xlApp.Workbooks.Add(Missing.Value);

                CreateTable();

                xlApp.Visible = true;
                xlApp.UserControl = true;
            }
            catch (Exception ex)
            {
                string error = string.Format("Error: {0}\nLine: {1}", ex.Message, ex.Source);
                MessageBox.Show(error, "Error");

                xlWB.Close(false, Type.Missing, Type.Missing);
                xlApp.Quit();
                xlWB = null;
                xlApp = null;

                throw;
            }
        }
        private void CreateTable()
        {
            string[] headers = new string[] 
            {
                 "Kód",
                 "Eladó",
                 "Oldal",
                 "Kerület",
                 "Lift",
                 "Szobák száma",
                 "Alapterület (m2)",
                 "Ár (mFt)",
                 "Négyzetméter ár (Ft/m2)"
            };
            for (int i = 0; i < headers.Length; i++)
                xlSheet.Cells[1, i + 1] = headers[i];

            object[,] values = new object[Flats.Count, headers.Length];

            int counter = 0;
            int floorColumn = 6;

            foreach (var Flat in Flats)
            {
                values[counter, 0] = Flats.Code;
                values[counter, 1] = Flats.Vencor;
                values[counter, 2] = Flats.Side;
                values[counter, 3] = Flats.District;

                if (lakas.Elevator)
                { values[counter, 4] = "Van";}
                else values[counter, 4] = "Nincs";}

                values[counter, 4] = Flats.Elevator;
                values[counter, 5] = Flats.NumberOfRooms;
                values[counter, 6] = Flats.FloorArea;
                values[counter, 7] = Flats.Price;
                values[counter, 8] = string.Format("={0}/{1}*{2}",
                    "H" + (counter + 2).ToString(),
                    GetCell(counter + 2), floorColumn + 1),
                    "1000000");
                counter++;
            }

        var range = xlSheet.get_Range(
            GetCell(2, 1),
            GetCell(1 + values.GetLenght(0), values.GetLength(1)));
            
        }
    private void FormatTable()
    {
        Excel.Range headerRange = xlSheet.get_Range(GetCell(1, 1), GetCell(1, headers.Length));
        headerRange.Font.Bold = true;
        headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
        headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        headerRange.EntireColumn.AutoFit();
        headerRange.RowHeight = 40;
        headerRange.Interior.Color = Color.LightBlue;
        headerRange.BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);
    }
   


    }
}

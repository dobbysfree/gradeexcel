using MahApps.Metro.Controls;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Controls;

namespace creategrade
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public static MainWindow Instance { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
        }

        private void AddItemClick(object sender, RoutedEventArgs e)
        {
            sp_items.Children.Add(new ItemUc());
        }

        private void CreateExcelClick(object sender, RoutedEventArgs e)
        {
            WriteToExcel();
        }

        void WriteToExcel()
        {
            Excel.Application app = new Excel.Application();
            if (app == null) return;

            Excel.Workbook workbook = app.Workbooks.Add();
            Excel.Worksheet worksheet = app.Sheets.Add();

            InputExcel(worksheet);

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "Save an Excel File";
            dialog.Filter = "Excel|*.xls";
            dialog.ShowDialog();

            if (!string.IsNullOrEmpty(dialog.FileName))
                app.ActiveWorkbook.SaveAs(dialog.FileName, Excel.XlFileFormat.xlWorkbookNormal);

            workbook.Close(false, Type.Missing, Type.Missing);
            app.Quit();
        }

        void InputExcel(Excel.Worksheet ws)
        {
            int num  = 35;
            var cntItem = sp_items.Children.Count;
       
            ws.Cells[1, 1] = "No";
            for (int row = 2; row <= num; row++)
            {
                ws.Cells[row, 1] = row - 1;
            }

            ws.Cells[1, 2] = "이름";
            ws.Cells[1, 3] = "학번";

            int column = 4;
            for (int i = 0; i < cntItem; i++)
            {
                var im              = (ItemUc)sp_items.Children[i];
                im.ColumnNum        = column;
                ws.Cells[1, column] = string.Format("{0}({1}%)", im.TbTitle, im.TbPercent);
                
                int refCell = column + cntItem + 6;

                for (int row = 2; row <= num; row++)
                {
                    ws.Cells[row, column].Formula = string.Format("={0}{1} * (100 / {2})", Dic.ColumnAlpha[refCell], row, im.TbScore);
                }
                column++;
            }

            ws.Cells[1, column] = "Fin Gr";
            for (int row = 2; row <= num; row++)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("=");

                for (int i = 0; i < cntItem; i++)
                {
                    var im = (ItemUc)sp_items.Children[i];
                    sb.Append(string.Format("AVERAGE({0}{1} * ({2} / 100))", Dic.ColumnAlpha[im.ColumnNum], row, im.TbPercent));
                    
                    if (i < cntItem - 1) sb.Append(" + ");
                }

                ws.Cells[row, column].Formula = sb.ToString();
            }

            ws.Cells[1, ++column] = "Rank";
            for (int row = 2; row <= num; row++)
            {
                ws.Cells[row, column].Formula = string.Format("=RANK({0}{1},${0}${2}:${0}${3},0)", Dic.ColumnAlpha[column - 1], row, 2, num);
            }

            ws.Range["A1", Dic.ColumnAlpha[column] + num].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            ws.Range["A1", Dic.ColumnAlpha[column] + num].Borders.Weight = Excel.XlBorderWeight.xlThin;
            ws.Range["A1", Dic.ColumnAlpha[column] + num].BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlMedium);

            /* 두번째 구간 */
            ws.Cells[1, column += 2] = "EZ";
            ws.Cells[1, column += 1] = "FLGr";

            int line = column - 1;
            ws.Range[Dic.ColumnAlpha[line] + "1", Dic.ColumnAlpha[column] + num].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            ws.Range[Dic.ColumnAlpha[line] + "1", Dic.ColumnAlpha[column] + num].Borders.Weight = Excel.XlBorderWeight.xlThin;

            /* 세번째 구간 */
            line = column += 2;
            for (int i = 0; i < cntItem; i++)
            {
                var im = (ItemUc)sp_items.Children[i];
                               
                ws.Cells[1, column++] = string.Format("{0}({1})", im.TbTitle, im.TbScore);
            }

            ws.Range[Dic.ColumnAlpha[line] + "1", Dic.ColumnAlpha[column - 1] + num].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            ws.Range[Dic.ColumnAlpha[line] + "1", Dic.ColumnAlpha[column - 1] + num].Borders.Weight = Excel.XlBorderWeight.xlThin;
            ws.Range[Dic.ColumnAlpha[line] + "1", Dic.ColumnAlpha[column - 1] + num].BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlMedium);


            /* 네번째 구간 */
            line = column += 1;

            string[] arr = new string[3] { "No", "이름", "학번" };
            for (int i = 0; i < 3; i++)
            {
                ws.Cells[1, column] = arr[i];

                for (int row = 2; row <= num; row++)
                {
                    ws.Cells[row, column].Formula = string.Format("={0}{1}", Dic.ColumnAlpha[i + 1], row);
                }

                column++;
            }

            ws.Range[Dic.ColumnAlpha[line] + "1", Dic.ColumnAlpha[column - 1] + num].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            ws.Range[Dic.ColumnAlpha[line] + "1", Dic.ColumnAlpha[column - 1] + num].Borders.Weight = Excel.XlBorderWeight.xlThin;
            ws.Range[Dic.ColumnAlpha[line] + "1", Dic.ColumnAlpha[column - 1] + num].BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlMedium);

            
            ws.Range["A1", Dic.ColumnAlpha[column] + "1"].Font.Bold = true; // title 굵게
            ws.Range["A1", Dic.ColumnAlpha[column] + "1"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; // title 가운데 정렬
        }
    }
}
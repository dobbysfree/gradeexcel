using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Excel = Microsoft.Office.Interop.Excel;

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
            
            app.ActiveWorkbook.SaveAs(@"C:\Users\sam\Desktop\ss\abc.xls", Excel.XlFileFormat.xlWorkbookNormal);

            workbook.Close();
            app.Quit();
        }

        void InputExcel(Excel.Worksheet ws)
        {
            int amount = 25;

            var cnt = sp_items.Children.Count;
            int num = 4 + cnt;

            ws.Cells[1, 1] = "No";
            ws.Cells[1, 2] = "이름";
            ws.Cells[1, 3] = "학번";

            ws.Cells[1, num] = "Fin Gr";
            ws.Cells[1, num + 1] = "Rank";
            ws.Cells[1, num + 3] = "EZ";
            ws.Cells[1, num + 4] = "FLGr";

            num = num + cnt + 6;

            ws.Cells[1, num + 1] = "No";
            ws.Cells[1, num + 2] = "이름";
            ws.Cells[1, num + 3] = "학번";

            int x = 4;
            for (int i = 0; i < cnt; i++)
            {
                var ite = (ItemUc)sp_items.Children[i];

                ws.Cells[1, x] = string.Format("{0}({1}%)", ite.TbTitle, ite.TbPercent);                
                ws.Cells[1, x + 8] = string.Format("{0}({1})", ite.TbTitle, ite.TbScore);

                for (int y = 2; y <= amount; y++)
                {
                    ws.Cells[y, x].Formula = string.Format("={0}{1} * (100 / {2})", Dic.Colums[x + 8], y, ite.TbScore);
                }
                x++;
            }
            
        }


        void OutOfFile()
        {

        }
    }
}
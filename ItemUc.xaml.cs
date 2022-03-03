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

namespace creategrade
{
    /// <summary>
    /// ItemUc.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ItemUc : UserControl
    {
        public string TbTitle { get { return tb_title.Text.Trim(); } }
        public int TbScore { get { return int.Parse(tb_score.Text.Trim()); } }
        public int TbPercent { get { return int.Parse(tb_percent.Text.Trim()); } }

        public ItemUc()
        {
            InitializeComponent();
        }

        private void DeleteItemClick(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.sp_items.Children.Remove(this);
        }
    }
}

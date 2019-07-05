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

namespace SmartGraph
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int minorCount = Int16.Parse(txtMC.Text);
            int xCellCount = Int16.Parse(txtXC.Text);
            int yCellCount = Int16.Parse(txtYC.Text);

            int xOffset = Int16.Parse(txtXOffset.Text);
            int yOffset = Int16.Parse(txtYOffset.Text);

            List<double> xReadings = new List<double>(txtXR.Text.Trim().Split('\n').Select((x) => double.Parse(x)).ToArray());
            List<double> yReadings = new List<double>(txtYR.Text.Trim().Split('\n').Select((x) => double.Parse(x)).ToArray());

            double xMax = Math.Ceiling(xReadings.Max());
            double xMin = Math.Floor(xReadings.Min());

            double yMax = Math.Ceiling(yReadings.Max());
            double yMin = Math.Floor(yReadings.Min());

            if (!chkEnableFractionsX.IsChecked.Value) { for (; ((xMax - xMin + 1) % xCellCount) != 0; xMax++) ; }
            if (!chkEnableFractionsY.IsChecked.Value) { for (; ((yMax - yMin + 1) % yCellCount) != 0; yMax++) ; }

            double xGap = (xMax - xMin + 1) / xCellCount;
            double yGap = (yMax - yMin + 1) / yCellCount;

            txtX.Text = "";
            txtXi.Text = "";
            txtY.Text = "";
            txtYi.Text = "";           
            List<double> xDots = new List<double>();
            List<double> yDots = new List<double>();

            for (int i = 0; i <= xCellCount; i++)
            {
                xDots.Add(xMin + xGap * i + xOffset);
                //txtX.Text += (xMin + xGap * i).ToString() + "\n";
                txtXi.Text += i.ToString() + "\n";
            }
            txtX.Text = String.Join<double>("\n", xDots.ToArray());

            
            for (int i = 0; i <= yCellCount; i++)
            {
                yDots.Add(yMin + yGap * i + yOffset);
                //txtY.Text += (yMin + yGap * i).ToString() + "\n";
                txtYi.Text += i.ToString() + "\n";
            }
            txtY.Text = String.Join<double>("\n", yDots.ToArray());

            txtXRo.Text = "";
            txtXRci.Text = "";
            txtXRmci.Text = "";

            txtYRo.Text = "";
            txtYRci.Text = "";
            txtYRmci.Text = "";

            foreach (double xr in xReadings)
            {
                txtXRo.Text += xr.ToString() + "\n";
                double xDot = xDots.Where(i=>xr-i<xGap && xr-i>=0).ToArray()[0];
                txtXRci.Text += xDot.ToString() + "\n";
                txtXRmci.Text += Math.Round((xr-xDot)/(xGap/minorCount),1).ToString() + "\n";
            }

            double xa = xReadings.Sum() / xReadings.Count;
            txtXRo.Text += "\n" + xa.ToString() + "\n";
            double xDotA = xDots.Where(i => xa - i < xGap && xa - i >= 0).ToArray()[0];
            txtXRci.Text += "\n" + xDotA.ToString() + "\n";
            txtXRmci.Text += "\n" + Math.Round((xa - xDotA) / (xGap / minorCount), 1).ToString() + "\n";

            foreach (double yr in yReadings)
            {
                txtYRo.Text += yr.ToString() + "\n";
                double yDot = yDots.Where(i=>yr-i<yGap && yr-i>=0).ToArray()[0];
                txtYRci.Text += yDot.ToString() + "\n";
                txtYRmci.Text += Math.Round((yr-yDot)/(yGap/minorCount),1).ToString() + "\n";
            }

            double ya = yReadings.Sum() / yReadings.Count;
            txtYRo.Text += "\n" + ya.ToString() + "\n";
            double yDotA = yDots.Where(i => ya - i < yGap && ya - i >= 0).ToArray()[0];
            txtYRci.Text += "\n" + yDotA.ToString() + "\n";
            txtYRmci.Text += "\n" + Math.Round((ya - yDotA) / (yGap / minorCount), 1).ToString() + "\n";

            txtDetails.Text = "X Gap : " + xGap.ToString() + "\nY Gap : " + yGap.ToString() + "\n\nX Min : " + xMin.ToString() + "\nX Max : " + xMax.ToString() + "\n\nY Min : " + yMin.ToString() + "\nY Max : " + yMax.ToString();







            /*
            int lineGap = 2;
            int topBegin = 40, leftBegin = 40;
            string htm = @"
<!DOCTYPE htm>
<html>
    <head>
        <title>Smart Graph</title>
        <style>
            .hl{
                position: absolute;
                width: 360px;
                border: solid #bbf 0.2px;                
                height: 0px;
                left: "+leftBegin+ @"px;
            }
            .vn{
                position: absolute;                
                color: #99f;
                font-size:10px;                
                left: 25px;
                transform:translateY(-50%);
            }
            .hn{
                position: absolute;                
                color: #99f;
                font-size:10px;                
                top: " + (topBegin + yCellCount * lineGap * minorCount) + @"px;
                transform:translateY(50%) translateX(-50%);
            }

            .hl.m,.vl.m{
                border-color: #99f;                
                border-width: 0.4px;
            }
            .vl{
                position: absolute;
                width: 0px;
                border: solid #bbf 0.2px;                
                height: 520px;
                top: " + topBegin+@"px;
            }
            .graph{
                position:absolute;
                display:block;
                width:450px;
                height:650px;
                
            }
        </style>
    </head>
    <body>
        <div class='graph'>
            ";
            
            
            for (int i = 0; i < yCellCount; i++)
            {
                htm += String.Format("<span class='vn' style='top:{1}px;'>{0}</span>", yCellCount - i, topBegin + i * lineGap * minorCount );
                htm += String.Format("<span class='hl m' style='top:{0}px'></span>", topBegin+ i*lineGap*minorCount);
                for (int j = 1; j < minorCount; j++)
                {
                    htm += String.Format("<span class='hl' style='top:{0}px'></span>", topBegin + i * lineGap * minorCount + j * lineGap);
                }
            }
            htm += String.Format("<span class='hl m' style='top:{0}px'></span>", topBegin + yCellCount * lineGap * minorCount);
            htm += String.Format("<span class='vn' style='top:{1}px;'>{0}</span>", 0, 10+topBegin + yCellCount * lineGap * minorCount);

            for (int i = 0; i < xCellCount; i++)
            {
                htm += String.Format("<span class='hn' style='left:{1}px;'>{0}</span>", i+1, leftBegin + (i+1) * lineGap * minorCount);
                htm += String.Format("<span class='vl m' style='left:{0}px'></span>", leftBegin + i * lineGap * minorCount);
                for (int j = 1; j < minorCount; j++)
                {
                    htm += String.Format("<span class='vl' style='left:{0}px'></span>", leftBegin + i * lineGap * minorCount + j * lineGap);
                }
            }
            htm += String.Format("<span class='vl m' style='left:{0}px'></span>", leftBegin + xCellCount * lineGap * minorCount);

            
            htm += htm + @"</div></body></html>";



            String filePath = System.IO.Path.GetTempPath() + @"\SmartGraph\" + DateTime.Now.ToString("h:mm:ss tt") + ".htm";
            System.IO.Directory.CreateDirectory(System.IO.Path.GetTempPath() + @"\SmartGraph");
            System.IO.File.WriteAllText(filePath, htm);


            System.Diagnostics.Process.Start(filePath);
            */
        }


        private void txtXR_TextChanged(object sender, TextChangedEventArgs e)
        {
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SmartGraphX");
            regKey.SetValue("XReadings", txtXR.Text);
            regKey.Close();
        }

        private void txtYR_TextChanged(object sender, TextChangedEventArgs e)
        {
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SmartGraph");
            regKey.SetValue("YReadings", txtYR.Text);
            regKey.Close();
        }

        private void frmMain_Loaded(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SmartGraph");
            txtXR.Text = regKey.GetValue("XReadings", "").ToString();
            txtYR.Text = regKey.GetValue("YReadings", "").ToString();
            regKey.Close();

            if (txtXR.Text != "" && txtYR.Text != "") Button_Click(null, null);
        }

        private void txtXRo_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

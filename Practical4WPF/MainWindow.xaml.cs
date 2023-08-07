using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace Practical4WPF
{
    public partial class MainWindow : Window
    {
        private DateTime date = DateTime.Now;
        private List<int> now = new List<int>();
        public MainWindow()
        {
            InitializeComponent();
            SterAndDeser.data =(SterAndDeser.dester<List<Data>>(SterAndDeser.Sterfile)==null)? new List<Data>() : SterAndDeser.dester<List<Data>>(SterAndDeser.Sterfile);
            SterAndDeser.type = (SterAndDeser.dester<HashSet<string>>(SterAndDeser.SterfileType) == null) ? new HashSet<string>() : SterAndDeser.dester<HashSet<string>>(SterAndDeser.SterfileType);
            datePicker.Text = date.ToShortDateString();
            Sum();
            gridUpdate();
            TypeList.ItemsSource = SterAndDeser.type;
        }
        private void NewTypeBtn_Click(object sender, RoutedEventArgs e)
        {
            AddNewType w= new AddNewType();
            if(w.ShowDialog()==true )
                SterAndDeser.type.Add(AddNewType.newtype);
            w.Close();
            TypeList.ItemsSource = SterAndDeser.type;
        }
        private void NewEntryBtn_Click(object sender, RoutedEventArgs e)
        {
            SterAndDeser.data.Add(new Data(date, null,0,true,null));
            gridUpdate();
        }
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Grid.SelectedIndex != -1)
            {
                SterAndDeser.data.RemoveAt(now[Grid.SelectedIndex]);
                now.RemoveAt(Grid.SelectedIndex);
            }
            gridUpdate();
        }
        private void Grid_SelectionChanged(object sender, DataGridCellEditEndingEventArgs e)
        {
            Sum();
            if (Grid.SelectedIndex != -1)
                SterAndDeser.data[now[Grid.SelectedIndex]] = (Data)Grid.SelectedValue;

        }
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            SterAndDeser.ster(SterAndDeser.data, SterAndDeser.Sterfile);
            SterAndDeser.ster(SterAndDeser.type, SterAndDeser.SterfileType);
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            SterAndDeser.ster(SterAndDeser.data, SterAndDeser.Sterfile);
            SterAndDeser.ster(SterAndDeser.type, SterAndDeser.SterfileType);
        }
        private void gridUpdate()
        {
            now = new List<int>();
            date = Convert.ToDateTime(datePicker.Text);
            for (int i = 0; i < SterAndDeser.data.Count; i++)
                if (SterAndDeser.data[i].Date.ToShortDateString() == date.ToShortDateString())
                    now.Add(i);

            Grid.ItemsSource = null;
            List<Data> items = new List<Data>();
            foreach (int i in now)
                items.Add(SterAndDeser.data[i]);
            Grid.ItemsSource = items;
        }
        private void DatePicK(object sender, SelectionChangedEventArgs e)
        {
            gridUpdate();
        }
        private void Sum()
        {
            int sum = 0;
            foreach (Data i in SterAndDeser.data)
                sum += (i.Pluss) ? (int)i.Money : (int)i.Money * -1;
            Cost.Text = "Итог:" +sum.ToString();
        }
    }
    
    class Data
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public uint Money { get; set; }
        public bool Pluss { get; set; }
        public string Type { get; set; }
        public Data(DateTime date, string name, uint mony, bool pluss, string typ)
        {
            Date = date;
            Name = name;
            Money = mony;
            Pluss = pluss;
            Type = typ;
        }
    }
    internal class SterAndDeser
    {
        public static List<Data> data = new List<Data>();
        public static HashSet<string> type = new HashSet<string>();
        public const string Sterfile = "\\RiderProjects\\Practical4WPF\\Practical4WPF\\Sterfile.json";
        public const string SterfileType = "\\RiderProjects\\Practical4WPF\\Practical4WPF\\Type.json";
        public static void ster<T>(T listdata, string paff)
        {
            string paf = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string json = JsonConvert.SerializeObject(listdata);
            File.WriteAllText(paf + paff, json);
        }
        public static T dester<T>(string paff)
        {
            string paf = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string jsontext = File.ReadAllText(paf + paff);
            T list = JsonConvert.DeserializeObject<T>(jsontext);
            return list;
        }
    }
}

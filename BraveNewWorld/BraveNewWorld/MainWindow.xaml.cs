using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BraveNewWorld.Models;
using BraveNewWorld.Services;

namespace BraveNewWorld
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int MapSize = 400;
        private int MapX = 400;
        private int MapY = 400;
        private ObservableCollection<IMapItem> AllOfThem = new ObservableCollection<IMapItem>();
        private readonly IMapService MapHandler = new MapService();

        public MainWindow()
        {
            InitializeComponent();
            MainContainer.ItemsSource = AllOfThem;
            DotInput.Text = "1000";
        }

        public void LoadMap(int DotCount)
        {
            var rnd = new Random();

            var points = new HashSet<BenTools.Mathematics.Vector>();
            points.Clear();
            for (int i = 0; i < DotCount; i++)
            {
                var x = rnd.NextDouble()*(MapX);
                var y = rnd.NextDouble()*(MapY);

                points.Add(new BenTools.Mathematics.Vector(x,y));
            }

            AllOfThem.Clear();


            MapHandler.LoadMap(new LoadMapParams(points, true, MapX, MapY));

            AddThemAll();
        }

        private void AddThemAll()
        {
            foreach (Center o in App.AppMap.Centers.Values)
            {
                AllOfThem.Add(o);
            }

            foreach (Corner c in App.AppMap.Corners.Values)
            {
                AllOfThem.Add(c);
            }

            foreach (Edge ed in App.AppMap.Edges.Values)
            {
                AllOfThem.Add(ed);
            }
        }

        private void LoadMapStuff(object sender, RoutedEventArgs e)
        {
            App.ResetMap();
            LoadMap(int.Parse(DotInput.Text));
        }

        private void SmoothShit(object sender, RoutedEventArgs e)
        {
            MapItemFactory fact = new MapItemFactory();
            var org = new Corner[App.AppMap.Corners.Values.Count];
            App.AppMap.Corners.Values.CopyTo(org, 0);

            Dictionary<int, Corner> bck = new Dictionary<int, Corner>();

            foreach (Center c in App.AppMap.Centers.Values)
            {
                foreach (Corner co in c.Corners)
                {
                    if (bck.ContainsKey(co.Key))
                        continue;
                    
                    var back = new Corner(0, 0);
                    back.Key = co.Key;

                    var qx = co.Touches.Sum(x => x.Point.X) / co.Touches.Count;
                    var qy = co.Touches.Sum(x => x.Point.Y) / co.Touches.Count;

                    var rx = co.Protrudes.Sum(x => x.Midpoint.X) / co.Protrudes.Count;
                    var ry = co.Protrudes.Sum(x => x.Midpoint.Y) / co.Protrudes.Count;

                    var div = co.Protrudes.Count;
                    var newp = ((new Vector(qx, qy) / div) +
                               (2 * new Vector(rx, ry) / div) +
                               ((div - 3) * new Vector(co.Point.X, co.Point.Y) / div));

                    //var newp = new Point((qx / div) + (2 * rx / div) + (co.Point.X / div),
                    //    (qy / div) + (2 * ry / div) + (co.Point.Y / div));

                    back.Point = new Point(newp.X,newp.Y);

                    bck.Add(co.Key, back);
                }
            }

            foreach (Center c in App.AppMap.Centers.Values)
            {
                var buff = new Edge[c.Borders.Count];
                c.Borders.CopyTo(buff);
                c.Borders.Clear();

                foreach (Edge edge in buff)
                {
                    if (edge.DelaunayEnd == null || edge.DelaunayStart == null)
                        continue;

                    var po = new Point((edge.VoronoiStart.Point.X + edge.VoronoiEnd.Point.X + edge.DelaunayEnd.Point.X + edge.DelaunayStart.Point.X) / 4,
                        (edge.VoronoiStart.Point.Y + edge.VoronoiEnd.Point.Y + edge.DelaunayEnd.Point.Y + edge.DelaunayStart.Point.Y) / 4);

                    fact.RemoveEdge(edge);

                    var co = fact.CornerFactory(po.X, po.Y);
                    c.Corners.Add(co);
                    var e1 = fact.EdgeFactory(edge.VoronoiStart, co, edge.DelaunayStart, edge.DelaunayEnd);
                    c.Borders.Add(e1);
                    var e2 = fact.EdgeFactory(co, edge.VoronoiEnd, edge.DelaunayStart, edge.DelaunayEnd);
                    c.Borders.Add(e2);

                    co.Protrudes.Add(e1);
                    co.Protrudes.Add(e2);
                    co.Touches.Add(edge.DelaunayStart);
                    co.Touches.Add(edge.DelaunayEnd);

                    edge.VoronoiStart.Protrudes.Remove(edge);
                    edge.VoronoiStart.Protrudes.Add(e1);

                    edge.VoronoiEnd.Protrudes.Remove(edge);
                    edge.VoronoiEnd.Protrudes.Add(e2);

                }
            }

            foreach (var t in bck)
            {
                App.AppMap.Corners[t.Key].Point = t.Value.Point;
            }

            foreach (Center center in App.AppMap.Centers.Values)
            {
                center.OrderCorners();
            }

            AllOfThem.Clear();
            AddThemAll();
        }
    }
}

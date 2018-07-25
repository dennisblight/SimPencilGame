using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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

namespace Deanor.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Deanor.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Deanor.Controls;assembly=Deanor.Controls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:HexagonalButton/>
    ///
    /// </summary>
    public class HexagonalButton : Button
    {
        static HexagonalButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HexagonalButton), new FrameworkPropertyMetadata(typeof(HexagonalButton)));
        }

        private PointCollection _leftHexPoint;
        public PointCollection LeftHexPoints
        {
            get
            {
                if (_leftHexPoint == null)
                {
                    _leftHexPoint = new PointCollection
                    {
                        Point.Add(LeftOrigin, DirectionVector(HexRadius, 0)),
                        Point.Add(LeftOrigin, DirectionVector(HexRadius, 60)),
                        Point.Add(LeftOrigin, DirectionVector(HexRadius, 120)),
                        Point.Add(LeftOrigin, DirectionVector(HexRadius, 180)),
                        Point.Add(LeftOrigin, DirectionVector(HexRadius, -120)),
                        Point.Add(LeftOrigin, DirectionVector(HexRadius, -60))
                    };
                }
                return _leftHexPoint;
            }
        }

        private PointCollection _innerHexPoint;
        public PointCollection InnerHexPoints
        {
            get
            {
                if (_innerHexPoint == null)
                {
                    var eightyPercent = HexRadius * 0.8;
                    _innerHexPoint = new PointCollection
                    {
                        Point.Add(LeftOrigin, DirectionVector(eightyPercent, 0)),
                        Point.Add(LeftOrigin, DirectionVector(eightyPercent, 60)),
                        Point.Add(LeftOrigin, DirectionVector(eightyPercent, 120)),
                        Point.Add(LeftOrigin, DirectionVector(eightyPercent, 180)),
                        Point.Add(LeftOrigin, DirectionVector(eightyPercent, -120)),
                        Point.Add(LeftOrigin, DirectionVector(eightyPercent, -60))
                    };
                }
                return _innerHexPoint;
            }
        }

        private Point[] _rightHexPoint;
        public ObservableCollection<Point> RightHexPoints
        {
            get
            {
                if (_rightHexPoint == null)
                {
                    var eightyPercent = HexRadius * 0.8;
                    _rightHexPoint = new Point[]
                    {
                        Point.Add(RightOrigin, DirectionVector(eightyPercent, 0)),
                        Point.Add(RightOrigin, DirectionVector(eightyPercent, 60)),
                        Point.Add(RightOrigin, DirectionVector(eightyPercent, 120)),
                        Point.Add(RightOrigin, DirectionVector(eightyPercent, 180)),
                        Point.Add(RightOrigin, DirectionVector(eightyPercent, -120)),
                        Point.Add(RightOrigin, DirectionVector(eightyPercent, -60))
                    };
                }
                return new ObservableCollection<Point>(_rightHexPoint);
            }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if(e.Property.Equals(ActualHeightProperty) || e.Property.Equals(HeightProperty))
            {
                _leftHexPoint = new PointCollection
                {
                    Point.Add(LeftOrigin, DirectionVector(HexRadius, 0)),
                    Point.Add(LeftOrigin, DirectionVector(HexRadius, 60)),
                    Point.Add(LeftOrigin, DirectionVector(HexRadius, 120)),
                    Point.Add(LeftOrigin, DirectionVector(HexRadius, 180)),
                    Point.Add(LeftOrigin, DirectionVector(HexRadius, -120)),
                    Point.Add(LeftOrigin, DirectionVector(HexRadius, -60))
                };
            }
        }

        //private double _RenderedHeight => double.IsNaN(Height) ? ActualHeight : Height;
        private double HexRadius => 0.57735026918962576450914878050196 * ActualHeight;
        private Point LeftOrigin => new Point(HexRadius, ActualHeight / 2);
        private Point RightOrigin => new Point(ActualWidth - 0.8 * HexRadius, ActualHeight / 2);

        private static Vector DirectionVector(double distance, double direction)
        {
            const double Degrees = Math.PI / 180;
            return new Vector(distance * Math.Cos(Degrees * direction), distance * Math.Sin(Degrees * direction));
        }
    }
}

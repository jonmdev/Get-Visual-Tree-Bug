using System.Diagnostics;

namespace Get_Visual_Tree_Bug {
    public partial class App : Application {
        ContentPage mainPage;
        AbsoluteLayout abs;
        public App() {
            InitializeComponent();

            mainPage = new();
            mainPage.BackgroundColor = Colors.Black;
            MainPage = mainPage;

            List<View> resizeList = new();

            abs = new();
            mainPage.Content = abs;
            resizeList.Add(abs);
            
            AbsoluteLayout abs2 = new();
            abs.Add(abs2);
            resizeList.Add(abs2);

            VerticalStackLayout vert = new();
            abs2.Add(vert);
            resizeList.Add(vert);

            Border border = new();
            border.BackgroundColor = Colors.Green;
            vert.Add(border);
            resizeList.Add(border);

            TapGestureRecognizer tap = new();
            tap.Tapped += (s, e) => {
                checkTargets((Point)e.GetPosition(border));
            };
            border.GestureRecognizers.Add(tap); 

            mainPage.SizeChanged += delegate {
                for (int i = 0; i < resizeList.Count; i++) {
                    resizeList[i].HeightRequest = mainPage.Height;
                    resizeList[i].WidthRequest = mainPage.Width;

                }
            };
        }
        public void checkTargets(Point point) {

            var elementsList = mainPage.GetVisualTreeElements(point); //needs to be on main thread
            //var elementsList = abs.GetVisualTreeElements(point); //needs to be on main thread

            for (int i = 0; i < elementsList.Count; i++) {
                if (elementsList[i] as VisualElement != null) {
                    VisualElement element = elementsList[i] as VisualElement;
                    if (element != null) {
                        Debug.WriteLine("FOUND at point " + point + ": " + element.GetType());
                    }
                }

            }
        }
    }
}

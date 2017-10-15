namespace WpfApplication.BO
{
    public class UICanvasIcon
    {

        public static string CheckCircle = "M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-2 15l-5-5 1.41-1.41L10 14.17l7.59-7.59L19 8l-9 9z";
        public static string HourGlassEmpty = "M6 2v6h.01L6 8.01 10 12l-4 4 .01.01H6V22h12v-5.99h-.01L18 16l-4-4 4-3.99-.01-.01H18V2H6zm10 14.5V20H8v-3.5l4-4 4 4zm-4-5l-4-4V4h8v3.5l-4 4z";
        public static string Clock = "M12,20A7,7 0 0,1 5,13A7,7 0 0,1 12,6A7,7 0 0,1 19,13A7,7 0 0,1 12,20M12,4A9,9 0 0,0 3,13A9,9 0 0,0 12,22A9,9 0 0,0 21,13A9,9 0 0,0 12,4M12.5,8H11V14L15.75,16.85L16.5,15.62L12.5,13.25V8M7.88,3.39L6.6,1.86L2,5.71L3.29,7.24L7.88,3.39M22,5.72L17.4,1.86L16.11,3.39L20.71,7.25L22,5.72Z";
        public static string ErrorCircle = "M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z";
        public static string Loop = "M9,14V21H2V19H5.57C4,17.3 3,15 3,12.5A9.5,9.5 0 0,1 12.5,3A9.5,9.5 0 0,1 22,12.5A9.5,9.5 0 0,1 12.5,22H12V20H12.5A7.5,7.5 0 0,0 20,12.5A7.5,7.5 0 0,0 12.5,5A7.5,7.5 0 0,0 5,12.5C5,14.47 5.76,16.26 7,17.6V14H9Z";


        public static string Ok = "green";
        public static string Active = "blue";
        public static string Passive = "gray";
        public static string Warning = "red";

        public string Fill { get; set; }
        public string Data { get; set; }


        public static UICanvasIcon Init     = new UICanvasIcon()  { Data = HourGlassEmpty, Fill = Active };
        public static UICanvasIcon Waiting  = new UICanvasIcon() { Data = Clock, Fill = Active };
        public static UICanvasIcon Disabled = new UICanvasIcon() { Data = Clock, Fill = Passive };
        public static UICanvasIcon Running  = new UICanvasIcon() { Data = Loop, Fill = Ok };
        public static UICanvasIcon Error    = new UICanvasIcon() { Data = ErrorCircle, Fill = Warning };
      

    }
}
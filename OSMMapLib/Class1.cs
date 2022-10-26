using System.Text;
using MapRendererLib;

namespace OSMMapLib
{
    public class Tile
    {
        public Tile(int X, int Y, int ZOOM,string URL)
        {
            this.x = X;
            this.y = Y; 
            this.Zoom = ZOOM;
            this.url = URL;
        }
        public int x { get; private set; }
        public int y { get; private set; }
        public string url { get; private set; }
        int zoom;
        public int Zoom
        {
            get { return zoom; }
            private set
            {
                if (value < 1)
                {
                    zoom = 1;
                } else
                {
                    zoom = value; 
                }
            }
        }
        public override string ToString()
        {
            return $"[{x},{y},{Zoom}]: {url}";
        }

    }

    public class Layer
    {
       
        public Layer(string URLTEMPLATE = "https://{c}.tile.openstreetmap.org/{z}/{x}/{y}.png", int MAXZOOM = 10)
        {
            UrlTemplate = URLTEMPLATE;
            MaxZoom = MAXZOOM;
        }
        
        public string? UrlTemplate { get; private set; }
        public int MaxZoom { get; private set; }

        public string? FormatUrl(int y, int x, int z)
        {
            Random rnd = new Random();
            string[] array = { "a", "b", "c" };
            string c = array[rnd.Next(0,3)];
            StringBuilder sb = new StringBuilder(UrlTemplate);

            return sb.Replace("{c}", c).Replace("{z}", $"{z}").Replace("{x}", $"{x}").Replace("{y}", $"{y}").ToString();
        }

        public Tile this[int x, int y, int z]
        {
            get
            {
                return new Tile(x, y, z, FormatUrl(y, x, z));
            }
        }
    }

    public class Map
    {

        public Layer? Layer { get; set; }
        private double Lat;
        private double Lon;
        public int Zoom { get; set; } = 5;
        public double lat
        {
            get{
                return Lat;
            }
            set
            {
                Lat = value;
                while (Lat < -90)
                {
                    Lat += 180;
                }
                while (Lat > 90)
                {
                    Lat -= 180;
                }
            }
        }
        public double lng
        {
            get { return Lon; }
            set
            {
                Lon = value;
                while (Lon < -180)
                {
                    Lon += 360;
                }
                while (Lon > 180)
                {
                    Lon -= 360;
                }
            }
        }

        private int CenterTileX()
        {
            return (int)((Lon + 180.0) / 360.0 * (1 << Zoom));
        }

        private int CenterTileY()
        {
            return (int)((1.0 - Math.Log(Math.Tan(lat* Math.PI / 180.0) + 1.0 / Math.Cos(lat* Math.PI / 180.0)) / Math.PI) / 2.0 * (1 << Zoom));

        }

    
        public void Render(string FileName)
        {
            MapRenderer mapRenderer = new MapRenderer(4, 4);
            for (int x = -2; x < 2; x++)
            {
                for (int y = -2; y < 2; y++)
                {
                    Tile tile = this.Layer[this.CenterTileX() + x, this.CenterTileY() + y, this.Zoom];

                    Console.WriteLine(tile);

                    mapRenderer.Set(x + 2, y + 2, tile.url);
                }
            }
            mapRenderer.Flush();
            mapRenderer.Render(FileName);
        } 


    }
}
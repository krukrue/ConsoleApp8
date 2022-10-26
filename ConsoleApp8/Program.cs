// See https://aka.ms/new-console-template for more information
using OSMMapLib;



Layer layer = new Layer();
Console.WriteLine(layer.FormatUrl(5,6,7));

Map map = new Map();
map.Layer = new Layer("https://{c}.tile.openstreetmap.org/{z}/{x}/{y}.png", 10);
map.Layer.FormatUrl(5,6,7);
map.lng = 43;
map.lat = 26;
map.Zoom = 5;
map.Render("file.png");
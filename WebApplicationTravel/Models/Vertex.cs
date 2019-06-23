using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationTravel.Models
{
    public class Vertex : IComparable<Vertex>
    {
        public Vertex parent { get; set; }
        public string data { get; set; }
        public double pathVal { get; set; }
        public string key { get; set; }
        public LinkedList<Edge> edges=new LinkedList<Edge>();

        public Vertex(string data)
        {
            this.data = data;
        }
        public void addEdge(Edge e)
        {
            this.edges.AddFirst(e);
        }
        public LinkedList<Edge> getEdges()
        {
            return this.edges;
        }

       

        public int CompareTo(Vertex other)
        {
            double sub = this.pathVal - other.pathVal;
            if (sub > 0) return 1;
            else if (sub < 0) return -1;
            else return 0;
        }
    }
    public class Edge
    {
        public double price { get; set; }
        public double duration { get; set; }
        public string type { get; set; }
        public Vertex destination { get; set; }

        public Edge(double price, double duration, Vertex destination, string type)
        {
            this.price = price;
            this.duration = duration;
            this.destination = destination;
            this.type = type;
        }
    }
   
}
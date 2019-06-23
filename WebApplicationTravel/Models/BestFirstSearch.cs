using QuickGraph.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationTravel.Models
{
    public class BestFirstSearch
    {
        PriorityQueue<Vertex> open = new PriorityQueue<Vertex>();
        ISet<Vertex> closed=new HashSet<Vertex>();
        Vertex initial;
        Vertex goal;
        MSGDBContext db = new MSGDBContext();
        Dictionary<string, Vertex> vertices = new Dictionary<string, Vertex>();
        LinkedList<string> path = new LinkedList<string>();

        public void BuildGraph()
        {
            foreach (City c in db.Cities)
            {
                if (!vertices.ContainsKey(c.CityName))
                {
                    Vertex newV = new Vertex(c.CityName);
                    vertices.Add(c.CityName, newV);
                }
            }
            foreach (KeyValuePair<string, Vertex> kvp in vertices)
            {
                Vertex v=kvp.Value;
                
                foreach(Connections con in db.Connections)
                {
                    if (con.SourceCity.CityName.Equals(v.data))
                    {
                        Vertex dest;
                        vertices.TryGetValue(con.DestCity.CityName, out dest);
                        if (con.FlightAvailabilty)
                        {
                            Edge e = new Edge(con.FlightPrice.Value, con.FlightDuration.Value , dest, "flight");
                            v.addEdge(e);
                        }
                        if (con.CarAvailabilty)
                        { 
                            Edge e = new Edge(con.FlightPrice.Value, con.FlightDuration.Value, dest, "car");
                            v.addEdge(e);
                        }

                    }
                }
            }
        }

        public void Search(string initial, string goal, string key)
        {
            vertices.TryGetValue(initial, out this.initial);
            vertices.TryGetValue(goal, out this.goal);
            open.Enqueue(this.initial);
            Vertex n;
            Vertex s;
            while (open.Count() > 0)
            {
                n = open.Dequeue();
                closed.Add(n);
                if (n.Equals(this.goal))
                {
                    BackTrace();
                }
                foreach(Edge e in n.getEdges())
                {
                    s = e.destination;
                    if (!closed.Contains(s) && !open.Contains(s))
                    {
                        s.parent = n;
                        s.key = e.type;
                        if (key.Equals("price"))
                        {
                            s.pathVal += e.price;
                        }
                        else if (key.Equals("time"))
                        {
                            s.pathVal += e.duration;
                        }
                        open.Enqueue(s);
                    }
                    else if (open.Contains(s))
                    {
                        if (key.Equals("price"))
                        {
                            if (s.pathVal > n.pathVal + e.price)
                            {
                                s.pathVal = n.pathVal + e.price;
                                s.parent = n;
                                s.key = e.type;
                            }
                        }
                        if (key.Equals("time"))
                        {
                            if (s.pathVal > n.pathVal + e.duration)
                            {
                                s.pathVal = n.pathVal + e.duration;
                                s.parent = n;
                                s.key = e.type;
                            }
                        }
                        open.Update(s);
                    }

                }
            }

        }
        public void BackTrace()
        {
            Vertex v = this.goal;
            while (v.parent != null)
            {
                path.AddFirst(v.data);
                path.AddFirst(v.key);
                v = v.parent;
            }
            path.AddFirst(v.data);
        }
    }
}

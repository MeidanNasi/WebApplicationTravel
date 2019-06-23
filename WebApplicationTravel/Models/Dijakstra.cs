using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuickGraph;
using QuickGraph.Algorithms;
using QuickGraph.Algorithms.ShortestPath;
using QuickGraph.Algorithms.Observers;
using System.Text.RegularExpressions;

namespace WebApplicationTravel.Models
{
    public class Dijakstra
    {
        AdjacencyGraph<string, Edge<string>> graph;
        private MSGDBContext db = new MSGDBContext();
        Dictionary<Edge<string>, double> edgeTime;
        Dictionary<Edge<string>, double> edgeCost;
        String message = "start get path \n";

        public Dijakstra()
        {
            this.graph = new AdjacencyGraph<string, Edge<string>>(true);
            this.edgeTime = new Dictionary<Edge<string>, double>(db.Connections.Count()*2);
            this.edgeCost = new Dictionary<Edge<string>, double>(db.Connections.Count() * 2);
            foreach (City c in db.Cities)
            {
                graph.AddVertex(c.CityName);
            }
            foreach(Connections c in db.Connections)
            {
                if (c.FlightAvailabilty)
                {
                    Edge<string> s_dFT = new Edge<string>(c.SourceCity.CityName, c.DestCity.CityName);
                    graph.AddEdge(s_dFT);
                    edgeTime.Add(s_dFT, c.FlightDuration.GetValueOrDefault() );
                    Edge<string> s_dFC = new Edge<string>(c.SourceCity.CityName, c.DestCity.CityName);
                    graph.AddEdge(s_dFC);
                    edgeCost.Add(s_dFC, c.FlightPrice.GetValueOrDefault());
                }
                if (c.CarAvailabilty)
                {
                    Edge<string> s_dCT = new Edge<string>(c.SourceCity.CityName, c.DestCity.CityName);
                    graph.AddEdge(s_dCT);
                    edgeTime.Add(s_dCT, c.CarDuration.GetValueOrDefault());
                    Edge<string> s_dCC = new Edge<string>(c.SourceCity.CityName, c.DestCity.CityName);
                    graph.AddEdge(s_dCC);
                    edgeCost.Add(s_dCC, c.CarPrice.GetValueOrDefault());
                }
            }
            
            
        }
        public void calcPath(string searchKey, string from, string destination)
        {
            City source = new City();
            City dest=new City();
            IEnumerable<City> fromQ = db.Cities.Where(s => s.CityName == from);
            IEnumerable<City> destQ = db.Cities.Where(s => s.CityName == destination);
            foreach(City c in fromQ)
            {
                source = c;
            }
            foreach (City c in destQ)
            {
                dest = c;
            }
            DijkstraShortestPathAlgorithm<string, Edge<string>> path;
            if (searchKey.Equals("Time"))
            {
                Func<Edge<string>, double> edgeTimeAlgo = AlgorithmExtensions.GetIndexer(edgeTime);
                path = new DijkstraShortestPathAlgorithm<string, Edge<string>>(this.graph, edgeTimeAlgo);
                VertexDistanceRecorderObserver<string, Edge<string>> distObserver = new VertexDistanceRecorderObserver<string, Edge<string>>(edgeTimeAlgo);
                using (distObserver.Attach(path)) ;
                VertexPredecessorRecorderObserver<string, Edge<string>> predecessorObserver = new VertexPredecessorRecorderObserver<string, Edge<string>>();
                using (predecessorObserver.Attach(path)) ;
                path.Compute(from);
                foreach(KeyValuePair<string, double> kvp in distObserver.Distances)
                {
                    Console.WriteLine("Distance from root to node {0} is {1} hours", kvp.Key, kvp.Value);
                }
                foreach(KeyValuePair<string,Edge<string>> kvp in predecessorObserver.VertexPredecessors)
                {
                    Console.WriteLine("If you want to get to {0} you have to enter through the in edge {1}", kvp.Key, kvp.Value);
                }
            }
            else if (searchKey.Equals("Price"))
            {
                Func<Edge<string>, double> edgeCostAlgo = AlgorithmExtensions.GetIndexer(edgeCost);
                path = new DijkstraShortestPathAlgorithm<string, Edge<string>>(this.graph, edgeCostAlgo);
               
                VertexDistanceRecorderObserver<string, Edge<string>> distObserver = new VertexDistanceRecorderObserver<string, Edge<string>>(edgeCostAlgo);
                distObserver.Attach(path);
                VertexPredecessorRecorderObserver<string, Edge<string>> predecessorObserver = new VertexPredecessorRecorderObserver<string, Edge<string>>();
                predecessorObserver.Attach(path);
                path.Compute(from);
               
                String outString = "";
                IEnumerable<Edge<string>> solution;
                if(predecessorObserver.TryGetPath(from, out solution))
                {
                    foreach(var u in solution)
                    {
                        outString += u + ";";
                    }
                }
                string[] outEdges = Regex.Split(outString, ";");
                if (outEdges.Length > 0)
                {
                    for(int i=0; i<outEdges.Length; i++)
                    {
                        this.message += outEdges[i] + "\n";
                        string[] outPoint = Regex.Split(outEdges[i], "->");
                    }
                }
                //foreach (KeyValuePair<City, double> kvp in distObserver.Distances)
                //{
                //    Console.WriteLine("Price from root to node {0} is {1}$ ", kvp.Key.CityName, kvp.Value);
                //}
                //foreach (KeyValuePair<City, Edge<City>> kvp in predecessorObserver.VertexPredecessors)
                //{
                //    Console.WriteLine("If you want to get to {0} you have to enter through the in edge {1}", kvp.Key.CityName, kvp.Value);
                //}
            }
           
        }
    }
}
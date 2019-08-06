using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationTravel.Models
{
    public class ReservationBuilder
    {
        public LinkedList<Connections> Connections { get; set; }
        public LinkedList<string> path { get; set; }
        public LinkedList<double> prices { get; set; }
        public LinkedList<double> durations { get; set; }
        public double totalPrice { get; set; }
        public double totalTime { get; set; }

        public ReservationBuilder(LinkedList<string> path)
        {

            this.path = path;
            this.prices = new LinkedList<double>();
            this.durations = new LinkedList<double>();
            this.Connections = new LinkedList<Connections>();
            this.totalPrice = 0;
            this.totalTime = 0;

        }
        public void bulidReservation()
        {
            City source = new City();
            City dest = new City();
            Connections connection = new Connections();
            string type;
            for (int i = 0; i < path.Count() - 2; i += 2)
            {
                source = getCitybyName(path.ElementAt(i));
                dest = getCitybyName(path.ElementAt(i + 2));
                type = path.ElementAt(i + 1);
                connection = getConnection(source, dest);
                Connections.AddLast(connection);
                if (type.Equals("flight"))
                {
                    prices.AddLast(connection.FlightPrice.Value);
                    durations.AddLast(connection.FlightDuration.Value);
                    totalPrice += connection.FlightPrice.Value;
                    totalTime += connection.FlightDuration.Value;
                }
                if (type.Equals("car"))
                {
                    prices.AddLast(connection.CarPrice.Value);
                    durations.AddLast(connection.CarDuration.Value);
                    totalPrice += connection.CarPrice.Value;
                    totalTime += connection.CarDuration.Value;
                }

            }
            this.totalPrice = System.Math.Round(this.totalPrice, 2);
            this.totalTime = System.Math.Round(this.totalTime, 2);
        }
        public City getCitybyName(string name)
        {
            MSGDBContext db = new MSGDBContext();
            City city = new City();
            IEnumerable<City> q = db.Cities.Where(s => s.CityName == name);
            foreach (City c in q)
            {
                city = c;
            }
            return city;
        }
        public Connections getConnection(City s, City d)
        {
            MSGDBContext db = new MSGDBContext();
            Connections con = new Connections();
            IEnumerable<Connections> q = db.Connections.Where(c => c.SourceCity.CityId == s.CityId && c.DestCity.CityId == d.CityId);
            foreach (Connections c in q)
            {
                con = c;
            }
            return con;
        }
        public LinkedList<string> updateReservation()
        {
            if (this.path.Count() == 0) return null;
            LinkedList<string> myReservation = new LinkedList<string>();
            string s = "";
            myReservation.AddLast("From: " + path.ElementAt(0) + " To: " + path.Last()+"");
            for (int i = 0, j = 0; i < path.Count() - 2; i += 2, j++)
            {
                s += path.ElementAt(i) + " -> " + path.ElementAt(i + 2) + " by " + path.ElementAt(i + 1) + ". Price: " + prices.ElementAt(j) + "$" + " Duration: " + durations.ElementAt(j) + " hours.";
                myReservation.AddLast(s);
                s = "";
            }
            myReservation.AddLast("Total Price: " + this.totalPrice + "$ ,Total Duration: " + this.totalTime+" hours.");
            return myReservation;
        }
    }
}
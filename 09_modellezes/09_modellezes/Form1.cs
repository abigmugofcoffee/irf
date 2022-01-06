using _09_modellezes.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _09_modellezes
{
    public partial class Form1 : Form
    {
        List<Person> Population = new List<Person>();
        List<ChildbirthProbability> ChildBirthProbabilities = new List<ChildbirthProbability>();
        List<DeathProbability> DeathProbabilities = new List<DeathProbability>();
        Random rnd = new Random(1234);
        public Form1()
        {
            InitializeComponent();
            //MessageBox.Show(Directory.GetCurrentDirectory().ToString());
            //string csvpath;
            Population = GetPopulation(@"../../../../../09_modellezes/nep-teszt.csv");
            ChildBirthProbabilities = GetChildBirth(@"../../../../../09_modellezes/szuletes.csv");
            DeathProbabilities = GetDeaths(@"../../../../../09_modellezes/halal.csv");
                        
            for (int year = 2005; year < 2024; year++)
            {
                for (int i = 0; i < Population.Count; i++)
                {
                    SimStep(year, Population[i]);
                }
                int NbrOfMales = (from x in Population
                                  where x.Gender == Gender.Male && x.IsAlive
                                  select x).Count();
                int NbrOfFemales = (from x in Population
                                  where x.Gender == Gender.Female && x.IsAlive
                                  select x).Count();
                Console.WriteLine(string.Format("Év:{0} Fiúk:{1} Lányok:{2}", year, NbrOfMales, NbrOfFemales));
            }

        }
        public List<Person> GetPopulation(string csvpath)
        {
            List<Person> population = new List<Person>();

            StreamReader sr = new StreamReader(csvpath, Encoding.Default);
            using (sr)
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    population.Add(new Person()
                    {
                        BirthYear = int.Parse(line[0]),
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[1]),
                        NbrOfChildren = int.Parse(line[2])
                    });
                }
            }
            return population;
        }
        public List<ChildbirthProbability> GetChildBirth(string csvpath)
        {
            List<ChildbirthProbability> childbirth = new List<ChildbirthProbability>();

            StreamReader sr = new StreamReader(csvpath, Encoding.Default);
            using (sr)
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    childbirth.Add(new ChildbirthProbability()
                    {
                        Age = int.Parse(line[0]),
                        NbrOfChildren = int.Parse(line[1]),
                        Probability = double.Parse(line[2])
                    });
                }
            }
            return childbirth;
        }

        public List<DeathProbability> GetDeaths(string csvpath)
        {
            List<DeathProbability> deaths = new List<DeathProbability>();

            StreamReader sr = new StreamReader(csvpath, Encoding.Default);
            using (sr)
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    deaths.Add(new DeathProbability()
                    {
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[0]),
                        Age = int.Parse(line[1]),
                        Probability = double.Parse(line[2])
                    });
                }
            }
            return deaths;
        }

        private void SimStep(int year, Person person)
        {
            if (!person.IsAlive) return;
            byte age = (byte)(year - person.BirthYear);

            double pdeath = (from x in DeathProbabilities
                             where x.Gender == person.Gender && x.Age == age
                             select x.Probability).FirstOrDefault();

            if (rnd.NextDouble() <= pdeath)
            {
                person.IsAlive = false;
                return;
            }
            if (person.Gender == Gender.Male) return;

            double pchildbirth = (from x in ChildBirthProbabilities
                                  where x.Age == age && x.NbrOfChildren == person.NbrOfChildren
                                  select x.Probability).FirstOrDefault();

            if (rnd.NextDouble() <= pchildbirth)
            {
                Population.Add(new Person()
                {
                    BirthYear = year,
                    Gender = (Gender)(rnd.Next(1, 3)),
                    NbrOfChildren = 0,
                    IsAlive = true
                });
            }
        }
    }
}

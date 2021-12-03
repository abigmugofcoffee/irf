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
        public Form1()
        {
            InitializeComponent();
            string csvpath = "";
            Population = GetPopulation(csvpath);
            ChildBirthProbabilities = GetChildBirth(csvpath);
            DeathProbabilities = GetDeaths(csvpath);

            Random rng = new Random(1234);

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
                        BirthYear = int.Parse(line[0]),
                        NbrOfChildren = int.Parse(line[2]),
                        Probability = double.Parse(line[3])
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
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[1]),
                        BirthYear = int.Parse(line[0]),
                        Probability = double.Parse(line[3])
                    });
                }
            }
            return deaths;
        }
    }
}

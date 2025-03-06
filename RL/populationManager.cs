using System;
using System.Collections.Generic;
using NumSharp;
using System.IO;
using System.Text.Json;

namespace Motion
{
    public class PopulationManager
    {
        public List<NDArray> Observations { get; private set; }
        public List<float> Rewards { get; private set; }
        public List<int> Actions { get; private set; }

        public PopulationManager()
        {
            Observations = new List<NDArray>();
            Rewards = new List<float>();
            Actions = new List<int>();
        }

        public void AddData(NDArray observation, float reward, int action)
        {
            Observations.Add(observation);
            Rewards.Add(reward);
            Actions.Add(action);
        }

        public void SaveToFile(string filePath)
        {
            var data = new
            {
                Observations = Observations.ConvertAll(o => o.ToArray<double>()),
                Rewards,
                Actions
            };

            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public void LoadFromFile(string filePath)
        {
            if (!File.Exists(filePath)) return;

            string json = File.ReadAllText(filePath);
            var data = JsonSerializer.Deserialize<dynamic>(json);

            Observations.Clear();
            Rewards.Clear();
            Actions.Clear();

            foreach (var obs in data["Observations"])
            {
                Observations.Add(np.array(obs.ToObject<double[]>())); // Konverter tilbake til NDArray
            }

            foreach (var reward in data["Rewards"])
            {
                Rewards.Add((float)reward);
            }

            foreach (var action in data["Actions"])
            {
                Actions.Add((int)action);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using NumSharp;
using System.IO;
using System.Text.Json;

namespace Motion
{
    public class PopulationManager
    {
        public List<NDArray> States { get; private set; }
        public List<int> Actions { get; private set; }
        public List<float> Rewards { get; private set; }
        public List<NDArray> NextStates { get; private set; }
        public List<bool> Dones { get; private set; }

        public PopulationManager()
        {
            States = new List<NDArray>();
            Actions = new List<int>();
            Rewards = new List<float>();
            NextStates = new List<NDArray>();
            Dones = new List<bool>();
        }

        public void AddExperience(NDArray state, int action, float reward, NDArray nextState, bool done)
        {
            States.Add(state);
            Actions.Add(action);
            Rewards.Add(reward);
            NextStates.Add(nextState);
            Dones.Add(done);
        }

        public void SaveToFile(string filePath)
        {
            var data = new
            {
                States = States.ConvertAll(s => s.ToArray<double>()),
                Actions,
                Rewards,
                NextStates = NextStates.ConvertAll(s => s.ToArray<double>()),
                Dones
            };

            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public void LoadFromFile(string filePath)
        {
            if (!File.Exists(filePath)) return;

            string json = File.ReadAllText(filePath);
            var data = JsonSerializer.Deserialize<dynamic>(json);

            States.Clear();
            Actions.Clear();
            Rewards.Clear();
            NextStates.Clear();
            Dones.Clear();

            foreach (var state in data["States"])
            {
                States.Add(np.array(state.ToObject<double[]>()));
            }

            foreach (var action in data["Actions"])
            {
                Actions.Add((int)action);
            }

            foreach (var reward in data["Rewards"])
            {
                Rewards.Add((float)reward);
            }

            foreach (var nextState in data["NextStates"])
            {
                NextStates.Add(np.array(nextState.ToObject<double[]>()));
            }

            foreach (var done in data["Dones"])
            {
                Dones.Add((bool)done);
            }
        }
    }
}
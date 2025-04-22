using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Avalonia.OpenGL.Angle;

using System;
using System.Threading;
using NumSharp;
using SixLabors.ImageSharp;
using Gym.Environments;
using Gym.Environments.Envs.Classic;
using Gym.Rendering.Avalonia;

namespace Motion{

    class GeneticAlgorithm {
        public Agent[] population;
        public Config config = Config.Instance();

        public GeneticAlgorithm() {
            this.population = InitializePopulation();
        }
      
        // public static float Evaluate(Agent agent, int stepsize){
        //     agent.Fitness = Random.NextDouble();

        // }

        public static void Evaluate(Agent agent, int stepsize){
            CartPoleEnv cp = new CartPoleEnv(AvaloniaEnvViewer.Factory); // or AvaloniaEnvViewer.Factory
            bool done = true;
            float total_rerward = 0;
            var action = 0.0; 
            
            for (int i = 0; i < 100; i++)
            {
                if (done)
                {
                    NDArray observation = cp.Reset();
                    done = false;
                }
                else
                {
                    var (observation, reward, _done, information) = cp.Step((Int32)action); // switching between left and right
                    action = agent.ForwardPass(observation)[0]; // switching between left and right
                    done = _done;
                    // Do something with the reward and observation.
                    total_rerward += reward;
                    //Console.WriteLine(observation.ToString());
                    
                }

                //Thread.Sleep(15); // Prevent the loop from finishing instantly.
            }

            agent.Fitness =  total_rerward;
        }
        
        public Agent[] InitializePopulation(){
            Agent[] population = new Agent[config.PopulationSize];
            for (int i = 0; i < config.PopulationSize; i++)
            {
                population[i] = Agent.InitializeAgent(4, 1, 5);    // must add up to population-size
            }
            return population;
        }


        public Agent GA(Agent[] population){

            for (int i = 0; i < config.NumGenerations; i++) {

                // evaluate
                for (int j = 0; j < population.Length; j++)
                {
                    Evaluate(population[j], 1);
                }

                List<List<Agent>> species = Speciation.SpeciateAndFitnessSharing(population, 1.0, 0.8, 0.8, 0.3, 0.5);

                List<Agent> nextGeneration = new List<Agent>();
                int totalPopulation = population.Length;

                foreach (var specie in species)
                {
                    if (specie.Count == 0) continue;

                    // Determine number of offspring for this species
                    double totalAdjustedFitness = species.Sum(s => s.Sum(a => a.AdjustedFitness));
                    double speciesFitness = specie.Sum(a => a.AdjustedFitness);
                    int offspringCount = (int)Math.Round((speciesFitness / totalAdjustedFitness) * totalPopulation);

                    // (Optional) Elitism: keep the best agent
                    Agent elite = specie.OrderByDescending(a => a.Fitness).First();
                    nextGeneration.Add(elite);
                    offspringCount--;

                    while (offspringCount-- > 0)
                    {
                        var (parent1, parent2) = Selection.RouletteWheelSelection(specie); // species level selection
                        Agent child = Crossover.ApplyCrossover(parent1, parent2);
                        Mutate.ApplyMutations(child); // implement this yourself
                        nextGeneration.Add(child);
                    }
                }
                population = nextGeneration.ToArray();
                Console.WriteLine($"Generation {i + 1}:   AVG NFitness: { population.Average(a => a.AdjustedFitness)}");
            }
            return population.OrderByDescending(a => a.AdjustedFitness).First();

        }



        private bool IsNotEdgeInChromosome(EdgeChromosome edge, EdgeChromosome[] chromosomeEdges){
            foreach (EdgeChromosome chromosomeEdge in chromosomeEdges)
            {
                if (edge.FromId == chromosomeEdge.FromId && edge.ToId == chromosomeEdge.ToId)
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsNotNodeInChromosome(NodeChromosome node, NodeChromosome[] chromosomeNodes){
            foreach (NodeChromosome chromosomeNode in chromosomeNodes)
            {
                if (node.Id == chromosomeNode.Id)
                {
                    return false;
                }
            }
            return true;
        }


        public void Main(){

            // Initialize the population
            Agent[] population = InitializePopulation();
            Agent bestAgent =  GA(population);
            Console.WriteLine($"Best agent fitness: {bestAgent.Fitness}");


            
            
        }

        

    }
}












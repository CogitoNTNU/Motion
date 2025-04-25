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
        public List<Agent> population;
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
            
            for (int i = 0; i < 1000; i++)
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
                    // Console.WriteLine($"Action: {action}");
                    //cp.Render();
                    if (action > 0.01){
                        action = 1;
                    }else{
                        action = 0;
                    }
                    done = _done;
                    // Do something with the reward and observation.
                    total_rerward += reward;
                    //Console.WriteLine(observation.ToString());
                    
                }

                //Thread.Sleep(15); // Prevent the loop from finishing instantly.
            }

            agent.Fitness =  total_rerward;
        }
        
        public List<Agent> InitializePopulation(){
            List<Agent> population = new List<Agent>(); // Initialize the population with the specified size
            for (int i = 0; i < config.PopulationSize; i++)
            {
                Agent agent = Agent.InitializeAgent(4, 1, 5);
                population.Add(agent);
            }
            return population;
        }


        public Agent GA(List<Agent> population)
        {
            for (int i = 0; i < config.NumGenerations; i++) 
            {
                // Evaluation
                Parallel.For(0, population.Count, j => 
                {
                    Evaluate(population[j], 10);
                });

                // Speciation
                List<List<Agent>> species = Speciation.SpeciateAndFitnessSharing(
                    population, 0.3, 
                    1.0, 0.8, 0.3, 0.9);

                Console.WriteLine($"Species count: {species.Count}");
                List<Agent> nextGeneration = new List<Agent>();
                
                // Calculate total adjusted fitness
                double totalAdjustedFitness = species.Sum(s => s.Sum(a => a.AdjustedFitness));
                if (totalAdjustedFitness <= 0) totalAdjustedFitness = 1; // Prevent division by zero

                // Process each species
                foreach (var specie in species.Where(s => s.Count > 0))
                {
                    // Calculate this species' proportion of next generation
                    double speciesFitness = specie.Sum(a => a.AdjustedFitness);
                    int offspringCount = (int)Math.Round(
                        (speciesFitness / totalAdjustedFitness) * config.PopulationSize);

                    // Apply elitism (keep best individual)
                    if (specie.Count > 1)
                    {
                        var elite = specie.OrderByDescending(a => a.Fitness).First();
                        nextGeneration.Add(elite);
                        offspringCount--;
                    }

                    // Generate offspring
                    for (int j = 0; j < offspringCount; j++)
                    {
                        var (parent1, parent2) = Selection.RouletteWheelSelection(specie);
                        Agent child = Crossover.ApplyCrossover(parent1, parent2);
                        Mutate.ApplyMutations(child);
                        nextGeneration.Add(child);
                    }
                }

                // Enforce exact population size
                population = nextGeneration
                    .OrderByDescending(a => a.AdjustedFitness)
                    .Take(config.PopulationSize)
                    .ToList();

                // Logging
                Console.WriteLine($"Generation {i + 1}:");
                Console.WriteLine($"  Population: {population.Count}");
                Console.WriteLine($"  Population: {population.Count}");
                Console.WriteLine($"  Avg Fitness: {population.Average(a => a.Fitness):F2}");
                Console.WriteLine($"  Best Fitness: {population.Max(a => a.Fitness):F2}");
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



        public void Run(Agent agent, int stepsize){

            CartPoleEnv cp = new CartPoleEnv(AvaloniaEnvViewer.Factory); 
            bool done = true;
            float total_rerward = 0;
            var action = 0.0; 
            for (int i = 0; i < 100_000; i++)
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
                    // Console.WriteLine($"Action: {action}");
                    //cp.Render();
                    if (action > 0.01){
                        action = 1;
                    }else{
                        action = 0;
                    }
                    done = _done;
                    // Do something with the reward and observation.
                }

                SixLabors.ImageSharp.Image img = cp.Render(); //returns the image that was rendered.
                Thread.Sleep(15); //this is to prevent it from finishing instantly !
            }

            //cp.Close();

        }


        public void Main(){

            // Initialize the population
            List<Agent> population = InitializePopulation();
            Agent bestAgent =  GA(population);
            Console.WriteLine($"Best agent fitness: {bestAgent.Fitness}");
            Run(bestAgent, 1);
            

            
            
        }

        

    }
}












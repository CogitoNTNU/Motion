
namespace Motion{

    class Speciation{

        public static (int, int) GetMinMaxInnovationNumber(List<EdgeChromosome> edges) {
            
            int min = int.MaxValue;
            int max = int.MinValue;
            
            foreach (var edge in edges) {
                if (edge.InnovationNumber < min) min = edge.InnovationNumber;
                if (edge.InnovationNumber > max) max = edge.InnovationNumber;
            }

            return (min, max);
        }

        public static (int, int) GetInnovationRange(Agent agent1, Agent agent2) {
            
            int start = int.MaxValue;
            int end = int.MinValue;

            List<EdgeChromosome> edges1 = agent1.Edges;
            List<EdgeChromosome> edges2 = agent2.Edges;

            (int, int) range1 =  GetMinMaxInnovationNumber(edges1);
            (int, int) range2 = GetMinMaxInnovationNumber(edges2);

            start = int.Max(range1.Item1, range2.Item1);
            end = int.Min(range1.Item2, range2.Item2);
            
            return (start, end);
            
        }

        // public static int GetNumberOfDisjointEdges(EdgeChromosome[] edges1, EdgeChromosome[] edges2, (int, int) innovationRange) {
        //     int disjointEdges = 0;
        //     foreach (var edge in edges1) {
        //         if (edge.InnovationNumber < innovationRange.Item1 || edge.InnovationNumber > innovationRange.Item2) {
        //             continue;
        //         }
                
        //         foreach (var edge2 in edges2) {
        //             if (edge.InnovationNumber == edge2.InnovationNumber) {
        //                 break;
        //             }
        //             disjointEdges++;
        //         }
        //     }   
        //     return disjointEdges;           
        // }

        public static int GetNumberOfDisjointEdges(List<EdgeChromosome> edges1, List<EdgeChromosome> edges2, (int, int) innovationRange)
        {
            var set2 = new HashSet<int>(edges2.Select(e => e.InnovationNumber));
            int disjointEdges = 0;
            foreach (var edge in edges1)
            {
                if (edge.InnovationNumber >= innovationRange.Item1 &&
                    edge.InnovationNumber <= innovationRange.Item2 &&
                    !set2.Contains(edge.InnovationNumber))
                {
                    disjointEdges++;
                }
            }
            return disjointEdges;
        }

        
        public static int GetNumberOFExcessEdges(List<EdgeChromosome> edges1, List<EdgeChromosome> edges2, (int,int) innovationRange) {
            int excessEdges = 0;
            foreach (var edge in edges1) {
                if (edge.InnovationNumber < innovationRange.Item1 || edge.InnovationNumber > innovationRange.Item2) {
                    excessEdges++;
                }
            }
            foreach (var edge in edges2) {
                if (edge.InnovationNumber < innovationRange.Item1 || edge.InnovationNumber > innovationRange.Item2) {
                    excessEdges++;
                }
            }   
            return excessEdges;           
        }




        public static double Distance(Agent agent1, Agent agent2){
            List<EdgeChromosome> edges1 = agent1.Edges;
            List<EdgeChromosome> edges2 = agent2.Edges;
            List<NodeChromosome> nodes1 = agent1.Nodes;
            List<NodeChromosome> nodes2 = agent2.Nodes;

            double C1 = 1.0;
            double C2 = 0.8;
            double C3 = 0.3;

            double N = Math.Max(edges1.Count, edges2.Count);
            double W = 0;
            int matching = 0;
            foreach (EdgeChromosome edge1 in edges1) {
                foreach (EdgeChromosome edge2 in edges2) {
                    if (edge1.InnovationNumber == edge2.InnovationNumber) {
                        W += Math.Abs(edge1.Weight - edge2.Weight);
                        matching++;
                        break;
                    }
                }
            }
            if (matching > 0) W /= matching;

            
            (int, int) innovationRange = GetInnovationRange(agent1, agent2);
            int D = GetNumberOfDisjointEdges(edges1, edges2, innovationRange);
            int E = GetNumberOFExcessEdges(edges1, edges2, innovationRange);

            double distance =  (C1 * E / N ) + (C2 * D / N) + C3 * W;
            Console.WriteLine(distance);
            return distance;
            
        }



        public static List<List<Agent>> Speciate(List<Agent> population, double distanceThreshold) {
            List<List<Agent>> species = new List<List<Agent>>(); 

            foreach (Agent agent in population) {
                bool foundSpecies = false;
                foreach (List<Agent> specie in species) {
                    // Compare with the representative (first member) of the species.
                    if (Distance(agent, specie[0]) < distanceThreshold) {
                        specie.Add(agent);
                        foundSpecies = true;
                        break;
                    }
                }
                if (!foundSpecies) {
                    // Create a new species if no match was found.
                    species.Add(new List<Agent> { agent });
                }
            }

            return species;
        }

        public static void FitnessSharingWithoutSpecies(List<Agent> population, double c1, double c2, double c3, double deltaT)
        {
            // Iterate over each agent in the population.
            for (int i = 0; i < population.Count; i++)
            {
                double sharingSum = 0.0;

                // Compare the current agent with every other agent.
                for (int j = 0; j < population.Count; j++)
                {
                    double distance = Distance(population[i], population[j]);
                    if (distance < deltaT)
                    {
                        // The sharing function decays linearly with the distance.
                        sharingSum += 1.0 - (distance / deltaT);
                    }
                }

                // Adjust the fitness by dividing the raw fitness by the sharing sum.
                // If sharingSum is 0 (to avoid division by zero), leave the fitness unchanged.
                if (sharingSum > 0)
                {
                    population[i].AdjustedFitness = population[i].Fitness / sharingSum;
                }
                else
                {
                    population[i].AdjustedFitness = population[i].Fitness;
                }
            }
        }

        public static void FitnessSharingAcrossSpecies(List<List<Agent>> species) {
            foreach (List<Agent> specie in species) {
                int speciesCount = specie.Count;
                foreach (Agent agent in specie) {
                    agent.AdjustedFitness = agent.Fitness / speciesCount;
                }
            }
        }

        public static List<List<Agent>> SpeciateAndFitnessSharing(List<Agent> population, double distanceThreshold, double c1, double c2, double c3, double deltaT) {
            List<List<Agent>> species = Speciate(population, distanceThreshold);
            FitnessSharingAcrossSpecies(species);
            return species;
        }

    }

}
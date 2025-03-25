
namespace Motion{

    class Speciation{

        public static (int, int) GetMinMaxInnovationNumber(EdgeChromosome[] edges) {
            
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

            EdgeChromosome[] edges1 = agent1.Edges;
            EdgeChromosome[] edges2 = agent2.Edges;

            (int, int) range1 =  GetMinMaxInnovationNumber(edges1);
            (int, int) range2 = GetMinMaxInnovationNumber(edges2);

            start = int.Max(range1.Item1, range2.Item1);
            end = int.Min(range1.Item2, range2.Item2);
            
            return (start, end);
            
        }

        public static int GetNumberOFDisjointEdges(EdgeChromosome[] edges1, EdgeChromosome[] edges2, (int, int) innovationRange) {
            int disjointEdges = 0;
            foreach (var edge in edges1) {
                if (edge.InnovationNumber < innovationRange.Item1 || edge.InnovationNumber > innovationRange.Item2) {
                    disjointEdges++;
                }
            }   
            return disjointEdges;           
        }
        
        public static int GetNumberOFExcessEdges(EdgeChromosome[] edges1, EdgeChromosome[] edges2, (int,int) innovationRange) {
            int excessEdges = 0;
            foreach (var edge in edges2) {
                if (edge.InnovationNumber < innovationRange.Item1 || edge.InnovationNumber > innovationRange.Item2) {
                    excessEdges++;
                }
            }   
            return excessEdges;           
        }

    // THIS WONT WORK, NEED TO REFACTOR
        // public static List<Agent[]> Speciate(Agent[] population, double distanceThreshold) {
        //     var species = new List<Agent[]>();
        //     foreach (Agent agent in population) {
        //         var specie = new List<Agent>();
        //         foreach (Agent agent2 in population) {
        //             double distance = Distance(agent, agent2);
        //             if (distance < distanceThreshold && distance > 0 && specie.Contains(agent2) == false) {
        //                 specie.Add(agent2);
        //             }
        //         }
        //     }
        //     return species;

        // }


        public static void AdjustedFitness(Agent[] population){
            foreach (Agent agent in population) {


                agent.AdjustedFitness = 0;
            }
        }










































































        public static double Distance(Agent agent1, Agent agent2){
            EdgeChromosome[] edges1 = agent1.Edges;
            EdgeChromosome[] edges2 = agent2.Edges;
            NodeChromosome[] nodes1 = agent1.Nodes;
            NodeChromosome[] nodes2 = agent2.Nodes;

            double C1 = 1.0;
            double C2 = 0.8;
            double C3 = 0.3;

            int N = Math.Max(edges1.Length, edges2.Length);
            double W = 0;

            foreach (EdgeChromosome edge1 in edges1) {
                foreach (EdgeChromosome edge2 in edges2) {
                    if (edge1.InnovationNumber == edge2.InnovationNumber) {
                        W += Math.Abs(edge1.Weight - edge2.Weight);
                    }
                }
            }
            
            (int, int) innovationRange = GetInnovationRange(agent1, agent2);
            int D = GetNumberOFDisjointEdges(edges1, edges2, innovationRange);
            double E = GetNumberOFExcessEdges(edges1, edges2, innovationRange);

            return (C1 * E / N ) + (C2 * D / N) + C3 * W;
            
        }


    }

}
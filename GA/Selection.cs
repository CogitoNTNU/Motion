using System.Security.Cryptography;

namespace Motion{

    class Selection{

        public static double[] Normalize(double[] values)
        {
            // Calculate the sum of all elements.
            double sum = 0;
            for (int i = 0; i < values.Length; i++)
            {
                sum += values[i];
            }
            
            // If the sum is zero, return a uniform distribution.
            if (sum == 0)
            {
                double uniformValue = 1.0 / values.Length;
                double[] uniformDistribution = new double[values.Length];
                for (int i = 0; i < values.Length; i++)
                {
                    uniformDistribution[i] = uniformValue;
                }
                return uniformDistribution;
            }
            
            // Create a new array for normalized values.
            double[] normalized = new double[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                normalized[i] = values[i] / sum;
            }
            
            return normalized;
        }


        public static (Agent, Agent) RouletteWheelSelection(List<Agent> population){

            Random Random = new Random();
            
            double[] probabilities = new double[population.Length];
            for (int i = 0; i < population.Count; i++)
            {
                probabilities[i] = population[i].AdjustedFitness;
            }
            probabilities = Normalize(probabilities);

            // choose 2 parents randomly based on the probabilities
            double cumulative  = 0;
            double randomValue1 = Random.NextDouble();
            double randomValue2 = Random.NextDouble();
            Agent parent1 = null;
            Agent parent2 = null;
            for (int i = 0; i < probabilities.Length; i++)
            {
                cumulative += probabilities[i];
                if (randomValue1 < cumulative)
                {
                    parent1 = population[i];
                    break;
                }
            }
            for (int i = 0; i < probabilities.Length; i++)
            {
                cumulative += probabilities[i];
                if (randomValue2 < cumulative)
                {
                    parent2 = population[i];
                    break;
                }
            }
            if (parent1 == null || parent2 == null)
            {
                return RouletteWheelSelection(population);
            }
            return (parent1, parent2);
        }    
        
        public static (Agent, Agent) TournamentSelection(List<Agent> population, int tournamentSize)
        {
            Random random = new Random();
            Agent parent1 = SelectTournamentWinner(population, tournamentSize, random);
            Agent parent2 = SelectTournamentWinner(population, tournamentSize, random);
            return (parent1, parent2);
        }

        private static Agent SelectTournamentWinner(List<Agent> population, int tournamentSize, Random random)
        {
            // Start by selecting a random agent as the best candidate.
            Agent best = population[random.Next(population.Count)];
            
            // Iterate the remaining tournamentSize - 1 selections.
            for (int i = 1; i < tournamentSize; i++)
            {
                Agent candidate = population[random.Next(population.Count)];
                if (candidate.AdjustedFitness > best.AdjustedFitness)
                {
                    best = candidate;
                }
            }
            
            return best;
        }

    }

    class ParentSelection : Selection{
        
    }

    class SurvivorSelection : Selection{

    }
    
}
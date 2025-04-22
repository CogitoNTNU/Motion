using JetBrains.Annotations;

namespace Motion{

    class Innovation { // Singleton class

        private Innovation() { } 

        private static Innovation? _instance;

        private Dictionary<Tuple<int, int>, int> dict = new Dictionary<Tuple<int, int>, int>();

        public static Innovation GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Innovation();
            }
            return _instance;
        }

        public int GetInnovationNumber(int to, int from) {
            Tuple<int, int> tuple = new Tuple<int, int>(to, from);

            if (dict.ContainsKey(tuple)) {
                return dict[tuple];
            }

            int index = dict.Count + 1;
            dict.Add(tuple, index);
            return index;
        }
    }

    

    enum NodeType {
        Input,
        Output,
        Hidden
    }

    class NodeChromosome {
        private int id;
        private double bias;
        private String activation;
        private NodeType type;
        private Boolean active = true;

        /// <summary>
        /// Creates a new NodeChromosome object.
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="bias">double</param>
        /// <param name="activation">String: activation function</param>
        /// <param name="type">NodeType: input, hidden or output</param>
        /// <returns>
        /// NodeChromosome
        /// </returns>
        /// <remarks>
        /// Constructor for NodeChromosome
        /// </remarks>
        public NodeChromosome(int id, double bias, String activation, NodeType type) {
            this.id = id;
            this.bias = bias;
            this.activation = activation;
            this.type = type;
        }

       

        public int Id { get { return id; } }

        public double Bias { get { return bias; } set { bias = value; } }

        public String Activation { get { return activation; } }

        public NodeType Type { get { return type; } }

        public Boolean Active { get { return active; } set { active = value; } }

    }

    public readonly record struct EdgeChromosome1(int edge, int fromId, int toId, double weight);

    class EdgeChromosome {
        private int innovationNumber;
        private int fromId;
        private int toId;
        private double weight;
        private Boolean active = true;

        public EdgeChromosome(int innovationNumber, int fromId, int toId, double weight) {
            this.innovationNumber = innovationNumber;
            this.fromId = fromId;
            this.toId = toId;
            this.weight = weight;
        }

        public int InnovationNumber { get { return innovationNumber; } } 
        
        public int FromId { get { return fromId; } set { fromId = value; } }

        public int ToId { get { return toId; } set { toId = value; } }

        public double Weight { get { return weight; }  set { weight = value; } }

        public Boolean Active { get { return active; } set { active = value; } }

    }
    
    class Agent {
        private List<NodeChromosome> chromosomeNodes = new();
        private List<EdgeChromosome> chromosomeEdges = new();
        private double fitness;
        private double adjustedFitness;

        public Agent() {
            this.fitness = 0.0;
            this.adjustedFitness = 0.0;
        }

        public List<NodeChromosome> Nodes { get { return chromosomeNodes; } }

        public List<EdgeChromosome> Edges { get { return chromosomeEdges; } }

        public double Fitness { get { return fitness; } set { fitness = value; } }

        public double AdjustedFitness { get { return adjustedFitness; } set { adjustedFitness = value; } }

        public NodeChromosome GetNodeChromosomeFromId(int id) {
            foreach (var node in chromosomeNodes) {
                if (node.Id == id) {
                    return node;
                }
            }
            return null;
        }

        public void AddNode(NodeChromosome node) {
            chromosomeNodes.Add(node);
        }

        public void AddEdge(EdgeChromosome edge) {
            chromosomeEdges.Add(edge);
        }

        private static List<NodeChromosome> NodeFactory(int numNodes, double bias, string activation, NodeType nodeType) {
            List<NodeChromosome> chromosomeNodes = new List<NodeChromosome>();
            for (int i = 0; i < numNodes; i++) {
                chromosomeNodes.Add(new NodeChromosome(i, bias, activation, nodeType));
            }
            return chromosomeNodes;
        }

        // TODO: implement strategy pattern for Agent
        public static Agent InitializeAgent(int inputs, int outputs, int initialHidden) {

            List<EdgeChromosome> chromosomeEdges = new List<EdgeChromosome>(0); // initialize empty list of edges
            // nodes and edges are initialized here
            List<NodeChromosome> inputChromosomeNodes = NodeFactory(inputs, 0.0, "relu", NodeType.Input); //TODO: remove relu
            List<NodeChromosome> hiddenChromosomeNodes = NodeFactory(initialHidden, 0.5, "relu", NodeType.Hidden);
            List<NodeChromosome> outputChromosomeNodes = NodeFactory(outputs, 0.5, "relu", NodeType.Output);

            List<NodeChromosome> chromosomeNodes = inputChromosomeNodes.Concat([.. hiddenChromosomeNodes, .. outputChromosomeNodes]).ToList();

            int currentNodeId = 0;
            int nextNodeId = 0;
            for (int layer = 0; layer < 3; layer++) {
                
                int numNodes = layer switch {
                    0 => inputs,
                    1 => initialHidden,
                    2 => outputs,
                    _ => 0
                };

                int numNodesNextLayer = layer switch {
                    0 => initialHidden,
                    1 => outputs,
                    _ => 0
                };

                nextNodeId += numNodes;
                for (int node = 0; node < numNodes; node++) {

                    for (int nextLayerNode = 0; nextLayerNode < numNodesNextLayer; nextLayerNode++)
                        chromosomeEdges.Append(
                            new EdgeChromosome(
                                Innovation.GetInstance().GetInnovationNumber(nextNodeId + nextLayerNode, currentNodeId), 
                                currentNodeId, 
                                nextNodeId+nextLayerNode, 
                                0.5
                                )
                            );
        
                    currentNodeId ++;
                }
            }

            return new Agent(chromosomeNodes, chromosomeEdges);
        }

        public Agent(List<NodeChromosome> chromosomeNodes, List<EdgeChromosome> chromosomeEdges){
            this.chromosomeNodes = chromosomeNodes;
            this.chromosomeEdges = chromosomeEdges;
            this.fitness = 0.0;
            this.adjustedFitness = 0.0;
        } 

        private List<NodeChromosome> TopologicalSort()
        {
            var graph = chromosomeEdges
                .Where(e => e.Active)
                .GroupBy(e => e.FromId)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ToId).ToList());

            var visited = new HashSet<int>();
            var sorted = new List<NodeChromosome>();

            void Visit(int nodeId)
            {
                if (visited.Contains(nodeId)) return;
                visited.Add(nodeId);

                if (graph.ContainsKey(nodeId))
                    foreach (int to in graph[nodeId]) Visit(to);

                var node = GetNodeChromosomeFromId(nodeId);
                if (node != null)
                    sorted.Add(node);
            }

            foreach (var node in chromosomeNodes.Where(n => n.Type == NodeType.Input))
            {
                Visit(node.Id);
            }

            return sorted.DistinctBy(n => n.Id).ToList();
        }


        public double[] ForwardPass(NumSharp.NDArray inputs) {

            Dictionary<int, double> nodeValues = new Dictionary<int, double>();

            for (int i = 0; i < inputs.size; i++) {
                nodeValues[chromosomeNodes[i].Id] = inputs[i];
            }

            foreach (var node in TopologicalSort()) {

                if (!nodeValues.ContainsKey(node.Id) && node.Active) {
                    nodeValues[node.Id] = node.Bias;
                }

                var incomingEdges = chromosomeEdges.Where(e => e.ToId == node.Id && e.Active);
                foreach (var edge in incomingEdges) {
                    if (nodeValues.ContainsKey(edge.FromId)) {
                        nodeValues[node.Id] += nodeValues[edge.FromId];
                    }
                }

                nodeValues[node.Id] = Activate(nodeValues[node.Id], node.Activation);
            }
            
            return chromosomeNodes
                .Where(n => n.Type.Equals(NodeType.Output))
                .Select(n => nodeValues[n.Id])
                .ToArray();
        }

        private double Activate(double value, string activation) {
            return activation switch {
                "relu" => Math.Max(0, value)    // add more activation functions if needed
            };
        }

    }
}
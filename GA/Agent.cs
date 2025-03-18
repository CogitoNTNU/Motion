using JetBrains.Annotations;

namespace Motion{

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

        public NodeChromosome(int id, double bias, String activation, NodeType type) {
            this.id = id;
            this.bias = bias;
            this.activation = activation;
            this.type = type;
        }

       

        public int Id { get { return id; } }

        public double Bias { get { return bias; } }

        public String Activation { get { return activation; } }

        public NodeType Type { get { return type; } }

        public Boolean Active { get { return active; } set { active = value; } }

    }

    public readonly record struct EdgeChromosome1(int edge, int fromId, int toId, double weight);

    class EdgeChromosome {
        private int edge;
        private int fromId;
        private int toId;
        private double weight;
        private Boolean active = true;

        public EdgeChromosome(int edge, int fromId, int toId, double weight) {
            this.edge = edge;
            this.fromId = fromId;
            this.toId = toId;
            this.weight = weight;
        }

        public int Edge { get { return edge; } } 
        
        public int FromId { get { return fromId; } }

        public int ToId { get {return toId; } }

        public double Weight { get { return weight; } }

        public Boolean Active { get { return active; } set { active = value; } }


    }
    class Agent {
        public NodeChromosome[] chromosomeNodes;
        public EdgeChromosome[] chromosomeEdges;
        public double fitness;
        public double adjustedFitness;

        public Agent() {
            this.fitness = 0.0;
            this.adjustedFitness = 0.0;
        }

         public NodeChromosome GetNodeChromosomeFromId(int id) {
            foreach (var node in chromosomeNodes) {
                if (node.Id == id) {
                    return node;
                }
            }
            return null;
        }

        private static NodeChromosome[] NodeFactory(int numNodes, double bias, string activation, NodeType nodeType) {
            NodeChromosome[] chromosomeNodes = new NodeChromosome[numNodes];
            for (int i = 0; i < numNodes; i++) {
                chromosomeNodes[i] = new NodeChromosome(i, bias, activation, nodeType);
            }
            return chromosomeNodes;
        }

        // TODO: implement strategy pattern for Agent
        public static Agent InitializeAgent(int inputs, int outputs, int initialHidden) {

            EdgeChromosome[] chromosomeEdges = new EdgeChromosome[0];
            // nodes and edges are initialized here
            NodeChromosome[] inputChromosomeNodes = NodeFactory(inputs, 0.0, "relu", NodeType.Input); //TODO: remove relu
            NodeChromosome[] hiddenChromosomeNodes = NodeFactory(initialHidden, 0.5, "relu", NodeType.Hidden);
            NodeChromosome[] outputChromosomeNodes = NodeFactory(outputs, 0.5, "relu", NodeType.Output);

            NodeChromosome[] chromosomeNodes = inputChromosomeNodes.Concat(hiddenChromosomeNodes.Concat(outputChromosomeNodes).ToArray()).ToArray();

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
                        chromosomeEdges.Append(new EdgeChromosome(chromosomeEdges.Length, currentNodeId, nextNodeId+nextLayerNode, 0.5));
        
                    currentNodeId ++;
                }
            }

            return new Agent(chromosomeNodes, chromosomeEdges);
        }

        public Agent(NodeChromosome[] chromosomeNodes, EdgeChromosome[] chromosomeEdges){
            this.chromosomeNodes = chromosomeNodes;
            this.chromosomeEdges = chromosomeEdges;
            this.fitness = 0.0;
            this.adjustedFitness = 0.0;
        } 

        public double[] ForwardPass(double[] inputs) {

            Dictionary<int, double> nodeValues = new Dictionary<int, double>();

            for (int i = 0; i < inputs.Length; i++) {
                nodeValues[chromosomeNodes[i].Id] = inputs[i];
            }

            foreach (var node in chromosomeNodes) {

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
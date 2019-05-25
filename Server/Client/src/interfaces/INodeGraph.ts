

export interface INodeGraph {
    addNode(node : Node) : void;
    addNodes(nodes : Array<Node>) : void;
    removeNode(node : Node) : void;
}
import {Node} from './Node';

export class NodePairConnection {

    public nodeLeft : Node;
    public nodeRight : Node;
    public connections : Array<any>;

    constructor(nodeLeft : Node, nodeRight : Node, connections : Array<any>){
        this.nodeLeft = nodeLeft;
        this.nodeRight = nodeRight;
        this.connections = connections;
    }

}
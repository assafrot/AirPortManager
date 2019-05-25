import {Node} from './Node';

export class NodePairConnection {

    public from : Node;
    public to : Node;
    public type : string;

    constructor(from : Node, to : Node, type : string){
        this.from = from;
        this.to = to;
        this.type = type;
    }

}
import { NodeConnection } from "./NodeConnection";
import { Airplane } from "./Airplane";

export class Node {

  public x : number;
  public y : number;
  public width : number;
  public height : number;
  public connections : Array<NextNode>;
  public hovered : boolean;
  public airplane : Airplane;

  constructor(x,y,width,height) {
    this.x = x;
    this.y = y;
    this.width = width;
    this.height = height;
    this.connections = [];
    this.hovered = false;
    this.airplane = null;
  }

}

export class NextNode {

  public node : Node;
  public type : string;

  constructor(node : Node, type: string) {
    this.node = node;
    this.type = type;
  }

}
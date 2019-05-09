
export class Node {

  constructor(x,y,width,height) {
    this.x = x;
    this.y = y;
    this.width = width;
    this.height = height;
    this.connections = [];
    this.hovered = false;
    this.airplane = false;
  }

}

export class NodeConnection {

  constructor(node, connectionType) {
    this.node = node;
    this.type = connectionType;
  }

}
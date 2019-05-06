
export class NodeGraph {

  constructor() {
    this.nodes = [];
    this.cursor = {x : 0,y : 0};
    this.movingNodeIdx = -1;
  }

  addNode(node) {
    this.nodes.push(node);
  }

  removeNode(node) {
    let idx = this.nodes.indexOf(node);
    this.nodes.splice(idx,1);
  }

  onCursorPressed() {
    let hoveredNode = getHoveredNode(this.nodes, this.cursor);
    
    if(hoveredNode) {
      this.movingNodeIdx = this.nodes.indexOf(hoveredNode);
    } else {
      this.movingNodeIdx = -1;
    }

  }

  onCursorReleased() {
    this.movingNodeIdx = -1;
  }

  onCursorMoved(position) {
    
    this.nodes.forEach(node => {
      node.hovered = isCursorHovering(node, this.cursor);
    });
    
    if(this.movingNodeIdx > -1) {
      this.nodes[this.movingNodeIdx].x += position.x - this.cursor.x;
      this.nodes[this.movingNodeIdx].y += position.y - this.cursor.y;
    }
    
    this.cursor = position;
  }

}

function isCursorHovering(node, cursor) {
  return(between(cursor.x, node.x, node.x + node.width) &&
         between(cursor.y, node.y, node.y + node.height));
}

function between(num , min, max) {
  return num >= min && num <= max;
}

function getHoveredNode(nodes, cursor) {
  let hoveredNode;

  nodes.forEach(node => {
    if(isCursorHovering(node, cursor)) {
      hoveredNode = node;
    }
  });

  return hoveredNode;
}
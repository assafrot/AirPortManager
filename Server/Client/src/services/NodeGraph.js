
export class NodeGraph {

  constructor() {
    this.nodes = [];
    this.cursor = {x : 0,y : 0};
    this.movingNodeIdx = -1;
    this.generatedConnections = this.generateConnections();
    this.nodeLineList = [];
  }

  generateLines() {
    
    let nodeSideCons = [];

    this.nodes.forEach((node,idx) => {
      nodeSideCons[idx] = [];
    })

    this.nodes.forEach((node,idx) => {
      node.connections.forEach(connection => {
        let dir = this.getDir(node, connection.node)
        
        if(nodeSideCons[idx][dir.from]) {
          nodeSideCons[idx][dir.from]++;
        } else {
          nodeSideCons[idx][dir.from] = 1;
        }

        let secondNodeIdx = this.nodes.indexOf(connection.node);
        
        if(nodeSideCons[secondNodeIdx][dir.to]) {
          nodeSideCons[secondNodeIdx][dir.to]++;
        } else {
          nodeSideCons[secondNodeIdx][dir.to] = 1;
        }

      });
    })

    console.log(nodeSideCons);
    
  }

  generateConnections() {
    let gConnections = [];

    this.nodes.forEach((node, idx) => {

      node.connections.forEach(connection => {
        
        let cNodeIdx = this.nodes.indexOf(connection.node);
        let gConnection = gConnections.find(con => con.nodeLeft == connection.node && con.nodeRight == node);

        if(idx > cNodeIdx && gConnection) {
            
          gConnection.connections.push({
            from: node,
            to: connection.node,
            type: connection.type
          });

        } else {
            
          gConnections.push({
            nodeLeft: node, 
            nodeRight: connection.node, 
            connections: [{
              from: node,
              to: connection.node,
              type: connection.type
            }]
          })

        }

    })});

    return gConnections;
  }

  getDir(nodeFrom, nodeTo) {

    let first = 0;
    let second = 0;

    if(nodeFrom.x > nodeTo.x) {
      first = 2;
    } else {
      first = 0;
    }

    if(nodeFrom.y > nodeTo.y) {
      second = 3;
    } else {
      second = 1;
    }


    if(Math.abs(nodeFrom.x - nodeTo.x) > 
       Math.abs(nodeFrom.y - nodeTo.y)) {
        return {from:first, to: second }
      } else {
        return {from:second, to: first }
    }

  }

  addNode(node) {
    this.nodes.push(node);
    this.generatedConnections = this.generateConnections();
    this.nodeLineList = this.generateLines();
  }
  
  removeNode(node) {
    let idx = this.nodes.indexOf(node);
    this.nodes.splice(idx,1);
    this.generatedConnections = this.generateConnections();
    this.nodeLineList = this.generateLines();
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
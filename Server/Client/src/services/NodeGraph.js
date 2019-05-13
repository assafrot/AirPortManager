
export class NodeGraph {

  constructor() {
    this.nodes = [];
    this.cursor = {x : 0,y : 0};
    this.movingNodeIdx = -1;
    this.nodeConnectionList = this.generateConnectionList();
    this.connections = this.generateConnections();
    this.style = {
      connection:{
        lineWidth: 4,
        lineMargin: 10,
        arrowSize: 20,
        colors : {
          border: "black",
          AT_LANDING:"red",
          AT_TAKEOFF:"blue"
        } 
      }
    };
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

  generateConnectionList() {
    let list = [];

    this.nodes.forEach((node, idx) => {

      node.connections.forEach(connection => {
        
        let cNodeIdx = this.nodes.indexOf(connection.node);
        let gConnection = list.find(con => con.nodeLeft == connection.node && con.nodeRight == node);

        if(idx > cNodeIdx && gConnection) {
            
          gConnection.connections.push({
            from: node,
            to: connection.node,
            type: connection.type
          });

        } else {
            
          list.push({
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

    return list;
  }

  generateConnections() {
    let connections = [];
    this.nodeConnectionList.forEach(graphConnection => {
      graphConnection.connections.forEach((connection,idx) => {
        let dir = getDir(connection.from, connection.to);
        let offset = this.calcOffset(idx, graphConnection.connections.length);
        connections.push({
        dir : dir
        ,offset : offset
        ,pointFrom : this.getConnectionPoint(connection.from, dir.from, offset)
        ,pointTo : this.getConnectionPoint(connection.to, dir.to, offset)
        ,color : this.style.connection.colors[connection.type]
        })
      })
    })

    return connections;
  }

  addNode(node) {
    this.nodes.push(node);
    this.nodeConnectionList = this.generateConnectionList();
    this.connections = this.generateConnections();
  }

  addNodes(nodes) {
    this.nodes.push(...nodes);
    this.nodeConnectionList = this.generateConnectionList();
    this.connections = this.generateConnections();
  }
  
  removeNode(node) {
    let idx = this.nodes.indexOf(node);
    this.nodes.splice(idx,1);
    this.nodeConnectionList = this.generateConnectionList();
    this.connections = this.generateConnections();
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
      this.connections = this.generateConnections();
    }
    
    this.cursor = position;
  }

  calcOffset(idx, connectionCount) {
    return (this.style.connection.lineWidth * ((-connectionCount / 2) + idx + 0.5)) +
    (this.style.connection.lineMargin * (idx - 0.5))
  }

  getConnectionPoint(node, dir, offset) {
    let lineWidth = this.style.connection.lineWidth;
    switch(dir) {
      case 0: 
        return {x: node.x + node.width, y: node.y + (node.height / 2) + offset}
        break;
      case 1: 
        return {x: node.x + (node.width / 2) + offset, y: node.y + node.height}
        break;
      case 2: 
        return {x: node.x , y: node.y + (node.height / 2) + offset}
        break;
      case 3: 
        return {x: node.x + (node.width / 2) + offset, y: node.y}
        break;
    }
  }

}

export function getDir(nodeFrom, nodeTo) {

  if(Math.abs(nodeFrom.x - nodeTo.x) > 
     Math.abs(nodeFrom.y - nodeTo.y)) {
      if(nodeFrom.x > nodeTo.x) {
        return {from: 2, to: 0 }
      } else {
        return {from: 0, to: 2 }
      } 
    } else {
      if(nodeFrom.y > nodeTo.y) {
        return {from: 3, to: 1 }
      } else {
        return {from: 1, to: 3 }
      } 
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
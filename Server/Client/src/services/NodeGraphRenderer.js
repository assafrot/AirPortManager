
import {AT_TAKEOFF, AT_LANDING} from "../models/Events";

export class NodeGraphRenderer {

  constructor(ctx) {
    this.ctx = ctx;
    this.style = {
      connection:{
        lineWidth: 2,
        arrowSize: 15,
        colors : {
          AT_LANDING:"red",
          AT_TAKEOFF:"blue"
        } 
      }
    };
  }

  drawNodeGraph(graph) {
    this.ctx.clearRect(0, 0, 800, 600);
  
    this.drawConnections(graph);
  
    graph.nodes.forEach(node => {
      this.drawNode(node);  
    })
    
  }

  drawNode(node) {
    this.ctx.beginPath();
    this.ctx.fillStyle = "grey";
    this.ctx.fillRect(node.x, node.y, node.width, node.height); 
    this.ctx.fill();
     
    // if(node.hovered) {
    //   this.drawOutlineForNode(node);
    // }
  
    // if(node.airplane) {
    //   this.drawAirplaneInNode(node);
    // }
  }

  drawOutlineForNode(node) {
    this.ctx.beginPath();
    this.ctx.moveTo(node.x, node.y);
    this.ctx.strokeStyle = "lightgreen";
    this.ctx.strokeRect(node.x, node.y, node.width, node.height);
    this.ctx.stroke();
  }

  drawAirplaneInNode(node) {
    this.ctx.beginPath();
    this.ctx.fillStyle = "orange";

    let querterWidth = (node.width/4);
    let querterHeight = (node.height/4);

    this.ctx.moveTo(node.x + querterWidth, node.y + querterHeight);
    this.ctx.fillRect(node.x + querterWidth, node.y + querterHeight,
                      querterWidth*2, querterHeight*2);
    this.ctx.fill();
  }

  drawConnections(graph) {
    
    let color;

    graph.nodes.forEach(node => {
      node.connections.forEach((connection) => {
        this.drawConnection(node, connection, this.style.connection.colors[connection.type]);
      });
    })
    // graph.nodes.forEach(node => {
    //   node.connections.forEach((connection) => {
    //     let ap = this.calcArrowPosition(node, connection.node, this.style.connection.arrowSize);
    //     this.drawTriangle(ap, ap.dir * 90, this.style.connection.arrowSize, this.style.connection.colors[connection.type]);
    //   });
    // })

    graph.generatedConnections.forEach(graphConnection => {
      this.drawGraphConnection(graphConnection);
    })

  }

  drawGraphConnection(graphConnection) {
    


    graphConnection.connections.forEach((connection,idx) => {
      
      let modifiedNodeFrom = {}
      Object.assign(modifiedNodeFrom, graphConnection.nodeLeft);
      modifiedNodeFrom.x -= (this.style.connection.lineWidth * idx)
      
      let modifiedNodeTo = {}
      Object.assign( modifiedNodeTo, graphConnection.nodeRight);
      modifiedNodeTo.y -= (this.style.connection.lineWidth * idx)
      
      this.drawConnection(modifiedNodeFrom, modifiedNodeTo, 
        this.style.connection.colors[connection.type]);
    })
  }

  drawConnection(nodeFrom, nodeTo, color) {
    this.ctx.beginPath();

    this.ctx.moveTo(nodeFrom.x + (nodeFrom.width / 2), 
                    nodeFrom.y + (nodeFrom.height / 2));
  
    if(Math.abs(nodeFrom.x - nodeTo.x) > 
       Math.abs(nodeFrom.y - nodeTo.y)) {

      this.ctx.lineTo(nodeTo.x + (nodeTo.width / 2),
                      nodeFrom.y + (nodeFrom.height / 2));

    } else {
  
      this.ctx.lineTo(nodeFrom.x + (nodeFrom.width / 2),
                      nodeTo.y + (nodeTo.height / 2));
    
    }
  
    this.ctx.lineTo(nodeTo.x + (nodeTo.width / 2), 
                    nodeTo.y + (nodeTo.height / 2));
    this.ctx.strokeStyle = color;
    this.ctx.lineWidth = this.style.connection.lineWidth;
    this.ctx.stroke();
  }


  drawTriangle(cPoint, angle = 0, size = 10, color = "black") {
    angle = angle * 0.01745329 ;
    this.ctx.beginPath();
    this.ctx.fillStyle = color;
    
    let fp = {x: size/2, y: 0};
    let fpX = this.rotateX(fp, angle); 
    let fpY = this.rotateY(fp, angle); 
    this.ctx.moveTo(cPoint.x+ fpX, cPoint.y + fpY);
    
    let sp = {x: -1*size/2, y: size/2};
    let spX = this.rotateX(sp, angle); 
    let spY = this.rotateY(sp, angle); 
    this.ctx.lineTo(cPoint.x + spX, cPoint.y + spY);
    
    let tp = {x: -1*size/2, y: -1*size/2};
    let tpX = this.rotateX(tp, angle); 
    let tpY = this.rotateY(tp, angle); 
    this.ctx.lineTo(cPoint.x + tpX, cPoint.y + tpY);

    this.ctx.lineTo(cPoint.x + fpX, cPoint.y + fpY);

    this.ctx.fill();
  }


  calcArrowPosition(nodeFrom, nodeTo, arrowSize) {

    let pos = {};

    if(Math.abs(nodeFrom.x - nodeTo.x) > Math.abs(nodeFrom.y - nodeTo.y)) {

      
      if(nodeFrom.y + (nodeFrom.height/2) < nodeTo.y) {
        pos.x = nodeTo.x + (nodeTo.width / 2);
        pos.y = nodeTo.y - (arrowSize / 2);
        pos.dir = 1;
      } 
      else if (nodeFrom.y + (nodeFrom.height/2) <= nodeTo.y + nodeTo.height)
      {
        
        if(nodeTo.x < nodeFrom.x) {
          pos.x = nodeTo.x  + nodeTo.width + (arrowSize / 2);
          pos.dir = 2;
        } else {
          pos.x = nodeTo.x - (arrowSize / 2);
          pos.dir = 0;
        }
        
        pos.y = nodeFrom.y + (nodeFrom.height / 2);
      }
      else {
        pos.x = nodeTo.x + (nodeTo.width / 2);
        pos.y = nodeTo.y + nodeTo.height + (arrowSize / 2);
        pos.dir = 3;
      }
      
    } 
    else 
    {
      
      if(nodeFrom.x + (nodeFrom.width/2) < nodeTo.x) {
        pos.y = nodeTo.y + (nodeTo.height / 2);
        pos.x = nodeTo.x - (arrowSize / 2);
        pos.dir = 0;
      }
      else if (nodeFrom.x + (nodeFrom.width/2) <= nodeTo.x + nodeTo.width)
      {
        
        if(nodeTo.y < nodeFrom.y) {
          pos.y = nodeTo.y  + nodeTo.height + (arrowSize / 2);
          pos.dir = 3;
        } else {
          pos.y = nodeTo.y - (arrowSize / 2);
          pos.dir = 1;
        }
        
        pos.x = nodeFrom.x + (nodeFrom.width / 2);
      }
      else 
      {
        pos.y = nodeTo.y + (nodeTo.height / 2);
        pos.x = nodeTo.x + nodeTo.width + (arrowSize / 2);
        pos.dir = 2;
      }

    }

    return pos;

  }

  rotateX(point, angle) { return (point.x * Math.cos(angle)) +  
                                 (point.y * -Math.sin(angle))};

  rotateY(point, angle) { return (point.x * Math.sin(angle)) + 
                                  point.y * Math.cos(angle)};


}


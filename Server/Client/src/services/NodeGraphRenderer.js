
import {AT_TAKEOFF, AT_LANDING} from "../models/Events";
import { getDir } from "../services/NodeGraph"

export class NodeGraphRenderer {

  constructor(ctx) {
    this.ctx = ctx;
    this.style = {};
  }

  drawNodeGraph(graph) {
    this.ctx.clearRect(0, 0, 800, 600);
    this.style = graph.style;

    this.drawConnections(graph);
  
    graph.nodes.forEach(node => {
      this.drawNode(node);  
    })
    
  }

  drawNode(node) {
    this.ctx.beginPath();
    this.ctx.fillStyle = this.style.node.color;
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
    
    graph.connections.forEach(connection => {
      this.drawConnection(connection.pointFrom, connection.pointTo, connection.color);
    })
    
    graph.connections.forEach(connection => {
      this.drawArrow(connection.pointFrom, connection.pointTo, connection.dir, connection.color);
    })

  }

  drawArrow(pointFrom, pointTo, dir, color) {
    let m = this.calcIncine(pointFrom, pointTo);
    let arrowPoint = this.calcArrowPosition(pointTo, pointFrom, dir.to, m);
    let angle = -this.calcDegree(pointFrom, pointTo) + 90;

    this.drawTriangle(arrowPoint, angle,
    this.style.connection.arrowSize,color);
  }

  calcIncine(pointA, pointB) {
    return (pointA.y - pointB.y) / (pointA.x - pointB.x);
  }

  calcDegree(pointFrom, pointTo) {
    let x = pointTo.x - pointFrom.x;
    let y = pointTo.y - pointFrom.y;
  
    let angle = Math.atan2(x, y);
    let dAngle = this.toDegree(angle);
    
    return dAngle;
  }

  calcArrowPosition(pointTo,pointFrom, dir, m) {
    let arrowSize = this.style.connection.arrowSize;
    
    switch(dir) {
      case 0:
        return {x: pointTo.x + (arrowSize / 2), y : pointTo.y + (m * (arrowSize / 2))}
        break;
      case 1:{
        if(m == Infinity) {
          return {x: pointTo.x , y : pointTo.y + (arrowSize / 2)}
        }
        let x = ((pointTo.y + (arrowSize / 2)) - pointFrom.y + m * pointFrom.x) / m;
        return {x: x  , y : pointTo.y + (arrowSize / 2)}
      }break;
      case 2:
      return {x: pointTo.x - (arrowSize / 2), y : pointTo.y - (m * (arrowSize / 2)) }
      break;
      case 3:{
        if(m == -Infinity){
          return {x: pointTo.x , y : pointTo.y - (arrowSize / 2)}
        }
        let x = ((pointTo.y - (arrowSize / 2)) - pointFrom.y + m * pointFrom.x) / m;
        return {x: x , y : pointTo.y - (arrowSize / 2)}
      }break;
    }
  }

  drawConnection(pointFrom, pointTo, color) {
    this.ctx.beginPath();
    this.ctx.moveTo(pointFrom.x, pointFrom.y);
    this.ctx.lineTo(pointTo.x, pointTo.y);
    this.ctx.strokeStyle = color;
    this.ctx.lineWidth = this.style.connection.lineWidth;
    this.ctx.stroke();
  }

  toDegree(angle) {
    return angle / 0.01745329 ;
  }

  toRadian(angle) {
    return angle * 0.01745329 ;
  }

  drawTriangle(cPoint, angle = 0, size = 10, color = "black") {
    angle = this.toRadian(angle);

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

  //angle in radians
  rotateX(point, angle) { return (point.x * Math.cos(angle)) +  
    (point.y * -Math.sin(angle))};
    
  //angle in radians
  rotateY(point, angle) { return (point.x * Math.sin(angle)) + 
                                  point.y * Math.cos(angle)};

}


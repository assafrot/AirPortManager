
import { writable, get } from "svelte/store"
import { AirportState } from "../models/AirportState"
import { NodeGraph } from "./NodeGraph";
import { Node , NodeConnection} from "../models/Node";
import { AT_LANDING, AT_TAKEOFF } from "../models/Events";

export let AirportStateStore = writable(new AirportState());

export class AirportService {

  constructor() {
    this.fetchAirportState();
  }

  fetchAirportState() {
    //initDummy();

    let connection = new signalR.HubConnectionBuilder().withUrl("/airport").configureLogging(signalR.LogLevel.Information).build();
    
    connection.start().then(async () => {
      let state = await connection.invoke("GetAirportState");
      
      
      let newState = new AirportState();

      //dummy queue
      newState.airplanesInQueue.push({
        flightNumber : "123",
        requestTime : new Date(),
        actionType : "take off"
      })
      newState.airplanesInQueue.push({
        flightNumber : "123",
        requestTime : new Date(),
        actionType : "take off"
      })
      newState.airplanesInQueue.push({
        flightNumber : "123",
        requestTime : new Date(),
        actionType : "take off"
      })
      newState.airplanesInQueue.push({
        flightNumber : "123",
        requestTime : new Date(),
        actionType : "take off"
      })

      let graph = new NodeGraph();
      
      let nodes = this.convertNodesToClient(state);
      console.log(nodes);
      
      graph.addNodes(nodes);
      newState.graph = graph;
      
      AirportStateStore.set(newState);

    });

  }

  convertNodesToClient(serverNodes) {
    let nodeList = serverNodes.map(node => {
      let newNode = new Node(node.x, node.y, node.width, node.height, node.id)
      
      newNode.isStartPoint = node.startPoint;
      newNode.isEndPoint = node.endPoint;
      
      return newNode;
    });
    console.log(serverNodes);
    
    serverNodes.forEach((node,idx) => { 

      if(node.nextStations) { 
      
        Object.keys(node.nextStations).forEach(type => {
      
          node.nextStations[type].forEach((tNode,tIdx) => {

            nodeList[idx].connections.push(
              new NodeConnection(nodeList[tNode.id-1],
                this.actionTypeToClient(type)));

          })
      
        })
      
      }
      
    });
        

    return nodeList;
  }

  actionTypeToClient(type) {
    switch (type) {
      case "takeoff":
        return "AT_TAKEOFF"
      break;
      case "landing":
        return "AT_LANDING"
        break;
    }
  }

}



function initDummy() {
    let width = 50;
    let height = 50;
    let newState = new AirportState();
    let graph = new NodeGraph();

    let node1 = new Node(50,50, width, height);
    let node2 = new Node(200,230, width, height);
    let node3 = new Node(210,40, width, height);
    let node4 = new Node(100,330, width, height);
    let node5 = new Node(300,330, width, height);
    
    node1.connections.push(new NodeConnection(node3, AT_LANDING))
    node3.connections.push(new NodeConnection(node2, AT_LANDING))
    node2.connections.push(new NodeConnection(node4, AT_LANDING))
    node2.connections.push(new NodeConnection(node5, AT_LANDING))
    
    node5.connections.push(new NodeConnection(node2, AT_TAKEOFF))
    node4.connections.push(new NodeConnection(node2, AT_TAKEOFF))
    node2.connections.push(new NodeConnection(node3, AT_TAKEOFF))
    node3.connections.push(new NodeConnection(node1, AT_TAKEOFF))

    let nodes = [node1,node2,node3,node4,node5];    
    graph.addNodes(nodes);
    newState.graph = graph;
    AirportStateStore.set(newState);
}
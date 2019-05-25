
import { writable, get } from "svelte/store"
import { AirportState } from "../models/AirportState"
import { NodeGraph } from "./NodeGraph";
import { Node, NextNode } from "../models/Node";
import { NodeConnection} from "../models/NodeConnection";
import { AT_LANDING, AT_TAKEOFF } from "../models/Events";

export let AirportStateStore = writable(new AirportState());

export class AirportService {

  constructor() {
    this.fetchAirportState();
  }

  fetchAirportState() {
    initDummy();

    let connection = new signalR.HubConnectionBuilder().withUrl("/airport").configureLogging(signalR.LogLevel.Information).build();
    
    connection.start().then(async () => {
      let state = await connection.invoke("GetAirportState");
      
      let convertedState = this.convertToClient(state);
      
      console.log(convertedState);
      

    });

  }

  convertToClient(state) {
    let newState = state.map(node => {

      if(node.nextStations != null) {

        let connections = node.nextStations;
        delete node.nextStations;
        
        node.connections = [];
        
        Object.keys(connections).forEach(type => {

          connections[type].forEach(tNode => {
            node.connections.push({
              node : tNode,
              type : type
            })
          })

        })
      }
      
      return node;
    }) 

    return newState;
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
    
    node1.connections.push(new NextNode(node3, AT_LANDING))
    node3.connections.push(new NextNode(node2, AT_LANDING))
    node2.connections.push(new NextNode(node4, AT_LANDING))
    node2.connections.push(new NextNode(node5, AT_LANDING))
    
    node5.connections.push(new NextNode(node2, AT_TAKEOFF))
    node4.connections.push(new NextNode(node2, AT_TAKEOFF))
    node2.connections.push(new NextNode(node3, AT_TAKEOFF))
    node3.connections.push(new NextNode(node1, AT_TAKEOFF))

    let nodes = [node1,node2,node3,node4,node5];    
    graph.addNodes(nodes);
    newState.graph = graph;
    AirportStateStore.set(newState);
}
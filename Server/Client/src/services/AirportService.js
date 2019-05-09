
import { writable, get } from "svelte/store"
import { AirportState } from "../models/AirportState"
import { NodeGraph } from "./NodeGraph";
import { Node , NodeConnection} from "../models/Node";
import { AT_LANDING, AT_TAKEOFF } from "../models/Events";

export let AirportStateStore = writable(new AirportState());

export class AirportService {

  constructor() {
    this.url = "http://localhost:3000";
    this.fetchAirportState();
  }

  fetchAirportState() {
    initDummy();
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

    graph.addNode(node1);
    graph.addNode(node2);
    graph.addNode(node3);
    graph.addNode(node4);
    graph.addNode(node5);
    newState.graph = graph;
    AirportStateStore.set(newState);
}